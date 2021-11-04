using System;
using System.Threading;
namespace TetrisMain.Models {
    public class TetrisPlayboard {
        private char[,] playboard;
        public char[,] drawboard;
        private int[] lineBlockCount;
        private bool gameInProgress;
        private bool gameOver;
        Timers.Timer gameClock = null;
        private TetrisBlock currentPiece = new("J-block"); //placeholder current piece, TO-DO: add method to create current piece when last was placed on playboard
        private static readonly TetrisPlayboard instance = new TetrisPlayboard();

        private TetrisPlayboard() {
            playboard = new char[24, 10];
            drawboard = new char[24, 10];
            lineBlockCount = new int[24];
            gameInProgress = false;
            gameOver = false;
            gameClock = new Timers.Timer(this);
            for (var i = 0; i < 24; i++)
                for (var j = 0; j < 10; j++) {
                    playboard[i, j] = '.';
                    drawboard[i, j] = ' ';
                }
            //TEMPORARY
            lineBlockCount[0] = 8;
            for (int i = 0; i < 10; i++) {
                if (i == 4 || i == 5)
                    continue;
                playboard[0, i] = '▓';
            }

            lineBlockCount[3] = 8;
            for (int i = 0; i < 10; i++) {
                if (i == 4 || i == 5)
                    continue;
                playboard[3, i] = '▓';
            }

            lineBlockCount[6] = 8;
            for (int i = 0; i < 10; i++) {
                if (i == 4 || i == 5)
                    continue;
                playboard[6, i] = '▓';
            }
            //TEMPORARY END
        }

        public static TetrisPlayboard GetInstance() {
            return instance;
        }
        public void StartGame() {
            gameInProgress = true;
            gameClock.EnableTimer();
        }
        public void StopGame() {
            gameInProgress = false;
            gameClock.DisableTimer();
        }
        public bool IsGameInProgress() {
            return gameInProgress;
        }
        public bool IsOccupied(int posx, int posy) {
            if (posx < 0 || posy < 0 || posy > 9 || playboard[posx, posy] != '.')
                return true;
            return false;
        }

        public void DrawBoard() {
            for (var i = 0; i < 24; i++)
                for (var j = 0; j < 10; j++)
                    drawboard[i, j] = playboard[i, j];

            var tempPosition = currentPiece.GetPosition();

            for (var i = 0; i < 4; i++)
                drawboard[tempPosition[i].GetPos().Item1, tempPosition[i].GetPos().Item2] = '▓'; //■
        }
        public bool CheckCollision(string direction, Square[] tempPosition) {
            switch (direction) {
                case "down":
                    return (IsOccupied(tempPosition[0].GetPos().Item1 - 1, tempPosition[0].GetPos().Item2) || IsOccupied(tempPosition[1].GetPos().Item1 - 1, tempPosition[1].GetPos().Item2) || IsOccupied(tempPosition[2].GetPos().Item1 - 1, tempPosition[2].GetPos().Item2) || IsOccupied(tempPosition[3].GetPos().Item1 - 1, tempPosition[3].GetPos().Item2));

                case "left":
                    return (IsOccupied(tempPosition[0].GetPos().Item1, tempPosition[0].GetPos().Item2 - 1) || IsOccupied(tempPosition[1].GetPos().Item1, tempPosition[1].GetPos().Item2 - 1) || IsOccupied(tempPosition[2].GetPos().Item1, tempPosition[2].GetPos().Item2 - 1) || IsOccupied(tempPosition[3].GetPos().Item1, tempPosition[3].GetPos().Item2 - 1));

                case "right":
                    return (IsOccupied(tempPosition[0].GetPos().Item1, tempPosition[0].GetPos().Item2 + 1) || IsOccupied(tempPosition[1].GetPos().Item1, tempPosition[1].GetPos().Item2 + 1) || IsOccupied(tempPosition[2].GetPos().Item1, tempPosition[2].GetPos().Item2 + 1) || IsOccupied(tempPosition[3].GetPos().Item1, tempPosition[3].GetPos().Item2 + 1));
            }
            return false;
        }
        public void PlaceBlock() {
            var tempPosition = currentPiece.GetPosition();
            for (int i = 0; i < 4; i++) {
                playboard[tempPosition[i].GetPos().Item1, tempPosition[i].GetPos().Item2] = '▓';
                lineBlockCount[tempPosition[i].GetPos().Item1]++;
            }
            currentPiece = new TetrisBlock("I-block"); //TEMPORARY
            if (CheckCollision("down", currentPiece.SimulatedBlockMove(-1)))
                gameOver = true;
        }

        private readonly string highScoreFile;
        public int Score { get; private set; }
        public int HighScore
        {
            get; private set;
        }

