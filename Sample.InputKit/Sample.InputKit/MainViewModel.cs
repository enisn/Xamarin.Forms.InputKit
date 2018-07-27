using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace Sample.InputKit
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            FillData();
        }
        public IList<SampleClass> MyList { get; set; } = new ObservableCollection<SampleClass>();
        private SampleClass _selectedItem;
        private int _selectedIndex;

        public SampleClass SelectedItem
        {
            get => _selectedItem;
            set { _selectedItem = value; OnPropertyChanged(); }
        }

        void FillData()
        {
            for (int i = 0; i < 6; i++)
            {
                MyList.Add(new SampleClass { Id = i, Name = "Option " + (i + 1) });
            }
        }
        

  

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string propName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        #endregion
    }
}
