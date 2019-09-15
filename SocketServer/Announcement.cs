using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocketServer
{
    class Announcement
    {
        private string text;
        private int lineNumber;

        public string Text
        {
            get;
            set;
        }

        public int LineNumber
        {
            get
            {
                return lineNumber;

            }
            set
            {
                if (value <= 8 && value >= 0)
                    lineNumber = value;
                else
                {
                    Console.WriteLine("Incorrect line number! (Default value 0)");
                    lineNumber = 0;
                }
            }
        }

        //Переводимо назад в об'єкт
        public Announcement ToAnnouncement(byte[] bytelist)
        {
            Announcement announcement = new Announcement();

            announcement.LineNumber = BitConverter.ToInt32(bytelist, 4);
            int length = BitConverter.ToInt32(bytelist, 8);
            announcement.Text = Encoding.ASCII.GetString(bytelist, 12, length);

            return announcement;
        }
    }
}
