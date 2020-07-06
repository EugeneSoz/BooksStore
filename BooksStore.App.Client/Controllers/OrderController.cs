using System;
using System.Collections.Generic;
using BooksStore.App.Client.Infrastructure;
using BooksStore.App.Contracts.Command;
using BooksStore.App.Handlers.Command;
using BooksStore.Domain.Contracts.Models.Orders;
using Microsoft.AspNetCore.Mvc;

namespace BooksStore.App.Client.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderCommandHandler _orderCommandHandler;

        public OrderController(OrderCommandHandler orderCommandHandler)
        {
            _orderCommandHandler = orderCommandHandler ?? throw new ArgumentNullException(nameof(orderCommandHandler));
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
            var order = HttpContext.Session.GetJson<Order>("order") ?? new Order();

            return View("CheckoutSummary", order);
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
