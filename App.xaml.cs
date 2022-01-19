using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TetrisGUI.Stores;
using TetrisGUI.ViewModel;
using TetrisMain.UI;

namespace TetrisGUI {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {

        private readonly NavigationStore navigationStore;
        public JukeBox jukeBox = new JukeBox();
        public App() {
            navigationStore = new NavigationStore();
        }
        protected override void OnStartup(StartupEventArgs e) {
            jukeBox.PlayMusic(0);
            navigationStore.currentViewModel = new MainMenuVM(navigationStore,CreateGameViewModel, CreateScoreboardViewModel, CreateSettingsViewModel);
            MainWindow = new MainWindow() {
                DataContext = new MainViewModel(navigationStore)
        };
            MainWindow.Show();
            base.OnStartup(e);
        }
        private ViewModelBase CreateGameViewModel() {
            return new GameVM(navigationStore, CreateMainMenuViewModel);
        }
        private ViewModelBase CreateScoreboardViewModel() {
            return new ScoreboardVM(navigationStore, CreateMainMenuViewModel);
        }
        private ViewModelBase CreateSettingsViewModel() {
            return new SettingsVM(navigationStore, CreateMainMenuViewModel);
        }
        private ViewModelBase CreateMainMenuViewModel() {
            return new MainMenuVM(navigationStore, CreateGameViewModel, CreateScoreboardViewModel, CreateSettingsViewModel);
        }
    }
    
}
