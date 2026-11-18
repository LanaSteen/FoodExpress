using ConsoleProject.Models;
using ConsoleProject.Services;

namespace ConsoleProject.Menu
{
	internal class ClientMenu
	{
		private readonly FoodService _foodService;
		private readonly OrderService _orderService;

		public ClientMenu(FoodService foodService, OrderService orderService)
		{
			_foodService = foodService;
			_orderService = orderService;
		}

		public void Show(User user)
		{
			while (true)
			{
				Console.WriteLine("\n--- Client menu ---");
				Console.WriteLine("1. Show foods");
				Console.WriteLine("2. Search food");
				Console.WriteLine("3. Place order");
				Console.WriteLine("4. My orders");
				Console.WriteLine("0. Logout");
				Console.Write("Choose: ");
				string choice = Console.ReadLine() ?? "";

				try
				{
					if (choice == "1") ShowFoods(_foodService.GetAllFoods());
					else if (choice == "2") SearchFoods();
					else if (choice == "3") PlaceOrder(user);
					else if (choice == "4") ShowMyOrders(user);
					else if (choice == "0") return;
					else Console.WriteLine("Invalid choice.");
				}
				catch (Exception error)
				{
					Console.WriteLine("Error: " + error.Message);
				}
			}
		}

		private void SearchFoods()
		{
			Console.Write("Search text: ");
			string text = Console.ReadLine() ?? "";
			ShowFoods(_foodService.SearchFoods(text));
		}

		private void PlaceOrder(User user)
		{
			List<Food> foods = _foodService.GetAllFoods();
			if (foods.Count == 0)
			{
				Console.WriteLine("The food list is empty.");
				return;
			}

			ShowFoods(foods);
			int foodId = ReadInt("Food id: ");
			Food? food = _foodService.GetFoodById(foodId);
			if (food == null)
			{
				Console.WriteLine("Food was not found.");
				return;
			}

			int quantity = ReadInt("Quantity: ");
			Order order = _orderService.CreateOrder(user, food, quantity);
			Console.WriteLine("Order created. Order id: " + order.Id);
		}

		private void ShowMyOrders(User user)
		{
			List<Order> orders = _orderService.GetOrdersForUser(user.Id);
			foreach (Order order in orders)
				Console.WriteLine($"{order.Id}. {order.FoodName}, quantity: {order.Quantity}, date: {order.CreatedAt}");

			if (orders.Count == 0)
				Console.WriteLine("You have no orders.");
		}

		private static void ShowFoods(List<Food> foods)
		{
			foreach (Food food in foods)
				Console.WriteLine($"{food.Id}. {food.Name} - {food.Description}");

			if (foods.Count == 0)
				Console.WriteLine("No foods found.");
		}

		private static int ReadInt(string message)
		{
			Console.Write(message);
			if (int.TryParse(Console.ReadLine(), out int number))
				return number;
			throw new ArgumentException("Please enter a number.");
		}
	}
}
