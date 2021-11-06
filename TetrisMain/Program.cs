using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;
using TetrisMain.Timers;
using System.Media;
using TetrisMain.Models;
using TetrisMain.UI;


namespace TetrisMain {

    class Program {
        public static GamePrinter GamePrinter;
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

            bool ShowMenu = true;
            SoundPlayer player = new SoundPlayer { //TO-DO fix sound player
                SoundLocation = "Simple Melody.wav"
            };
            options = new List<Option>{
                new Option("Start Game", () => PrepareGame()),
                new Option("Continue", () =>  WriteTemporaryMessage("Continue")),
                new Option("Scoreboard", () =>  WriteTemporaryMessage("Scoreboard")),
                new Option("Exit", () => Exit()),
            };
            int index = 0;

            WriteMenu(options, options[index]);
            ConsoleKeyInfo keyinfo;
            do {
                keyinfo = Console.ReadKey(true);
                if (keyinfo.Key == ConsoleKey.DownArrow) {
                    if (index + 1 < options.Count) {
                        index++;
                        WriteMenu(options, options[index]);
                    }
                }
                if (keyinfo.Key == ConsoleKey.UpArrow) {
                    if (index - 1 >= 0) {
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
            Console.WriteLine(message);
            Thread.Sleep(3000);
            WriteMenu(options, options.Last());
        }
        static void PrepareGame() {
            bool showMenu = false;
            ConsoleKey keyPress;
            TetrisPlayboard playboard = TetrisPlayboard.GetInstance();
            playboard=playboard.Reset();
            GamePrinter = new GamePrinter(playboard.drawboard);
            playboard.RenderNextPiece();
            playboard.RenderBlockCount();
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
                        WriteMenu(options, options.First());
                        showMenu = true;
                    }
                    else Console.Beep();
                }
                
            }
        }
        ConsoleColor[] colors = (ConsoleColor[])ConsoleColor.GetValues(typeof(ConsoleColor));
        ConsoleColor currentForeground = Console.ForegroundColor;
        static void WriteMenu(List<Option> options, Option selectedOption) {
            Console.Clear();
            Console.Write(@"

▄▄▄█████▓▓█████▄▄▄█████▓ ██▀███   ██▓  ██████ 
▓  ██▒ ▓▒▓█   ▀▓  ██▒ ▓▒▓██ ▒ ██▒▓██▒▒██    ▒ 
▒ ▓██░ ▒░▒███  ▒ ▓██░ ▒░▓██ ░▄█ ▒▒██▒░ ▓██▄   
░ ▓██▓ ░ ▒▓█  ▄░ ▓██▓ ░ ▒██▀▀█▄  ░██░  ▒   ██▒
  ▒██▒ ░ ░▒████▒ ▒██▒ ░ ░██▓ ▒██▒░██░▒██████▒▒
  ▒ ░░   ░░ ▒░ ░ ▒ ░░   ░ ▒▓ ░▒▓░░▓  ▒ ▒▓▒ ▒ ░
    ░     ░ ░  ░   ░      ░▒ ░ ▒░ ▒ ░░ ░▒  ░ ░
  ░         ░    ░        ░░   ░  ▒ ░░  ░  ░  
            ░  ░           ░      ░        ░  
                                              

");
            foreach (Option option in options) {
                if (option == selectedOption) {
                    Console.Write("> ");
                }
                else {
                    Console.Write(" ");
                }

                Console.WriteLine(option.Name);
            }
            Console.Write(@"       
  ___ ____ ____ ____ ____ ____     ____
||  |||  |||  |||  |||  |||  ||   ||  ||
||__|||__|||__|||__|||__|||__||   ||  ||___
|/__\|/__\|/__\|/__\|/__\|/__\|   ||__||___|
 ____ ____ ____ ____ ____ ____     --------  ____ 
||  |||  |||  |||  |||  |||  ||             ||  ||
||__|||__|||__|||__|||__|||__||             ||__||
|/__\|/__\|/__\|/__\|/__\|/__\|             |/__\|
 ____ ____ ____ ____ ____ ____ ____ ____ ____ ___
||  |||  |||  |||  |||  |||  |||  |||  ||   ||| 
");
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