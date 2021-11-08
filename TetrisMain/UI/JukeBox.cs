using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using TetrisMain.Models;

namespace TetrisMain.UI {
    class JukeBox {
        private SoundPlayer[] music = new SoundPlayer[6];
        private string[] SFX = new string[15];
        private Settings settings = Settings.GetSettings();

        [DllImport("winmm.dll")]
        private static extern long mciSendString(string Cmd, StringBuilder StrReturn, int ReturnLength, IntPtr HwndCallback);
        public JukeBox() {
            for (int i = 0; i < 6; i++)
                music[i] = new SoundPlayer();
            music[0].SoundLocation = "SFX/mainmenumusic.wav";
            music[1].SoundLocation = "SFX/classictheme.wav";
            music[2].SoundLocation = "SFX/blockrushtheme.wav";
            music[3].SoundLocation = "SFX/gravitytheme.wav";
            music[4].SoundLocation = "SFX/highscoretheme.wav";
            music[5].SoundLocation = "SFX/gameover.wav";

            SFX[0] = "SFX/menuforward.wav";//
            SFX[1] = "SFX/singleline.wav";//
            SFX[2] = "SFX/doubleline.wav";//
            SFX[3] = "SFX/tripleline.wav";//
            SFX[4] = "SFX/tetraline.wav";//
            SFX[5] = "SFX/stuck.wav";//
            SFX[6] = "SFX/menuup.wav";//
            SFX[7] = "SFX/menudown.wav";//
            SFX[8] = "SFX/gameover.wav";//
            SFX[9] = "SFX/teleport.wav";//
            SFX[10] = "SFX/glitch1.wav";
            SFX[11] = "SFX/glitch2.wav";
            SFX[12] = "SFX/glitch3.wav";
            SFX[13] = "SFX/glitch4.wav";
            SFX[14] = "SFX/menuback.wav";
            for (int i = 0; i < 6; i++)
                music[i].Load();
        }
        public void PlaySound(int sound,long time) {
            if (!settings.wantsSFX)
                return;
            mciSendString("open \"" + SFX[sound] + "\" type waveaudio alias " + time.ToString(), null, 0, IntPtr.Zero) ;
            mciSendString("play "+time.ToString(), null, 0, IntPtr.Zero);
        }
        public void PlayMusic(int music) {
            if (settings.wantsMusic)
                this.music[music].PlayLooping();
        }
        public void PlayMusicOnce(int music) {
            if(settings.wantsMusic)
                this.music[music].Play();
        }
        public void StopMusic() {
            music[0].Stop();
        }
        public void StartMusic() {
            music[0].PlayLooping();
        }
    }
}
