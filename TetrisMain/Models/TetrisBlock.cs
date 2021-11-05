using System;
namespace TetrisMain.Models {
    public class TetrisBlock {
        private string type;
        private Square[] tetrisBlock; 

        public Square[] GetPosition() {
            return tetrisBlock;
        }

        public string GetBlockType() {
            return type;
        }

        // Center tile for each block may need to be assigned
        public TetrisBlock(string type) {
            this.type = type;
            tetrisBlock = new Square[4];
            switch (type) {
                case "Z-block":
                    tetrisBlock[0] = new Square(21, 4);
                    tetrisBlock[1] = new Square(21, 5);
                    tetrisBlock[2] = new Square(20, 5);
                    tetrisBlock[3] = new Square(20, 6);
                    break;

                case "J-block":
                    tetrisBlock[0] = new Square(22, 5);
                    tetrisBlock[1] = new Square(21, 5);
                    tetrisBlock[2] = new Square(20, 4);
                    tetrisBlock[3] = new Square(20, 5);
                    break;

                case "L-block":
                    tetrisBlock[0] = new Square(22, 4);
                    tetrisBlock[1] = new Square(21, 4);
                    tetrisBlock[2] = new Square(20, 4);
                    tetrisBlock[3] = new Square(20, 5);
                    break;

                case "T-block":
                    tetrisBlock[0] = new Square(21, 4);
                    tetrisBlock[1] = new Square(21, 5);
                    tetrisBlock[2] = new Square(21, 6);
                    tetrisBlock[3] = new Square(20, 5);
                    break;

                case "S-block":
                    tetrisBlock[0] = new Square(21, 5);
                    tetrisBlock[1] = new Square(21, 6);
                    tetrisBlock[2] = new Square(20, 4);
                    tetrisBlock[3] = new Square(20, 5);
                    break;

                case "I-block":
                    tetrisBlock[0] = new Square(23, 4);
                    tetrisBlock[1] = new Square(22, 4);
                    tetrisBlock[2] = new Square(21, 4);
                    tetrisBlock[3] = new Square(20, 4);
                    break;

                case "O-block":
                    tetrisBlock[0] = new Square(21, 4);
                    tetrisBlock[1] = new Square(21, 5);
                    tetrisBlock[2] = new Square(20, 4);
                    tetrisBlock[3] = new Square(20, 5);
                    break;
                default:
                    tetrisBlock[0] = new Square(20, 1);
                    tetrisBlock[1] = new Square(20, 3);
                    tetrisBlock[2] = new Square(20, 6);
                    tetrisBlock[3] = new Square(20, 10);
                    break;
            }
        }

        public void MoveTetrisBlock(string direction) {
            switch (direction) {
                case "left":
                    for (var i = 0; i < 4; i++) tetrisBlock[i].MovePos("left");
                    break;

                case "right":
                    for (var i = 0; i < 4; i++) tetrisBlock[i].MovePos("right");
                    break;

                case "down":
                    for (var i = 0; i < 4; i++) tetrisBlock[i].MovePos("down");
                    break;
            }
        }
        public Square[] SimulatedBlockMove(int lines) {
            Square[] virtualTetrisBlock = new Square[4];
            for (var i = 0; i < 4; i++) {
                Tuple<int, int> tempPos = tetrisBlock[i].GetPos();
                virtualTetrisBlock[i] = new Square(tempPos.Item1, tempPos.Item2);
                virtualTetrisBlock[i].MovePos("down", lines);
            }
            return virtualTetrisBlock;
        }
        public void SkipMoveTetrisBlock(int lines) {
            for (var i = 0; i < 4; i++) tetrisBlock[i].MovePos("down", lines);
        }

        // TODO change to work with substitute for Vector2Int
        // Block types could be enums

        // By default Tuple<int,int> item1 is X-coordinate
        // By default Tuple<int,int> item2 is Y-coordinate

        public Tuple<int, int>[,] JLSTZ_OFFSET_DATA {get; private set;}

