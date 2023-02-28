using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ClientTCP
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            const string ip = "127.0.0.1";
            const int port = 8080;

            var tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Console.WriteLine("Введите сообщение");
            var massage = Console.ReadLine();

            var data = Encoding.UTF8.GetBytes(massage);
            tcpSocket.Connect(tcpEndPoint);                                                                  //подключаемся к серверу
            tcpSocket.Send(data);                                                                            //после отправяляем массив данных(байт)

            var buffer = new byte[256];
            var size = 0;
            var answer = new StringBuilder();

            do
            {
                size = tcpSocket.Receive(buffer);                                                           //получаем данные(буффер)
                answer.Append(Encoding.UTF8.GetString(buffer, 0, size));
            } while (tcpSocket.Available > 0);

            Console.WriteLine(answer.ToString());                                                           //выводим полученные данные в консоль
            
            tcpSocket.Shutdown(SocketShutdown.Both);                                                    //закрываем соединение
            tcpSocket.Close();

            Console.ReadLine();
        }
    }
}