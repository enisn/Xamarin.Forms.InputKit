using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SandboxMAUI.ViewModels;
public class SampleItemsViewModel : INotifyPropertyChanged
{
    public IList<SampleItem> Items { get; set; } = new ObservableCollection<SampleItem>();
    private SampleItem _selectedItem;

    public SampleItemsViewModel()
    {
        FillData();
    }

    public SampleItem SelectedItem
    {
        get => _selectedItem;
        set { _selectedItem = value; OnPropertyChanged(); }
    }

    void FillData()
    {
        for (int i = 0; i < 4; i++)
        {
            Items.Add(new SampleItem { Id = i, Name = "Option " + (i + 1) });
        }

        Items[2].IsDisabled = true;
    }

    #region INotifyPropertyChanged Implementation
    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string propName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    #endregion
}