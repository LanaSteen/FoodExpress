using ConsoleProject.Models;

namespace ConsoleProject.Repositories
{
	internal class OrderRepository
	{
		private readonly string _filePath;

		public OrderRepository()
		{
			_filePath = GetFilePath("order.txt");
			CreateFileIfItDoesNotExist();
		}

		public void SaveOrder(Order order)
		{
			File.AppendAllText(_filePath, order + Environment.NewLine);
		}

		public List<Order> GetAllOrders()
		{
			var orders = new List<Order>();
			string[] lines = File.ReadAllLines(_filePath);

			foreach (string line in lines)
			{
				if (string.IsNullOrWhiteSpace(line))
					continue;

				string[] values = line.Split('|');
				if (values.Length != 6)
					continue;
				if (!int.TryParse(values[0], out int id))
					continue;
				if (!int.TryParse(values[1], out int userId))
					continue;
				if (!int.TryParse(values[2], out int foodId))
					continue;
				if (!int.TryParse(values[4], out int quantity))
					continue;
				if (!DateTime.TryParse(values[5], out DateTime createdAt))
					continue;

				orders.Add(new Order
				{
					Id = id,
					UserId = userId,
					FoodId = foodId,
					FoodName = values[3],
					Quantity = quantity,
					CreatedAt = createdAt
				});
			}

			return orders;
		}

		private static string GetFilePath(string fileName)
		{
			return Path.GetFullPath(Path.Combine(
				AppContext.BaseDirectory, "..", "..", "..", "Data", fileName));
		}

		private void CreateFileIfItDoesNotExist()
		{
			Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
			if (!File.Exists(_filePath))
				File.Create(_filePath).Close();
		}
	}
}
