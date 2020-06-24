using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sample.InputKit.ViewModels
{
    public class RadioButtonsViewModel : INotifyPropertyChanged
    {
        private Command<int> _SelectionChangedLocalCommand;
        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        #endregion

        public Command<int> SelectionChangedLocalCommand
        {
            get => _SelectionChangedLocalCommand;
            set
            {
                _SelectionChangedLocalCommand = value;
                OnPropertyChanged(nameof(SelectionChangedLocalCommand));
            }
        }

        public RadioButtonsViewModel()
        {
            SelectionChangedLocalCommand = new Command<int>(async (n) => await NotifyStuff(n));
        }

        private async Task NotifyStuff(int n)
        {
            System.Diagnostics.Debug.WriteLine("Current Selected Index = " + n);
            await Task.Delay(1000);
        }
    }
}
