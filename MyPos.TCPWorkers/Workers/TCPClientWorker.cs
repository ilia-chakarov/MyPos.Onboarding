using MyPos.TCPWokrers.XmlModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MyPos.TCPWokrers.Workers
{
    internal class TCPClientWorker : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int portNr = 6969;
            string address = "10.80.55.157";
            //int portNr = 9999;
            //string address = "127.0.0.1";

            var ipAddress = IPAddress.Parse(address);
            var endpoint = new IPEndPoint(ipAddress, portNr);

            using TcpClient client = new TcpClient();

            await client.ConnectAsync(endpoint);
            await using NetworkStream stream = client.GetStream();

            var buffer = new byte[1024];
            while (true)
            {
                string message = Console.ReadLine()?.ToString();

                if (message != null)
                    buffer = Encoding.UTF8.GetBytes(message);
                else
                    Console.WriteLine("Message is null");

                XmlSerializer serializer = new XmlSerializer(typeof(ChatMessage));
                ChatMessage msg = new ChatMessage { Message = message! };

                serializer.Serialize(stream, msg);

                //stream.BeginWrite(buffer, 0, buffer.Length, null, null);
                stream.Flush();
            }
            
            client.Close();
        }
    }
}
