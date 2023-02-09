using Bookso.DataAccess.Repository;
using Bookso.DataAccess.Repository.IRepository;
using Bookso.Models;
using Bookso.Models.ViewModels;
using Bookso.Utililty;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Stripe;
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
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public IActionResult Index()
        {
            return View();
        }

        [Route("Admin/Order/Details")]
        public IActionResult Details(int orderId)
        {
            OrderVM = new OrderVM()
            {
                OrderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == orderId, includeProperties: "ApplicationUser"),
                OrderDetail = _unitOfWork.OrderDetail.GetAll(u => u.OrderId == orderId, includeProperties: "Product"),
			};
            return View(OrderVM);
        }

        //[Route("Admin/Order/UpdateOrderDetail")]
        [HttpPost]
        [Authorize(Roles =SD.Role_Admin)]
        [ValidateAntiForgeryToken]
		public IActionResult UpdateOrderDetail()
		{
            // 1 - retreive the order header from db 
            var orderHeaderFromDb = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id,tracked:false);
            orderHeaderFromDb.Name = OrderVM.OrderHeader.Name; 
            orderHeaderFromDb.PhoneNumber = OrderVM.OrderHeader.PhoneNumber; 
            orderHeaderFromDb.StreetAddress = OrderVM.OrderHeader.StreetAddress; 
            orderHeaderFromDb.City = OrderVM.OrderHeader.City; 
            orderHeaderFromDb.State = OrderVM.OrderHeader.State; 
            orderHeaderFromDb.PostalCode = OrderVM.OrderHeader.PostalCode;
			if (OrderVM.OrderHeader.Carrier != null)
			{
				orderHeaderFromDb.Carrier = OrderVM.OrderHeader.Carrier;
			}
            if (OrderVM.OrderHeader.TrackingNumber != null)
            {
                orderHeaderFromDb.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
            }
            _unitOfWork.OrderHeader.Update(orderHeaderFromDb);
			_unitOfWork.Save();
			TempData["Success"] = "Order Details Updated Successfully.";
			return RedirectToAction("Details", "Order", new { orderId = orderHeaderFromDb.Id });
		}


		[HttpPost]
		[Authorize(Roles = SD.Role_Admin)]
		[ValidateAntiForgeryToken]
		public IActionResult StartProcessing()
		{
            _unitOfWork.OrderHeader.UpdateStatus(OrderVM.OrderHeader.Id, SD.StatusInProcess);
			_unitOfWork.Save();
			TempData["Success"] = "Order Status Updated Successfully.";
			return RedirectToAction("Details", "Order", new { orderId = OrderVM.OrderHeader.Id });
		}

		[HttpPost]
		[Authorize(Roles = SD.Role_Admin)]
		[ValidateAntiForgeryToken]
		public IActionResult ShipOrder()
		{
			var orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id, tracked: false);
            orderHeader.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
            orderHeader.Carrier = OrderVM.OrderHeader.Carrier;
            orderHeader.OrderStatus= OrderVM.OrderHeader.OrderStatus;
            orderHeader.ShippingDate = DateTime.Now;

            _unitOfWork.OrderHeader.Update(orderHeader);
			_unitOfWork.Save();
			TempData["Success"] = "Order Shipped Successfully.";
			return RedirectToAction("Details", "Order", new { orderId = OrderVM.OrderHeader.Id });
		}

		[HttpPost]
		[Authorize(Roles = SD.Role_Admin)]
		[ValidateAntiForgeryToken]
		public IActionResult CancelOrder()
		{
			var orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id, tracked: false);

            if(orderHeader.PaymentStatus == SD.PaymentStatusApproved)
            {
                var options = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderHeader.PaymentIntentId,
                };
                var service = new RefundService();
                Refund refund = service.Create(options);

                _unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id, SD.StatusCancelled, SD.StatusRefunded);
            }
            else
            {
                _unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id, SD.StatusCancelled, SD.StatusCancelled);
            }
            _unitOfWork.Save();

			TempData["Success"] = "Order Canceled Successfully.";
			return RedirectToAction("Details", "Order", new { orderId = OrderVM.OrderHeader.Id });
		}


		[Route("/Admin/Order/GetAll")]
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll(string status)   // gets order list
        {
            IEnumerable<OrderHeader> orderHeaders;
            if (User == null)
            {
                return BadRequest("User object is null");
            }

            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.IsInRole(SD.Role_Admin))
                {
                    if (_unitOfWork == null)
                    {
                        return BadRequest("Unit of work object is null");
                    }

                    orderHeaders = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser");
                }
                else
                {
                    var claimsIdentity = (ClaimsIdentity)User.Identity;
                    var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                    orderHeaders = _unitOfWork.OrderHeader.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties: "ApplicationUser");
                }
            }
            else
            {
                // Handle the case where the User object is null
                return BadRequest("User is not authenticated");
            }

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

