using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;
using TetrisMain.Timers;
using System.Media;
using TetrisMain.Models;
using TetrisMain.UI;


namespace TetrisMain
{

    class Program
    {
        public static int SelectedSettingIndex = 0;
        public static GamePrinter GamePrinter;
        public static readonly Scoreboard scoreboard = new Scoreboard("scoreboard.txt");
        static void ConsoleDraw(IEnumerable<string> lines, int x, int y)
        {
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
                select new
                {
                    Text = new String(line.Skip(trimLeft).Take(Math.Min(Console.WindowWidth - x, line.Length - trimLeft)).ToArray()),
                    X = x,
                    Y = y++
                };

            Console.Clear();
            foreach (var line in linesToPrint)
            {
                Console.SetCursorPosition(line.X, line.Y);
                Console.WriteLine(line.Text);
            }
        }

        static void Exit()
        {
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
            for (int y = -arr.Length; y < Console.WindowHeight + arr.Length; y++)
            {
                ConsoleDraw(arr, x, y);
                Thread.Sleep(100);
            }
            Environment.Exit(0);
        }
        public static List<Option> options;
        public static List<Option> options_settings;
        static void Main(string[] args)
        {
            ConsoleHelper.SetCurrentFont("Terminal", 32);
            Console.CursorVisible = false;
            Console.SetWindowSize(1, 1);
            Console.SetBufferSize(30, 30);
            Console.SetWindowSize(50, 30);
            ConsoleHelper.LockSize();
            Console.Title = "TETRIS";

            bool ShowMenu = true;
            SoundPlayer player = new SoundPlayer
            { //TO-DO fix sound player
                SoundLocation = "Simple Melody.wav"
            };
            options = new List<Option>{
                new Option("Start Game", () => PrepareGame()),
                new Option("Setting", () => Settings()),
                new Option("Scoreboard", () =>  WriteScoreboard(scoreboard.GetHighScoreList())),
                new Option("Exit", () => Exit()),
            };
            options_settings = new List<Option>
            {
                new Option("Alternate color pallete", () => Write(new List<(string, Action)>(
                    new[]{
                        ("Yes", new Action(
                            () =>
                            {
                                Models.Settings.GetSettings().wantsAlternativeColorPallete = true; 
                            })
                        ),  
                        ("No", new Action(
                            () =>
                            {
                                Models.Settings.GetSettings().wantsAlternativeColorPallete = false;
                            })
                        ) 
                    }))),
                new Option("Ghost piece", () => Write(new List<(string, Action)>(new[]{
                    ("Yes", new Action(() => 
                    {
                        Models.Settings.GetSettings().wantsGhostPiece = true;
                    })),  
                    ("No", new Action(() => 
                    {
                        Models.Settings.GetSettings().wantsGhostPiece = false;
                    })) 
                }))),
                new Option("Starting level", () => Write(
                    new List<(string, Action)>(Enumerable.Range(1,20).Select(x => (x.ToString(), new Action(() => Models.Settings.GetSettings().startingLevel = x)))
                    ))),
                                new Option("Game Mode", () => Write(
                    new List<(string, Action)>(Enumerable.Range(1,3).Select(x => (x.ToString(), new Action(() => Models.Settings.GetSettings().selectedGameMode = x)))
                    ))),
                new Option("Grid", () => Write(new List<(string, Action)>(new[]{
                    ("Yes", new Action(() => 
                    {
                        Models.Settings.GetSettings().wantsGrid = ' ';
                    })),
                    ("No", new Action(() => 
                    {
                        Models.Settings.GetSettings().wantsGrid = ',';
                    })) 
                }))),
                new Option("Music", () => Write(new List<(string, Action)>(new[]{
                    ("Yes", new Action(() => 
                    {
                        Models.Settings.GetSettings().wantsMusic = true;
                    })),
                    ("No",new Action(() => 
                    {
                        Models.Settings.GetSettings().wantsMusic = false;
                    })) 
                }))),
                new Option("Sounds", () => Write(new List<(string, Action)>(new[]{
                    ("Yes", new Action(() => 
                    {
                        Models.Settings.GetSettings().wantsSFX = true;
                    })),
                    ("No",new Action(() => 
                    {
                        Models.Settings.GetSettings().wantsSFX = false;
                    })) 
                }))),
            };

            int index = 0;
            WriteLogo();
            WriteBorder();
            WriteMenu(options, options[index], true);
            ConsoleKeyInfo keyinfo;
            do
            {
                keyinfo = Console.ReadKey(true);
                if (keyinfo.Key == ConsoleKey.DownArrow)
                {
                    if (index + 1 < options.Count)
                    {
                        index++;
                        WriteMenu(options, options[index]);
                    }
                }
                if (keyinfo.Key == ConsoleKey.UpArrow)
                {
                    if (index - 1 >= 0)
                    {
                        index--;
                        WriteMenu(options, options[index]);
                    }
                }
                if (keyinfo.Key == ConsoleKey.Enter)
                {
                    options[index].Selected.Invoke();
                    index = 0;
                }
            }
            while (ShowMenu == true);
            Console.ReadKey();
        }



        static void WriteTemporaryMessage(string message)
        { //TO-DO make actual options for menu
            //Console.Clear();
            CleanScreen();
            Console.SetCursorPosition(5, 10);
            Console.SetCursorPosition((Console.WindowWidth - message.Length) / 2, Console.CursorTop);
            Console.WriteLine(message);
            Thread.Sleep(3000);
            CleanScreen();
            WriteMenu(options, options.First(), true);
        }

