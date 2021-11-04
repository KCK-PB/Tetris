using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;
using TetrisMain.Timers;
using System.Media;
using TetrisMain.Models;

namespace TetrisMain {
    class Program {
        public static List<Option> options;
        static void Main(string[] args) {
            bool ShowMenu = true;
            SoundPlayer player = new SoundPlayer { //TO-DO fix sound player
                SoundLocation = "Simple Melody.wav"
            };
            options = new List<Option>{
                new Option("Start Game", () => PrepareGame()),
                new Option("Continue", () =>  WriteTemporaryMessage("Continue")),
                new Option("Scoreboard", () =>  WriteTemporaryMessage("Scoreboard")),
                new Option("Exit", () => Environment.Exit(0)),
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
            while (ShowMenu==true);
                Console.ReadKey();
        }

        static void WriteTemporaryMessage(string message) { //TO-DO make actual options for menu
            Console.Clear();
            Console.WriteLine(message);
            Thread.Sleep(3000);
            WriteMenu(options, options.Last());
        }
        static void PrepareGame() {
            //Console.Clear();
            //Timers.Timer gameClock = new Timers.Timer();
            ConsoleKey keyPress;
            TetrisPlayboard playboard = TetrisPlayboard.GetInstance();
            playboard.StartGame();
            while (playboard.IsGameInProgress()) {
                keyPress = Console.ReadKey(true).Key;
                if(playboard.IsGameInProgress())
                    switch (keyPress) {
                        case ConsoleKey.LeftArrow:
                            playboard.MoveTetrisBlock("left");
                            break;
                        case ConsoleKey.RightArrow:
                            playboard.MoveTetrisBlock("right");
                            break;
                        case ConsoleKey.UpArrow:
                            //playboard.RotateBlock();
                            break;
                        case ConsoleKey.Spacebar:
                            playboard.InstantPlaceBlock();
                            break;
                    }
                else if (keyPress == ConsoleKey.Enter) {
                    WriteMenu(options, options.First());
                }
            }
        }


        static void WriteMenu(List<Option> options, Option selectedOption) {
            Console.Clear();

            foreach (Option option in options) {
                if (option == selectedOption) {
                    Console.Write("> ");
                }
                else {
                    Console.Write(" ");
                }

                Console.WriteLine(option.Name);
            }
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