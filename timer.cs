using System;
using System.Timers;

class Timer{
    private static System.Timers.Timer aTimer;
    private static TetrisPlayboard playboard = TetrisPlayboard.GetInstance();
    public Timer(){
        aTimer=new System.Timers.Timer(1000);
        aTimer.Elapsed+=OnTimedEvent;
        aTimer.AutoReset=true;
        aTimer.Enabled=true;
    }
    public void DisableTimer(){
        aTimer.Enabled=false;
    }

    public void EnableTimer(){
        aTimer.Enabled=true;
    }
    private static void OnTimedEvent(Object source, ElapsedEventArgs e){ // Main game clock event, temporary implementation to test console drawing
        Console.Clear();
        //Console.WriteLine("timer");
        playboard.MoveTetrisBlock();
        //Console.WriteLine("test0");
        playboard.DrawBoard();
        //Console.WriteLine("test1");
        for(int i=23;i>=0;i--){
            for(int j=9;j>=0;j--){
                Console.Write(playboard.drawboard[i,j]);
                //Console.Write(j);
            }
            Console.Write("\n");
        }
        //Console.WriteLine("test2");
        aTimer.Enabled=false;
    }
}