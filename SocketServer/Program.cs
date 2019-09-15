using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO;

namespace SocketServer
{
    class Program
    {
        static Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        static List<string> boardList = new List<string>();

        static void Main(string[] args)
        {
            //З'єднуємося з клієнтом
            socket.Bind(new IPEndPoint(IPAddress.Any, 1030));
            socket.Listen(1);
            Socket client = socket.Accept();

            //Заповнюємо List "пустими" рядками
            for (int i = 0; i < 9; i++)
            {
                boardList.Add("null");
            }

            while (true)
            {
                using (StreamWriter streamWriter = new StreamWriter("E:\\result2.txt"))
                { //Отримуємо масив байтів від клієнта
                    byte[] buffer = new byte[1024];

                    client.Receive(buffer);
                    streamWriter.WriteLine("Receiving");
                    streamWriter.WriteLine(DateTime.Now.ToLongTimeString());
                    streamWriter.Close();

                    //Передаємо цей масив на обробку
                    FromByte(buffer);

                    Console.ReadKey();
                }
            }
        }

        //Метод Board, який додає об'яву на необхідний рядок і викликає метод виведення
        static void Board(byte[] byteList)
        {
            Announcement announcement = new Announcement();
            //Переводимо назад до об'єкту об'яви
            announcement = announcement.ToAnnouncement(byteList);

            //Якщо рядок порожній, просто додаємо на це місце об'яву (при цьому видаляємо "порожній" рядок)
            if ((boardList.ElementAt(announcement.LineNumber)).Equals("null"))
            {
                boardList.RemoveAt(announcement.LineNumber);
                boardList.Insert(announcement.LineNumber, announcement.Text);
            }

            //Якщо в заданому рядку вже є об'ява, то спочатку видаляємо її, а потім записуємо нову
            else
            {
                boardList.RemoveAt(announcement.LineNumber);
                boardList.Insert(announcement.LineNumber, announcement.Text);
            }

            //Викликаємо метод виведення об'яв
            BoardOut(boardList);
        }
        
        //Метод виведення об'яв
        static void BoardOut(List<string> boardList)
        {
            //Очистка консолі перед виводом нових об'яв
            Console.Clear();

            string lineValue;
            for (int i = 0; i < 9; i++)
            {
                lineValue = boardList.ElementAt(i);
                Console.Write(i + ":   ");
                //Вивід пустого рядка, якщо об'яви на цьому місці немає
                if (lineValue.Equals("null"))
                {
                    Console.WriteLine();
                }
                //Вивід об'яви
                else
                {
                    Console.WriteLine(lineValue);
                }
            }
        }

        //Переводимо перше int значення(опцію) і дізнаємося, яку команду виконувати
        static void FromByte(byte[] byteList)
        {
            int optionNumber = BitConverter.ToInt32(byteList, 0);

            //Перша команда Who
            if (optionNumber.Equals(1))
            {
                DoWho();
            }

            //Друга команда Annoucement
            else
            {
                //Викликаємо метод, який відповідає за вивід об'яви
                Board(byteList);
            }
        }

        static void DoWho()
        {
            Console.WriteLine("This program is written by Gavryliuk Oksana from group K26");
            Console.WriteLine("Variant number is 5: ");
            Console.WriteLine("bulletin board (Електронна дошка об'яв)");
        }
    }

}
