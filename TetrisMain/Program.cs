using System;
using TetrisMain.Timers;
using TetrisMain.Parser;
using System.Media;

namespace TetrisMain
{
    class Program
    {
        static void Main(string[] args)
        {
            bool showMenu = true;
            Timer gameClock = new Timer();
            while (true)
            {

            }
            SoundPlayer player = new SoundPlayer
            {
                SoundLocation = "Simple Melody.wav"
            };
            player.Play();
            while (showMenu)
            {
                //DrawBorder();
                //showMenu = MainMenu();
            }
        }
        private static bool MainMenu()
        {
            //Console.WriteLine(String.Format("\n|{0,10}|{1,10}|", "Choose an option", "1) Start game"));
            var menu = new[] { 
                (1, "Start game"),
                (2, "Level"),
                (3, "Store"),
                (4, "Exit")
            };

            Console.WriteLine(" Choose an option:\n");
            Console.WriteLine(menu.ToStringTable(new[] { "Key", "Option Name" }, x => x.Item1, x => x.Item2));
            //Console.WriteLine();
            //Console.WriteLine("2) Level");
            //Console.WriteLine("3) gbhvp;lkk8Store");
            //Console.WriteLine("4) Exit");
            Console.Write("\r\nSelect an option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    return true;
                case "2":
                    return true;
                case "3":
                    return false;
                case "4":
                    return false;
                default:
                    return true;
            }
        }
        public static void DrawBorder()
        {
            for (int lengthCount = 0; lengthCount <= 22; ++lengthCount)
            {
                Console.SetCursorPosition(0, lengthCount);
                Console.Write("|");
                Console.SetCursorPosition(21, lengthCount);
                Console.Write("|");
            }
            Console.SetCursorPosition(0, 23);
            for (int widthCount = 0; widthCount <= 10; widthCount++)
            {
                Console.Write("--");
            }

        }
    }
}