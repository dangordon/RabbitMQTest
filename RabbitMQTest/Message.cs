using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQTest
{
    class Message
    {
        public string Name { get; set; }
        public string Body { get; set; }
        public int Number { get; set; }

        public override string ToString()
        {
            return String.Format("{0}|{1}|{2}", Name, Body, Number);
        }

        public static Message FromString(string messageContent)
        {
            Message message = new Message();
            string[] fields = messageContent.Split('|');
            message.Name = fields[0];
            message.Body = fields[1];
            message.Number = Convert.ToInt32(fields[2]);
            return message;
        }
    }
}
