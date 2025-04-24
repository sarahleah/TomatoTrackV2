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
        var statement = $@"
            INSERT INTO ""{_tableName}"" VALUE {{
                'LogId': ?,
                'timestamp': ?,
                'eventType': ?,
                'description': ?
            }}";

        await _dynamoDb.ExecuteStatementAsync(new ExecuteStatementRequest
        {
            Statement = statement,
            Parameters = new List<AttributeValue>
            {
                new AttributeValue { S = log.LogId },
                new AttributeValue { S = log.Timestamp.ToString("o") },
                new AttributeValue { S = log.EventType.ToString() },
                new AttributeValue { S = log.Description }
            }
        });
    }

    public async Task<List<TomatoLog>> GetAllLogsAsync()
    {
        var logs = new List<TomatoLog>();
        var statement = $@"SELECT * FROM ""{_tableName}""";

        var result = await _dynamoDb.ExecuteStatementAsync(new ExecuteStatementRequest
        {
            Statement = statement
        });

        foreach (var item in result.Items)
        {
            logs.Add(new TomatoLog
            {
                LogId = item["LogId"].S,
                Timestamp = DateTime.Parse(item["timestamp"].S, null, System.Globalization.DateTimeStyles.RoundtripKind),
                EventType = Enum.Parse<EventType>(item["eventType"].S),
                Description = item["description"].S
            });
        }

        return logs;
    }

    public async Task<List<TomatoLog>> GetLogAsync(string logId)
    {
        var logs = new List<TomatoLog>();
        var statement = $@"SELECT * FROM ""{_tableName}"" WHERE LogId = ?";

        var result = await _dynamoDb.ExecuteStatementAsync(new ExecuteStatementRequest
        {
            Statement = statement,
            Parameters = new List<AttributeValue> { new AttributeValue { S = logId } }
        });

        foreach (var item in result.Items)
        {
            logs.Add(new TomatoLog
            {
                LogId = item["LogId"].S,
                Timestamp = DateTime.Parse(item["timestamp"].S, null, System.Globalization.DateTimeStyles.RoundtripKind),
                EventType = Enum.Parse<EventType>(item["eventType"].S),
                Description = item["description"].S
            });
        }

        return logs;
    }

    public async Task<TomatoLog> UpdateLogAsync(string logId, TomatoLog log)
    {
        var statement = $@"
            UPDATE ""{_tableName}""
            SET eventType = ?, description = ?
            WHERE LogId = ?";

        await _dynamoDb.ExecuteStatementAsync(new ExecuteStatementRequest
        {
            Statement = statement,
            Parameters = new List<AttributeValue>
            {
                new AttributeValue { S = log.EventType.ToString() },
                new AttributeValue { S = log.Description },
                new AttributeValue { S = logId }
            }
        });

        return log;
    }
}
