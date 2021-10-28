using System;

class TetrisBlock{
    private string type;
    private Square[] tetrisBlock = new Square[4];
    public Square[] GetPosition(){
        return tetrisBlock;
    }
    public string GetType(){
        return type;
    }
    public TetrisBlock(string type){
        this.type=type;
        switch(type){
            case "Z-block":
                tetrisBlock[0]=new Square(21,4);
                tetrisBlock[1]=new Square(21,5);
                tetrisBlock[2]=new Square(20,5);
                tetrisBlock[3]=new Square(20,6);
            break;

            case "J-block":
                tetrisBlock[0]=new Square(22,2);
                tetrisBlock[1]=new Square(21,2);
                tetrisBlock[2]=new Square(20,1);
                tetrisBlock[3]=new Square(20,2);
            break;

            case "L-block":
                tetrisBlock[0]=new Square(22,4);
                tetrisBlock[1]=new Square(21,4);
                tetrisBlock[2]=new Square(20,4);
                tetrisBlock[3]=new Square(20,5);
            break;

            case "T-block":
                tetrisBlock[0]=new Square(21,4);
                tetrisBlock[1]=new Square(21,5);
                tetrisBlock[2]=new Square(21,6);
                tetrisBlock[3]=new Square(20,5);
            break;

            case "S-block":
                tetrisBlock[0]=new Square(21,5);
                tetrisBlock[1]=new Square(21,6);
                tetrisBlock[2]=new Square(20,4);
                tetrisBlock[3]=new Square(20,5);
            break;

            case "I-block":
                tetrisBlock[0]=new Square(23,4);
                tetrisBlock[1]=new Square(22,4);
                tetrisBlock[2]=new Square(21,4);
                tetrisBlock[3]=new Square(20,4);
            break;

            case "O-block":
                tetrisBlock[0]=new Square(21,4);
                tetrisBlock[1]=new Square(21,5);
                tetrisBlock[2]=new Square(20,4);
                tetrisBlock[3]=new Square(20,5);
            break;
        }
    }
    public void MoveTetrisBlock(string direction){
        bool allowMovement=true;
        switch(direction){
            case "left":
                for(int i=0;i<4;i++){
                    tetrisBlock[i].MovePos("left");
                }
            break;

            case "right":
                for(int i=0;i<4;i++){
                    tetrisBlock[i].MovePos("right");
                }
            break;

            case "down":
                for(int i=0;i<4;i++){
                    tetrisBlock[i].MovePos("down");
                }
            break;
        }
    }
    public void RotateTetrisBlock(string type){
    
    }
}