using RabbitMQ.Client;using RabbitMQ.Client.Events;using System.Text;
var factory=new ConnectionFactory(){HostName="rabbitmq"};
using var connection=factory.CreateConnection();
using var channel=connection.CreateModel();
channel.QueueDeclare("tax-inspection",false,false,false);
var consumer=new EventingBasicConsumer(channel);
consumer.Received += (_,e)=> Console.WriteLine($"Налоговая получила: {Encoding.UTF8.GetString(e.Body.ToArray())}");
channel.BasicConsume("tax-inspection",true,consumer);
Console.ReadLine();
