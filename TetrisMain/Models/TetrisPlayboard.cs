namespace TetrisMain.Models {
    public class TetrisPlayboard {
        private char[,] playboard;
        public char[,] drawboard;
        private int[] lineBlockCount;
        private bool gameInProgress;
        private bool gameOver;
        private TetrisBlock currentPiece = new("J-block"); //placeholder current piece, TO-DO: add method to create current piece when last was placed on playboard
        private static TetrisPlayboard instance = null;

        private TetrisPlayboard() {
            playboard = new char[24, 10];
            drawboard = new char[24, 10];
            lineBlockCount = new int[24];
            gameInProgress = false;
            gameOver = false;
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
            if (instance == null)
                instance = new TetrisPlayboard();
            return instance;
        }
        public void StartGame() {
            gameInProgress = true;
        }
        public void StopGame() {
            gameInProgress = false;
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
                    this.Score++;
                    if (this.Score > this.HighScore)
                    {
                        this.HighScore = this.Score;
                    }
                    for (int j = i; j < 23; j++) {
                        lineBlockCount[j] = lineBlockCount[j + 1];
                        for (int k = 0; k < 10; k++)
                            playboard[j, k] = playboard[j + 1, k];
                    }
                }
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
            IsGameOver();
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
                //TO-DO display score etc.
            }

        }
    }
}