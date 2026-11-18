using ConsoleProject.Exceptions;
using ConsoleProject.Models;
using ConsoleProject.Services;

namespace ConsoleProject.Menu
{
	internal class UserMenu
	{
		private readonly UserService _userService;
		private readonly ClientMenu _clientMenu;
		private readonly AdminMenu _adminMenu;

		public UserMenu(UserService userService, ClientMenu clientMenu, AdminMenu adminMenu)
		{
			_userService = userService;
			_clientMenu = clientMenu;
			_adminMenu = adminMenu;
		}

		public void ShowMainMenu()
		{
			while (true)
			{
				Console.WriteLine("\n--- FoodExpress ---");
				Console.WriteLine("1. Register");
				Console.WriteLine("2. Verify email");
				Console.WriteLine("3. Login");
				Console.WriteLine("0. Exit");
				Console.Write("Choose: ");
				string choice = Console.ReadLine() ?? "";

				try
				{
					if (choice == "1") Register();
					else if (choice == "2") VerifyEmail();
					else if (choice == "3") Login();
					else if (choice == "0") return;
					else Console.WriteLine("Invalid choice.");
				}
				catch (Exception error)
				{
					Console.WriteLine("Error: " + error.Message);
				}
			}
		}

		private void Register()
		{
			User user = new User();
			Console.Write("Name: ");
			user.Name = Console.ReadLine() ?? "";
			Console.Write("Email: ");
			user.Email = Console.ReadLine() ?? "";
			Console.Write("Password: ");
			user.Password = Console.ReadLine() ?? "";

			_userService.RegisterUser(user);
			Console.WriteLine("Registration completed.");
		}

		private void VerifyEmail()
		{
			Console.Write("Email: ");
			string email = Console.ReadLine() ?? "";
			Console.Write("Verification code: ");
			string code = Console.ReadLine() ?? "";

			_userService.VerifyUser(email, code);
			Console.WriteLine("Email verified.");
		}

		private void Login()
		{
			Console.Write("Email: ");
			string email = Console.ReadLine() ?? "";
			Console.Write("Password: ");
			string password = Console.ReadLine() ?? "";

			User user = _userService.LoginUser(email, password);
			Console.WriteLine("Login successful. Welcome " + user.Name + "!");

			if (user.IsAdmin)
				_adminMenu.Show(user);
			else
				_clientMenu.Show(user);

			_userService.Logout(user);
		}
	}
}
