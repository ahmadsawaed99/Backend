using System;

namespace Backend.DTOs;

public class ResetPasswordDTO
{
    public required string Token { get; set; }
    public required string Email { get; set; }
    public required string NewPassword { get; set; }
}
