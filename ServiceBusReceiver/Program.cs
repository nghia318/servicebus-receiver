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

//// receive & read message
//var message = await serviceBusReceiver.ReceiveMessageAsync();
//string messageInJson = message.Body.ToString();

//// convert message into Employee object using Deserialization
//Employee employeeDetail = JsonSerializer.Deserialize<Employee>(messageInJson);

//Console.WriteLine(employeeDetail.FirstName);

// delete message with sequence number
//int sequenceNumber = "";
//ServiceBusReceivedMessage deferredMessage = await serviceBusReceiver.ReceiveDeferredMessageAsync(sequenceNumber);
//await serviceBusReceiver.CompleteMessageAsync(deferredMessage);

Console.WriteLine("msg received!");

class Employee
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Salary { get; set; }
}