using System;
using System.Threading.Tasks;

namespace Ettad.Shared.Common.Messaging
{
    public interface IMessageBus
    {
        Task PublishAsync<T>(T message, string topic = null) where T : class;
        Task SubscribeAsync<T>(Func<T, Task> handler, string topic = null) where T : class;
        Task UnsubscribeAsync<T>(string topic = null) where T : class;
    }

    public interface IMessageHandler<in T> where T : class
    {
        Task HandleAsync(T message);
    }
} 