        public Tuple<int, int>[,] I_OFFSET_DATA {get; private set;}
        public Tuple<int, int>[,] O_OFFSET_DATA {get; private set;}

        // should be 0 at the start for each new block???
        public int rotationIndex = 0;

        // Offset data
        // May be declared as constants or on the start of the program
        private void SetOffsetData()
        {
            JLSTZ_OFFSET_DATA = new Tuple<int, int>[5,4];
            JLSTZ_OFFSET_DATA[0, 0] = Tuple.Create(0,0);
            JLSTZ_OFFSET_DATA[0, 1] = Tuple.Create(0,0);
            JLSTZ_OFFSET_DATA[0, 2] = Tuple.Create(0,0);
            JLSTZ_OFFSET_DATA[0, 3] = Tuple.Create(0,0);

            JLSTZ_OFFSET_DATA[1, 0] = Tuple.Create(0, 0);
            JLSTZ_OFFSET_DATA[1, 1] = Tuple.Create(1, 0);
            JLSTZ_OFFSET_DATA[1, 2] = Tuple.Create(0, 0);
            JLSTZ_OFFSET_DATA[1, 3] = Tuple.Create(-1, 0);

            JLSTZ_OFFSET_DATA[2, 0] = Tuple.Create(0, 0);
            JLSTZ_OFFSET_DATA[2, 1] = Tuple.Create(1, -1);
            JLSTZ_OFFSET_DATA[2, 2] = Tuple.Create(0, 0);
            JLSTZ_OFFSET_DATA[2, 3] = Tuple.Create(-1, -1);

            JLSTZ_OFFSET_DATA[3, 0] = Tuple.Create(0, 0);
            JLSTZ_OFFSET_DATA[3, 1] = Tuple.Create(0, 2);
            JLSTZ_OFFSET_DATA[3, 2] = Tuple.Create(0, 0);
            JLSTZ_OFFSET_DATA[3, 3] = Tuple.Create(0, 2);

            JLSTZ_OFFSET_DATA[4, 0] = Tuple.Create(0, 0);
            JLSTZ_OFFSET_DATA[4, 1] = Tuple.Create(1, 2);
            JLSTZ_OFFSET_DATA[4, 2] = Tuple.Create(0, 0);
            JLSTZ_OFFSET_DATA[4, 3] = Tuple.Create(-1, 2);

            I_OFFSET_DATA = new Tuple<int, int>[5, 4];
            I_OFFSET_DATA[0, 0] = Tuple.Create(0, 0);
            I_OFFSET_DATA[0, 1] = Tuple.Create(-1, 0);
            I_OFFSET_DATA[0, 2] = Tuple.Create(-1, 1);
            I_OFFSET_DATA[0, 3] = Tuple.Create(0, 1);

            I_OFFSET_DATA[1, 0] = Tuple.Create(-1, 0);
            I_OFFSET_DATA[1, 1] = Tuple.Create(0, 0);
            I_OFFSET_DATA[1, 2] = Tuple.Create(1, 1);
            I_OFFSET_DATA[1, 3] = Tuple.Create(0, 1);

            I_OFFSET_DATA[2, 0] = Tuple.Create(2, 0);
            I_OFFSET_DATA[2, 1] = Tuple.Create(0, 0);
            I_OFFSET_DATA[2, 2] = Tuple.Create(-2, 1);
            I_OFFSET_DATA[2, 3] = Tuple.Create(0, 1);

            I_OFFSET_DATA[3, 0] = Tuple.Create(-1, 0);
            I_OFFSET_DATA[3, 1] = Tuple.Create(0, 1);
            I_OFFSET_DATA[3, 2] = Tuple.Create(1, 0);
            I_OFFSET_DATA[3, 3] = Tuple.Create(0, -1);

            I_OFFSET_DATA[4, 0] = Tuple.Create(2, 0);
            I_OFFSET_DATA[4, 1] = Tuple.Create(0, -2);
            I_OFFSET_DATA[4, 2] = Tuple.Create(-2, 0);
            I_OFFSET_DATA[4, 3] = Tuple.Create(0, 2);

            O_OFFSET_DATA = new Tuple<int, int>[1, 4];
            O_OFFSET_DATA[0, 0] = Tuple.Create(0, 0);
            O_OFFSET_DATA[0, 1] = Tuple.Create(0, -1);
            O_OFFSET_DATA[0, 2] = Tuple.Create(-1, 1);
            O_OFFSET_DATA[0, 3] = Tuple.Create(-1, 0);
        }

