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
        public void RotateTetrisBlock(string type) {
        }
        public static TetrisBlock GetRandomBlock() {
            Random rnd = new Random();
            int selection = rnd.Next(1, 8);
            switch (selection) {
                case 1:
                    return new TetrisBlock("Z-block");
                case 2:
                    return new TetrisBlock("J-block");
                case 3:
                    return new TetrisBlock("L-block");
                case 4:
                    return new TetrisBlock("T-block");
                case 5:
                    return new TetrisBlock("S-block");
                case 6:
                    return new TetrisBlock("I-block");
                case 7:
                    return new TetrisBlock("O-block");
            }
            return new TetrisBlock("unknown");
        }
    }
}