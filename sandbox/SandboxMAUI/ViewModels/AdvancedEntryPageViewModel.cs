using System;
using Microsoft.Maui.Controls;

namespace SandboxMAUI.ViewModels;

public class AdvancedEntryPageViewModel : BindableObject
{
    private string _nameSurname;
    private string _email;
    private string _phone;

    public AdvancedEntryPageViewModel()
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
            await Application.Current.MainPage.DisplayAlert("", "You successfully submitted the form", "OK");
        }

        //DO SOME STUFFS HERE
    }
}