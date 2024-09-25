using BestStoreMVC.Models;
using Microsoft.AspNetCore.Identity;

namespace BestStoreMVC.Services
{
	/// <summary>
	/// Initialize seeding class
	/// </summary>
	public class DatabaseInitializer
	{
		public static async Task SeedDataAsync(UserManager<ApplicationUser>? userManager,
			RoleManager<IdentityRole>? roleManager)
		{
			if (userManager == null || roleManager == null)
			{
				Console.WriteLine("userManager or roleManager is null => exit");
				return;
			}

			// check if we have the admin role or not
			var exists = await roleManager.RoleExistsAsync("admin");
			if (!exists) //if does not exist
			{
				//send message 
				Console.WriteLine("Admin role is not defined and will be created");

				//create new role as admin
				await roleManager.CreateAsync(new IdentityRole("admin"));
			}

			// check if we have the seller role or not
			exists = await roleManager.RoleExistsAsync("seller");
			if (!exists)
			{
                //send message 
                Console.WriteLine("Seller role is not defined and will be created");

                //create new role as seller
                await roleManager.CreateAsync(new IdentityRole("seller"));
			}


			// check if we have the client role or not
			exists = await roleManager.RoleExistsAsync("client");
			if (!exists)
			{

                //send message 
                Console.WriteLine("Client role is not defined and will be created");

                //create new role as client
                await roleManager.CreateAsync(new IdentityRole("client"));
			}


			// check if we have at least one admin user or not
			var adminUsers = await userManager.GetUsersInRoleAsync("admin");

			if (adminUsers.Any())
			{
				// Admin user already exists => exit
				Console.WriteLine("Admin user already exists => exit");
				return;
			}


			// create the admin user by mapping the following dumming data
			var user = new ApplicationUser()
			{
				FirstName = "Admin",
				LastName = "Admin",
				UserName = "admin@admin.com", // UserName will be used to authenticate the user
				Email = "admin@admin.com",
				CreatedAt = DateTime.Now,
			};

			string initialPassword = "admin123";


			var result = await userManager.CreateAsync(user, initialPassword); //creating the admin role over here by passing users and password
			if (result.Succeeded) //if role was created Ok
			{
				// set the user role
				await userManager.AddToRoleAsync(user, "admin");

				Console.WriteLine("Admin user created successfully! Please update the initial password!");
				Console.WriteLine("Email: " + user.Email);
				Console.WriteLine("Initial password: " + initialPassword);
			}
		}
	}
}
