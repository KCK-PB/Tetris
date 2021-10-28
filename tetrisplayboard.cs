using System;

class TetrisPlayboard{
    private char[,] playboard;
    public char[,] drawboard;
    private TetrisBlock currentPiece=new TetrisBlock("J-block");//placeholder current piece, TO-DO: add method to create current piece when last was placed on playboard
    private static TetrisPlayboard instance=null;
    private TetrisPlayboard(){
        playboard =  new char[24,10];
        drawboard =  new char[24,10];
        for(int i=0;i<24;i++){
            for(int j=0;j<10;j++){
                playboard[i,j]='.';
                drawboard[i,j]=' ';
            }
        }
    }
    public static TetrisPlayboard GetInstance(){
        if(instance==null){
            instance=new TetrisPlayboard();
        }
        return instance;
    }
    public bool IsOccupied(int posx,int posy){
        if(playboard[posx,posy]!=' ')
            return true;
        return false;
    }
    public void DrawBoard(){
        for(int i=0;i<24;i++){
            for(int j=0;j<10;j++){
                drawboard[i,j]=playboard[i,j];
            }
        }
        Square[] tempPosition=currentPiece.GetPosition();
        for(int i=0;i<4;i++){
            drawboard[tempPosition[i].GetPos().Item1,tempPosition[i].GetPos().Item2]='X';
        }
    }
    public void MoveTetrisBlock(){ //TO-DO add collisions and block placement at the bottom of playboard
        currentPiece.MoveTetrisBlock("down");
    }
}