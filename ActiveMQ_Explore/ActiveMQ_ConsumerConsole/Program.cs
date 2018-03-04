using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveMQ_ConsumerConsole
{
    class Program
    {
        public const string URI = "activemq:tcp://34.239.133.119:61616";
        public const string DESTINATION = "queue://aws.message.relay";

        static void Main(string[] args)
        {
            try
            {
                IConnectionFactory connectionFactory = new ConnectionFactory(URI);
                IConnection _connection = connectionFactory.CreateConnection();
                _connection.Start();
                ISession _session = _connection.CreateSession();
                IDestination dest = _session.GetDestination(DESTINATION);
                using (IMessageConsumer consumer = _session.CreateConsumer(dest))
                {
                    Console.WriteLine("Listener started.");
                    Console.WriteLine("Listener created.rn");
                    IMessage message;
                    while (true)
                    {
                        message = consumer.Receive();
                        if (message != null)
                        {
                            ITextMessage textMessage = message as ITextMessage;
                            if (!string.IsNullOrEmpty(textMessage.Text))
                            {
                                //Chat pMesg = JsonConvert.DeserializeObject<Chat>(textMessage.Text);

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
