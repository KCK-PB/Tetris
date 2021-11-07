using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;
using TetrisMain.Timers;
using System.Media;
using TetrisMain.Models;
using TetrisMain.UI;
using System.Runtime.InteropServices;



namespace TetrisMain {

    class Program {
        public static GamePrinter GamePrinter;
        public static readonly Scoreboard scoreboard = new Scoreboard("scoreboard.txt");
        static void ConsoleDraw(IEnumerable<string> lines, int x, int y) {
            if (x > Console.WindowWidth) return;
            if (y > Console.WindowHeight) return;

            var trimLeft = x < 0 ? -x : 0;
            int index = y;

            x = x < 0 ? 0 : x;
            y = y < 0 ? 0 : y;

            var linesToPrint =
                from line in lines
                let currentIndex = index++
                where currentIndex > 0 && currentIndex < Console.WindowHeight
                select new {
                    Text = new String(line.Skip(trimLeft).Take(Math.Min(Console.WindowWidth - x, line.Length - trimLeft)).ToArray()),
                    X = x,
                    Y = y++
                };

            Console.Clear();
            foreach (var line in linesToPrint) {
                Console.SetCursorPosition(line.X, line.Y);
                Console.WriteLine(line.Text);
            }
        }

        static void Exit() {
            Console.CursorVisible = false;

            var arr = new[]
            {
@"   _____                        ",
@"|       \                      ",
@"| $$$$$$$\ __    __   ______   ",
@"| $$__/ $$|  \  |  \ /      \  ",
@"| $$    $$| $$  | $$|  $$$$$$\ ",
@"| $$$$$$$\| $$  | $$| $$    $$ ",
@"| $$__/ $$| $$__/ $$| $$$$$$$$ ",
@"| $$    $$ \$$    $$ \$$     \ ",
@" \$$$$$$$  _\$$$$$$$  \$$$$$$$ ",
@"           |  \__| $$          ",
@"           \$$    $$           ",
@"            \$$$$$$            ",
       };

            var maxLength = arr.Aggregate(0, (max, line) => Math.Max(max, line.Length));
            var x = Console.BufferWidth / 2 - maxLength / 2;
            for (int y = -arr.Length; y < Console.WindowHeight + arr.Length; y++) {
                ConsoleDraw(arr, x, y);
                Thread.Sleep(100);
            }
            Environment.Exit(0);
        }
        public static List<Option> options;
        static void Main(string[] args) {
            
            ConsoleHelper.SetCurrentFont("Terminal", 32);
            Console.CursorVisible = false;
            Console.SetWindowSize(1, 1);
            Console.SetBufferSize(30, 30);
            Console.SetWindowSize(50, 30);
            ConsoleHelper.LockSize();
            Console.Title = "TETRIS";
            JukeBox jukeBox = new JukeBox();

            //scoreboard.AddToHighScoreFile(DateTime.Now, "JA", 250);
            //scoreboard.AddToHighScoreFile(DateTime.Now, "JA", 250);

            bool ShowMenu = true;
            SoundPlayer player = new SoundPlayer { //TO-DO fix sound player
                SoundLocation = "Simple Melody.wav"
            };
            options = new List<Option>{
                new Option("Start Game", () => PrepareGame()),
                new Option("Game mode", () =>  WriteTemporaryMessage("Game mode")),
                //new Option("Setting", () =>  ),
                new Option("Scoreboard", () =>  WriteScoreboard(scoreboard.GetHighScoreList())),
                new Option("Exit", () => Exit()),
            };
            int index = 0;
            //WriteLogo();
            WriteBorder();
            WriteMenu(options, options[index], true);
            ConsoleKeyInfo keyinfo;
            jukeBox.PlayMusic(0);
            do {
                
                keyinfo = Console.ReadKey(true);
                if (keyinfo.Key == ConsoleKey.DownArrow) {
                    if (index + 1 < options.Count) {
                        jukeBox.PlaySound(7,DateTime.Now.Ticks);
                        index++;
                        WriteMenu(options, options[index]);
                    }
                }
                if (keyinfo.Key == ConsoleKey.UpArrow) {
                    if (index - 1 >= 0) {
                        jukeBox.PlaySound(6, DateTime.Now.Ticks);
                        index--;
                        WriteMenu(options, options[index]);
                    }
                }
                if (keyinfo.Key == ConsoleKey.Enter) {
                    options[index].Selected.Invoke();
                    index = 0;
                }
            }
            while (ShowMenu == true);
            Console.ReadKey();
        }

