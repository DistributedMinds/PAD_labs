namespace Message_Agent.Common;

public class PayLoad
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Topic { get; set; }
    public string Message{get;set;}
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}