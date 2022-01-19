using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisMain.Models {
    public class Settings 
    {
        private readonly string SettingFile;

        public bool wantsGhostPiece { get; set; }
        public string selectedGameMode { get; set; }
        public bool wantsMusic { get; set; }
        public bool wantsSFX { get; set; }
        public bool wantsAlternativeColorPallete { get; set; }
        public char wantsGrid { get; set; }
        public int startingLevel { get; set; }
        public static Settings instance = new Settings();
        private Settings() {
            //TO-DO load settings from file
            wantsGhostPiece = true;
            selectedGameMode = "Classic";
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
