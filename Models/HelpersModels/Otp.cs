using System;

namespace Backend.Models.HelpersModels;

public class Otp
{
    public required string Email { get; set; }
    public required DateTime SendDate { get; set; }
    public required int Code { get; set; }
}
