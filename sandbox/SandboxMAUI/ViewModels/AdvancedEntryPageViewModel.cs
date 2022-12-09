using System;
using Microsoft.Maui.Controls;

namespace SandboxMAUI.ViewModels;

public class AdvancedEntryPageViewModel : BindableObject
{
    private string _nameSurname;
    private string _email;
    private string _phone;
    private bool isUserAgreementApproved;

    public AdvancedEntryPageViewModel()
    {
        SubmitCommand = new Command(Submit);
    }
    public Command SubmitCommand { get; set; }
    public bool IsValidated { get; set; }
    public string NameSurname { get => _nameSurname; set { _nameSurname = value; OnPropertyChanged(); } }
    public string Email { get => _email; set { _email = value; OnPropertyChanged(); } }
    public string Phone { get => _phone; set { _phone = value; OnPropertyChanged(); } }
    public bool IsUserAgreementApproved { get => isUserAgreementApproved; set { isUserAgreementApproved = value; OnPropertyChanged(); } }

    async void Submit()
    {
        if (!IsValidated)
        {
            await Application.Current.MainPage.DisplayAlert("", "Please fill all the fields correctly!", "OK");
            return;
        }

        await Application.Current.MainPage.DisplayAlert("Submitted Parameters",
            $"NameSurname: {NameSurname}\nEmail: {Email}\nPhone: {Phone}\nIsUserAgreementApproved: {IsUserAgreementApproved}",
            "CLOSE");

        //DO SOME STUFFS HERE
    }
}