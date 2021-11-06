using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisMain.Models {
    class Settings {
        public bool wantsGhostPiece;
        public int selectedGameMode;
        public bool wantsMusic;
        public bool wantsSFX;
        public bool wantsAlternativeColorPallete;
        public char wantsGrid;
        public int startingLevel;
        public static Settings instance = new Settings();
        private Settings() {
            //TO-DO load settings from file
            wantsGhostPiece = true;
            selectedGameMode = 1;
            wantsMusic = true;
            wantsSFX = true;
            wantsGrid = ' ';
            wantsAlternativeColorPallete = false;
            startingLevel = 1;
    }
        public static Settings GetSettings() {
            return instance;
        }

    }
}
