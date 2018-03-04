using Apache.NMS;
using Apache.NMS.ActiveMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ActiveMQ_Managed_Client
{
    class Program
    {
        public const string URI = "activemq:ssl://b-4cc35174-5bb4-4573-886b-fa7ec7e3948e-1.mq.us-east-1.amazonaws.com:61617";
        public const string DESTINATION = "queue://aws.message.relay";

        static void Main(string[] args)
        {
            var consumerThread = new Thread(StartConsumser);
            consumerThread.Start();

            while (true)
            {
                Console.WriteLine("Enter a message to send to the client:\n");

                var messageToSend = Console.ReadLine();

                var result = SendMessage(messageToSend, GetSession());

                Console.WriteLine(result);

                Console.WriteLine();
            }
        }

        public static ISession GetSession()
        {
            
            IConnectionFactory connectionFactory = new ConnectionFactory(URI);
            IConnection connection = connectionFactory.CreateConnection("", "");
            connection.Start();
            ISession session = connection.CreateSession();

            return session;
        }

        public static string SendMessage(string message, ISession session)
        {
            string result = string.Empty;
            try
            {
                IDestination destination = session.GetDestination(DESTINATION);
                using (IMessageProducer producer = session.CreateProducer(destination))
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

        public static void StartConsumser()
        {
            try
            {
                ISession _session = GetSession();
                IDestination dest = _session.GetDestination(DESTINATION);
                using (IMessageConsumer consumer = _session.CreateConsumer(dest))
                {
                    Console.WriteLine("Listener started.");
  
                    IMessage message;
                    while (true)
                    {
                        message = consumer.Receive();
                        if (message != null)
                        {
                            ITextMessage textMessage = message as ITextMessage;
                            if (!string.IsNullOrEmpty(textMessage.Text))
                            {
                                Console.WriteLine($"Message Received: {textMessage.Text}");
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine("Press <ENTER> to exit.");
                Console.Read();
            }
        }
    }
}
