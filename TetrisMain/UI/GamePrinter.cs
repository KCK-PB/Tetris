using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TetrisMain.Models;
using System.Threading;

namespace TetrisMain.UI {
    public class GamePrinter {
        //tab[50][30]
        //metoda rysowanie przesunięcia klocka w lewo i prawo
        //metoda rysowanie przesunięcia klocka w dół
        //metoda odświeżenia ekranu
        // drawchanche(drawboard) - list of change 
        //Console.ForegroundColor = ConsoleColor.Red;
        private char[][] Game;
        private char[,] drawboard;
        private int[] blockCount = { 0, 0, 0, 0, 0, 0, 0 };
        private Settings settings = Settings.GetSettings();
        public Object printerBlocker = new Object();
        public bool upsideDown = false;
        public GamePrinter(char[,] _drawboard) {
            drawboard = _drawboard;

            Game = new char[30][];
            for (int i = 0; i < 30; i++) {
                Game[i] = new char[50];
            }

            //fill play board with ' '
            for (int i = 0; i < 29; i++) {
                for (int j = 0; j < 50; j++) {
                    Game[i][j] = ' ';
                }

            }
            ////game board border
            //Game[0][0] = '┌';
            //Game[0][49] = '┐';

            //for (int topBorder = 1; topBorder < 49; topBorder++)
            //{
            //    Game[0][topBorder] = '─';
            //}

            //for (int leftBorder = 1; leftBorder < 29; leftBorder++)
            //{
            //    Game[leftBorder][0] = '│';
            //}

            //for (int rightBorder = 1; rightBorder < 29; rightBorder++)
            //{
            //    Game[rightBorder][49] = '│';
            //}

            //Game[29][0] = '└';
            //Game[29][49] = '┘';

            //for (int bottomBorder = 1; bottomBorder < 49; bottomBorder++)
            //{
            //    Game[29][bottomBorder] = '─';
            //}

            //Tetris KCK
            Game[2][6] = 'T';
            Game[2][7] = 'E';
            Game[2][8] = 'T';
            Game[2][9] = 'R';
            Game[2][10] = 'I';
            Game[2][11] = 'S';

            Game[3][8] = 'K';
            Game[3][9] = 'C';
            Game[3][10] = 'K';

            Game[1][3] = '╔';
            Game[1][15] = '╗';
            Game[2][3] = '║';
            Game[3][3] = '╚';
            Game[3][4] = '═';
            Game[3][5] = '═';
            Game[3][6] = '╗';
            Game[4][6] = '║';
            Game[5][6] = '╚';
            Game[5][12] = '╝';
            Game[5][12] = '╝';
            Game[4][12] = '║';
            Game[3][12] = '╔';
            Game[3][13] = '═';
            Game[3][14] = '═';
            Game[3][15] = '╝';
            Game[2][15] = '║';


            for (int logoKckTop = 4; logoKckTop < 15; logoKckTop++) {
                Game[1][logoKckTop] = '═';
            }
            for (int logoKckBottom = 7; logoKckBottom < 12; logoKckBottom++) {
                Game[5][logoKckBottom] = '═';
            }

            //Statistic
            Game[7][5] = 'S';
            Game[7][6] = 'T';
            Game[7][7] = 'A';
            Game[7][8] = 'T';
            Game[7][9] = 'I';
            Game[7][10] = 'S';
            Game[7][11] = 'T';
            Game[7][12] = 'I';
            Game[7][13] = 'C';

            Game[6][3] = '╔';
            Game[6][15] = '╗';
            Game[27][3] = '╚';
            Game[27][15] = '╝';

            for (int statisticTopBottom = 4; statisticTopBottom < 15; statisticTopBottom++) {
                Game[6][statisticTopBottom] = '═';
                Game[27][statisticTopBottom] = '═';
            }
            for (int statisticLeftRight = 7; statisticLeftRight < 27; statisticLeftRight++) {
                Game[statisticLeftRight][3] = '║';
                Game[statisticLeftRight][15] = '║';
            }

            //'T' block
            Game[8][5] = '█';
            Game[8][6] = '█';
            Game[9][6] = '█';
            Game[8][7] = '█';

            Game[9][9] = '0';
            Game[9][10] = '0';
            Game[9][11] = '0';

            //'J' block
            Game[11][5] = '█';
            Game[11][6] = '█';
            Game[11][7] = '█';
            Game[12][7] = '█';

            Game[12][9] = '0';
            Game[12][10] = '0';
            Game[12][11] = '0';


            //'Z' block
            Game[14][5] = '█';
            Game[14][6] = '█';
            Game[15][6] = '█';
            Game[15][7] = '█';

            Game[15][9] = '0';
            Game[15][10] = '0';
            Game[15][11] = '0';

            //'O' block
            Game[17][6] = '█';
            Game[18][6] = '█';
            Game[17][7] = '█';
            Game[18][7] = '█';

            Game[18][9] = '0';
            Game[18][10] = '0';
            Game[18][11] = '0';

            //'S' block
            Game[21][5] = '█';
            Game[21][6] = '█';
            Game[20][6] = '█';
            Game[20][7] = '█';

            Game[21][9] = '0';
            Game[21][10] = '0';
            Game[21][11] = '0';

            //'L' block
            Game[23][5] = '█';
            Game[24][5] = '█';
            Game[23][6] = '█';
            Game[23][7] = '█';

            Game[24][9] = '0';
            Game[24][10] = '0';
            Game[24][11] = '0';

            //'I' block
            Game[26][4] = '█';
            Game[26][5] = '█';
            Game[26][6] = '█';
            Game[26][7] = '█';

            Game[26][9] = '0';
            Game[26][10] = '0';
            Game[26][11] = '0';

            //playboard border
            Game[6][19] = '┌';
            Game[6][30] = '┐';
            Game[27][19] = '└';
            Game[27][30] = '┘';

            for (int playTopBottom = 20; playTopBottom < 30; playTopBottom++) {
                Game[6][playTopBottom] = '─';
                Game[27][playTopBottom] = '─';
            }
            for (int playLeftRight = 7; playLeftRight < 27; playLeftRight++) {
                Game[playLeftRight][19] = '│';
                Game[playLeftRight][30] = '│';
            }

            //Best & Score
            //border
            Game[2][34] = '╔';
            Game[2][43] = '╗';
            Game[8][34] = '╚';
            Game[8][43] = '╝';

            for (int scoreTopBottom = 35; scoreTopBottom < 43; scoreTopBottom++) {
                Game[2][scoreTopBottom] = '═';
                Game[8][scoreTopBottom] = '═';
            }
            for (int scoreLetRight = 3; scoreLetRight < 8; scoreLetRight++) {
                Game[scoreLetRight][34] = '║';
                Game[scoreLetRight][43] = '║';
            }

            Game[3][36] = 'B';
            Game[3][37] = 'E';
            Game[3][38] = 'S';
            Game[3][39] = 'T';

            Game[6][36] = 'S';
            Game[6][37] = 'C';
            Game[6][38] = 'O';
            Game[6][39] = 'R';
            Game[6][40] = 'E';

            //Best & Score = 0000 when game start
            for (int scoreBest0 = 36; scoreBest0 < 42; scoreBest0++) {
                Game[4][scoreBest0] = '0';
                Game[7][scoreBest0] = '0';
            }

            //next block
            Game[10][36] = 'N';
            Game[10][37] = 'E';
            Game[10][38] = 'X';
            Game[10][39] = 'T';

            Game[9][34] = '╔';
            Game[9][41] = '╗';
            Game[15][34] = '╚';
            Game[15][41] = '╝';

            for (int nextTopBottom = 35; nextTopBottom < 41; nextTopBottom++) {
                Game[9][nextTopBottom] = '═';
                Game[15][nextTopBottom] = '═';
            }
            for (int nextLeftRigth = 10; nextLeftRigth < 15; nextLeftRigth++) {
                Game[nextLeftRigth][34] = '║';
                Game[nextLeftRigth][41] = '║';
            }

            //Lines
            Game[17][36] = 'L';
            Game[17][37] = 'I';
            Game[17][38] = 'N';
            Game[17][39] = 'E';
            Game[17][40] = 'S';

            //Lines when game start
            Game[18][37] = '0';
            Game[18][38] = '0';
            Game[18][39] = '0';
            Game[18][40] = '0';

            Game[16][34] = '╔';
            Game[16][43] = '╗';
            Game[19][34] = '╚';
            Game[19][43] = '╝';

            for (int linesTopBottom = 35; linesTopBottom < 43; linesTopBottom++) {
                Game[16][linesTopBottom] = '═';
                Game[19][linesTopBottom] = '═';
            }
            for (int linesLeftRigth = 17; linesLeftRigth < 19; linesLeftRigth++) {
                Game[linesLeftRigth][34] = '║';
                Game[linesLeftRigth][43] = '║';
            }

            //Level
            Game[21][36] = 'L';
            Game[21][37] = 'E';
            Game[21][38] = 'V';
            Game[21][39] = 'E';
            Game[21][40] = 'L';

            //Level when game start
            Game[22][38] = '0';
            Game[22][39] = '1';

            Game[20][34] = '╔';
            Game[20][43] = '╗';
            Game[23][34] = '╚';
            Game[23][43] = '╝';

            for (int levelTopBottom = 35; levelTopBottom < 43; levelTopBottom++) {
                Game[20][levelTopBottom] = '═';
                Game[23][levelTopBottom] = '═';
            }
            for (int levelLeftRigth = 21; levelLeftRigth < 23; levelLeftRigth++) {
                Game[levelLeftRigth][34] = '║';
                Game[levelLeftRigth][43] = '║';
            }

            int k = 0;
            for (int i = 19; i >= 0; i--) {
                for (int j = 0; j < 10; j++) {
                    Game[7 + k][20 + j] = drawboard[i, j];
                }
                k++;
            }
            //Console.BackgroundColor = ConsoleColor.DarkGray;
            for (int i = 0; i < 30; i++) {
                for (int j = 0; j < 49; j++) {
                    Console.Write(this.Game[i][j]);
                }
                Console.Write(' ');
            }
            Console.Out.Flush();
            //Console.BackgroundColor = ConsoleColor.Black;
            PrintColoredStatistic();

        }

