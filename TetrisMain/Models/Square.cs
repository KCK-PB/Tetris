namespace TetrisMain.Models
{
    using System;
    
    public class Square
    {
        private int _posx;
        private int _posy;

        public Square(int posx, int posy)
        {
            this._posx = posx;
            this._posy = posy;
        }

        public Tuple<int, int> GetPos()
        {
            return new Tuple<int, int>(_posx, _posy);
        }

        public void MovePos(string direction)
        {
            //bottom=table[0][x] left=table[x][0] right=table[x][9] top=table[23][x]
            switch (direction)
            {
                case "left":
                    _posy--;
                    break;

                case "right":
                    _posy++;
                    break;

                case "down":
                    _posx--;
                    break;
            }
        }
    }
}