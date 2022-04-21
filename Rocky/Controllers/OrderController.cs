using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rocky.DAL.Repository.Interfaces;
using Rocky.UI.Web.ViewModels;
using Rocky.UI.Web.VIewModels;
using Rocky.Utils;
using Rocky.Utils.BrainTree;
using System.Linq;

namespace Rocky.UI.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderHeaderRepository _orderHeaderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IBrainTreeGate _brainTree;

        [BindProperty]
        public OrderViewModel OrderViewModel { get; set; }

        public OrderController(IOrderDetailRepository orderDetailRepository, IOrderHeaderRepository orderHeaderRepository, IBrainTreeGate brainTree)
        {
            _orderDetailRepository = orderDetailRepository;
            _orderHeaderRepository = orderHeaderRepository;
            _brainTree = brainTree;
        }

        public IActionResult Index(string searchName, string searchEmail, string searchPhone, string Status)
        {
            OrderListViewModel orderListView = new OrderListViewModel()
            {
                OrderHList = _orderHeaderRepository.GetAll(),
                StatusList = WC.listStatus.ToList().Select(i => new SelectListItem
                {
                    Text = i,
                    Value = i
                })
            };

            if (!string.IsNullOrEmpty(searchName))
            {
                orderListView.OrderHList = orderListView.OrderHList.Where(u => u.FullName.ToLower().Contains(searchName.ToLower()));
            }

            if (!string.IsNullOrEmpty(searchEmail))
            {
                orderListView.OrderHList = orderListView.OrderHList.Where(u => u.Email.ToLower().Contains(searchEmail.ToLower()));
            }

            if (!string.IsNullOrEmpty(searchPhone))
            {
                orderListView.OrderHList = orderListView.OrderHList.Where(u => u.PhoneNumber.ToLower().Contains(searchPhone.ToLower()));
            }

            if (!string.IsNullOrEmpty(Status) && Status != "--Order Status--")
            {
                orderListView.OrderHList = orderListView.OrderHList.Where(u => u.OrderStatus.ToLower().Contains(Status.ToLower()));
            }

            return View(orderListView);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            OrderViewModel = new OrderViewModel()
            {
                OrderHeader = _orderHeaderRepository.FirstOrDefault(u => u.Id == id),
                OrderDetail = _orderDetailRepository.GetAll(a => a.OrderHeaderId == id, includProps: "Product")
            };

            return View(OrderViewModel);
        }

    }
}