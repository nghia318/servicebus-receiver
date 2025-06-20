using Azure.Messaging.ServiceBus;

public class Program
{
    private const string topicConnectionString = "Endpoint=sb://srvbusstandard.servicebus.windows.net/;SharedAccessKeyName=connection;SharedAccessKey=73jnWHoPwRFGvg+LDEVmo+ey1ur1ZQPRv+ASbFcDV8Y=;EntityPath=firsttopic";
    private const string topicName = "firsttopic";
    private const string subscriptionName = "subs1";
    public static async Task Main(string[] args)
    {
        await using var client = new ServiceBusClient(topicConnectionString);
        await using var receiver = client.CreateReceiver(topicName, subscriptionName);

        var msg = await receiver.ReceiveMessageAsync();
        Console.WriteLine(msg.Body.ToString());
    }
}