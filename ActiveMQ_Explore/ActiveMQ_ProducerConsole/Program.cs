using Apache.NMS;
using Apache.NMS.ActiveMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveMQ_ProducerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var producer = new Publisher();

            while (true)
            {
                Console.WriteLine("Enter a message to send to the client:\n");

                var messageToSend = Console.ReadLine();

                var result = producer.SendMessage(messageToSend);

                Console.WriteLine(result);

                Console.WriteLine();
            }
        }
    }

    public class BaseClass
    {
        public const string URI = "activemq:tcp://34.239.133.119:61616";
        public IConnectionFactory connectionFactory;
        public IConnection _connection;
        public ISession _session;

        public BaseClass()
        {
            connectionFactory = new ConnectionFactory(URI);
            if (_connection == null)
            {
                _connection = connectionFactory.CreateConnection();
                _connection.Start();
                _session = _connection.CreateSession();
            }
        }
    }

    public class Publisher : BaseClass
    {
        public const string DESTINATION = "queue://aws.message.relay";

        public Publisher()
        {
        }

        public string SendMessage(string message)
        {
            string result = string.Empty;
            try
            {
                IDestination destination = _session.GetDestination(DESTINATION);
                using (IMessageProducer producer = _session.CreateProducer(destination))
                {
                    var textMessage = producer.CreateTextMessage(message);
                    producer.Send(textMessage);
                }
                result = "Message sent successfully.";
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Console.WriteLine(ex.ToString());
            }
            return result;
        }
    }
}
