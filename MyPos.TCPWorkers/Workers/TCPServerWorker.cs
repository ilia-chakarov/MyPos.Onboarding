using MyPos.TCPWokrers.XmlModel;
using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MyPos.TCPWokrers.Workers
{
    public class TCPServerWorker : BackgroundService
    {

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int portNr = 9999;
            var listener = new TcpListener(IPAddress.Any, portNr);
            listener.Start();

            Console.WriteLine($"TCP server listening on port {portNr}");

            while (!stoppingToken.IsCancellationRequested)
            {
                if (!listener.Pending())
                {
                    await Task.Delay(1000);
                    continue;
                }
                Console.WriteLine("Client connected");
                var client = await listener.AcceptTcpClientAsync(stoppingToken);
                _ = Task.Run(() => HandleClientAsync(client));

                
            }

            listener.Stop();
        }
        private async Task HandleClientAsync(TcpClient client)
        {
            try
            {
                Console.WriteLine("Client connected. Receiving data...");

                using var networkStream = client.GetStream();
                using var reader = new StreamReader(networkStream, Encoding.UTF8);

                var buffer = new StringBuilder();
                char[] charBuf = new char[1024];

                XmlSerializer serializer = new XmlSerializer(typeof(ChatMessage));

                while (client.Connected)
                {
                    int bytesRead = await reader.ReadAsync(charBuf, 0, charBuf.Length);
                    if (bytesRead == 0) break; // connection closed

                    buffer.Append(charBuf, 0, bytesRead);

                    string content = buffer.ToString();

                    // Naively detect end of XML message
                    if (content.Contains("</ChatMessage>"))
                    {
                        string fullMessage = content.Substring(0, content.IndexOf("</ChatMessage>") + "</ChatMessage>".Length);

                        using var stringReader = new StringReader(fullMessage);
                        var desMes = (ChatMessage) serializer.Deserialize(stringReader);

                        Console.WriteLine($"Received message:\n{fullMessage}");
                        Console.WriteLine($"Deserialized message:\n{desMes?.Message} at {desMes?.Timestamp}");

                        //await SaveToFile(buffer, content, fullMessage);
                        buffer.Clear();
                    }
                }

                Console.WriteLine("Client disconnected.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling client: {ex.Message}");
            }
            finally
            {
                client.Close();
            }
        }

        private async Task SaveToFile(StringBuilder buffer, string content, string fullMessage)
        {
            // Save to file
            var directory = Path.Combine(AppContext.BaseDirectory, "ReceivedFiles");
            Directory.CreateDirectory(directory);

            var filePath = Path.Combine(directory, $"received_{DateTime.UtcNow:yyyyMMdd_HHmmss}.xml");
            await File.WriteAllTextAsync(filePath, fullMessage);

            // Prepare for next message
            buffer.Clear();
            if (content.Length > fullMessage.Length)
                buffer.Append(content.Substring(fullMessage.Length));
        }
    }
}
