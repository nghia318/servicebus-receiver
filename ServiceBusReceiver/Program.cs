using Azure.Messaging.ServiceBus;
using ServiceBusReceiverModule;
class Program
{
    private const string ConnectionString = "Endpoint=sb://mentossrvbus.servicebus.windows.net/;SharedAccessKeyName=queuepolicy;SharedAccessKey=YGl/cJ+F7sejJKEHfCZ0SHEZOBGd4Xxvs+ASbNPBePg=;EntityPath=firstqueue";
    private const string QueueName = "firstqueue";
    public static async Task Main(string[] args)
    {
        // mode: receive and delete
        Console.WriteLine("Using Receive and Delete mode."); await using var receiverModule = new ReceiverModule(ConnectionString, QueueName, ServiceBusReceiveMode.ReceiveAndDelete);

        // mode: peeklock
        //Console.WriteLine("Using PeekLock mode."); await using var receiverModule = new ReceiverModule(ConnectionString, QueueName, ServiceBusReceiveMode.PeekLock);

        int? sequenceNumber = null;

        //receive deadletter msg
        //var deadLetterMsg = await receiverModule.ReceiveDeadLetterMessageAsync();
        //if (deadLetterMsg != null)
        //{
        //    string messageBody = deadLetterMsg.Body.ToString();
        //    Console.WriteLine($"\nBody: '{messageBody}'");
        //}
        try
        {
            await receiverModule.InitializeAsync();
            if (sequenceNumber == null)
            {
                ServiceBusReceivedMessage? receivedMessage = await receiverModule.ReceiveMessageAsync();
                if (receivedMessage != null)
                {
                    string messageBody = receivedMessage.Body.ToString();
                    string seqNum = receivedMessage.SequenceNumber.ToString();
                    Console.WriteLine("\n============MESSAGE=============");
                    Console.WriteLine($"\nSequenceNumber: {seqNum} - Body: '{messageBody}'\n");
                    //await receiverModule.CompleteMessage(receivedMessage);
                    //await receiverModule.AbandonMessageAsync(receivedMessage);
                    //await receiverModule.DeferMessageAsync(receivedMessage);
                    //await receiverModule.DeadLetterMessageAsync(receivedMessage);
                }
                else
                {
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] No regular messages (waiting...)");
                }

            }

            if (sequenceNumber != null)
            {
                ServiceBusReceivedMessage? receivedMessage = await receiverModule.ReceiveDeferredMessageAsync((int)sequenceNumber);
                if (receivedMessage != null)
                {
                    string messageBody = receivedMessage.Body.ToString();
                    Console.WriteLine($"\nDeferred message Body: '{messageBody}'");
                }
                //await receiverModule.CompleteMessage(receivedMessage);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}");
        }
    }
}

//convert message into Employee object using Deserialization
//string messageInJson = message.Body.ToString();
//Employee employeeDetail = JsonSerializer.Deserialize<Employee>(messageInJson);
//Console.WriteLine(employeeDetail.FirstName);





