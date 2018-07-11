using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace Sample.InputKit
{
    class MainViewModel : INotifyPropertyChanged
    {
        private bool _ıisCheckedRB;

        public bool IsCheckedRB { get => _ıisCheckedRB;
            set
            {
                _ıisCheckedRB = value;
                Debug.WriteLine("Triggered");
            }
        }

        private double _price;

        public double Price
        {
            get { return _price; }
            set { _price = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
