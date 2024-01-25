﻿using Microsoft.AspNetCore.Identity;

namespace Contracts.Dto;

public class UserDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public int Rating { get; set; }
}