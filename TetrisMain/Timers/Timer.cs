namespace TetrisMain.Timers
{
    using System;
    using System.Timers;
    using Models;
    
    public class Timer
    {
        private static System.Timers.Timer aTimer; //XD?
        private static TetrisPlayboard playboard = TetrisPlayboard.GetInstance();
        public Timer()
        {
            aTimer=new System.Timers.Timer(1000);
            aTimer.Elapsed+=OnTimedEvent;
            aTimer.AutoReset=true;
            aTimer.Enabled=true;
        }
        
        public void DisableTimer()
        {
            aTimer.Enabled=false;
        }

        public void EnableTimer()
        {
            aTimer.Enabled=true;
        }
        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        { // Main game clock event, temporary implementation to test console drawing
            Console.Clear();
            playboard.MoveTetrisBlock();
            playboard.DrawBoard();
            for(int i=23;i>=0;i--){
                for(int j=9;j>=0;j--){
                    //Console.Write(playboard.DrawBoard[i,j]); //Fajnie sie tam bawisz ziomek XD
                }
                Console.Write("\n");
            }
        }
    }
}