        static void WriteTemporaryMessage(string message) { //TO-DO make actual options for menu
            //Console.Clear();
            CleanScreen();
            Console.SetCursorPosition(5, 10);
            Console.SetCursorPosition((Console.WindowWidth - message.Length)/ 2, Console.CursorTop);
            Console.WriteLine(message);
            Thread.Sleep(3000);
            CleanScreen();
            WriteMenu(options, options.First(), true);
        }

        static void WriteScoreboard(List<string> scoreLines)
        {
            if (scoreLines.Count == 0)
                return;
            const string title = "Scoreboard:";
            const int maxCountOfLines = 12;
            int startIndex = 0;
            int index = startIndex;
            ConsoleKeyInfo keyInfo = new ConsoleKeyInfo();
            CleanScreen();
            Console.SetCursorPosition(5, 7);
            Console.SetCursorPosition(((Console.WindowWidth - title.Length) / 2) - 2, Console.CursorTop);
            Console.WriteLine("Scoreboard:");
            int i = 0;
            for (; i <= maxCountOfLines; index++)
            {
                if (index >= scoreLines.Count)
                    break;
                string currentLine = scoreLines[index];
                Console.SetCursorPosition(5 + i, 9 + i);
                Console.SetCursorPosition(((Console.WindowWidth - currentLine.Length) / 2) - 2, Console.CursorTop);
                Console.WriteLine(currentLine);
                i++;
            }

            do
            {
                if (scoreLines.Count < maxCountOfLines)
                {
                    Console.SetCursorPosition(5 + i, 7 + i);
                    Console.ReadKey();
                    break;
                }
                
                keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.DownArrow && startIndex < scoreLines.Count - maxCountOfLines - 1)
                {
                    string currentLine = scoreLines[index];
                    Console.MoveBufferArea(6, 10, 34, 12, 6, 9);
                    Console.SetCursorPosition(5 + i - 1, 9 + i - 1);
                    Console.SetCursorPosition(((Console.WindowWidth - currentLine.Length) / 2) - 2, Console.CursorTop);
                    Console.WriteLine(currentLine);
                    index++;
                    startIndex++;
                }
                else if (keyInfo.Key == ConsoleKey.UpArrow && startIndex > 0)
                {
                    string currentLine = scoreLines[startIndex - 1];
                    Console.MoveBufferArea(6, 9, 34, 12, 6, 10);
                    Console.SetCursorPosition(5, 9);
                    Console.SetCursorPosition(((Console.WindowWidth - currentLine.Length) / 2) - 2, Console.CursorTop);
                    Console.WriteLine(currentLine);
                    startIndex--;
                    index--;
                }
            }
            while (keyInfo.Key != ConsoleKey.Escape);
            CleanScreen();
            WriteMenu(options, options.First(), true);
        }

        static void PrepareGame() {
            bool showMenu = false;
            ConsoleKey keyPress;
            Console.Clear();
            TetrisPlayboard playboard = TetrisPlayboard.GetInstance();
            playboard=playboard.Reset();
            GamePrinter = new GamePrinter(playboard.drawboard);
            playboard.RenderNextPiece();
            playboard.RenderBlockCount();
            playboard.RenderScore("best");
            GamePrinter.PrintInExactPlace(playboard.drawboard);
            playboard.StartGame();

            while (showMenu == false) {
                keyPress = Console.ReadKey(true).Key;
                if (playboard.IsGameInProgress())
                    switch (keyPress) {
                        case ConsoleKey.LeftArrow:
                            playboard.MoveTetrisBlock("left",playboard.GetBlock());
                            break;
                        case ConsoleKey.RightArrow:
                            playboard.MoveTetrisBlock("right", playboard.GetBlock());
                            break;
                        case ConsoleKey.UpArrow:
                            playboard.RotateAndUpdate();
                            break;
                        case ConsoleKey.Spacebar:
                            playboard.InstantPlaceBlock(playboard.GetBlock());
                            break;
                        case ConsoleKey.DownArrow:
                            playboard.MoveTetrisBlock("down", playboard.GetBlock());
                            break;
                    }
                else {
                    playboard.DrawBoard();
                    GamePrinter.PrintInExactPlace(playboard.drawboard);
                    if (keyPress == ConsoleKey.Enter) {
                        Console.Clear();
                        scoreboard.AddToHighScoreFile(DateTime.Now, Environment.UserName, playboard.GetCurrentScore());
                        WriteBorder();
                        WriteMenu(options, options.First(), true);
                        showMenu = true;
                    }
                    else Console.Beep();
                }
                
            }
        }
        ConsoleColor[] colors = (ConsoleColor[])ConsoleColor.GetValues(typeof(ConsoleColor));
        ConsoleColor currentForeground = Console.ForegroundColor;

