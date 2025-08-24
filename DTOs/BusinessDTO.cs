using System;

namespace Backend.DTOs;

public class CreateBusinessDTO
{
    public required string UserId { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public List<BusinessHours> BusinessHours { get; set; } = [];
    public List<ServiceDTO> Services { get; set; } = [];
}
public class BusinessHours{
    public int Day { get; set; }
    public required string FromHour { get; set; }
    public required string ToHour { get; set; }
}
public class ServiceDTO
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required double Cost { get; set; }
    public required int DurationInMin { get; set; }
    public required bool IsCycileService { get; set; }
}
