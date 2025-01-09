using PDP.University.Examine.Project.Web.API.DTOs.OrderDTOs;
using PDP.University.Examine.Project.Web.API.Models;

namespace PDP.University.Examine.Project.Web.API.Mappers.OrderMaps
{
    public static class OrderMappers
    {
        public static OrderDto FromOrderToOrderDto(this Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                TotalAmount = order.TotalAmount,
                OrderDate = order.OrderDate,
                OrderCustomer = order.Customer.FromCustomerToOrderCustomerDto(),
                OrderItemsInfo = order.OrderItems.Select(x => x.FromOrderItemToOrderItemDto()).ToList(),
                PaymentStatus = order.PaymentStatus,
            };
        }

        public static OrderCustomerInfo FromCustomerToOrderCustomerDto(this Customer customer)
        {
            if (customer == null)
            {
                return null;
            }

            return new OrderCustomerInfo
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
            };
        }

        public static OrderItemInfo FromOrderItemToOrderItemDto(this OrderItem itemModel)
        {   
            return new OrderItemInfo
            {
                ProductId = itemModel.ProductId,
                Quantity = itemModel.Quantity,
                ProductPrice = itemModel.Product.Price,
                ProductName = itemModel.Product.Name,
            };
        }

        public static OrderItemInfoForCustomer ToOrderItemInfoForCustomer(this OrderItem itemModel)
        {
            return new OrderItemInfoForCustomer
            {
                Quantity = itemModel.Quantity,
                ProductPrice = itemModel.Product.Price,
                ProductName = itemModel.Product.Name,
            };
        }

        public static Order ToOrder(this CreateOrderDto createOrderDto)
        {
            return new Order
            {
                CustomerId = createOrderDto.CustomerId,
                OrderDate = DateTime.Now,
                PaymentStatus = "Pending",
            };
        }

        public static Order ToOrderFromUpdate(this UpdateOrderRequestDto requestDto)
        {
            return new Order
            {
                CustomerId = requestDto.CustomerId,
                OrderItems = requestDto.OrderItems.Select(s => s.ToOrderItem()).ToList(),
            };
        }

        public static OrderStatus ToOrderStatus(this Order orderModel)
        {
            return new OrderStatus
            {
                PaymentStatus = orderModel.PaymentStatus
            };
        }

        public static OrderDtoForCustomer ToOrderDtoForCustomer(this Order orderModel)
        {
            return new OrderDtoForCustomer
            {
                OrderItemsInfo = orderModel.OrderItems.Select(oi => oi.ToOrderItemInfoForCustomer()).ToList(),
                OrderDate = orderModel.OrderDate,
                TotalAmount = orderModel.TotalAmount,
                PaymentStatus = orderModel.PaymentStatus
            };
        }
    }
}
