using ConsoleProject.Models;
using ConsoleProject.Repositories;

namespace ConsoleProject.Services
{
	internal class FoodService
	{
		private readonly FoodRepository _foodRepository;

		public FoodService(FoodRepository foodRepository)
		{
			_foodRepository = foodRepository;
		}

		public List<Food> GetAllFoods()
		{
			return _foodRepository.GetAllFoods();
		}

		public Food? GetFoodById(int id)
		{
			return _foodRepository.GetFoodById(id);
		}

		public void AddFood(string name, string description)
		{
			CheckFoodData(name, description);

			List<Food> foods = GetAllFoods();
			Food food = new Food();
			food.Id = GetNextId(foods);
			food.Name = name.Trim();
			food.Description = description.Trim();

			_foodRepository.SaveFood(food);
		}

		public bool UpdateFood(int id, string name, string description)
		{
			CheckFoodData(name, description);

			Food? food = GetFoodById(id);
			if (food == null)
				return false;

			food.Name = name.Trim();
			food.Description = description.Trim();
			_foodRepository.UpdateFood(food);
			return true;
		}

		public bool DeleteFood(int id)
		{
			Food? food = GetFoodById(id);
			if (food == null)
				return false;

			_foodRepository.DeleteFood(id);
			return true;
		}

		public List<Food> SearchFoods(string text)
		{
			List<Food> result = new List<Food>();
			List<Food> foods = GetAllFoods();
			if (string.IsNullOrWhiteSpace(text))
				return foods;

			foreach (Food food in foods)
			{
				if (food.Name.Contains(text, StringComparison.OrdinalIgnoreCase)
					|| food.Description.Contains(text, StringComparison.OrdinalIgnoreCase))
				{
					result.Add(food);
				}
			}

			return result;
		}

		private static int GetNextId(List<Food> foods)
		{
			int biggestId = 0;
			foreach (Food food in foods)
			{
				if (food.Id > biggestId)
					biggestId = food.Id;
			}
			return biggestId + 1;
		}

		private static void CheckFoodData(string name, string description)
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentException("Food name cannot be empty.");
			if (string.IsNullOrWhiteSpace(description))
				throw new ArgumentException("Food description cannot be empty.");
		}
	}
}
