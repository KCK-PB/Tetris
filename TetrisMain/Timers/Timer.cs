namespace TetrisMain.Timers {
    using System;
    using System.Timers;
    using Models;

    public class Timer {
        private static System.Timers.Timer aTimer;
        private static TetrisPlayboard playboard = TetrisPlayboard.GetInstance();
        public Timer() {
            aTimer = new System.Timers.Timer(333);
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        public void DisableTimer() {
            aTimer.Enabled = false;
        }

        public void EnableTimer() {
            aTimer.Enabled = true;
        }
        private static void OnTimedEvent(Object source, ElapsedEventArgs e) { // Main game clock event, temporary implementation to test console drawing
            Console.Clear();
            playboard.MoveTetrisBlock("down");
            playboard.DrawBoard();
            for (int i = 23; i >= 0; i--) {
                for (int j = 0; j < 10; j++) {
                    Console.Write(playboard.drawboard[i, j]);
                }
                Console.Write("\n");
            }
            Console.Out.Flush();
        }
    }
}