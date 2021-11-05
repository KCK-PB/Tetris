namespace TetrisMain.Timers {
    using System;
    using System.Timers;
    using Models;

    public class Timer {
        private static System.Timers.Timer aTimer;
        private static TetrisPlayboard playboard;
        public Timer(TetrisPlayboard plyboard) {
            playboard = plyboard;
            aTimer = new System.Timers.Timer(Math.Max(150,1100-(100*playboard.GetLevel())));
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = false;
        }

        public void DisableTimer() {
            aTimer.Enabled = false;
        }

        public void EnableTimer() {
            aTimer.Enabled = true;
        }
        public void LevelUpTimer() {
            aTimer.Interval = Math.Max(150, 1050 - (100 * playboard.GetLevel()));
        }
        private static void OnTimedEvent(Object source, ElapsedEventArgs e) { // Main game clock event, temporary implementation to test console drawing
            Console.Clear();
            playboard.MoveTetrisBlock("down");
            playboard.DrawBoard();
            playboard.IsGameOver();
            for (int i = 19; i >= 0; i--) {
                for (int j = 0; j < 10; j++) {
                    Console.Write(playboard.drawboard[i, j]);
                }
                Console.Write("\n");
            }
            Console.Out.Flush();
        }
    }
}