        static void WriteSettingOptions(List<(string key, Action action)> options, (string key, Action action) selectedOption, bool firstTimeRender = false)
        {
            int i = 0;
            int secondCol = 0;

            foreach (var option in options)
            {
                if (option == selectedOption)
                {
                    Console.SetCursorPosition(16+secondCol, 10 + i);
                    Console.Write(">");
                }
                else
                {
                    Console.SetCursorPosition(16+secondCol, 10 + i);
                    Console.Write(" ");
                }

                if (firstTimeRender)
                {
                    Console.SetCursorPosition(16 + i, 10 + i);
                    Console.SetCursorPosition(18 + secondCol, Console.CursorTop);
                    Console.WriteLine(option.key);
                }

                i++;
                if (i >= 10 && secondCol == 0)
                {
                    secondCol = 10;
                    i = 0;
                }
                //Console.WriteLine();

            }
        }

        static void Write(List<(string key, Action action)> options)
        { //TO-DO make actual options for menu
            //Console.Clear();
            CleanScreen();
            Console.SetCursorPosition(5, 7);
            Console.SetCursorPosition(((Console.WindowWidth - options_settings[SelectedSettingIndex].Name.Length) / 2) - 2, Console.CursorTop);
            Console.WriteLine(options_settings[SelectedSettingIndex].Name);
            int selectedValue = 0;

            bool ShowMenu = true;
            int index = 0;
            ConsoleKeyInfo keyinfo;
            WriteSettingOptions(options, options[index], true);
            do
            {
                keyinfo = Console.ReadKey(true);
                if (keyinfo.Key == ConsoleKey.DownArrow)
                {
                    if (index + 1 < options.Count)
                    {
                        index++;
                        WriteSettingOptions(options, options[index]);
                    }
                }
                if (keyinfo.Key == ConsoleKey.UpArrow)
                {
                    if (index - 1 >= 0)
                    {
                        index--;
                        WriteSettingOptions(options, options[index]);
                    }
                }
                if (keyinfo.Key == ConsoleKey.Enter)
                {
                    options[index].action();
                    index = 0;
                    ShowMenu = false;
                }
            }
            while (ShowMenu == true);

            CleanScreen();
            WriteSettings(options_settings, options_settings.First(), true);
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
                    Console.ReadKey(true);
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

        static void PrepareGame()
        {
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

            while (showMenu == false)
            {
                keyPress = Console.ReadKey(true).Key;
                if (playboard.IsGameInProgress())
                    switch (keyPress)
                    {
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
                else
                {
                    playboard.DrawBoard();
                    GamePrinter.PrintInExactPlace(playboard.drawboard);
                    if (keyPress == ConsoleKey.Enter)
                    {
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
            for (int x = 6; x < 40; x++)
            {
                for (int y = 5; y < 22; y++)
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
            foreach (Option option in options)
            {
                if (option == selectedOption)
                {
                    Console.SetCursorPosition(18, 10 + i);
                    Console.Write(">");
                }
                else
                {
                    Console.SetCursorPosition(18, 10 + i);
                    Console.Write(" ");
                }

                if (firstTimeRender)
                {
                    Console.SetCursorPosition(5 + i, 10 + i);
                    Console.SetCursorPosition((Console.WindowWidth - option.Name.Length) / 2, Console.CursorTop);
                    Console.WriteLine(option.Name);
                }
                i++;
                //Console.WriteLine();

            }
        }

        static void WriteSettings(List<Option> options_settings, Option selectedOption, bool firstTimeRender = false)
        {
            int i = 0;
            foreach (Option option in options_settings)
            {
                if (option == selectedOption)
                {
                    Console.SetCursorPosition(7, 10 + i);
                    Console.Write(">");
                }
                else
                {
                    Console.SetCursorPosition(7, 10 + i);
                    Console.Write(" ");
                }

                if (firstTimeRender)
                {
                    Console.SetCursorPosition(7 + i, 10 + i);
                    Console.SetCursorPosition((Console.WindowWidth - option.Name.Length) / 2, Console.CursorTop);
                    Console.WriteLine(option.Name);
                }
                i++;
                //Console.WriteLine();

            }
        }

        static void Settings()
        {
            const string title = "Settings:";
            CleanScreen();
            Console.SetCursorPosition(10, 7);
            Console.SetCursorPosition(((Console.WindowWidth - title.Length) / 2) - 2, Console.CursorTop);
            Console.WriteLine("Settings:");
            bool ShowMenu = true;

            int index = 0;
            ConsoleKeyInfo keyinfo;
            WriteSettings(options_settings, options_settings.First(), true);
            do
            {
                keyinfo = Console.ReadKey(true);
                if (keyinfo.Key == ConsoleKey.DownArrow)
                {
                    if (index + 1 < options_settings.Count)
                    {
                        index++;
                        WriteSettings(options_settings, options_settings[index]);
                    }
                }
                if (keyinfo.Key == ConsoleKey.UpArrow)
                {
                    if (index - 1 >= 0)
                    {
                        index--;
                        WriteSettings(options_settings, options_settings[index]);
                    }
                }
                if (keyinfo.Key == ConsoleKey.Enter)
                {
                    SelectedSettingIndex = index;
                    options_settings[index].Selected.Invoke();
                    index = 0;
                }
                if (keyinfo.Key == ConsoleKey.Escape)
                {
                    CleanScreen();
                    WriteMenu(options, options.First(), true);
                    break;
                }
            }
            while (ShowMenu == true);
            Console.ReadKey();

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
            Thread.Sleep(1000);
            ConsoleHelper.SetCurrentFont("Consolas", 40);
        }
    }


    public class Option
    {
        public string Name { get; }
        public Action Selected { get; }

        public Option(string name, Action selected)
        {
            Name = name;
            Selected = selected;
        }
    }

}