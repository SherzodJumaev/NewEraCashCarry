using PDP.University.Examine.Project.Web.API.DTOs.OrderDTOs;
using PDP.University.Examine.Project.Web.API.Models;

namespace PDP.University.Examine.Project.Web.API.Mappers.OrderMaps
{
    public static class OrderItemMappers
    {
        public static OrderItem ToOrderItem(this CreateOrderItemDto itemInfo)
        {
            return new OrderItem
            {
                Quantity = itemInfo.Quantity,
            };
        }
    }
}
