using Azure.Messaging.ServiceBus;
using System.Threading;

namespace ServiceBusQueueReceiver
{
    public class ReceiverModule : IAsyncDisposable
    {
        private readonly string _connectionString;
        private readonly string _queueName;
        private readonly ServiceBusReceiveMode _receiveMode;
        private ServiceBusClient? _client;
        private ServiceBusReceiver? _receiver;

        public ReceiverModule(string connectionString, string queueName, ServiceBusReceiveMode receiveMode)
        {
            _connectionString = connectionString; 
            _queueName = queueName;
            _receiveMode = receiveMode;
        }

        public async Task InitializeAsync()
        {
            if (_client == null)
            {
                _client = new ServiceBusClient(_connectionString);
                _receiver = _client.CreateReceiver(_queueName, new ServiceBusReceiverOptions { ReceiveMode = _receiveMode });
                Console.WriteLine("Initialized");
            }
        }
        public async Task<ServiceBusReceivedMessage> ReceiveMessageAsync()
        {
            if (_receiver == null)
            {
                await InitializeAsync();
            }

            Console.WriteLine("Waiting for message...");
            return await _receiver!.ReceiveMessageAsync();
        }

        public async Task<ServiceBusReceivedMessage> ReceiveDeferredMessageAsync(int sequenceNumber)
        {
            if (_receiver == null)
            {
                await InitializeAsync();
            }
            Console.WriteLine("Receiving deferred message");
            return await _receiver!.ReceiveDeferredMessageAsync(sequenceNumber);
        }
        public async Task CompleteMessage(ServiceBusReceivedMessage receivedMessage)
        {
            if (_receiver == null)
            {
                Console.Error.WriteLine("Receiver not initialized.");
                return;
            }
            await Task.Delay(100);
            await _receiver.CompleteMessageAsync(receivedMessage);
            Console.WriteLine("\n Message Complete");
        }

        public async Task AbandonMessageAsync(ServiceBusReceivedMessage receivedMessage)
        {
            if (_receiver == null)
            {
                Console.Error.WriteLine("Receiver not initialized.");
                return;
            }
            await Task.Delay(100);
            await _receiver.AbandonMessageAsync(receivedMessage);
            Console.WriteLine("\n Message abandoned");
        }

        public async Task DeferMessageAsync(ServiceBusReceivedMessage receivedMessage)
        {
            if (_receiver == null)
            {
                Console.Error.WriteLine("Receiver not initialized.");
                return;
            }
            await Task.Delay(100);
            await _receiver.DeferMessageAsync(receivedMessage);
            Console.WriteLine("\n Message Deferred");
        }

        public async Task DeadLetterMessageAsync(ServiceBusReceivedMessage receivedMessage)
        {
            if (_receiver == null)
            {
                Console.Error.WriteLine("Receiver not initialized.");
                return;
            }
            await Task.Delay(100);
            await _receiver.DeadLetterMessageAsync(receivedMessage);
            Console.WriteLine("\n Message move to dead-letter");
        }
        
        public async Task<ServiceBusReceivedMessage> ReceiveDeadLetterMessageAsync()
        {
            {
                if (_client == null)
                {
                    await InitializeAsync();
                }
                // IMPORTANT: Create a NEW ServiceBusReceiver specifically for the dead-letter sub-queue.
                await using var deadLetterReceiver = _client!.CreateReceiver(
                    _queueName,
                    new ServiceBusReceiverOptions() { SubQueue = SubQueue.DeadLetter, ReceiveMode = ServiceBusReceiveMode.PeekLock }
                );

                Console.WriteLine("ReceiverModule: waiting for message from DEAD-LETTER queue...");
                // Use a short timeout for dead-letter queue polling if you poll frequently
                return await deadLetterReceiver.ReceiveMessageAsync();
            }
        }
        public async ValueTask DisposeAsync()
        {
            Console.WriteLine("Disposing Service Bus resources in ReceiverModule...");
            if (_receiver != null)
            {
                await _receiver.DisposeAsync().ConfigureAwait(false);
                _receiver = null;
            }
            if (_client != null)
            {
                await _client.DisposeAsync().ConfigureAwait(false);
                _client = null;
            }
            GC.SuppressFinalize(this);
            Console.WriteLine("Service Bus resources disposed in ReceiverModule.");
        }
    }
}
