using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using TomatoTrackV2.Models;

namespace TomatoTrackV2.Services;

public class TomatoLogService
{
    private readonly IAmazonDynamoDB _dynamoDb;
    private readonly string _tableName = "TomatoLogs";

    public TomatoLogService(IAmazonDynamoDB dynamoDb)
    {
        _dynamoDb = dynamoDb;
    }

    public async Task WriteLogAsync(TomatoLog log)
    {
        var item = new Dictionary<string, AttributeValue>
        {
            // S means string
            ["logId"] = new() { S = log.LogId },
            ["timestamp"] = new() { S = log.Timestamp.ToString("o") }, // ISO 8601 format
            ["eventType"] = new() { S = log.EventType.ToString() },
            ["description"] = new() { S = log.Description },
        };

        var request = new PutItemRequest
        {
            TableName = _tableName,
            Item = item
        };

        await _dynamoDb.PutItemAsync(request);
    }
    
    public async Task<List<TomatoLog>> GetAllLogsAsync()
    {
        var request = new ScanRequest
        {
            TableName = _tableName
        };

        var response = await _dynamoDb.ScanAsync(request);
        var logs = new List<TomatoLog>();

        foreach (var item in response.Items)
        {
            var log = new TomatoLog
            {
                LogId = item["logId"].S,
                Timestamp = DateTime.Parse(item["timestamp"].S),
                EventType = Enum.Parse<EventType>(item["eventType"].S),
                Description = item["description"].S
            };
            logs.Add(log);
        }

        return logs;
    }
    
    public async Task<List<TomatoLog>> GetLogAsync(string logId)
    {
        var request = new QueryRequest
        {
            TableName = _tableName,
            KeyConditionExpression = "logId = :logId",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":logId", new AttributeValue { S = logId } }
            }
        };

        var response = await _dynamoDb.QueryAsync(request);

        var logs = response.Items.Select(item => new TomatoLog
        {
            LogId = item["logId"].S,
            Timestamp = DateTime.Parse(item["timestamp"].S, null, System.Globalization.DateTimeStyles.RoundtripKind),
            EventType = Enum.Parse<EventType>(item["eventType"].S)
        }).ToList();

        return logs;
    }
    
    public async Task<TomatoLog> UpdateLogAsync(string logId, TomatoLog log)
    {
        var item = new Dictionary<string, AttributeValue>();
        
        item["logId"] = new AttributeValue { S = logId };
        item["eventType"] = new AttributeValue { S = log.EventType.ToString() };
        item["description"] = new AttributeValue { S = log.Description };

        var request = new PutItemRequest
        {
            TableName = _tableName,
            Item = item,
        };

        await _dynamoDb.PutItemAsync(request);
        return log;
    }
}