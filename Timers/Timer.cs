namespace TetrisMain.Timers {
    using System;
    using System.Timers;
    using Models;
    using System.Threading;

    public class Timer {
        private static System.Timers.Timer aTimer;
        private static TetrisPlayboard playboard;
        public Timer(TetrisPlayboard plyboard) {
            playboard = plyboard;
            aTimer = new System.Timers.Timer(Math.Max(150, 1100 - (100 * playboard.GetLevel())));
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
            lock (playboard) {
                playboard.MoveTetrisBlock("down", playboard.GetBlock());
                playboard.IsGameOver();
            }
        }
    }
}