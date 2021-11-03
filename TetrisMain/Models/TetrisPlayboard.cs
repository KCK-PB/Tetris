namespace TetrisMain.Models
{
    public class TetrisPlayboard
    {
        private char[,] _playboard;
        public char[,] _drawboard;
        private int[] _lineBlockCount;

        private TetrisBlock
            _currentPiece =
                new("J-block"); //placeholder current piece, TO-DO: add method to create current piece when last was placed on playboard

        private static TetrisPlayboard instance = null;

        private TetrisPlayboard()
        {
            _playboard = new char[24, 10];
            _drawboard = new char[24, 10];
            _lineBlockCount = new int[24];
            for (var i = 0; i < 24; i++)
                for (var j = 0; j < 10; j++)
                {
                    _playboard[i, j] = '.';
                    _drawboard[i, j] = ' ';
                }
            //TEMPORARY
            _lineBlockCount[0] = 8;
            for(int i=0;i<10;i++)
            {
                if (i == 4 || i == 5)
                    continue;
                _playboard[0, i]='O';
            }

            _lineBlockCount[3] = 8;
            for (int i = 0; i < 10; i++)
            {
                if (i == 4 || i == 5)
                    continue;
                _playboard[3, i] = 'O';
            }

            _lineBlockCount[6] = 8;
            for (int i = 0; i < 10; i++)
            {
                if (i == 4 || i == 5)
                    continue;
                _playboard[6, i] = 'O';
            }
            //TEMPORARY END
        }

        public static TetrisPlayboard GetInstance()
        {
            if (instance == null) instance = new TetrisPlayboard();
            
            return instance;
        }

        public bool IsOccupied(int posx, int posy)
        {
            if (posx<0||posy<0||posy>9||_playboard[posx, posy] != '.')
                return true;
            return false;
        }

        public void DrawBoard()
        {
            for (var i = 0; i < 24; i++)
                for (var j = 0; j < 10; j++)
                    _drawboard[i, j] = _playboard[i, j];
            
            var tempPosition = _currentPiece.GetPosition();
            
            for (var i = 0; i < 4; i++)
                _drawboard[tempPosition[i].GetPos().Item1, tempPosition[i].GetPos().Item2] = 'X';
        }
        public bool CheckCollision(string direction)
        {
            var tempPosition = _currentPiece.GetPosition();
            switch (direction)
            {
                case "down":
                    return (IsOccupied(tempPosition[0].GetPos().Item1-1, tempPosition[0].GetPos().Item2)|| IsOccupied(tempPosition[1].GetPos().Item1-1, tempPosition[1].GetPos().Item2)|| IsOccupied(tempPosition[2].GetPos().Item1-1, tempPosition[2].GetPos().Item2)|| IsOccupied(tempPosition[3].GetPos().Item1-1, tempPosition[3].GetPos().Item2));

                case "left":
                    return (IsOccupied(tempPosition[0].GetPos().Item1, tempPosition[0].GetPos().Item2-1) || IsOccupied(tempPosition[1].GetPos().Item1, tempPosition[1].GetPos().Item2-1) || IsOccupied(tempPosition[2].GetPos().Item1, tempPosition[2].GetPos().Item2-1) || IsOccupied(tempPosition[3].GetPos().Item1, tempPosition[3].GetPos().Item2-1));

                case "right":
                    return (IsOccupied(tempPosition[0].GetPos().Item1, tempPosition[0].GetPos().Item2+1) || IsOccupied(tempPosition[1].GetPos().Item1, tempPosition[1].GetPos().Item2+1) || IsOccupied(tempPosition[2].GetPos().Item1, tempPosition[2].GetPos().Item2+1) || IsOccupied(tempPosition[3].GetPos().Item1, tempPosition[3].GetPos().Item2+1));
            }
            return false;
        }
        public void PlaceBlock()
        {
            var tempPosition = _currentPiece.GetPosition();
            for (int i = 0; i < 4; i++)
            {
                _playboard[tempPosition[i].GetPos().Item1, tempPosition[i].GetPos().Item2]='O';
                _lineBlockCount[tempPosition[i].GetPos().Item1]++;
            }
            _currentPiece = new TetrisBlock("J-block"); //TEMPORARY
        }
        public void ClearLines()
        {
            for(int i = 19; i >= 0; i--)
            {
                if (_lineBlockCount[i] == 10) //TO-DO add score count
                {
                    for(int j = i; j < 23; j++)
                    {
                        _lineBlockCount[j] = _lineBlockCount[j + 1];
                        for (int k = 0; k < 10; k++)
                            _playboard[j,k] = _playboard[j + 1,k];
                    }
                }
            }
        }
        public void MoveTetrisBlock()
        {
            if (CheckCollision("down")==true)
                PlaceBlock();
            else _currentPiece.MoveTetrisBlock("down");
            ClearLines();
        }
    }
}