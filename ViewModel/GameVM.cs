using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TetrisGUI.Commands;
using TetrisGUI.Stores;
using TetrisMain.Models;

namespace TetrisGUI.ViewModel {
    public class GameVM : ViewModelBase {
        public ICommand Return { get; }
        public List<List<SolidColorBrush>> drawBoard = new List<List<SolidColorBrush>>();
        public List<List<SolidColorBrush>> DrawBoard {
            get {
                return drawBoard;
            }
        }
        public GameVM(NavigationStore navigationStore,  Func<ViewModelBase> createMainMenuViewModel) {
            Return = new NavigateCommand(navigationStore, createMainMenuViewModel);
            
            TetrisPlayboard tetrisPlayboard = TetrisPlayboard.GetInstance();
            tetrisPlayboard = tetrisPlayboard.Reset();
            tetrisPlayboard.gameClock.LevelUpTimer();
            tetrisPlayboard.StartGame();
            int changeStamp = 0;
            CreateDrawboard();
            //while(tetrisPlayboard.IsGameInProgress()) {
            //    if (changeStamp != tetrisPlayboard.changeStamp) {
            //        changeStamp = tetrisPlayboard.changeStamp;
            //        GetDrawboard();
            //    }
            //}
        }
        public void CreateDrawboard() {
            for(int i = 0; i < 20; i++) {
                drawBoard.Add(new List<SolidColorBrush>());
                for(int j = 0; j < 10; j++) {
                    drawBoard[i].Add(new SolidColorBrush(Colors.Black));
                }
            }
        }
        private void GetDrawboard() {
            for (int i = 0; i < 20; i++) {
                for (int j = 0; j < 10; j++) {
                    drawBoard[i][j].Color=Colors.White;
                    OnPropertyChanged("DrawBoard[" + i + "][" + j + "]");
                }
            }
        }
    }
}
