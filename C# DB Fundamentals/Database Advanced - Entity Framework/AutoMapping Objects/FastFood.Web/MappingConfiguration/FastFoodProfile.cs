namespace FastFood.Web.MappingConfiguration
{
    using AutoMapper;
    using FastFood.Models.Enums;
    using FastFood.Web.ViewModels.Categories;
    using FastFood.Web.ViewModels.Employees;
    using FastFood.Web.ViewModels.Items;
    using FastFood.Web.ViewModels.Orders;
    using Models;
    using System;
    using ViewModels.Positions;

    public class FastFoodProfile : Profile
    {
        public FastFoodProfile()
        {
            //Positions
            this.CreateMap<CreatePositionInputModel, Position>()
                .ForMember(x => x.Name, y => y.MapFrom(s => s.PositionName));

            this.CreateMap<Position, PositionsAllViewModel>()
                .ForMember(x => x.Name, y => y.MapFrom(s => s.Name));

            //Employees
            this.CreateMap<Position, RegisterEmployeeViewModel>()
                .ForMember(p=> p.Position ,opt=>opt.MapFrom(vm=>vm.Name));

            this.CreateMap<RegisterEmployeeInputModel,Employee>();

            this.CreateMap<Employee, EmployeesAllViewModel>()
                .ForMember(vm=>vm.Position,opt=>opt.MapFrom(e=>e.Position.Name));

            //Items
            this.CreateMap<CreateCategoryInputModel, Category>()
                .ForMember(c => c.Name, opt => opt.MapFrom(vm => vm.CategoryName));

            this.CreateMap<CategoryAllViewModel, Category>();

            this.CreateMap<CreateItemInputModel, Item>();

            this.CreateMap<Category,CreateItemViewModel>()
                 .ForMember(vm => vm.Category, opt => opt.MapFrom(i => i.Name));

            this.CreateMap<Item, ItemsAllViewModels>()
                .ForMember(c => c.Category, opt => opt.MapFrom(i => i.Category.Name));

            //Orders
            this.CreateMap<CreateOrderInputModel, Order>()
                .ForMember(o => o.Type, opt => opt.MapFrom(e => Enum.Parse<OrderType>(e.Type)));

            this.CreateMap<Order, OrderAllViewModel>()
                .ForMember(e => e.DateTime, opt => opt.MapFrom(e => e.DateTime.ToString("g")))
                .ForMember(e => e.OrderId, opt => opt.MapFrom(e => e.Id))
                .ForMember(e => e.Employee, opt => opt.MapFrom(e => e.Employee.Name));
        }
    }
}
