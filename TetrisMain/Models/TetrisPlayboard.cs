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
        private static TetrisPlayboard instance=null;
        private static Object instanceLock = new Object();
        private Settings settings;
        private int clearedLines;
        private static readonly int[] linesPerLevel = { 10, 30, 60, 70, 120, 180, 250, 330, 420, 520, 620, 720, 820, 920, 1020, 1120, 1220, 1320, 1420, 9999 };
        //private static readonly int[] linesPerLevel = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };
        private int score;
        private int highScore;
        private char blockSymbol;
        private int blockRushCounter;
        

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
                    playboard[i, j] = settings.wantsGrid;
                    drawboard[i, j] = ' ';
                }
            nextBlock = TetrisBlock.GetRandomBlock();
            currentBlock = TetrisBlock.GetRandomBlock();
            blockSymbol = currentBlock.GetBlockType()[0];
            highScore = Program.scoreboard.GetHighScore();
            if (settings.wantsGhostPiece == true)
                UpdateGhostPiece();
        }
        public TetrisBlock GetBlock() {
            return currentBlock;
        }
        public int GetLevel() {
            return level;
        }
        private void LevelUp() {
            if (level >= 20)
                return;
            level++;
            gameClock.LevelUpTimer();
            RenderLevel();
        }
        public static TetrisPlayboard GetInstance() {
            if (instance == null) {
                lock (instanceLock) {
                    if (instance == null) {
                        instance = new TetrisPlayboard();
                    }
                }
            }
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
            if (posx < 0 || posy < 0 || posy > 9 || (playboard[posx, posy] != ' '&& playboard[posx, posy] != '.')) {
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
        public void PlaceBlock(TetrisBlock blockToPlace, bool enemyBlock=false) {
            var tempPosition = blockToPlace.GetPosition();
            if (enemyBlock)
                blockSymbol = 'G';
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
                if (lineBlockCount[i] == 10) 
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
            Render();
            RenderLines();
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
            while (clearedLines > linesPerLevel[level])
                LevelUp();
            blockRushCounter += tempClearedLines;
            if (blockRushCounter >= 5 && settings.selectedGameMode==2) {
                blockRushCounter -= 5;
                GenerateAndPlaceRandomBlock();
            }
        }
        private readonly int[] ScorePerLines = { 1, 40, 100, 300, 1200 };
        public void AddToScore(int line) {
            this.score += ScorePerLines[line] * level;
            if (this.score > this.highScore) {
                this.highScore = this.score;
                RenderScore("best");
            }
            RenderScore("normal");
        }

        public int GetCurrentScore()
        {
            return this.score;
        }

        public void RotateAndUpdate() {
            lock (this) {
                RotateTetrisBlock();
                Render();
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
            if (CheckCollision("here", blockClone.GetPosition())) {
                //playStuckSound();
                return;
            }
            currentBlock = blockClone.GetClone();
        }

        public void MoveTetrisBlock(string direction, TetrisBlock blockToMove) {
            switch (direction) {
                case "down":
                    if (CheckCollision(direction, blockToMove.GetPosition()) == true)
                        PlaceBlock(currentBlock);
                    else blockToMove.MoveTetrisBlock(direction);
                    ClearLines();
                    break;
                default:
                    lock (this) {
                        if (CheckCollision(direction, blockToMove.GetPosition()) == true) ;
                        //PlayStuckSound();
                        else {
                            blockToMove.MoveTetrisBlock(direction);
                            Render();
                        }
                        break;
                    }
            }
        }
        public void InstantPlaceBlock(TetrisBlock blockToPlace) {
            bool animation = false;
            if (blockToPlace != currentBlock)
                animation = true;
            bool stuck = false;
            int checkLinesBelow = 0;
            while (stuck == false) {
                if (CheckCollision("down", blockToPlace.SimulatedBlockMove(checkLinesBelow))) {
                    blockToPlace.SkipMoveTetrisBlock(checkLinesBelow);
                    PlaceBlock(blockToPlace,animation);
                    stuck = true;
                }
                else checkLinesBelow++;
            }
            if (animation) {
                gameClock.DisableTimer();
                RenderBlockRushAnimation(blockToPlace);
                gameClock.EnableTimer();
            }
            ClearLines();
            score += 5; //adding 5 score for faster,riskier gameplay
            if (score > highScore)
                highScore = score;
            RenderScore("best");
            RenderScore("normal");
        }
        public void IsGameOver() {
            if (gameOver == true) {
                gameInProgress = false;
                RenderGameOver();
                StopGame();
                //TO-DO display score etc.
            }
        }
        private void CycleNextBlock() {
            currentBlock = nextBlock;
            nextBlock = TetrisBlock.GetRandomBlock();
            RenderNextPiece();
            RenderBlockCount();
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
        private void GenerateAndPlaceRandomBlock() {
            Random rnd = new Random();
            int repeats = rnd.Next(1, 4);
            for (int i = 0; i < repeats; i++) 
                RotateTetrisBlock();
            TetrisBlock enemyBlock = currentBlock.GetClone();
            CycleNextBlock();
            for(int i = 0; i < 4 - repeats; i++) 
                RotateTetrisBlock();
            repeats = rnd.Next(0, 5);
            if (rnd.Next(1, 2) % 2 == 0) {
                for (int i = 0; i < repeats; i++)
                    MoveTetrisBlock("left", enemyBlock);
            }
            else {
                for (int i = 0; i < repeats; i++)
                    MoveTetrisBlock("right", enemyBlock);
            }
            InstantPlaceBlock(enemyBlock);
            //playblockrushspawnsound() WORMS TELEPORT.WAV SOUND
        }

        private void Render() {
            if (settings.wantsGhostPiece == true)
                UpdateGhostPiece();
            DrawBoard();
            Program.GamePrinter.PrintInExactPlace(drawboard);
        }
        public void RenderScore(string which) {
            if(which=="best") 
                Program.GamePrinter.PrintScore(highScore, 4);
            else Program.GamePrinter.PrintScore(score, 7);
        }
        private void RenderLevel() {
            Program.GamePrinter.PrintLevel(level);
        }
        private void RenderLines() {
            Program.GamePrinter.PrintLines(clearedLines);
        }
        private void RenderGameOver() {
            Program.GamePrinter.PrintGameOver();
        }
        public void RenderNextPiece() {
            Program.GamePrinter.PrintNextPiece(nextBlock.GetBlockType());
        }
        public void RenderBlockCount() {
            Program.GamePrinter.PrintBlockCount(currentBlock.GetBlockType());
        }
        public void RenderBlockRushAnimation(TetrisBlock positions) {
            Program.GamePrinter.PrintBlockRushAnimation(positions);
        }
        public TetrisPlayboard Reset() {
            instance = new TetrisPlayboard();
            return instance;
        }
    }
    
}