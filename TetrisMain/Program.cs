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
        public static int SelectedSettingIndex = 0;
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
        public static List<Option> options_settings;
        static void Main(string[] args) {
            ConsoleHelper.SetCurrentFont("Terminal", 32);
            Console.CursorVisible = false;
            Console.SetWindowSize(1, 1);
            Console.SetBufferSize(50, 30);
            Console.SetWindowSize(50, 30);
            ConsoleHelper.LockSize();
            Console.Title = "TETRIS";
            JukeBox jukeBox = new JukeBox();
            bool ShowMenu = true;

            options = new List<Option>{
                new Option("Start Game", () => PrepareGame()),
                new Option("Settings", () => Settings()),
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
                    new List<(string, Action)>(new[] {"Classic","Block Rush","Gravity Glitch"}.Select(x => (x.ToString(), new Action(() => Models.Settings.GetSettings().selectedGameMode = x)))
                    ))),
                new Option("Grid", () => Write(new List<(string, Action)>(new[]{
                    ("Yes", new Action(() =>
                    {
                        Models.Settings.GetSettings().wantsGrid = '.';
                    })),
                    ("No", new Action(() =>
                    {
                        Models.Settings.GetSettings().wantsGrid = ' ';
                    }))
                }))),
                new Option("Music", () => Write(new List<(string, Action)>(new[]{
                    ("Yes", new Action(() =>
                    {
                        Models.Settings.GetSettings().wantsMusic = true;
                        jukeBox.StartMusic();
                    })),
                    ("No",new Action(() =>
                    {
                        Models.Settings.GetSettings().wantsMusic = false;
                        jukeBox.StopMusic();
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
            jukeBox.PlayMusic(0);
            do {

                keyinfo = Console.ReadKey(true);
                if (keyinfo.Key == ConsoleKey.DownArrow) {
                    if (index + 1 < options.Count) {
                        jukeBox.PlaySound(7, DateTime.Now.Ticks);
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
                    jukeBox.PlaySound(0, DateTime.Now.Ticks);

                    jukeBox.PlaySound(0, DateTime.Now.Ticks);
                    options[index].Selected.Invoke();
                    index = 0;
                }
            }
            while (ShowMenu == true);
            Console.ReadKey();
        }



        static void WriteTemporaryMessage(string message) {
            CleanScreen();
            Console.SetCursorPosition(5, 10);
            Console.SetCursorPosition((Console.WindowWidth - message.Length) / 2, Console.CursorTop);
            Console.WriteLine(message);
            Thread.Sleep(3000);
            CleanScreen();
            WriteMenu(options, options.First(), true);
        }

        static void WriteSettingOptions(List<(string key, Action action)> options, (string key, Action action) selectedOption, bool firstTimeRender = false) {
            int i = 0;
            int secondCol = 0;
            if (options.Count() < 3)
                secondCol += 6;
            if (options.Count() > 10 || options.Count() < 3)
                foreach (var option in options) {
                    if (option == selectedOption) {
                        Console.SetCursorPosition(16 + secondCol, 10 + i);
                        Console.Write(">");
                    }
                    else {
                        Console.SetCursorPosition(16 + secondCol, 10 + i);
                        Console.Write(" ");
                    }

                    if (firstTimeRender) {
                        Console.SetCursorPosition(20 + i, 10 + i);
                        Console.SetCursorPosition(18 + secondCol, Console.CursorTop);
                        Console.WriteLine(option.key);
                    }

                    i++;
                    if (i >= 10 && secondCol == 0) {
                        secondCol = 10;
                        i = 0;
                    }
                    //Console.WriteLine();

                }
            else {
                foreach (var option in options) {
                    string title = option.key;
                    int minus = 2;
                    if (title.Length == 7)
                        minus = 6;
                    if (title.Length == 10)
                        minus = 4;
                    if (option == selectedOption) {
                        Console.SetCursorPosition(((Console.WindowWidth - title.Length + 1) / 2) - minus, 10 + i);
                        Console.Write(">");
                    }
                    else {
                        Console.SetCursorPosition(((Console.WindowWidth - title.Length + 1) / 2) - minus, 10 + i);
                        Console.Write(" ");
                    }

                    if (firstTimeRender) {
                        Console.SetCursorPosition(20 + i, 10 + i);
                        Console.SetCursorPosition(((Console.WindowWidth - title.Length + 1) / 2), Console.CursorTop);
                        Console.WriteLine(option.key);
                    }
                    i++;
                }
            }
        }

        static void Write(List<(string key, Action action)> options) {
            CleanScreen();
            Console.SetCursorPosition(5, 7);
            Console.SetCursorPosition(((Console.WindowWidth - options_settings[SelectedSettingIndex].Name.Length) / 2), Console.CursorTop);
            Console.WriteLine(options_settings[SelectedSettingIndex].Name);
            int selectedValue = 0;
            JukeBox jukeBox = new JukeBox();
            bool ShowMenu = true;
            int index = 0;
            ConsoleKeyInfo keyinfo;
            WriteSettingOptions(options, options[index], true);
            do {
                keyinfo = Console.ReadKey(true);
                if (keyinfo.Key == ConsoleKey.DownArrow) {
                    if (index + 1 < options.Count) {
                        jukeBox.PlaySound(7, DateTime.Now.Ticks);
                        index++;
                        WriteSettingOptions(options, options[index]);
                    }
                }
                if (keyinfo.Key == ConsoleKey.UpArrow) {
                    if (index - 1 >= 0) {
                        index--;
                        jukeBox.PlaySound(6, DateTime.Now.Ticks);
                        WriteSettingOptions(options, options[index]);
                    }
                }
                if (keyinfo.Key == ConsoleKey.Enter) {
                    jukeBox.PlaySound(0, DateTime.Now.Ticks);

                    options[index].action();
                    index = 0;
                    ShowMenu = false;
                }
            }
            while (ShowMenu == true);

            CleanScreen();
            WriteSettings(options_settings, options_settings.First(), true);
        }

        static void WriteScoreboard(List<string> scoreLines) {
            if (scoreLines.Count == 0)
                return;
            const string title = "Scoreboard:";
            const int maxCountOfLines = 12;
            int startIndex = 0;
            int index = startIndex;
            ConsoleKeyInfo keyInfo = new ConsoleKeyInfo();
            JukeBox jukeBox = new JukeBox();
            CleanScreen();
            Console.SetCursorPosition(5, 7);
            Console.SetCursorPosition(((Console.WindowWidth - title.Length) / 2) - 2, Console.CursorTop);
            Console.WriteLine("Scoreboard:");
            int i = 0;
            for (; i <= maxCountOfLines; index++) {
                if (index >= scoreLines.Count)
                    break;
                string currentLine = scoreLines[index];
                Console.SetCursorPosition(5 + i, 9 + i);
                Console.SetCursorPosition(((Console.WindowWidth - currentLine.Length) / 2) - 2, Console.CursorTop);
                Console.WriteLine(currentLine);
                i++;
            }
            do {
                if (scoreLines.Count < maxCountOfLines) {
                    Console.SetCursorPosition(5 + i, 7 + i);
                    Console.ReadKey(true);
                    break;
                }

                keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.DownArrow && startIndex < scoreLines.Count - maxCountOfLines - 1) {
                    jukeBox.PlaySound(7, DateTime.Now.Ticks);
                    string currentLine = scoreLines[index];
                    Console.MoveBufferArea(6, 10, 34, 12, 6, 9);
                    Console.SetCursorPosition(5 + i - 1, 9 + i - 1);
                    Console.SetCursorPosition(((Console.WindowWidth - currentLine.Length) / 2) - 2, Console.CursorTop);
                    Console.WriteLine(currentLine);
                    index++;
                    startIndex++;
                }
                else if (keyInfo.Key == ConsoleKey.UpArrow && startIndex > 0) {
                    jukeBox.PlaySound(6, DateTime.Now.Ticks);
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
            jukeBox.PlaySound(14, DateTime.Now.Ticks);
            CleanScreen();
            WriteMenu(options, options.First(), true);
        }

        static void PrepareGame() {
            Models.Settings settings = Models.Settings.GetSettings();
            JukeBox jukeBox = new JukeBox();
            if (settings.selectedGameMode == "Classic")
                jukeBox.PlayMusic(1);
            if (settings.selectedGameMode == "Block Rush")
                jukeBox.PlayMusic(2);
            if (settings.selectedGameMode == "Gravity Glitch")
                jukeBox.PlayMusic(3);
            bool showMenu = false;
            ConsoleKey keyPress;
            Console.Clear();
            TetrisPlayboard playboard = TetrisPlayboard.GetInstance();
            playboard = playboard.Reset();
            GamePrinter = new GamePrinter(playboard.drawboard);
            playboard.RenderNextPiece();
            playboard.RenderBlockCount();
            playboard.RenderScore("best");
            playboard.RenderLevel();
            playboard.gameClock.LevelUpTimer();
            playboard.Render();
            GamePrinter.PrintMode(settings.selectedGameMode);
            GamePrinter.PrintInExactPlace(playboard.drawboard);
            GlitchTimer glitchTimer = new GlitchTimer();
            if (settings.selectedGameMode == "Gravity Glitch")
                glitchTimer.EnableTimer();
            playboard.StartGame();

            while (showMenu == false) {
                if (!playboard.IsGameInProgress() && settings.selectedGameMode == "Gravity Glitch")
                    glitchTimer.DisableTimer();
                keyPress = Console.ReadKey(true).Key;
                if (playboard.IsGameInProgress())
                    switch (keyPress) {
                        case ConsoleKey.LeftArrow:
                            playboard.MoveTetrisBlock("left", playboard.GetBlock());
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

                }

            }
        }
        ConsoleColor[] colors = (ConsoleColor[])ConsoleColor.GetValues(typeof(ConsoleColor));
        ConsoleColor currentForeground = Console.ForegroundColor;

        static void WriteBorder() {
            Console.SetCursorPosition(0, 0);
            Console.Write(@"
 ____ ____ ____ ____ ________ ____ ____ ____ ____ 
||  |||  |||  |||  |||      |||  |||  |||  |||  ||
||__|||__|||__|||__|||______|||__|||__|||__|||__||
|/__\|/__\|/__\|/__\|/______\|/__\|/__\|/__\|/__\|
 ____                                        ____ 
||  ||                                      ||  ||
||__||                                      ||__||
|/__\|                                      |/__\|
 ____                                        ____ 
||  ||                                      ||  ||
||__||                                      ||__||
|/__\|                                      |/__\|
 ____                                        ____ 
||  ||                                      ||  ||
||__||                                      ||__||
|/__\|                                      |/__\|
 ____                                        ____ 
||  ||                                      ||  ||
||__||                                      ||__||
|/__\|                                      |/__\|
 ____                                        ____ 
||  ||                                      ||  ||
||__||                                      ||__||
|/__\|                                      |/__\|
 ____ ____ ____ ____ ________ ____ ____ ____ ____ 
||  |||  |||  |||  |||      |||  |||  |||  |||  ||
||__|||__|||__|||__|||______|||__|||__|||__|||__||
|/__\|/__\|/__\|/__\|/______\|/__\|/__\|/__\|/__\|

        ");
        }

        static void CleanScreen() {
            //6,5 startpoint
            //34,17 tyle trzeba wyczyscic
            for (int x = 6; x < 40; x++) {
                for (int y = 5; y < 22; y++) {
                    Console.SetCursorPosition(x, y);
                    Console.Write(" ");
                }
            }

            Console.SetCursorPosition(6, 5);
        }

        static void WriteMenu(List<Option> options, Option selectedOption, bool firstTimeRender = false) {
            int i = 0;
            foreach (Option option in options) {
                if (option == selectedOption) {
                    Console.SetCursorPosition(18, 10 + i);
                    Console.Write(">");
                }
                else {
                    Console.SetCursorPosition(18, 10 + i);
                    Console.Write(" ");
                }

                if (firstTimeRender) {
                    Console.SetCursorPosition(5 + i, 10 + i);
                    Console.SetCursorPosition((Console.WindowWidth - option.Name.Length) / 2, Console.CursorTop);
                    Console.WriteLine(option.Name);
                }
                i++;
                //Console.WriteLine();

            }
        }

        static void WriteSettings(List<Option> options_settings, Option selectedOption, bool firstTimeRender = false) {
            int i = 0;
            const string title = "Settings:";
            Console.SetCursorPosition(10, 7);
            Console.SetCursorPosition(((Console.WindowWidth - title.Length) / 2) - 2, Console.CursorTop);
            Console.WriteLine("   Settings:");
            foreach (Option option in options_settings) {
                if (option == selectedOption) {
                    Console.SetCursorPosition(10, 10 + i);
                    Console.Write(">");
                }
                else {
                    Console.SetCursorPosition(10, 10 + i);
                    Console.Write(" ");
                }

                if (firstTimeRender) {
                    Console.SetCursorPosition(12, 10 + i);
                    Console.WriteLine(option.Name);
                }
                i++;
                //Console.WriteLine();

            }
        }

        static void Settings() {
            const string title = "Settings:";
            CleanScreen();
            Console.SetCursorPosition(10, 7);
            Console.SetCursorPosition(((Console.WindowWidth - title.Length) / 2) - 2, Console.CursorTop);
            Console.WriteLine("   Settings:");
            bool ShowMenu = true;
            JukeBox jukeBox = new JukeBox();
            int index = 0;
            ConsoleKeyInfo keyinfo;
            WriteSettings(options_settings, options_settings.First(), true);
            do {
                keyinfo = Console.ReadKey(true);
                if (keyinfo.Key == ConsoleKey.DownArrow) {
                    if (index + 1 < options_settings.Count) {
                        index++;
                        jukeBox.PlaySound(7, DateTime.Now.Ticks);
                        WriteSettings(options_settings, options_settings[index]);
                    }
                }
                if (keyinfo.Key == ConsoleKey.UpArrow) {
                    if (index - 1 >= 0) {
                        jukeBox.PlaySound(6, DateTime.Now.Ticks);
                        index--;
                        WriteSettings(options_settings, options_settings[index]);
                    }
                }
                if (keyinfo.Key == ConsoleKey.Enter) {
                    jukeBox.PlaySound(0, DateTime.Now.Ticks);

                    SelectedSettingIndex = index;
                    options_settings[index].Selected.Invoke();
                    index = 0;
                }
                if (keyinfo.Key == ConsoleKey.Escape) {
                    jukeBox.PlaySound(14, DateTime.Now.Ticks);
                    CleanScreen();
                    WriteMenu(options, options.First(), true);
                    break;
                }
            }
            while (ShowMenu == true);
            Console.ReadKey();

        }

        static void WriteLogo() {
            Console.Clear();
            Console.Write(@"
 ____ ____ ____ ____ ________ ____ ____ ____ ____
||  |||  |||  |||  |||      |||  |||  |||  |||  ||
||__|||__|||__|||__|||______|||__|||__|||__|||__||
|/__\|/__\|/__\|/__\|/______\|/__\|/__\|/__\|/__\|
 ____                                        ____ 
||  ||                                      ||  ||
||__||                                      ||__||
|/__\|                                      |/__\|
 ____                                        ____ 
||  ||                                      ||  ||
||__||                                      ||__||
|/__\|                                      |/__\|
 ____    ██████╗█████╗█████╗█████╗█╗████╗    ____ 
||  ||   ╚═█╔══╝█╔═══╝╚═█╔═╝█╔══█║█║█╔══╝   ||  ||
||__||     █║   ████╗   █║  █████╝█║████╗   ||__||
|/__\|     █║   █╔══╝   █║  █╔═█╗ █║╚══█║   |/__\|
 ____      █║   █████╗  █║  █║ ╚█╗█║████║    ____ 
||  ||     ╚╝   ╚════╝  ╚╝  ╚╝  ╚╝╚╝╚═══╝   ||  ||
||__||                                      ||__||
|/__\|                                      |/__\|
 ____                                        ____ 
||  ||                                      ||  ||
||__||                                      ||__||
|/__\|                                      |/__\|
 ____ ____ ____ ____ ________ ____ ____ ____ ____ 
||  |||  |||  |||  |||      |||  |||  |||  |||  ||
||__|||__|||__|||__|||______|||__|||__|||__|||__||
|/__\|/__\|/__\|/__\|/______\|/__\|/__\|/__\|/__\|

  ");
            Thread.Sleep(2000);
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