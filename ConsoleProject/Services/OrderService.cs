using ConsoleProject.Models;
using ConsoleProject.Repositories;

namespace ConsoleProject.Services
{
    internal class OrderService
    {
        private readonly OrderRepository _orderRepository;

        public OrderService(OrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public Order CreateOrder(User user, Food food, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");

            List<Order> orders = _orderRepository.GetAllOrders();
            Order order = new Order();
            order.Id = GetNextId(orders);
            order.UserId = user.Id;
            order.FoodId = food.Id;
            order.FoodName = food.Name;
            order.Quantity = quantity;

            _orderRepository.SaveOrder(order);
            return order;
        }

        public List<Order> GetOrdersForUser(int userId)
        {
            List<Order> result = new List<Order>();
            List<Order> orders = _orderRepository.GetAllOrders();

            foreach (Order order in orders)
            {
                if (order.UserId == userId)
                    result.Add(order);
            }

            return result;
        }

        public List<Order> GetAllOrders()
        {
            return _orderRepository.GetAllOrders();
        }

        private static int GetNextId(List<Order> orders)
        {
            int biggestId = 0;
            foreach (Order order in orders)
            {
                if (order.Id > biggestId)
                    biggestId = order.Id;
            }
            return biggestId + 1;
        }
    }
}
