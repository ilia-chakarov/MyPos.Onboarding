using MyPos.TCPWokrers.XmlModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml.Serialization;

namespace MyPos.TCPWokrers.Workers
{
    internal class TCPClientWorker : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int portNr = 6969;
            string address = "10.80.55.157";
            var ipAddress = IPAddress.Parse(address);
            var endpoint = new IPEndPoint(ipAddress, portNr);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using TcpClient client = new TcpClient();
                    Console.WriteLine("Connecting to server...");

                    await client.ConnectAsync(endpoint, stoppingToken);
                    Console.WriteLine("Connected to server.");

                    await using NetworkStream stream = client.GetStream();
                    var serializer = new XmlSerializer(typeof(ChatMessage));

                    while (!stoppingToken.IsCancellationRequested)
                    {
                        string? message = Console.ReadLine();

                        if (!string.IsNullOrWhiteSpace(message))
                        {
                            var msg = new ChatMessage
                            {
                                Message = message,
                                Timestamp = DateTime.Now
                            };

                            serializer.Serialize(stream, msg);
                            await stream.FlushAsync(stoppingToken);
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Cancellation requested. Exiting...");
                    break;
                }
                catch (SocketException se)
                {
                    Console.WriteLine($"Socket error: {se.Message}. Reconnecting in 5 seconds...");
                    await Task.Delay(5000, stoppingToken);
                }
                catch (IOException ioe)
                {
                    Console.WriteLine($"Connection lost: {ioe.Message}. Reconnecting in 5 seconds...");
                    await Task.Delay(5000, stoppingToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected error: {ex.Message}. Reconnecting in 5 seconds...");
                    await Task.Delay(5000, stoppingToken);
                }
            }
        }
    }
}
