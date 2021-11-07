using System;
using System.Timers;
using System.Threading;

namespace TetrisMain.Timers {
    class GlitchTimer {
        private static System.Timers.Timer aTimer;
        public GlitchTimer() {
            aTimer = new System.Timers.Timer(1000);
            aTimer.Elapsed += OnTimedEvent2;
            aTimer.AutoReset = true;
            aTimer.Enabled = false;
        }

        public void DisableTimer() {
            aTimer.Enabled = false;
        }

        public void EnableTimer() {
            aTimer.Enabled = true;
        }
        private static void OnTimedEvent2(Object source, ElapsedEventArgs e) { // Main game clock event, temporary implementation to test console drawing
            Random rnd = new Random();
            
        }
    }
}
