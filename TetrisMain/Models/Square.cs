namespace TetrisMain.Models {
    using System;

    public class Square {
        private int posx;
        private int posy;

        public Square(int posx, int posy) {
            this.posx = posx;
            this.posy = posy;
        }

        public Tuple<int, int> GetPos() {
            return new Tuple<int, int>(posx, posy);
        }
        
        public void MovePos(string direction, int lines = 1) {
            //bottom=table[0][x] left=table[x][0] right=table[x][9] top=table[23][x]
            switch (direction) {
                case "left":
                    posy--;
                    break;

                case "right":
                    posy++;
                    break;

                case "down":
                    posx-=lines;
                    break;
            }
        }

        //public Tuple<int, int> coordinates;

        // Rotate tile around it's center position
        // Center position is always tetrisBlock[0]
        public void RotateTile(Tuple<int, int> centerTile, bool clockwise)
        {
            Tuple<int, int> relativePosition = new Tuple<int, int>((GetPos().Item1 - centerTile.Item1), (GetPos().Item2 - centerTile.Item2));

            // Setting rotation matrix according to the clockwise parameter
            Tuple<int, int>[] rotationMatrix = clockwise ? new Tuple<int, int>[2] { new Tuple<int, int>(0, -1), new Tuple<int, int>(1, 0) }
                                                         : new Tuple<int, int>[2] { new Tuple<int, int>(0, 1), new Tuple<int, int>(-1, 0) };

            // Calculating new position by multiplying rootMatrix by realtive position
            int newXPos = (rotationMatrix[0].Item1 * relativePosition.Item1) + (rotationMatrix[1].Item1 * relativePosition.Item2);
            int newYPos = (rotationMatrix[0].Item2 * relativePosition.Item1) + (rotationMatrix[1].Item2 * relativePosition.Item2);

            //Tuple<int, int> newPos = new Tuple<int, int>(newXPos, newYPos);
            //newPos += originPosition;
            Tuple<int, int> newPosition = new Tuple<int, int>(newXPos += centerTile.Item1, newYPos += centerTile.Item2);

            UpdatePosition(newPosition);
        }

        // Update position of single tile
        private void UpdatePosition(Tuple<int, int> newPos)
        {
            //coordinates = newPos;
            //Tuple<int, int> newV3Pos = new Tuple<int, int>(newPos.Item1, newPos.Item2);
            //gameObject.transform.position = newV3Pos;
            posx = newPos.Item1;
            posy = newPos.Item2;
        }

        public bool CanTileMove(Tuple<int, int> endPosition)
        {

            if (!IsInBounds(endPosition))
            {
                return false;
            }
            if (!IsPosEmpty(endPosition))
            {
                return false;
            }
            return true;
        }

        // May be moved to other class
        private bool IsInBounds(Tuple<int, int> coordToTest)
        {
            // gridSizeX = 10 ?
            int gridSizeX = 10;
            if (coordToTest.Item1 < 0 || coordToTest.Item1 >= gridSizeX || coordToTest.Item2 < 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // May be moved to other class
        public bool IsPosEmpty(Tuple<int, int> coordToTest)
        {
            if (coordToTest.Item2 >= 20)
            {
                return true;
            }
            // TODO GridUnit[,] fullGrid should changed for a substitute
            //TetrisPlayboard.IsOccupied()???
            TetrisPlayboard tetrisPlayboard = TetrisPlayboard.GetInstance();
            if(tetrisPlayboard.IsOccupied(coordToTest.Item1, coordToTest.Item2))
            {
                return false;
            }
            //if (fullGrid[coordToTest.Item1, coordToTest.Item2].isOccupied)
            //{
            //    return false;
            //}
            else
            {
                return true;
            }
        }
        // May be swapped for substitute
        public void MoveTile(Tuple<int, int> movement)
        {
            Tuple<int, int> endPos = new(GetPos().Item1 + movement.Item1, GetPos().Item2 + movement.Item2);
            UpdatePosition(endPos);
        }
    }
}