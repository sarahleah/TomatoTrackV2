using System.Text.Json.Serialization;

namespace TomatoTrackV2.Models;

public class TomatoLog
{
    [JsonPropertyName("logId")]
    public string LogId { get; set; } = Guid.NewGuid().ToString();
    
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    
    [JsonPropertyName("eventType")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EventType EventType { get; set; }
    
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
}