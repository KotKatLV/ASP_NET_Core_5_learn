using Braintree;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Rocky.DAL.Repository.Interfaces;
using Rocky.Domain;
using Rocky.Utils;
using Rocky.Utils.BrainTree;
using Rocky.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Rocky.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IApplicationTypeRepository _applicationTypeRepository;
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IInquiryHeaderRepository _inquiryHeaderRepository;
        private readonly IInquiryDetailRepository _inquiryDetailRepository;
        private readonly IOrderHeaderRepository _orderHeaderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSender _emailSender;
        private readonly IBrainTreeGate _brainTree;

        [BindProperty]
        public ProductUserViewModel ProductUserViewModel { get; set; }

        public CartController(IWebHostEnvironment webHostEnvironment, IEmailSender emailSender, IProductRepository productRepository, IApplicationTypeRepository applicationTypeRepository, IApplicationUserRepository applicationUserRepository, IInquiryDetailRepository inquiryDetailRepository, IInquiryHeaderRepository inquiryHeaderRepository, IOrderDetailRepository orderDetailRepository, IOrderHeaderRepository orderHeaderRepository, IBrainTreeGate brainTree)
        {
            _productRepository = productRepository;
            _applicationTypeRepository = applicationTypeRepository;
            _applicationUserRepository = applicationUserRepository;
            _inquiryDetailRepository = inquiryDetailRepository;
            _inquiryHeaderRepository = inquiryHeaderRepository;
            _webHostEnvironment = webHostEnvironment;
            _emailSender = emailSender;
            _orderDetailRepository = orderDetailRepository;
            _orderHeaderRepository = orderHeaderRepository;
            _brainTree = brainTree;
        }

        public IActionResult Index()
        {
            List<ShoppingCart> shoppingCarts = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Any())
            {
                // session exists
                shoppingCarts = HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).ToList();
            }

            List<int> prodInCart = shoppingCarts.Select(p => p.ProductId).ToList();
            IEnumerable<Product> productListTmp = _productRepository.GetAll(p => prodInCart.Contains(p.Id));
            IList<Product> productList = new List<Product>();

            foreach (var cartObj in shoppingCarts)
            {
                Product product = productListTmp.FirstOrDefault(u => u.Id == cartObj.ProductId);
                product.TempSqFt = cartObj.SqFt;
                productList.Add(product);
            }

            return View(productList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost(IEnumerable<Product> products)
        {
            List<ShoppingCart> shoppingCarts = new List<ShoppingCart>();

            foreach (var prod in products)
            {
                shoppingCarts.Add(new ShoppingCart { ProductId = prod.Id, SqFt = prod.TempSqFt });
            }

            HttpContext.Session.Set(WC.SessionCart, shoppingCarts);
            return RedirectToAction(nameof(Summary));
        }

        [HttpGet]
        public IActionResult Summary()
        {
            ApplicationUser applicationUser;

            if (User.IsInRole(WC.AdminRole))
            {
                if (HttpContext.Session.Get<int>(WC.SessionInquiryId) != 0)
                {
                    // cart has been loaded using inquiry
                    InquiryHeader inquiryHeader = _inquiryHeaderRepository.FirstOrDefault(u => u.Id == HttpContext.Session.Get<int>(WC.SessionInquiryId));
                    applicationUser = new ApplicationUser
                    {
                        FullName = inquiryHeader.FullName,
                        Email = inquiryHeader.Email,
                        PhoneNumber = inquiryHeader.PhoneNumber
                    };
                }
                else
                {
                    applicationUser = new ApplicationUser();
                }

                var gateWay = _brainTree.GetGateWay();
                var clientToken = gateWay.ClientToken.Generate();
                ViewBag.ClientToken = clientToken;
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                //var userId = User.FindFirstValue(ClaimTypes.Name);

                applicationUser = _applicationUserRepository.FirstOrDefault(u => u.Id == claim.Value);
            }

            List<ShoppingCart> shoppingCarts = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Any())
            {
                // session exists
                shoppingCarts = HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).ToList();
            }

            List<int> prodInCart = shoppingCarts.Select(p => p.ProductId).ToList();
            IEnumerable<Product> productList = _productRepository.GetAll(p => prodInCart.Contains(p.Id));

            ProductUserViewModel = new ProductUserViewModel
            {
                ApplicationUser = applicationUser,
            };

            foreach (var cartObj in shoppingCarts)
            {
                Product prodTemp = _productRepository.FirstOrDefault(u => u.Id == cartObj.ProductId);
                prodTemp.TempSqFt = cartObj.SqFt;
                ProductUserViewModel.ProductList.Add(prodTemp);
            }

            return View(ProductUserViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPostAsync(IFormCollection collection, ProductUserViewModel productUserViewModel)
        {
            var clainsIdentity = (ClaimsIdentity)User.Identity;
            var claim = clainsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (User.IsInRole(WC.AdminRole))
            {
                OrderHeader orderHeader = new()
                {
                    CreatedByUserId = claim.Value,
                    FinalOrderTotal = (double)ProductUserViewModel.ProductList.Sum(x => x.TempSqFt * x.Price),
                    City = ProductUserViewModel.ApplicationUser.City,
                    StreetAddress = ProductUserViewModel.ApplicationUser.StreetAddress,
                    State = ProductUserViewModel.ApplicationUser.State,
                    PostalCode = ProductUserViewModel.ApplicationUser.PostalCode,
                    FullName = ProductUserViewModel.ApplicationUser.FullName,
                    Email = ProductUserViewModel.ApplicationUser.Email,
                    PhoneNumber = ProductUserViewModel.ApplicationUser.PhoneNumber,
                    OrderDate = DateTime.Now,
                    OrderStatus = WC.OrderStatusPending
                };

                _orderHeaderRepository.Add(orderHeader);
                _orderHeaderRepository.Save();

                foreach (var prod in ProductUserViewModel.ProductList)
                {
                    OrderDetail orderDetail = new OrderDetail()
                    {
                        OrderHeaderId = orderHeader.Id,
                        PricePerSqFt = (double)prod.Price,
                        Sqft = prod.TempSqFt,
                        ProductId = prod.Id
                    };

                    _orderDetailRepository.Add(orderDetail);
                }

                _orderDetailRepository.Save();

                string nonceFromTheClient = collection["payment_method_nonce"];

                var request = new TransactionRequest
                {
                    Amount = (decimal)orderHeader.FinalOrderTotal,
                    PaymentMethodNonce = nonceFromTheClient,
                    OrderId = orderHeader.Id.ToString(),
                    Options = new TransactionOptionsRequest
                    {
                        SubmitForSettlement = true
                    }
                };

                var gateway = _brainTree.GetGateWay();
                Result<Transaction> result = gateway.Transaction.Sale(request);

                if (result.Target.ProcessorResponseText == "Approved")
                {
                    orderHeader.TransactionId = result.Target.Id;
                    orderHeader.OrderStatus = WC.OrderStatusApproved;
                }
                else
                {
                    orderHeader.OrderStatus = WC.OrderStatusCancelled;
                }

                _orderHeaderRepository.Save();
                return RedirectToAction(nameof(InquiryConfirmation), new { id = orderHeader.Id });
            }
            else
            {
                var pathToTemplate = _webHostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString() + "templates" + Path.DirectorySeparatorChar.ToString() + "Inquiry.html";
                var subject = "New Inquiry";
                string htmlBody = "";
                using (StreamReader sr = System.IO.File.OpenText(pathToTemplate))
                {
                    htmlBody = sr.ReadToEnd();
                }

                StringBuilder productListSB = new StringBuilder();
                foreach (var prod in productUserViewModel.ProductList)
                {
                    productListSB.Append($" - Name: {prod.Name} <span style='font-size 14px'>(ID: {prod.Id})</span> <br />");
                }

                string messageBody = string.Format(htmlBody,
                    productUserViewModel.ApplicationUser.FullName,
                    productUserViewModel.ApplicationUser.Email,
                    productUserViewModel.ApplicationUser.PhoneNumber,
                    productListSB.ToString());

                await _emailSender.SendEmailAsync(WC.AdminEmail, subject, messageBody);

                InquiryHeader inquiryHeader = new InquiryHeader()
                {
                    ApplicationUserId = claim.Value,
                    FullName = productUserViewModel.ApplicationUser.FullName,
                    Email = productUserViewModel.ApplicationUser.Email,
                    PhoneNumber = productUserViewModel.ApplicationUser.PhoneNumber,
                    InquiryDate = DateTime.Now
                };

                _inquiryHeaderRepository.Add(inquiryHeader);
                _inquiryHeaderRepository.Save();

                foreach (var prod in ProductUserViewModel.ProductList)
                {
                    InquiryDetail inquiryDetail = new InquiryDetail()
                    {
                        InquiryHeaderId = inquiryHeader.Id,
                        ProductId = prod.Id
                    };

                    _inquiryDetailRepository.Add(inquiryDetail);

                }

                _inquiryDetailRepository.Save();
            }

            return RedirectToAction(nameof(InquiryConfirmation));
        }

        public IActionResult InquiryConfirmation(int id = 0)
        {
            OrderHeader orderHeader = _orderHeaderRepository.FirstOrDefault(u => u.Id == id);
            HttpContext.Session.Clear();
            return View(orderHeader);
        }

        public IActionResult Remove(int id)
        {
            List<ShoppingCart> shoppingCarts = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                // session exists
                shoppingCarts = HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).ToList();
            }

            shoppingCarts.Remove(shoppingCarts.FirstOrDefault(u => u.ProductId == id));

            List<int> prodInCart = shoppingCarts.Select(p => p.ProductId).ToList();
            IEnumerable<Product> productList = _productRepository.GetAll(p => prodInCart.Contains(p.Id));
            HttpContext.Session.Set(WC.SessionCart, shoppingCarts);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateCart(IEnumerable<Product> products)
        {
            List<ShoppingCart> shoppingCarts = new List<ShoppingCart>();

            foreach (var prod in products)
            {
                shoppingCarts.Add(new ShoppingCart { ProductId = prod.Id, SqFt = prod.TempSqFt });
            }

            HttpContext.Session.Set(WC.SessionCart, shoppingCarts);
            return RedirectToAction(nameof(Index));
        }
    }
}