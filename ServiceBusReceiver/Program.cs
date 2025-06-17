// See https://aka.ms/new-console-template for more information
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Amqp;
using Microsoft.Azure.Amqp.Framing;
using System.Reflection;
using System.Text.Json;

string connectionString = "Endpoint=sb://mentossrvbus.servicebus.windows.net/;SharedAccessKeyName=queuepolicy;SharedAccessKey=YGl/cJ+F7sejJKEHfCZ0SHEZOBGd4Xxvs+ASbNPBePg=;EntityPath=firstqueue";
string queueName = "firstqueue";
// service bus client
ServiceBusClient serviceBusClient = new ServiceBusClient(connectionString);

// service bus receiver
ServiceBusReceiver serviceBusReceiver = serviceBusClient.CreateReceiver(queueName);

// receive message
//ServiceBusReceivedMessage message = await serviceBusReceiver.ReceiveMessageAsync();

// receive deferred message by sequenceNumber
//int sequenceNumber = 11;
//ServiceBusReceivedMessage message = await serviceBusReceiver.ReceiveDeferredMessageAsync(sequenceNumber);

//convert message into Employee object using Deserialization
//string messageInJson = message.Body.ToString();
//Employee employeeDetail = JsonSerializer.Deserialize<Employee>(messageInJson);
//Console.WriteLine(employeeDetail.FirstName);

// complete message 
//await serviceBusReceiver.CompleteMessageAsync(message);

//abandon message 
//await serviceBusReceiver.AbandonMessageAsync(message);

//deferred message
//await serviceBusReceiver.DeferMessageAsync(message);

//move to dead-letter
//await serviceBusReceiver.DeadLetterMessageAsync(message);
//read dead-letter message
//var receiverForDeadLetterQueue = serviceBusClient.CreateReceiver(queueName, new ServiceBusReceiverOptions() { SubQueue = SubQueue.DeadLetter });
//var msgObtainedFromDeadLetterQueue = await receiverForDeadLetterQueue.ReceiveMessageAsync();
//Console.WriteLine(msgObtainedFromDeadLetterQueue);

Console.WriteLine("msg received!");

class Employee
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Salary { get; set; }
}