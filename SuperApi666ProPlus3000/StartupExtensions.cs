using Microsoft.AspNetCore.Identity;
using SuperApi666ProPlus3000.BackendModels;
using SuperApi666ProPlus3000.DbContexts;

namespace SuperApi666ProPlus3000;

public static class StartupExtensions
{
	private static readonly string[] RequiredRoleNames = {"Admin", "User"};

	public static async Task SynchronizeIdentityRoles(this IServiceProvider serviceProvider)
	{
		using var scope = serviceProvider.CreateScope();
		var context = scope.ServiceProvider.GetService<MainDbContext>() ?? throw new Exception();
		await context.Database.EnsureCreatedAsync();

		var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole<long>>>() ?? throw new Exception();


		var existRoles = roleManager.Roles.ToArray();
		var missingRoleNames = RequiredRoleNames.Where(rs => existRoles.All(r => r.Name != rs));
		foreach (var roleName in missingRoleNames) await roleManager.CreateAsync(new IdentityRole<long>(roleName));
	}

	public static void AddMyAuth(this IServiceCollection services)
	{
		services.AddIdentity<User, IdentityRole<long>>()
			.AddRoles<IdentityRole<long>>()
			.AddEntityFrameworkStores<MainDbContext>();

		services.AddAuthorization();

		services.Configure<IdentityOptions>(options =>
		{
			// Password settings
			options.Password.RequireDigit = false;
			options.Password.RequiredLength = 4;
			options.Password.RequireNonAlphanumeric = false;
			options.Password.RequireUppercase = false;
			options.Password.RequireLowercase = false;

			// Lockout settings
			options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
			options.Lockout.MaxFailedAccessAttempts = 10;
			options.Lockout.AllowedForNewUsers = true;

			// User settings
			options.User.RequireUniqueEmail = false;
		});

		services.ConfigureApplicationCookie(options =>
		{
			options.Cookie.HttpOnly = true;

			options.Events.OnRedirectToLogin = context =>
			{
				context.Response.StatusCode = 401;
				return Task.CompletedTask;
			};

			options.Events.OnRedirectToAccessDenied = context =>
			{
				context.Response.StatusCode = 403;
				return Task.CompletedTask;
			};
		});

		
	}
}