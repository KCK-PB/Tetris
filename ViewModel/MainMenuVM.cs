using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TetrisGUI.Commands;
using TetrisGUI.Stores;

namespace TetrisGUI.ViewModel {
    class MainMenuVM : ViewModelBase {
        public ICommand EnterGame { get; }
        public ICommand EnterSettings { get; }
        public ICommand EnterScoreboard { get; }
        public ICommand Exit { get; }

        public MainMenuVM(NavigationStore navigationStore, Func<ViewModelBase> createGameViewModel, Func<ViewModelBase> createScoreboardViewModel, Func<ViewModelBase> createSettingsViewModel) {
            EnterGame = new NavigateCommand(navigationStore, createGameViewModel);
            EnterScoreboard = new NavigateCommand(navigationStore, createScoreboardViewModel);
            EnterSettings = new NavigateCommand(navigationStore, createSettingsViewModel);
            Exit = new ExitApp();
        }

    }
}
