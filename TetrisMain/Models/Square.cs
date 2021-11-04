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
    }
}