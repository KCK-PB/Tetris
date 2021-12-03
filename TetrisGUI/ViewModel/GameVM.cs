using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TetrisGUI.Commands;
using TetrisGUI.Stores;

namespace TetrisGUI.ViewModel {
    class GameVM : ViewModelBase {
        public ICommand Return { get; }

        public GameVM(NavigationStore navigationStore,  Func<ViewModelBase> createMainMenuViewModel) {
            Return = new NavigateCommand(navigationStore, createMainMenuViewModel);
        }
    }
}
