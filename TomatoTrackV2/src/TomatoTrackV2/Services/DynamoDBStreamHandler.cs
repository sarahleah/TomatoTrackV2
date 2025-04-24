using System.Text.Json;
using Amazon.Lambda.Core;
using Amazon.Lambda.DynamoDBEvents;
using TomatoTrackV2.Models;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace TomatoTrackV2.Services;

public class DynamoDbStreamHandler
{
    public async Task Handle(DynamoDBEvent dynamoEvent, ILambdaContext context)
    {
        foreach (var record in dynamoEvent.Records)
        {
            context.Logger.LogLine($"Event ID: {record.EventID}");
            context.Logger.LogLine($"Event Name: {record.EventName}");

            if (record.Dynamodb.NewImage != null)
            {
                var newLog = JsonSerializer.Deserialize<TomatoLog>(
                    record.Dynamodb.NewImage.ToJson());
                context.Logger.LogLine($"New Log: {JsonSerializer.Serialize(newLog)}");
            }
        }

        await Task.CompletedTask;
    }
    
    public async Task SecondHandle(DynamoDBEvent dynamoEvent, ILambdaContext context)
    {
        foreach (var record in dynamoEvent.Records)
        {
            context.Logger.LogLine($"Second Handler!");
            context.Logger.LogLine($"record: {JsonSerializer.Serialize(record)}");

            if (record.Dynamodb.NewImage != null)
            {
                var newLog = JsonSerializer.Deserialize<TomatoLog>(
                    record.Dynamodb.NewImage.ToJson());
                context.Logger.LogLine($"New Log: {JsonSerializer.Serialize(newLog)}");
            }
        }

        await Task.CompletedTask;
    }
}