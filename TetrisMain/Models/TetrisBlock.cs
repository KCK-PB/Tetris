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

        // TODO change Vector2Int for substitute
        // TODO change to work with substitute for Vector2Int
        // Block types could be enums

        public Vector2Int[,] JLSTZ_OFFSET_DATA {get; private set;}
        public Vector2Int[,] I_OFFSET_DATA {get; private set;}
        public Vector2Int[,] O_OFFSET_DATA {get; private set;}

        public int rotationIndex { get; private set; }

        private void SetOffsetData()
        {
            JLSTZ_OFFSET_DATA = new Vector2Int[5, 4];
            JLSTZ_OFFSET_DATA[0, 0] = Vector2Int.zero;
            JLSTZ_OFFSET_DATA[0, 1] = Vector2Int.zero;
            JLSTZ_OFFSET_DATA[0, 2] = Vector2Int.zero;
            JLSTZ_OFFSET_DATA[0, 3] = Vector2Int.zero;

            JLSTZ_OFFSET_DATA[1, 0] = Vector2Int.zero;
            JLSTZ_OFFSET_DATA[1, 1] = new Vector2Int(1,0);
            JLSTZ_OFFSET_DATA[1, 2] = Vector2Int.zero;
            JLSTZ_OFFSET_DATA[1, 3] = new Vector2Int(-1, 0);

            JLSTZ_OFFSET_DATA[2, 0] = Vector2Int.zero;
            JLSTZ_OFFSET_DATA[2, 1] = new Vector2Int(1, -1);
            JLSTZ_OFFSET_DATA[2, 2] = Vector2Int.zero;
            JLSTZ_OFFSET_DATA[2, 3] = new Vector2Int(-1, -1);

            JLSTZ_OFFSET_DATA[3, 0] = Vector2Int.zero;
            JLSTZ_OFFSET_DATA[3, 1] = new Vector2Int(0, 2);
            JLSTZ_OFFSET_DATA[3, 2] = Vector2Int.zero;
            JLSTZ_OFFSET_DATA[3, 3] = new Vector2Int(0, 2);

            JLSTZ_OFFSET_DATA[4, 0] = Vector2Int.zero;
            JLSTZ_OFFSET_DATA[4, 1] = new Vector2Int(1, 2);
            JLSTZ_OFFSET_DATA[4, 2] = Vector2Int.zero;
            JLSTZ_OFFSET_DATA[4, 3] = new Vector2Int(-1, 2);

            I_OFFSET_DATA = new Vector2Int[5, 4];
            I_OFFSET_DATA[0, 0] = Vector2Int.zero;
            I_OFFSET_DATA[0, 1] = new Vector2Int(-1, 0);
            I_OFFSET_DATA[0, 2] = new Vector2Int(-1, 1);
            I_OFFSET_DATA[0, 3] = new Vector2Int(0, 1);

            I_OFFSET_DATA[1, 0] = new Vector2Int(-1, 0);
            I_OFFSET_DATA[1, 1] = Vector2Int.zero;
            I_OFFSET_DATA[1, 2] = new Vector2Int(1, 1);
            I_OFFSET_DATA[1, 3] = new Vector2Int(0, 1);

            I_OFFSET_DATA[2, 0] = new Vector2Int(2, 0);
            I_OFFSET_DATA[2, 1] = Vector2Int.zero;
            I_OFFSET_DATA[2, 2] = new Vector2Int(-2, 1);
            I_OFFSET_DATA[2, 3] = new Vector2Int(0, 1);

            I_OFFSET_DATA[3, 0] = new Vector2Int(-1, 0);
            I_OFFSET_DATA[3, 1] = new Vector2Int(0, 1);
            I_OFFSET_DATA[3, 2] = new Vector2Int(1, 0);
            I_OFFSET_DATA[3, 3] = new Vector2Int(0, -1);

            I_OFFSET_DATA[4, 0] = new Vector2Int(2, 0);
            I_OFFSET_DATA[4, 1] = new Vector2Int(0, -2);
            I_OFFSET_DATA[4, 2] = new Vector2Int(-2, 0);
            I_OFFSET_DATA[4, 3] = new Vector2Int(0, 2);

            O_OFFSET_DATA = new Vector2Int[1, 4];
            O_OFFSET_DATA[0, 0] = Vector2Int.zero;
            O_OFFSET_DATA[0, 1] = Vector2Int.down;
            O_OFFSET_DATA[0, 2] = new Vector2Int(-1, -1);
            O_OFFSET_DATA[0, 3] = Vector2Int.left;
        }


        public void RotateTetrisBlock(bool clockwise, string type, bool shouldOffset)
        {
            int oldRotationIndex = rotationIndex;
            rotationIndex += clockwise ? 1 : -1;
            rotationIndex = CalculateModulo(rotationIndex, 4);

            foreach(var tile in _tetrisBlock)
            {
                tile.RotateTile(tile[0].coordinates, clockwise);
            }

            if (!shouldOffset) // Probably not needed, shouldOffset always true
            {
                return;
            }

            bool canOffset = Offset(oldRotationIndex, rotationIndex);

            if (!canOffset)
            {
                RotatePiece(!clockwise, false);
            }
        }

        private int CalculateModulo(int x, int m)
        {
            return(x % m + m) % m;
        }

        private void RotateTile(Vector2Int originPosition, bool clockwise)
        {
            Vector2Int relativePos = coordinates - originPos;
            Vector2Int[] rotMatrix = clockwise ? new Vector2Int[2] { new Vector2Int(0, -1), new Vector2Int(1, 0) }
                                            : new Vector2Int[2] { new Vector2Int(0, 1), new Vector2Int(-1, 0) };
            int newXPos = (rotMatrix[0].x * relativePos.x) + (rotMatrix[1].x * relativePos.y);
            int newYPos = (rotMatrix[0].y * relativePos.x) + (rotMatrix[1].y * relativePos.y);
            Vector2Int newPos = new Vector2Int(newXPos, newYPos);

            newPos += originPos;
            UpdatePosition(newPos);
        }

        private void UpdatePosition(Vector2Int newPos)
        {
            coordinates = newPos;
            Vector3 newV3Pos = new Vector3(newPos.x, newPos.y);
            gameObject.transform.position = newV3Pos;
        }

        private bool Offset(int oldRotIndex, int newRotIndex)
        {
            Vector2Int offsetVal1, offsetVal2, endOffset;
            Vector2Int[,] curOffsetData;
            
            if(type.Equals("O-block"))
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

            endOffset = Vector2Int.zero;

            bool movePossible = false;

            for (int testIndex = 0; testIndex < 5; testIndex++)
            {
                offsetVal1 = curOffsetData[testIndex, oldRotIndex];
                offsetVal2 = curOffsetData[testIndex, newRotIndex];
                endOffset = offsetVal1 - offsetVal2;
                if (CanMovePiece(endOffset))
                {
                    movePossible = true;
                    break;
                }
            }

            if (movePossible)
            {
                MovePiece(endOffset);
            }
            else
            {
                Debug.Log("Move impossible");
            }
            return movePossible;
        }

        private bool CanMovePiece(Vector2Int movement)
        {
            for (int i = 0; i < tiles.Length; i++)
            {
                if (!tiles[i].CanTileMove(movement + tiles[i].coordinates))
                {
                    return false;
                }
            }
            return true;
        }

        private bool CanTileMove(Vector2Int endPosition)
        {
            if (!BoardController.instance.IsInBounds(endPosition))
            {
                return false;
            }
            if (!BoardController.instance.IsPosEmpty(endPosition))
            {
                return false;
            }
            return true;
        }

        private bool IsInBounds(Vector2Int coordToTest)
        {
            if (coordToTest.x < 0 || coordToTest.x >= gridSizeX || coordToTest.y < 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private bool MovePiece(Vector2Int movement)
        {
            for (int i = 0; i < tiles.Length; i++)
            {
                if (!tiles[i].CanTileMove(movement + tiles[i].coordinates))
                {
                    Debug.Log("Cant Go there!");
                    if(movement == Vector2Int.down)
                    {
                        SetPiece();
                    }
                    return false;
                }
            }

            for(int i = 0; i< tiles.Length; i++)
            {
                tiles[i].MoveTile(movement);
            }

            return true;
        }
    }
}