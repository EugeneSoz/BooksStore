using System;
using System.Collections.Generic;
using BooksStore.App.Client.Infrastructure;
using BooksStore.App.Contracts.Command;
using BooksStore.App.Contracts.Query;
using BooksStore.App.Handlers.Command;
using BooksStore.App.Handlers.Query;
using BooksStore.Domain.Contracts.Models.Orders;
using Microsoft.AspNetCore.Mvc;

namespace BooksStore.App.Client.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderCommandHandler _orderCommandHandler;
        private readonly OrderQueryHandler _orderQueryHandler;

        public OrderController(OrderCommandHandler orderCommandHandler, OrderQueryHandler orderQueryHandler)
        {
            _orderCommandHandler = orderCommandHandler ?? throw new ArgumentNullException(nameof(orderCommandHandler));
            _orderQueryHandler = orderQueryHandler ?? throw new ArgumentNullException(nameof(orderQueryHandler));
        }

        [HttpGet]
        public IActionResult ShowCheckoutDetails()
        {
            var customer = HttpContext.Session.GetJson<Customer>("customer") ?? new Customer();

            return View("CheckoutDetails", customer);
        }

        [HttpPost]
        public RedirectToActionResult ShowCheckoutDetails(CreateCustomerCommand command)
        {
            var result = _orderCommandHandler.Handle(command);
            HttpContext.Session.SetJson("customer", result);

            return RedirectToAction("ShowCheckoutPayment");
        }

        [HttpGet]
        public IActionResult ShowCheckoutPayment()
        {
            var payment = HttpContext.Session.GetJson<Payment>("payment") ?? new Payment();

            return View("CheckoutPayment", payment);
        }

        [HttpPost]
        public RedirectToActionResult ShowCheckoutPayment(CreatePaymentCommand command)
        {
            command.Lines = HttpContext.Session.GetJson<List<CartLine>>("cart") ?? new List<CartLine>();
            var result = _orderCommandHandler.Handle(command);
            HttpContext.Session.SetJson("payment", result);

            return RedirectToAction("ShowCheckoutSummary");
        }

        [HttpGet]
        public IActionResult ShowCheckoutSummary()
        {
            var customer = HttpContext.Session.GetJson<Customer>("customer");
            var payment = HttpContext.Session.GetJson<Payment>("payment");
            var lines = HttpContext.Session.GetJson<List<CartLine>>("cart");
            if (customer == null || payment == null || lines == null)
            {
                return RedirectToAction("ShowCheckoutPayment");
            }

            var query = new OrderQuery
            {
                Customer = customer,
                Payment = payment,
                Lines = lines
            };

            var result = _orderQueryHandler.Handle(query);

            return View("CheckoutSummary", result);
        }

        [HttpPost]
        public RedirectToActionResult ShowCheckoutSummary(CreateOrderCommand command)
        {
            var customer = HttpContext.Session.GetJson<Customer>("customer");
            var payment = HttpContext.Session.GetJson<Payment>("payment");
            var lines = HttpContext.Session.GetJson<List<CartLine>>("cart");
            if (customer == null || payment == null || lines == null)
            {
                return RedirectToAction("ShowCheckoutDetails");
            }

            command.Customer = customer;
            command.Payment = payment;
            command.Lines = lines;
            _orderCommandHandler.Handle(command);

            return RedirectToAction("ShowComfirmedOrder");
        }

        [HttpGet]
        public IActionResult ShowComfirmedOrder()
        {
            return View("OrderConfirmation", new Order());
        }
    }
}
