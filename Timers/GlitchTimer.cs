using System;
using System.Timers;
using System.Threading;
using TetrisMain.UI;
namespace TetrisMain.Timers {
    class GlitchTimer {
        private static System.Timers.Timer aTimer;
        static JukeBox jukeBox = new JukeBox();
        public GlitchTimer() {
            aTimer = new System.Timers.Timer(10000);
            aTimer.Elapsed += OnTimedEvent2;
            aTimer.AutoReset = true;
            aTimer.Enabled = false;
        }

        public void DisableTimer() {
            aTimer.Enabled = false;
            aTimer.Elapsed -= OnTimedEvent2;
            aTimer.Interval = 1000000;
        }

        public void EnableTimer() {
            aTimer.Enabled = true;
        }
        private static void OnTimedEvent2(Object source, ElapsedEventArgs e) { // Main game clock event, temporary implementation to test console drawing
            Random rnd = new Random();
            jukeBox.PlaySound(10 + rnd.Next(0, 4), DateTime.Now.Ticks);
            //lock (Program.GamePrinter.printerBlocker) {
            //    if (Program.GamePrinter.upsideDown)
            //        Program.GamePrinter.upsideDown = false;
            //    else Program.GamePrinter.upsideDown = true;
            //}

            aTimer.Interval = 1000 * (rnd.Next(4, 10));
        }
    }
}
