using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TetrisGUI.Commands;
using TetrisGUI.Stores;

namespace TetrisGUI.ViewModel {
    class SettingsVM : ViewModelBase {
        public ICommand Return { get; }

        public SettingsVM(NavigationStore navigationStore, Func<ViewModelBase> createMainMenuViewModel) {
            Return = new NavigateCommand(navigationStore, createMainMenuViewModel);
        }
    }
}
