namespace FastFood.Web.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Linq;

    using Data;
    using ViewModels.Orders;
    using FastFood.Models;
    using AutoMapper.QueryableExtensions;

    public class OrdersController : Controller
    {
        private readonly FastFoodContext context;
        private readonly IMapper mapper;

        public OrdersController(FastFoodContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IActionResult Create()
        {
            var viewOrder = new CreateOrderViewModel
            {
                Items = this.context.Items.OrderBy(x => x.Id).Select(e => e.Name).ToList(),
                Employees = this.context.Employees.OrderBy(x => x.Id).Select(e=>e.Name).ToList(),
            };

            return this.View(viewOrder);
        }

        [HttpPost]
        public IActionResult Create(CreateOrderInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Error", "Home");
            }
            var order = this.mapper.Map<Order>(model);

            var item = this.context.Items.FirstOrDefault(i => i.Id == model.ItemId);

            order.DateTime = DateTime.Now;

            order.OrderItems.Add(new OrderItem()
            {
                Item = item,
                Quantity = model.Quantity

            });

            order.TotalPrice = model.Quantity * item.Price;

            this.context.Orders.Add(order);

            this.context.SaveChanges();

            return this.RedirectToAction("All", "Orders");
        }

        public IActionResult All()
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Error", "Home");
            }

            var orders = this.context.Orders.ProjectTo<OrderAllViewModel>(mapper.ConfigurationProvider)
                .ToList();

            return this.View(orders);
        }
    }
}
