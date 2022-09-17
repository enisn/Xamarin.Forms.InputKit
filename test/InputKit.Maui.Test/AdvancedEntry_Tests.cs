using InputKit.Maui.Test.TestClasses;
using InputKit.Shared.Controls;
using Shouldly;
using System.Windows.Input;

namespace InputKit.Maui.Test;
public class AdvancedEntry_Tests
{
	public AdvancedEntry_Tests()
	{
        ApplicationExtensions.CreateAndSetMockApplication();
    }

    [Fact]
    public void Text_BindingForInitializtion_FromSource()
    {
        var control = new AdvancedEntry();
        var viewModel = new TestViewModel { Text = "Text Initial Value" };
        control.BindingContext = viewModel;
        control.SetBinding(AdvancedEntry.TextProperty, new Binding(nameof(TestViewModel.Text)));

        // Assert
        control.Text.ShouldBe(viewModel.Text);
    }

    [Fact]
    public void Text_Binding_FromSource()
    {
        var control = new AdvancedEntry();
        var viewModel = new TestViewModel { Text = "Text Initial Value" };
        control.BindingContext = viewModel;
        control.SetBinding(AdvancedEntry.TextProperty, new Binding(nameof(TestViewModel.Text)));

        // Act
        viewModel.Text = "Changed Value";

        // Assert
        control.Text.ShouldBe(viewModel.Text);
    }

    public class TestViewModel : InputKitBindableObject
    {
        private bool isChecked;
        private string text;

        public bool IsChecked { get => isChecked; set => SetProperty(ref isChecked, value); }

        public string Text { get => text; set => SetProperty(ref text, value); }

        public ICommand Command { get; set; }

        public object CommandParameter { get; set; } = "My Command Parameter 1";
    }
}
