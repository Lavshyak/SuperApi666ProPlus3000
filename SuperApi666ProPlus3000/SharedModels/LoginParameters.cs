namespace SuperApi666ProPlus3000.SharedModels;

public class LoginParameters
{
	public required string UserName { get; set; }

	public required string Password { get; set; }

	public bool RememberMe { get; set; }
}