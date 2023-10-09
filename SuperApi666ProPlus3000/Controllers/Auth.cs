using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SuperApi666ProPlus3000.BackendModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace SuperApi666ProPlus3000.Controllers
{
	[ApiController]
	[Authorize]
	[Route("[controller]/[action]")]
	public class Auth : ControllerBase
	{
		private readonly SignInManager<User> _signInManager;
		private readonly UserManager<User> _userManager;

		public Auth(UserManager<User> userManager, SignInManager<User> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		[AllowAnonymous]
		[HttpPost]
		public async Task<IActionResult> Register(RegisterParameters parameters)
		{
			var user = new User
			{
				UserName = parameters.UserName
			};

			var result = await _userManager.CreateAsync(user, parameters.Password);
			if (!result.Succeeded) return BadRequest(result.Errors.FirstOrDefault()?.Description);

			//добавляет пользователю роль "User"
			await _userManager.AddToRoleAsync(user, "User");

			return await Login(new LoginParameters
			{
				UserName = parameters.UserName,
				Password = parameters.Password
			});
		}

		[AllowAnonymous]
		[HttpPost]
		public async Task<IActionResult> Login(LoginParameters parameters)
		{
			var user = await _userManager.FindByNameAsync(parameters.UserName);
			if (user == null) return BadRequest("User does not exist");
			var singInResult = await _signInManager.CheckPasswordSignInAsync(user, parameters.Password, false);
			if (!singInResult.Succeeded) return BadRequest("Invalid password");

			await _signInManager.SignInAsync(user, parameters.RememberMe);

			return Ok();
		}

		[HttpGet]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return Ok();
		}

		[HttpGet]
		public async Task<IEnumerable<string>> MyRole()
		{
			return await this._userManager.GetRolesAsync(await _userManager.GetUserAsync(this.User) ??
			                                             throw new Exception());
		}

		
		[Authorize(Roles = "Admin")]
		[HttpGet]
		public bool CheckAdmin()
		{
			return true;
		}

		[HttpGet]
		public async Task BecomeAdmin()
		{
			BackendModels.User? user = await _userManager.GetUserAsync(this.User);

			if (user == null) throw new Exception("Не юзер");

			await _userManager.AddToRoleAsync(user, "Admin");
			await _signInManager.RefreshSignInAsync(user);
		}

		[HttpGet]
		public long MyId()
		{
			ClaimsPrincipal claimsPrincipal = this.User;
			if (!long.TryParse(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
				throw new Exception($"NameIdentifier is not valid (parse to {userId.GetType().Name} failed)");
			return userId;
		}

		[HttpGet]
		public bool CheckAuthorized()
		{
			return true;
		}

		[HttpGet]
		public async Task<bool> CheckUnauthorized()
		{
			BackendModels.User? user = await _userManager.GetUserAsync(this.User);

			if (user == null) return true; //не авторизован

			return false;
		}
	}
}