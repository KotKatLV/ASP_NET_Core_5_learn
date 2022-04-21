using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rocky.DAL.Repository.Interfaces;
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

        public OrderController(IOrderDetailRepository orderDetailRepository, IOrderHeaderRepository orderHeaderRepository, IBrainTreeGate brainTree)
        {
            _orderDetailRepository = orderDetailRepository;
            _orderHeaderRepository = orderHeaderRepository;
            _brainTree = brainTree;
        }

        public IActionResult Index()
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

            return View(orderListView);
        }

    }
}