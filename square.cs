using System;

class Square{
    private int posx;
    private int posy;
    public Square(int posx, int posy){
        this.posx=posx;
        this.posy=posy;
    }
    public Tuple<int,int> GetPos(){
        return new Tuple<int, int>(posx,posy);
    }
    public void MovePos(string direction){ //bottom=table[0][x] left=table[x][0] right=table[x][9] top=table[23][x]
        switch(direction){
            case "left":
                this.posy--;
            break;

            case "right":
                this.posy++;
            break;

            case "down":
                this.posx--;
            break;
        }
    }
}