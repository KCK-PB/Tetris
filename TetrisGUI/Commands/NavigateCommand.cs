using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TetrisGUI.Stores;
using TetrisGUI.ViewModel;

namespace TetrisGUI.Commands {
    class NavigateCommand : CommandBase {
        private readonly NavigationStore navigationStore;
        private readonly Func<ViewModelBase> createViewModel;
        public NavigateCommand(NavigationStore navigationStore, Func<ViewModelBase> createViewModel) {
            this.navigationStore = navigationStore;
            this.createViewModel = createViewModel;
        }
        public override void Execute(object parameter) {
            navigationStore.currentViewModel = createViewModel();
        }
    }
}
