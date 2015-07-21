using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQTest
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Rabbit rabbit = new Rabbit())
            {
                rabbit.Connect();
                Console.WriteLine("There are {0} messages already in the queue", rabbit.Count());
                int total = 10000000;
                List<Message> messages = new List<Message>();
                for (int i = 0; i < total; i++)
                {
                    messages.Add(NewMessage(i));
                }
                Stopwatch sending = Stopwatch.StartNew();
                foreach (Message message in messages)
                {
                    rabbit.SendMessage(message);
                    if (message.Number % 10000 == 0)
                        Console.WriteLine("Sent message {0}", message.Number);
                }
                sending.Stop();


                Message fm = null;
                Stopwatch fetching = Stopwatch.StartNew();
                for (int i = 0; i < total; i++)
                {
                     fm = FetchMessge(rabbit);
                }
                fetching.Stop();
                Console.WriteLine("Last message number {0}", fm.Number);
                Console.WriteLine("Sent {0} messages in {1} milliseconds", total, sending.ElapsedMilliseconds);
                Console.WriteLine("Fetched {0} messages in {1} milliseconds", total, fetching.ElapsedMilliseconds);
                
                Console.WriteLine("There are {0} messages left in the queue", rabbit.Count());
                while (rabbit.Count() > 0)
                {
                    FetchMessge(rabbit);
                }
                Console.WriteLine("Queue cleared");
                Console.ReadKey();
            }
        }

        private static Message FetchMessge(Rabbit rabbit)
        {
            Message fetchedMessage = rabbit.FetchMessage();
            if(fetchedMessage.Number % 10000 == 0)
                Console.WriteLine("Got message {0}", fetchedMessage.Number);
            return fetchedMessage;
        }

        static Message NewMessage(int i)
        {
            Message newMessage = new Message();
            newMessage.Name = "Name";
            newMessage.Body = "Body";
            newMessage.Number = i;
            return newMessage;
        }
    }
}
