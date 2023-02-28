using System;
using System.Data.SqlTypes;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerTCP
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            const string ip = "127.0.0.1";                                                               //чтобы сделать соединение нужны ip-адрес и порт
            const int port = 8080;

            var tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);                                 //точка подклюения. У одного сервера может быть несколько endPoint.
            var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); //сокет - это дырка\дверь, через которую можно заходить. Для tcp аргументы не меняются
            tcpSocket.Bind(tcpEndPoint);                                                                 //указываем, что этому сокету нужно слушать именно этот порт
            tcpSocket.Listen(5);                                                                   //запускаем сокет на прослушивание. 5 - это кол-во клиентов для подключения

            while (true)                                                                                  //процесс просушивани. Должен быть бесконечным
            {
                var listener = tcpSocket.Accept();                                                  //
                var buffer = new byte[256];
                var size = 0;
                var data = new StringBuilder();

                do
                {
                    size = listener.Receive(buffer);
                    data.Append(Encoding.UTF8.GetString(buffer, 0, size));
                } while (listener.Available > 0);

                Console.WriteLine(data);

                listener.Send(Encoding.UTF8.GetBytes("Успех"));
                
                listener.Shutdown(SocketShutdown.Both);
                listener.Close();

                Console.ReadLine();
            }
        }
    }
}