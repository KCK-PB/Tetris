using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TetrisGUI.ViewModel;

namespace TetrisGUI.Stores {
    class NavigationStore {
        private ViewModelBase _currentViewModel;
        public ViewModelBase currentViewModel {
            get => _currentViewModel;
            set {
                _currentViewModel = value;
                OnCurrentViewModelChanged();
            }
        }
        public event Action CurrentViewModelChanged;
        public void OnCurrentViewModelChanged() {
            CurrentViewModelChanged?.Invoke();
        }
    }
}
