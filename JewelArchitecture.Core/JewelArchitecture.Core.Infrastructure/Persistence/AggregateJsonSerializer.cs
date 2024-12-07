﻿using System.Text.Json;
using System.Text;
using JewelArchitecture.Core.Domain.Interfaces;

namespace JewelArchitecture.Core.Infrastructure.Persistence
{
    public class AggregateJsonSerializer<TAggregate, TId>
        where TAggregate : IAggregateRoot<TId>
        where TId : notnull 
    {
        public async Task<string> SerializeAsync(TAggregate aggregate)
        {
            using var memoryStream = new MemoryStream();

            await JsonSerializer.SerializeAsync(memoryStream, aggregate);

            // Convert the memory stream to a JSON string
            memoryStream.Position = 0;
            using var reader = new StreamReader(memoryStream, Encoding.UTF8);

            return await reader.ReadToEndAsync();
        }

        public async Task<TAggregate> DeserializeAsync(string jsonString)
        {
            using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));

            var aggregate = await JsonSerializer.DeserializeAsync<TAggregate>(memoryStream);

            return aggregate ?? throw new InvalidDataException($"Unable to deserialize the following JSON string" +
                    $" to an aggregate of type '{typeof(TAggregate)}': \"{jsonString}\"");
        }
    }
}