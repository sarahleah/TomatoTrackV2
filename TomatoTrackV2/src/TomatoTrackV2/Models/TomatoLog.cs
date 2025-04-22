using System.Text.Json.Serialization;

namespace TomatoTrackV2.Models;

public class TomatoLog
{
    public string LogId { get; set; } = Guid.NewGuid().ToString();
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EventType EventType { get; set; }
    public string Description { get; set; } = string.Empty;
}