        static void WriteBorder()
        {
            Console.Clear();
            Console.Write(@"
  ___ ____ ____ ____ ____ ____ ____ ____ ____     
||  |||  |||  |||  |||  |||  |||  |||  |||  || 
||__|||__|||__|||__|||__|||__|||__|||__|||__||  
|/__\|/__\|/__\|/__\|/__\|/__\|/__\|/__\|/__\|
 ___                                     ____ 
||  ||                                  ||  ||
||__||                                  ||__||
|/__\|                                  |/__\|
 ___                                     ____
||  ||                                  ||  ||
||__||                                  ||__||
|/__\|                                  |/__\| 

 ___                                     ____ 
||  ||                                  ||  ||
||__||                                  ||__||
|/__\|                                  |/__\|
  ___                                    ____ 
||  ||                                  ||  ||
||__||                                  ||__||
|/__\|                                  |/__\|  
  ___ ____ ____ ____ ____ ____ ____ ____ ____   
||  |||  |||  |||  |||  |||  |||  |||  |||  ||  
||__|||__|||__|||__|||__|||__|||__|||__|||__||   
|/__\|/__\|/__\|/__\|/__\|/__\|/__\|/__\|/__\|

        ");
        }

        static void CleanScreen()
        {
            //6,5 startpoint
            //34,17 tyle trzeba wyczyscic
            for(int x=6; x < 40; x++)
            {
                for(int y=5; y<22; y++)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(" ");
                }
            }

            Console.SetCursorPosition(6, 5);
        }

        static void WriteMenu(List<Option> options, Option selectedOption, bool firstTimeRender = false)
        {
            int i = 0;
            foreach (Option option in options) {
                if (option == selectedOption) {
                    Console.SetCursorPosition(18 , 10+i);
                    Console.Write(">");
                }
                else {
                    Console.SetCursorPosition(18, 10+i);
                    Console.Write(" ");
                }

                if(firstTimeRender)
                {
                    Console.SetCursorPosition(5 + i, 10 + i);
                    Console.SetCursorPosition((Console.WindowWidth - option.Name.Length) / 2, Console.CursorTop);
                    Console.WriteLine(option.Name);
                }
                i++;
                //Console.WriteLine();
                
            }
        }

        static void WriteLogo()
        {
            Console.Clear();
            Console.Write(@"
  ___ ____ ____ ____ ____ ____ ____ ____ ____     
||  |||  |||  |||  |||  |||  |||  |||  |||  || 
||__|||__|||__|||__|||__|||__|||__|||__|||__||  
|/__\|/__\|/__\|/__\|/__\|/__\|/__\|/__\|/__\|
 ___                                     ____ 
||  ||                                  ||  ||
||__||                                  ||__||
|/__\|                                  |/__\|
 ___                                     ____
||  || ███████╗█████╗█████╗████╗█╗████╗ ||  ||
||__|| ╚═█╔══╝█╔═══╝╚═█╔═╝█╔══█║█║█╔══╝ ||__||
|/__\|   █║   ████╗   █║  █████╝█║████╗ |/__\| 
         █║   █╔══╝   █║  █╔══█╗█║╚══█║
 ___     █║   █████╗  █║  █║  █║█║████║  ____ 
||  ||   ╚╝   ╚════╝  ╚╝  ╚╝  ╚╝╚╝╚═══╝ ||  ||
||__||                                  ||__||
|/__\|                                  |/__\|
 ___                                     ____ 
||  ||                                  ||  ||
||__||                                  ||__||
|/__\|                                  |/__\|  
  ___ ____ ____ ____ ____ ____ ____ ____ ____   
||  |||  |||  |||  |||  |||  |||  |||  |||  ||  
||__|||__|||__|||__|||__|||__|||__|||__|||__||   
|/__\|/__\|/__\|/__\|/__\|/__\|/__\|/__\|/__\|

  ");
            Thread.Sleep(10000);
            ConsoleHelper.SetCurrentFont("Consolas", 40);
        }
    }


    public class Option {
        public string Name { get; }
        public Action Selected { get; }

        public Option(string name, Action selected) {
            Name = name;
            Selected = selected;
        }
    }

}