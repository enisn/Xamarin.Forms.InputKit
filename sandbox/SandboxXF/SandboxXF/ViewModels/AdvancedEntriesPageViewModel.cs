using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace SandboxXF.ViewModels
{
    public class AdvancedEntriesPageViewModel : INotifyPropertyChanged
    {
        private string _nameSurname;
        private string _email;
        private string _phone;

        public AdvancedEntriesPageViewModel()
        {
            SubmitCommand = new Command(Submit);
        }
        public Command SubmitCommand { get; set; }
        public bool IsValidated { get; set; }
        public string NameSurname { get => _nameSurname; set { _nameSurname = value; OnPropertyChanged(); } }
        public string Email { get => _email; set { _email = value; OnPropertyChanged(); } }
        public string Phone { get => _phone; set { _phone = value; OnPropertyChanged(); } }

        async void Submit()
        {
            if (!IsValidated)
            {
                await Application.Current.MainPage.DisplayAlert("", "You must fill all areas correctly!", "OK");
            }

            //DO SOME STUFFS HERE
        }

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        #endregion
    }
}
