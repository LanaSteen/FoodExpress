using ConsoleProject.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodExpress.Models
{
	internal class Menu
	{

		public List<Food> Foods { get; set; }
		public Restaurant Restaurant { get; set; }
	}
}