        public void ClearLines() {
            for (int i = 19; i >= 0; i--) {
                if (lineBlockCount[i] == 10) //TO-DO add score count
                {
                    for (int j = i; j < 23; j++) {
                        lineBlockCount[j] = lineBlockCount[j + 1];
                        for (int k = 0; k < 10; k++)
                            playboard[j, k] = playboard[j + 1, k];
                    }
                }
            }
        }
        private readonly int[] ScorePerLines = { 1, 40, 100, 300, 1200 };
        /*public void AddToScore(int level, int line)
        {
            this.Score += ScorePerLines[line] * level;
            if (this.Score > this.HighScore)
            {
                this.HighScore = this.Score;
            }
        }

        public void MoveTetrisBlock(string direction) {
            switch (direction) {
                case "down":
                    if (CheckCollision(direction,currentPiece.GetPosition()) == true)
                        PlaceBlock();
                    else currentPiece.MoveTetrisBlock(direction);
                    ClearLines();
                    break;
                default:
                    if (CheckCollision(direction,currentPiece.GetPosition()) == true) ;
                        //PlayStuckSound();
                    else currentPiece.MoveTetrisBlock(direction);
                    break;
            }
        }
        public void InstantPlaceBlock() {
            bool stuck = false;
            int checkLinesBelow = 0;
            while (stuck == false) {
                if (CheckCollision("down", currentPiece.SimulatedBlockMove(checkLinesBelow))) {
                    currentPiece.SkipMoveTetrisBlock(checkLinesBelow);
                    PlaceBlock();
                    stuck = true;
                }
                else checkLinesBelow++;
            }
        }
        public void IsGameOver() {
            if (gameOver == true) {
                gameInProgress = false;
                DrawGameOverScreen();
                StopGame();
                //TO-DO display score etc.
            }
        }
        private void DrawGameOverScreen() {
            drawboard[15, 0] = ' '; drawboard[15, 1] = ' '; drawboard[15, 2] = ' '; drawboard[15, 3] = 'G'; drawboard[15, 4] = 'A'; drawboard[15, 5] = 'M'; drawboard[15, 6] = 'E'; drawboard[15, 7] = ' '; drawboard[15, 8] = ' '; drawboard[15, 9] = ' ';
            drawboard[14, 0] = ' '; drawboard[14, 1] = ' '; drawboard[14, 2] = ' '; drawboard[14, 3] = 'O'; drawboard[14, 4] = 'V'; drawboard[14, 5] = 'E'; drawboard[14, 6] = 'R'; drawboard[14, 7] = ' '; drawboard[14, 8] = ' '; drawboard[14, 9] = ' ';
            drawboard[12, 0] = ' '; drawboard[12, 1] = ' '; drawboard[12, 2] = 'S'; drawboard[12, 3] = 'C'; drawboard[12, 4] = 'O'; drawboard[12, 5] = 'R'; drawboard[12, 6] = 'E'; drawboard[12, 7] = ':'; drawboard[12, 8] = ' '; drawboard[12, 9] = ' ';
            drawboard[11, 0] = ' '; drawboard[11, 1] = ' '; drawboard[11, 2] = '0'; drawboard[11, 3] = '0'; drawboard[11, 4] = '0'; drawboard[11, 5] = '0'; drawboard[11, 6] = '0'; drawboard[11, 7] = '0'; drawboard[11, 8] = ' '; drawboard[11, 9] = ' ';
            drawboard[7, 0] = ' '; drawboard[7, 1] = 'P'; drawboard[7, 2] = 'R'; drawboard[7, 3] = 'E'; drawboard[7, 4] = 'S'; drawboard[7, 5] = 'S'; drawboard[7, 6] = ' '; drawboard[7, 7] = ' '; drawboard[7, 8] = ' '; drawboard[7, 9] = ' ';
            drawboard[6, 0] = ' '; drawboard[6, 1] = 'E'; drawboard[6, 2] = 'N'; drawboard[6, 3] = 'T'; drawboard[6, 4] = 'E'; drawboard[6, 5] = 'R'; drawboard[6, 6] = ' '; drawboard[6, 7] = 'T'; drawboard[6, 8] = 'O'; drawboard[6, 9] = ' ';
            drawboard[5, 0] = ' '; drawboard[5, 1] = 'R'; drawboard[5, 2] = 'E'; drawboard[5, 3] = 'T'; drawboard[5, 4] = 'U'; drawboard[5, 5] = 'R'; drawboard[5, 6] = 'N'; drawboard[5, 7] = ' '; drawboard[5, 8] = 'T'; drawboard[5, 9] = 'O';
            drawboard[4, 0] = ' '; drawboard[4, 1] = 'M'; drawboard[4, 2] = 'A'; drawboard[4, 3] = 'I'; drawboard[4, 4] = 'N'; drawboard[4, 5] = ' '; drawboard[4, 6] = 'M'; drawboard[4, 7] = 'E'; drawboard[4, 8] = 'N'; drawboard[4, 9] = 'U';
        }
    }
}