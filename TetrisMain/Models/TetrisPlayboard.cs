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
        private TetrisBlock currentBlock;
        private TetrisBlock nextBlock;
        private TetrisBlock ghostPiece;
        private int level;
        private static readonly TetrisPlayboard instance = new TetrisPlayboard();
        private Settings settings;
        private int clearedLines;
        private static readonly int[] linesPerLevel = { 10, 30, 60, 70, 120, 180, 250, 330, 420, 520, 620, 720, 820, 920, 1020, 1120, 1220, 1320, 1420 };
        //private static readonly int[] linesPerLevel = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
        private int score;
        private int highScore;
        private char blockSymbol;

        private TetrisPlayboard() {
            settings = Settings.GetSettings();
            playboard = new char[24, 10];
            drawboard = new char[24, 10];
            lineBlockCount = new int[24];
            clearedLines = 0;
            gameInProgress = false;
            gameOver = false;
            gameClock = new Timers.Timer(this);
            level = settings.startingLevel;
            for (var i = 0; i < 24; i++)
                for (var j = 0; j < 10; j++) {
                    playboard[i, j] = ' ';
                    drawboard[i, j] = ' ';
                }
            nextBlock = TetrisBlock.GetRandomBlock();
            CycleNextBlock();
            if (settings.wantsGhostPiece == true)
                UpdateGhostPiece();
        }
        public int GetLevel() {
            return level;
        }
        private void LevelUp() {
            if (level >= 20)
                return;
            level++;
            gameClock.LevelUpTimer();
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
            if (posx < 0 || posy < 0 || posy > 9 || playboard[posx, posy] != ' ') {
                Console.WriteLine(posx + " " + posy);

                return true;

            }
            return false;
        }

        public void DrawBoard() {
            for (var i = 0; i < 24; i++)
                for (var j = 0; j < 10; j++)
                    drawboard[i, j] = playboard[i, j];
            Square[] tempPosition;
            if (settings.wantsGhostPiece == true) {
                tempPosition = ghostPiece.GetPosition();//ghost piece render
                for (var i = 0; i < 4; i++)
                    drawboard[tempPosition[i].GetPos().Item1, tempPosition[i].GetPos().Item2] = '░';
            }

            tempPosition = currentBlock.GetPosition();//current block render
            for (var i = 0; i < 4; i++)
                drawboard[tempPosition[i].GetPos().Item1, tempPosition[i].GetPos().Item2] = blockSymbol;
        }
        public bool CheckCollision(string direction, Square[] tempPosition) {
            switch (direction) {
                case "down":
                    return (IsOccupied(tempPosition[0].GetPos().Item1 - 1, tempPosition[0].GetPos().Item2) || IsOccupied(tempPosition[1].GetPos().Item1 - 1, tempPosition[1].GetPos().Item2) || IsOccupied(tempPosition[2].GetPos().Item1 - 1, tempPosition[2].GetPos().Item2) || IsOccupied(tempPosition[3].GetPos().Item1 - 1, tempPosition[3].GetPos().Item2));

                case "left":
                    return (IsOccupied(tempPosition[0].GetPos().Item1, tempPosition[0].GetPos().Item2 - 1) || IsOccupied(tempPosition[1].GetPos().Item1, tempPosition[1].GetPos().Item2 - 1) || IsOccupied(tempPosition[2].GetPos().Item1, tempPosition[2].GetPos().Item2 - 1) || IsOccupied(tempPosition[3].GetPos().Item1, tempPosition[3].GetPos().Item2 - 1));

                case "right":
                    return (IsOccupied(tempPosition[0].GetPos().Item1, tempPosition[0].GetPos().Item2 + 1) || IsOccupied(tempPosition[1].GetPos().Item1, tempPosition[1].GetPos().Item2 + 1) || IsOccupied(tempPosition[2].GetPos().Item1, tempPosition[2].GetPos().Item2 + 1) || IsOccupied(tempPosition[3].GetPos().Item1, tempPosition[3].GetPos().Item2 + 1));

                case "here":
                    return (IsOccupied(tempPosition[0].GetPos().Item1, tempPosition[0].GetPos().Item2) || IsOccupied(tempPosition[1].GetPos().Item1, tempPosition[1].GetPos().Item2) || IsOccupied(tempPosition[2].GetPos().Item1, tempPosition[2].GetPos().Item2) || IsOccupied(tempPosition[3].GetPos().Item1, tempPosition[3].GetPos().Item2));
            }
            return false;
        }
        public void PlaceBlock() {
            var tempPosition = currentBlock.GetPosition();
            for (int i = 0; i < 4; i++) {
                playboard[tempPosition[i].GetPos().Item1, tempPosition[i].GetPos().Item2] = blockSymbol;
                lineBlockCount[tempPosition[i].GetPos().Item1]++;
            }
            CycleNextBlock();
            if (CheckCollision("down", currentBlock.SimulatedBlockMove(-1)))
                gameOver = true;
        }


        public void ClearLines() {
            int tempClearedLines = 0;
            for (int i = 19; i >= 0; i--) {
                if (lineBlockCount[i] == 10) //TO-DO add score count
                {
                    tempClearedLines++;
                    clearedLines++;
                    for (int j = i; j < 23; j++) {
                        lineBlockCount[j] = lineBlockCount[j + 1];
                        for (int k = 0; k < 10; k++)
                            playboard[j, k] = playboard[j + 1, k];
                    }
                }
            }
            if (settings.wantsGhostPiece == true)
                UpdateGhostPiece();
            switch (tempClearedLines) {
                case 1:
                    AddToScore(1);
                    break;
                case 2:
                    AddToScore(2);
                    break;
                case 3:
                    AddToScore(3);
                    break;
                case 4:
                    AddToScore(4);
                    break;
                default:
                    return;
            }
            if (clearedLines > linesPerLevel[level])
                LevelUp();
        }
        private readonly int[] ScorePerLines = { 1, 40, 100, 300, 1200 };
        public void AddToScore(int line) {
            this.score += ScorePerLines[line] * level;
            if (this.score > this.highScore) {
                this.highScore = this.score;
            }
        }
        public void RotateAndUpdate() {
            lock (this) {
                RotateTetrisBlock();
                if (settings.wantsGhostPiece == true)
                    UpdateGhostPiece();
            }
        }
        private void RotateTetrisBlock() {
            TetrisBlock blockClone = currentBlock.GetClone();
            blockClone.RotateTetrisBlock(true, true);
            TetrisBlock rotatedClone = blockClone.GetClone();
            if (CheckCollision("here", blockClone.GetPosition())) {
                if (blockClone.GetBlockType().Equals("I-block")) {
                    blockClone.MoveTetrisBlock("up");
                    if (CheckCollision("here", blockClone.GetPosition())) {
                        blockClone.MoveTetrisBlock("up");
                        if (CheckCollision("here", blockClone.GetPosition())) {
                            blockClone = rotatedClone.GetClone();
                        }
                        else {
                            currentBlock = blockClone.GetClone();
                            return;
                        }
                    }
                    else {
                        currentBlock = blockClone.GetClone();
                        return;
                    }
                    blockClone.MoveTetrisBlock("left");
                    if (CheckCollision("here", blockClone.GetPosition())) {
                        blockClone.MoveTetrisBlock("left");
                        if (CheckCollision("here", blockClone.GetPosition())) {
                            blockClone = rotatedClone.GetClone();
                        }
                        else {
                            currentBlock = blockClone.GetClone();
                            return;
                        }
                    }
                    else {
                        currentBlock = blockClone.GetClone();
                        return;
                    }
                    blockClone.MoveTetrisBlock("right");
                    if (CheckCollision("here", blockClone.GetPosition())) {
                        blockClone.MoveTetrisBlock("right");
                        if (CheckCollision("here", blockClone.GetPosition())) {
                            blockClone = rotatedClone.GetClone();
                        }
                        else {
                            currentBlock = blockClone.GetClone();
                            return;
                        }
                    }
                    else {
                        currentBlock = blockClone.GetClone();
                        return;
                    }
                }
                else {
                    blockClone.MoveTetrisBlock("up");
                    if (CheckCollision("here", blockClone.GetPosition())) {
                        blockClone = rotatedClone.GetClone();
                    }
                    else {
                        currentBlock = blockClone.GetClone();
                        return;
                    }
                    blockClone.MoveTetrisBlock("left");
                    if (CheckCollision("here", blockClone.GetPosition())) {
                        blockClone = rotatedClone.GetClone();
                    }
                    else {
                        currentBlock = blockClone.GetClone();
                        return;
                    }
                    blockClone.MoveTetrisBlock("right");
                    if (CheckCollision("here", blockClone.GetPosition())) {
                        blockClone = rotatedClone.GetClone();
                    }
                    else {
                        currentBlock = blockClone.GetClone();
                        return;
                    }
                }
            }
            currentBlock = blockClone.GetClone();
        }

        public void MoveTetrisBlock(string direction) {
            switch (direction) {
                case "down":
                    if (CheckCollision(direction, currentBlock.GetPosition()) == true)
                        PlaceBlock();
                    else currentBlock.MoveTetrisBlock(direction);
                    ClearLines();
                    break;
                default:
                    lock (this) {
                        if (CheckCollision(direction, currentBlock.GetPosition()) == true) ;
                        //PlayStuckSound();
                        else {
                            currentBlock.MoveTetrisBlock(direction);
                            if (settings.wantsGhostPiece == true)
                                UpdateGhostPiece();
                        }
                        break;
                    }
            }
        }
        public void InstantPlaceBlock() {
            bool stuck = false;
            int checkLinesBelow = 0;
            while (stuck == false) {
                if (CheckCollision("down", currentBlock.SimulatedBlockMove(checkLinesBelow))) {
                    currentBlock.SkipMoveTetrisBlock(checkLinesBelow);
                    PlaceBlock();
                    stuck = true;
                }
                else checkLinesBelow++;
            }
            score += 5; //adding 5 score for faster,riskier gameplay
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
        private void CycleNextBlock() {
            currentBlock = nextBlock;
            nextBlock = TetrisBlock.GetRandomBlock();
            blockSymbol = '█';
            if (currentBlock.GetBlockType() == "Z-block")
                blockSymbol = 'Z';
            else if (currentBlock.GetBlockType() == "J-block")
                blockSymbol = 'J';
            else if (currentBlock.GetBlockType() == "L-block")
                blockSymbol = 'L';
            else if (currentBlock.GetBlockType() == "T-block")
                blockSymbol = 'T';
            else if (currentBlock.GetBlockType() == "S-block")
                blockSymbol = 'S';
            else if (currentBlock.GetBlockType() == "I-block")
                blockSymbol = 'I';
            else if (currentBlock.GetBlockType() == "O-block")
                blockSymbol = 'O';
            if (settings.wantsGhostPiece == true)
                UpdateGhostPiece();
        }
        private void UpdateGhostPiece() {
            ghostPiece = currentBlock.GetClone();
            bool stuck = false;
            int checkLinesBelow = 0;
            while (stuck == false) {
                if (CheckCollision("down", currentBlock.SimulatedBlockMove(checkLinesBelow))) {
                    ghostPiece.SkipMoveTetrisBlock(checkLinesBelow);
                    stuck = true;
                }
                else checkLinesBelow++;
            }
        }
        private void GenerateAndPlaceRandomBlock(int number) {

        }
    }
}