using ConsoleProject.Models;

namespace ConsoleProject.Repositories
{
	internal class FoodRepository
	{
		private readonly string _filePath;

		public FoodRepository()
		{
			_filePath = GetFilePath("food.txt");
			CreateFileIfItDoesNotExist();
		}

		public void SaveFood(Food food)
		{
			File.AppendAllText(_filePath, food + Environment.NewLine);
		}

		public List<Food> GetAllFoods()
		{
			var foods = new List<Food>();
			string[] lines = File.ReadAllLines(_filePath);

			foreach (string line in lines)
			{
				if (string.IsNullOrWhiteSpace(line))
					continue;

				string[] values = line.Split('|');
				if (values.Length != 3)
					continue;
				if (!int.TryParse(values[0], out int id))
					continue;

				foods.Add(new Food
				{
					Id = id,
					Name = values[1],
					Description = values[2]
				});
			}

			return foods;
		}

		public Food? GetFoodById(int id)
		{
			List<Food> foods = GetAllFoods();

			foreach (Food food in foods)
			{
				if (food.Id == id)
					return food;
			}

			return null;
		}

		public void DeleteFood(int id)
		{
			List<Food> foods = GetAllFoods();
			int index = -1;
			for (int i = 0; i < foods.Count; i++)
			{
				if (foods[i].Id == id)
				{
					index = i;
					break;
				}
			}

			if (index == -1)
				return;

			foods.RemoveAt(index);
			SaveAllFoods(foods);
		}

		public void UpdateFood(Food newFood)
		{
			List<Food> foods = GetAllFoods();

			foreach (Food food in foods)
			{
				if (food.Id == newFood.Id)
				{
					food.Name = newFood.Name;
					food.Description = newFood.Description;
				}
			}

			SaveAllFoods(foods);
		}

		private void SaveAllFoods(List<Food> foods)
		{
			List<string> lines = new List<string>();
			foreach (Food food in foods)
				lines.Add(food.ToString());

			File.WriteAllLines(_filePath, lines);
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
