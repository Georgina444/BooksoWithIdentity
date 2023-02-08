using Bookso.DataAccess.Repository;
using Bookso.DataAccess.Repository.IRepository;
using Bookso.Models;
using Bookso.Models.ViewModels;
using Bookso.Utililty;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;

namespace BooksoWeb.Areas.Admin.Controllers
{
    [Authorize]
	public class OrderController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public OrderVM OrderVM { get; set; }


        public IActionResult Index()
		{
			return View();
		}

        public IActionResult Details(int orderId)
        {
            OrderVM = new OrderVM()
            {
                OrderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == orderId, includeProperties: "ApplicationUser"),
                OrderDetail = _unitOfWork.OrderDetail.GetAll(u => u.OrderId == orderId, includeProperties: "Product"),
            };
            return View(OrderVM);
        }


        [Route("/Admin/Order/GetAll")]
		#region API CALLS
		[HttpGet]
		public IActionResult GetAll(string status)   // gets order list
		{
			IEnumerable<OrderHeader> orderHeaders;

            if (User.IsInRole(SD.Role_Admin))
            {
            orderHeaders = _unitOfWork.OrderHeader.GetAll(includeProperties:"ApplicationUser");
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                orderHeaders = _unitOfWork.OrderHeader.GetAll(u=>u.ApplicationUserId==claim.Value,includeProperties:"ApplicationUser");

            }

            // orderHeaders = _unitOfWork.OrderHeader.GetAll().Include(x => x.ApplicationUser);

            switch (status)
            {
                case "pending":
                    orderHeaders = orderHeaders.Where(u => u.PaymentStatus == SD.PaymentStatusPending);
                    break;
                case "inprocess":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusInProcess);
                    break;
                case "completed":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusShipped);
                    break;
                case "approved":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusApproved);
                    break;
                default:
                    break;
            }

            return Json(new { data = orderHeaders });
		}
		#endregion
	}

}
