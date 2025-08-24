using System;

namespace Backend.Data;

public class Service
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public double Cost { get; set; }
    public int DurationInMin { get; set; }
    public bool IsCycileService { get; set; }
    public int BusinessId { get; set; }
    public virtual Business Business { get; set; } = null!;
}