        public void PrintInExactPlace(char[,] drawboard) {
            lock (printerBlocker) {
                int startposition = 26;
                if (upsideDown)
                    startposition = 7;
                for (int i = 0; i < 20; i++) {
                    for (int j = 0; j < 10; j++) {
                        int drawI = i;
                        if (upsideDown)
                            drawI*=-1;
                        Console.SetCursorPosition(j + 20, -drawI + startposition);
                        ConsoleColor tempColor = GetPieceColor(drawboard[i, j].ToString());
                        if (drawboard[i, j] >= 'A' && drawboard[i, j] <= 'Z')
                            drawboard[i, j] = '█';
                        WriteInColor(tempColor, drawboard[i, j].ToString());
                    }
                }
            }
        }
        public void PrintScore(int score, int where) {
            string tempScore = (score % 100000).ToString("D6");
            lock (printerBlocker) {
                Console.SetCursorPosition(36, where);
                WriteInColor(ConsoleColor.Gray, tempScore);
            }
        }
        public void PrintLevel(int level) {
            lock (printerBlocker) {
                Console.SetCursorPosition(38, 22);
                string tempLevel = (level % 100).ToString("D2");
                WriteInColor(ConsoleColor.Gray, tempLevel);
            }
        }
        public void PrintLines(int lines) {
            lock (printerBlocker) {
                Console.SetCursorPosition(37, 18);
                string tempLines = (lines % 10000).ToString("D4");
                WriteInColor(ConsoleColor.Gray, tempLines);
            }
        }
        public void PrintGameOver() {
            lock (printerBlocker) {
                Console.SetCursorPosition(20, 2);
                WriteInColor(ConsoleColor.Gray, "GAME OVER");

                Console.SetCursorPosition(19, 3);
                WriteInColor(ConsoleColor.Gray, "PRESS ENTER");

                Console.SetCursorPosition(20, 4);
                WriteInColor(ConsoleColor.Gray, "TO RETURN");

                Console.SetCursorPosition(19, 5);
                WriteInColor(ConsoleColor.Gray, "TO MAIN MENU");
            }
        }
        public void PrintNextPiece(string type) {
            ConsoleColor tempColor = GetPieceColor(type);
            lock (printerBlocker) {
                Console.SetCursorPosition(36, 12);
                if (type == "I-block") {
                    WriteInColor(tempColor, "████");
                    Console.SetCursorPosition(36, 13);
                    WriteInColor(tempColor, "    ");
                }

                if (type == "J-block") {
                    WriteInColor(tempColor, "███ ");
                    Console.SetCursorPosition(36, 13);
                    WriteInColor(tempColor, "  █ ");
                }
                if (type == "O-block") {
                    WriteInColor(tempColor, "██  ");
                    Console.SetCursorPosition(36, 13);
                    WriteInColor(tempColor, "██  ");
                }
                if (type == "L-block") {
                    WriteInColor(tempColor, "███ ");
                    Console.SetCursorPosition(36, 13);
                    WriteInColor(tempColor, "█   ");
                }
                if (type == "Z-block") {
                    WriteInColor(tempColor, "██  ");
                    Console.SetCursorPosition(36, 13);
                    WriteInColor(tempColor, " ██ ");
                }
                if (type == "T-block") {
                    WriteInColor(tempColor, "███ ");
                    Console.SetCursorPosition(36, 13);
                    WriteInColor(tempColor, " █  ");
                }
                if (type == "S-block") {
                    WriteInColor(tempColor, " ██ ");
                    Console.SetCursorPosition(36, 13);
                    WriteInColor(tempColor, "██  ");
                }
            }
        }
        private void WriteInColor(ConsoleColor color, string message) {
            Console.ForegroundColor = color;
            Console.Write(message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        private ConsoleColor GetPieceColor(string type) {
            ConsoleColor tempColor = ConsoleColor.Gray;
            if (type[0] == 'I')
                tempColor = ConsoleColor.Cyan;
            if (type[0] == 'J')
                tempColor = ConsoleColor.Blue;
            if (type[0] == 'O')
                tempColor = ConsoleColor.Yellow;
            if (type[0] == 'L')
                tempColor = ConsoleColor.DarkRed;
            if (type[0] == 'Z')
                tempColor = ConsoleColor.Red;
            if (type[0] == 'T')
                tempColor = ConsoleColor.DarkMagenta;
            if (type[0] == 'S')
                tempColor = ConsoleColor.Green;
            return tempColor;
        }
        public void PrintBlockCount(string type) {
            int tempIndex = 0;
            if (type == "T-block") {
                Console.SetCursorPosition(9, 9);
            }
            if (type == "J-block") {
                tempIndex = 1;
                Console.SetCursorPosition(9, 12);
            }
            if (type == "Z-block") {
                tempIndex = 2;
                Console.SetCursorPosition(9, 15);
            }
            if (type == "O-block") {
                tempIndex = 3;
                Console.SetCursorPosition(9, 18);
            }
            if (type == "S-block") {
                tempIndex = 4;
                Console.SetCursorPosition(9, 21);
            }
            if (type == "L-block") {
                tempIndex = 5;
                Console.SetCursorPosition(9, 24);
            }
            if (type == "I-block") {
                tempIndex = 6;
                Console.SetCursorPosition(9, 26);
            }
            string tempCount = (++blockCount[tempIndex] % 1000).ToString("D3");
            WriteInColor(ConsoleColor.Gray, tempCount);
        }
        private void PrintColoredStatistic() {

            ConsoleColor[] tempColors = { ConsoleColor.DarkMagenta, ConsoleColor.Blue, ConsoleColor.Red, ConsoleColor.Yellow, ConsoleColor.Green, ConsoleColor.DarkRed, ConsoleColor.Cyan };
            if (settings.wantsAlternativeColorPallete) {
                tempColors[0] = ConsoleColor.Blue;
                tempColors[1] = ConsoleColor.Blue;
                tempColors[2] = ConsoleColor.Blue;
                tempColors[3] = ConsoleColor.Blue;
                tempColors[4] = ConsoleColor.Blue;
                tempColors[5] = ConsoleColor.Blue;
                tempColors[6] = ConsoleColor.Blue;
            }
            Console.SetCursorPosition(5,8);
            WriteInColor(tempColors[0], "███");
            Console.SetCursorPosition(5,9);
            WriteInColor(tempColors[0], " █");

            Console.SetCursorPosition(5,11);
            WriteInColor(tempColors[1], "███");
            Console.SetCursorPosition(5, 12);
            WriteInColor(tempColors[1], "  █ ");

            Console.SetCursorPosition(5, 14);
            WriteInColor(tempColors[2], "██");
            Console.SetCursorPosition(5, 15);
            WriteInColor(tempColors[2], " ██");

            Console.SetCursorPosition(6, 17);
            WriteInColor(tempColors[3], "██");
            Console.SetCursorPosition(6, 18);
            WriteInColor(tempColors[3], "██");

            Console.SetCursorPosition(5, 20);
            WriteInColor(tempColors[4], " ██");
            Console.SetCursorPosition(5, 21);
            WriteInColor(tempColors[4], "██");

            Console.SetCursorPosition(5, 23);
            WriteInColor(tempColors[5], "███");
            Console.SetCursorPosition(5, 24);
            WriteInColor(tempColors[5], "█");

            Console.SetCursorPosition(4, 26);
            WriteInColor(tempColors[6], "████");
            Console.SetCursorPosition(4, 27);
            WriteInColor(tempColors[6], "");
        }
        public void PrintBlockRushAnimation(TetrisBlock positions) {
            lock (printerBlocker) {
                foreach(Square coords in positions.GetPosition()) {
                    Console.SetCursorPosition(coords.GetPos().Item2 + 20, -1*coords.GetPos().Item1 + 26);
                    WriteInColor(ConsoleColor.Gray, "`");
                }
                Thread.Sleep(40);
                foreach (Square coords in positions.GetPosition()) {
                    Console.SetCursorPosition(coords.GetPos().Item2 + 20, -coords.GetPos().Item1 + 26);
                    WriteInColor(ConsoleColor.Gray, "*");
                }
                Thread.Sleep(40);
                foreach (Square coords in positions.GetPosition()) {
                    Console.SetCursorPosition(coords.GetPos().Item2 + 20, -coords.GetPos().Item1 + 26);
                    WriteInColor(ConsoleColor.Gray, "+");
                }
                Thread.Sleep(40);
                foreach (Square coords in positions.GetPosition()) {
                    Console.SetCursorPosition(coords.GetPos().Item2 + 20, -coords.GetPos().Item1 + 26);
                    WriteInColor(ConsoleColor.Gray, "#");
                }
                Thread.Sleep(40);
                foreach (Square coords in positions.GetPosition()) {
                    Console.SetCursorPosition(coords.GetPos().Item2 + 20, -coords.GetPos().Item1 + 26);
                    WriteInColor(ConsoleColor.Gray, "░");
                }
                Thread.Sleep(40);
                foreach (Square coords in positions.GetPosition()) {
                    Console.SetCursorPosition(coords.GetPos().Item2 + 20, -coords.GetPos().Item1 + 26);
                    WriteInColor(ConsoleColor.Gray, "▓");
                }
                Thread.Sleep(40);
                foreach (Square coords in positions.GetPosition()) {
                    Console.SetCursorPosition(coords.GetPos().Item2 + 20, -coords.GetPos().Item1 + 26);
                    WriteInColor(ConsoleColor.Gray, "█");
                }
            }
            
        }
    }
}