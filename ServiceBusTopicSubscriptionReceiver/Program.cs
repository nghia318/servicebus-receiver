using Azure.Messaging.ServiceBus;

public class Program
{
    private const string topicConnectionString = "";
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