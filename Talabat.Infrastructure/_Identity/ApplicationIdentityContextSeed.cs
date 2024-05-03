using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entitites.Identity;

namespace Talabat.Infrastructure._Identity
{
	public class ApplicationIdentityContextSeed
	{
		public static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
		{
			if (!userManager.Users.Any())
			{
				var user = new ApplicationUser()
				{
					DisplayName = "Manar Mahmoud",
					Email = "manargh193@gmail.com",
					UserName = "manar.mahmoud",
					PhoneNumber = "01122334455"
				};
				await userManager.CreateAsync(user, "P@ssw0rd");
			}
		}
	}

}
