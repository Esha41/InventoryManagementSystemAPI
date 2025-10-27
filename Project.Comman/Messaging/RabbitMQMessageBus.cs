using System;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace Ettad.Shared.Common.Messaging
{
    public class RabbitMQMessageBus : IMessageBus, IDisposable
    {
        private readonly IConnection _connection;
        private readonly string _exchangeName;
        private readonly ConcurrentDictionary<string, RabbitMQ.Client.IModel> _channels;
        private readonly object _lock = new object();

        public RabbitMQMessageBus(string connectionString, string exchangeName = "project_exchange")
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri(connectionString);
            factory.DispatchConsumersAsync = true; // Enable async consumer dispatch
            _connection = factory.CreateConnection();
            _exchangeName = exchangeName;
            _channels = new ConcurrentDictionary<string, RabbitMQ.Client.IModel>();

            // Create the exchange using a temporary channel
            using (var channel = _connection.CreateModel())
            {
                channel.ExchangeDeclare(_exchangeName, ExchangeType.Topic, durable: true);
            }
        }

        private RabbitMQ.Client.IModel GetOrCreateChannel(string purpose)
        {
            return _channels.GetOrAdd(purpose, _ =>
            {
                var channel = _connection.CreateModel();
                channel.ExchangeDeclare(_exchangeName, ExchangeType.Topic, durable: true);
                return channel;
            });
        }

        public Task PublishAsync<T>(T message, string topic = null) where T : class
        {
            var routingKey = topic ?? typeof(T).Name.ToLower();
            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            var channel = GetOrCreateChannel("publish");
            lock (_lock)
            {
                channel.BasicPublish(
                    exchange: _exchangeName,
                    routingKey: routingKey,
                    basicProperties: null,
                    body: body);
            }

            return Task.CompletedTask;
        }

        public Task SubscribeAsync<T>(Func<T, Task> handler, string topic = null) where T : class
        {
            var routingKey = topic ?? typeof(T).Name.ToLower();
            var queueName = $"{routingKey}_queue";
            var channelKey = $"subscribe_{queueName}";

            var channel = GetOrCreateChannel(channelKey);

            lock (_lock)
            {
                channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false);
                channel.QueueBind(queueName, _exchangeName, routingKey);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var json = Encoding.UTF8.GetString(body);
                    var message = JsonSerializer.Deserialize<T>(json);

                    try
                    {
                        await handler(message);
                        channel.BasicAck(ea.DeliveryTag, multiple: false);
                    }
                    catch
                    {
                        channel.BasicNack(ea.DeliveryTag, multiple: false, requeue: true);
                    }
                };

                channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
            }

            return Task.CompletedTask;
        }

        public Task UnsubscribeAsync<T>(string topic = null) where T : class
        {
            var routingKey = topic ?? typeof(T).Name.ToLower();
            var queueName = $"{routingKey}_queue";
            var channelKey = $"subscribe_{queueName}";

            if (_channels.TryRemove(channelKey, out var channel))
            {
                lock (_lock)
                {
                    try
                    {
                        channel.QueueUnbind(queueName, _exchangeName, routingKey);
                        channel.Close();
                        channel.Dispose();
                    }
                    catch
                    {
                        // Ignore errors during cleanup
                    }
                }
            }

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            foreach (var channel in _channels.Values)
            {
                try
                {
                    channel.Close();
                    channel.Dispose();
                }
                catch
                {
                    // Ignore errors during cleanup
                }
            }

            _channels.Clear();
            _connection?.Dispose();
        }
    }
}