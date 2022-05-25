﻿using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
namespace MessagePublisher
{
    public class Program
    {
        private const string storageConnectionString = "";//Conn string pega no AZ
        private const string queueName = "";//Nome da fila
        private const int numOfMessages = 25;
        static ServiceBusClient client;
        static ServiceBusSender sender;
        public static async Task Main(string[] args)
        {
            client = new ServiceBusClient(storageConnectionString);
            sender = client.CreateSender(queueName);
            using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();
            for (int i = 1; i <= numOfMessages; i++)
            {
                if (!messageBatch.TryAddMessage(new ServiceBusMessage($"Teste azure service bus {i}")))
                {
                    throw new Exception($"The message {i} is too large to fit in the batch.");
                }
            }
            try
            {
                await sender.SendMessagesAsync(messageBatch);
                Console.WriteLine($"A batch of {numOfMessages} messages has been published to the queue.");
            }
            finally
            {
                await sender.DisposeAsync();
                await client.DisposeAsync();
            }
        }
    }
}