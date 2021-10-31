namespace TetrisMain.Models
{
    public class TetrisPlayboard
    {
        private readonly char[,] _playboard;
        private readonly char[,] _drawboard;

        private readonly TetrisBlock
            _currentPiece =
                new("J-block"); //placeholder current piece, TO-DO: add method to create current piece when last was placed on playboard

        private static TetrisPlayboard instance = null;

        private TetrisPlayboard()
        {
            _playboard = new char[24, 10];
            _drawboard = new char[24, 10];
            
            for (var i = 0; i < 24; i++)
                for (var j = 0; j < 10; j++)
                {
                    _playboard[i, j] = '.';
                    _drawboard[i, j] = ' ';
                }
        }

        public static TetrisPlayboard GetInstance()
        {
            if (instance == null) instance = new TetrisPlayboard();
            
            return instance;
        }

        public bool IsOccupied(int posx, int posy)
        {
            if (_playboard[posx, posy] != ' ')
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

        public void MoveTetrisBlock()
        {
            //TO-DO add collisions and block placement at the bottom of playboard
            _currentPiece.MoveTetrisBlock("down");
        }
    }
}