        // Perform rotation by changing position of each tile and offsetting it's position
        // May add tetris block type later
        public void RotateTetrisBlock(bool clockwise, bool shouldOffset)
        {
            int oldRotationIndex = rotationIndex;
            rotationIndex += clockwise ? 1 : -1;
            rotationIndex = CalculateModulo(rotationIndex, 4);

            for (int i = 0; i < tetrisBlock.Length; i++)
            {
                tetrisBlock[i].RotateTile(tetrisBlock[0].GetPos(), clockwise);
            }

            if (!shouldOffset)
            {
                return;
            }

            bool canOffset = Offset(oldRotationIndex, rotationIndex);

            if (!canOffset)
            {
                RotateTetrisBlock(!clockwise, false);
            }
        }

        private int CalculateModulo(int x, int m)
        {
            return(x % m + m) % m;
        }

        private bool Offset(int oldRotationIndex, int newRotationIndex)
        {
            Tuple<int, int> offsetVal1, offsetVal2, endOffset;
            Tuple<int, int>[,] curOffsetData;

            if (type.Equals("O-block"))
            {
                curOffsetData = O_OFFSET_DATA;
            }
            else if(type.Equals("I-block"))
            {
                curOffsetData = I_OFFSET_DATA;
            }
            else
            {
                curOffsetData = JLSTZ_OFFSET_DATA;
            }

            endOffset = new Tuple<int, int>(0, 0);

            bool movePossible = false;

            for (int testIndex = 0; testIndex < 5; testIndex++)
            {
                offsetVal1 = curOffsetData[testIndex, oldRotationIndex];
                offsetVal2 = curOffsetData[testIndex, newRotationIndex];
                endOffset = Tuple.Create(offsetVal1.Item1 - offsetVal2.Item1, offsetVal1.Item2 - offsetVal2.Item2);
                if (CanMovePiece(endOffset))
                {
                    movePossible = true;
                    break;
                }
            }

            if (movePossible)
            {
                // TODO assign endOffset
                MovePiece(endOffset);
            }
            else
            {
                //Debug.Log("Move impossible");
            }
            return movePossible;
        }

        private bool CanMovePiece(Tuple<int, int> movement)
        {
            for (int i = 0; i < tetrisBlock.Length; i++)
            {
                Tuple<int, int> destinationPosition = new Tuple<int, int>(movement.Item1 + tetrisBlock[i].GetPos().Item1, movement.Item2 + tetrisBlock[i].GetPos().Item2);
                if (!tetrisBlock[i].CanTileMove(destinationPosition))
                {
                    return false;
                }
            }
            return true;
        }

        // May be moved to Square.cs later
        public bool MovePiece(Tuple<int, int> movement)
        {
            for (int i = 0; i < tetrisBlock.Length; i++)
            {
                Tuple<int, int> destinationPosition = new Tuple<int, int>(movement.Item1 + tetrisBlock[i].GetPos().Item1, movement.Item2 + tetrisBlock[i].GetPos().Item2);
                if (!tetrisBlock[i].CanTileMove(destinationPosition))
                {
                    // Debug.Log("Cant Go there!");
                    // movement == (0, -1)
                    if (movement.Item1 == 0 && movement.Item1 == -1)
                    {
                        // TODO SetPiece() Shoud be changed for a substitute
                        // TetrisPlayboard.PlaceBlock(); ?
                        //SetPiece();
                    }
                    return false;
                }
            }

            for(int i = 0; i< tetrisBlock.Length; i++)
            {
                // TODO Should be adjusted after MovePiece adjustments
                tetrisBlock[i].MoveTile(movement);
            }

            return true;
        }
    }
}