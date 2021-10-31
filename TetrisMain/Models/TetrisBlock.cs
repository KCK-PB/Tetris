namespace TetrisMain.Models
{
    public class TetrisBlock
    {
        private readonly string type;
        private readonly Square[] _tetrisBlock = new Square[4];

        public Square[] GetPosition()
        {
            return _tetrisBlock;
        }

        public string GetType()
        {
            return type;
        }

        public TetrisBlock(string type)
        {
            this.type = type;
            switch (type)
            {
                case "Z-block":
                    _tetrisBlock[0] = new Square(21, 4);
                    _tetrisBlock[1] = new Square(21, 5);
                    _tetrisBlock[2] = new Square(20, 5);
                    _tetrisBlock[3] = new Square(20, 6);
                    break;

                case "J-block":
                    _tetrisBlock[0] = new Square(22, 2);
                    _tetrisBlock[1] = new Square(21, 2);
                    _tetrisBlock[2] = new Square(20, 1);
                    _tetrisBlock[3] = new Square(20, 2);
                    break;

                case "L-block":
                    _tetrisBlock[0] = new Square(22, 4);
                    _tetrisBlock[1] = new Square(21, 4);
                    _tetrisBlock[2] = new Square(20, 4);
                    _tetrisBlock[3] = new Square(20, 5);
                    break;

                case "T-block":
                    _tetrisBlock[0] = new Square(21, 4);
                    _tetrisBlock[1] = new Square(21, 5);
                    _tetrisBlock[2] = new Square(21, 6);
                    _tetrisBlock[3] = new Square(20, 5);
                    break;

                case "S-block":
                    _tetrisBlock[0] = new Square(21, 5);
                    _tetrisBlock[1] = new Square(21, 6);
                    _tetrisBlock[2] = new Square(20, 4);
                    _tetrisBlock[3] = new Square(20, 5);
                    break;

                case "I-block":
                    _tetrisBlock[0] = new Square(23, 4);
                    _tetrisBlock[1] = new Square(22, 4);
                    _tetrisBlock[2] = new Square(21, 4);
                    _tetrisBlock[3] = new Square(20, 4);
                    break;

                case "O-block":
                    _tetrisBlock[0] = new Square(21, 4);
                    _tetrisBlock[1] = new Square(21, 5);
                    _tetrisBlock[2] = new Square(20, 4);
                    _tetrisBlock[3] = new Square(20, 5);
                    break;
            }
        }

        public void MoveTetrisBlock(string direction)
        {
            var allowMovement = true;
            switch (direction)
            {
                case "left":
                    for (var i = 0; i < 4; i++) _tetrisBlock[i].MovePos("left");
                    break;

                case "right":
                    for (var i = 0; i < 4; i++) _tetrisBlock[i].MovePos("right");
                    break;

                case "down":
                    for (var i = 0; i < 4; i++) _tetrisBlock[i].MovePos("down");
                    break;
            }
        }

        public void RotateTetrisBlock(string type)
        {
        }
    }
}