using System;
using RabbitMQ.Client;
using System.Text;


public interface IQueueService : IDisposable
{
    void Enqueue(string message);
    string Dequeue();
}

public class RabbitMQService : IQueueService
{
    private readonly string _queueName = "testQueue";
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMQService()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        try
        {
            
            _channel.QueueDeclarePassive(_queueName);
        }
        catch (RabbitMQ.Client.Exceptions.OperationInterruptedException)
        {
            
            _channel.QueueDeclare(
                queue: _queueName,
                durable: true,       
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
        }
    }

    
    public void Enqueue(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);

        
        var properties = _channel.CreateBasicProperties();
        properties.Persistent = true;

        _channel.BasicPublish(
            exchange: "",          
            routingKey: _queueName, 
            basicProperties: properties,
            body: body
        );

        Console.WriteLine($"[x] Sent: {message}");
    }

    
    public string Dequeue()
    {
        BasicGetResult result = _channel.BasicGet(_queueName, autoAck: true); 
        if (result != null)
        {
            string message = Encoding.UTF8.GetString(result.Body.ToArray());
            Console.WriteLine($"[x] Received: {message}");
            return message;
        }
        return null;
    }

    
    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
    }
}


public class QueueManager
{
    private readonly IQueueService _queueService;

    public QueueManager(IQueueService queueService)
    {
        _queueService = queueService;
    }

    
    public void SendMessage(string message)
    {
        _queueService.Enqueue(message);
    }

    
    public void ReceiveMessage()
    {
        string message = _queueService.Dequeue();
        if (message == null)
        {
            Console.WriteLine("No messages in queue.");
        }
    }
}


class Program
{
    static void Main()
    {
        using (IQueueService queueService = new RabbitMQService())
        {
            QueueManager queueManager = new QueueManager(queueService);

            
            queueManager.SendMessage("Hello, RabbitMQ!");

            
            queueManager.ReceiveMessage();
        }
    }
}