﻿using System.ComponentModel.DataAnnotations;

namespace SuperApi666ProPlus3000;

public class RegisterParameters
{
	public required string UserName { get; set; }

	public required string Password { get; set; }
}