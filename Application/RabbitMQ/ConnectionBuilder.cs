using RabbitMQ.Client;

namespace Application.RabbitMQ;

public static class ConnectionBuilder
{
    public static IModel GetConnection()
    {
        var factory = new ConnectionFactory
        {
            HostName = "rabbitmq",
            Port = 5672,
            UserName = "guest",
            Password = "guest"
        };

        var connection = factory.CreateConnection();
        return connection.CreateModel();
    }

}
