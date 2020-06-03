﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Timers;
using Akka.Actor;
using System;

namespace Minesweeper.WPF
{
    
    public class MineMapViewModel : INotifyPropertyChanged, IMineMapViewModel
    {
        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        ActorSystem System { get; }
        IActorRef Actor { get; }
        public int _colCount { get; set; }
        public int _rowCount { get; set; }
        public int BombCount { get; private set; }
        public string ResetContent { get; set; } = "map Reset";
        public string _boombStatue { get; set; } = "Collapsed";
        public string _successStatue { get; set; } = "Collapsed";
        public DelegateCommand ResetCommand { get; set; }
        private string _enableButton { get; set; }
        public MineMap MineMap { get; set; }    
        public ObservableCollection<MineItemViewModel> MineItemViewModels { get; set; } = new ObservableCollection<MineItemViewModel>();
        public int ColCount
        {
            get
            {
                return _colCount;
            }
            set
            {
                _colCount = value;
                OnPropertyChanged();
            }
        }
        public int RowCount
        {
            get
            {
                return _rowCount;
            }
            set
            {
                _rowCount = value;
                OnPropertyChanged();
            }
        }
        public string BoombStatue
        {
            get
            {
                return _boombStatue;
            }
            set
            {
                _boombStatue = value;
                OnPropertyChanged(nameof(BoombStatue));
            }
        }
        public string SuccessStatue
        {
            get
            {
                return _successStatue;
            }
            set
            {
                _successStatue = value;
                OnPropertyChanged(nameof(SuccessStatue));
            }
        }
        public string EnableButton
        {
            get
            {
                return _enableButton;
            }
            set
            {
                _enableButton = value;
                OnPropertyChanged(nameof(EnableButton));
            }
        }
        //public MineMapViewModel(ActorSystem system, Props props)
        public MineMapViewModel(ActorSystem system, Func<IMineMapViewModel, Props> propsFunc)
        {
            System = system;
            var props = propsFunc?.Invoke(this);
            Actor = System?.ActorOf(props);

            RowCount = 5;
            ColCount = 5;
            BombCount = 4;
            MineMap = new MineMap(RowCount, ColCount);  
            ResetCommand = new DelegateCommand( _ => ResetAction());
        }
        public void PrepareGame()
        {
            MineMap.GenerateBombs(BombCount);
            MineMap.GenerateCountNearBombs();
            CreateMineItemViewModels();
        }
        public void CreateMineItemViewModels()
        {
            MineItemViewModels.Clear();

            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColCount; j++)
                {
                    int x = j;
                    int y = i;
                    MineItemViewModels.Add(new MineItemViewModel(MineMap.MineItems[i, j], () =>
                    {
                        Click(y, x);
                    }));
                }
            }
        }
        private void Click(int y, int x)
        {
            MineMap.Click(y, x);
            CheckEndGame();
            CreateMineItemViewModels();
        }
        private void CheckEndGame()
        {
            int ItemCount = (RowCount * ColCount) - BombCount;

            if (MineMap.CheckEndGame())
            {
                for (int i = 0; i < RowCount; i++)
                {
                    for (int j = 0; j < ColCount; j++)
                    {

                        if (this.MineMap.MineItems[i, j].IsCovered == false)
                        {
                            ItemCount--;
                            if (this.MineMap.MineItems[i, j].IsBomb == true)
                            {
                                BoombStatue = "Visible";
                                EnableButton = "false";
                                ShowMap();
                                return;
                            }
                        }

                        if (ItemCount == 0)
                        {
                            SuccessStatue = "Visible";
                            EnableButton = "false";
                            ShowMap();
                            return;
                        }

                    }
                }
            }
        }
        private void ShowMap()
        {
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColCount; j++)
                {
                    this.MineMap.MineItems[i, j].IsCovered = false;
                }
            }
        }
        private void ResetAction()
        {
            int row = RowCount;
            int col = ColCount;
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    MineMap.MineItems[i, j] = null;
                    MineMap = new MineMap(row, col);
                }
            }

            PrepareGame();
            EnableButton = "true";
            BoombStatue = "Hidden";
            SuccessStatue = "Hidden";

        }
    }
}