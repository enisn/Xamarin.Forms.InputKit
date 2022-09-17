using InputKit.Handlers;
using InputKit.Maui.Test.TestClasses;
using Shouldly;
using System.Windows.Input;
using CheckBox = InputKit.Shared.Controls.CheckBox;

namespace InputKit.Maui.Test;

public class CheckBox_Tests
{
    public CheckBox_Tests()
    {
        ApplicationExtensions.CreateAndSetMockApplication();
    }

    [Fact]
    public void IsChecked_Binding_ToSource()
    {
        var control = AnimationReadyHandler.Prepare(new CheckBox());
        var viewModel = new TestViewModel();
        control.BindingContext = viewModel;
        control.SetBinding(CheckBox.IsCheckedProperty, new Binding(nameof(TestViewModel.IsChecked)));

        // Act
        control.IsChecked = true;

        // Assert
        viewModel.IsChecked.ShouldBeTrue();
    }

    [Fact]
    public void IsChecked_Binding_FromSource()
    {
        var control = AnimationReadyHandler.Prepare(new CheckBox());
        var viewModel = new TestViewModel();
        control.BindingContext = viewModel;
        control.SetBinding(CheckBox.IsCheckedProperty, new Binding(nameof(TestViewModel.IsChecked)));

        // Act
        viewModel.IsChecked = true;

        // Assert
        viewModel.IsChecked.ShouldBeTrue();
    }

    [Fact]
    public void Text_BindingForInitializtion_FromSource()
    {
        var control = AnimationReadyHandler.Prepare(new CheckBox());
        var viewModel = new TestViewModel { Text = "Text Initial Value" };
        control.BindingContext = viewModel;
        control.SetBinding(CheckBox.TextProperty, new Binding(nameof(TestViewModel.Text)));

        // Assert
        control.Text.ShouldBe(viewModel.Text);
    }

    [Fact]
    public void Text_Binding_FromSource()
    {
        var control = AnimationReadyHandler.Prepare(new CheckBox());
        var viewModel = new TestViewModel { Text = "Text Initial Value" };
        control.BindingContext = viewModel;
        control.SetBinding(CheckBox.TextProperty, new Binding(nameof(TestViewModel.Text)));

        // Act
        viewModel.Text = "Changed Value";

        // Assert
        control.Text.ShouldBe(viewModel.Text);
    }

    [Fact]
    public void CheckChangedCommand_ShouldBeExecuted_InSource()
    {
        var viewModel = new TestViewModel();
        var isCommandExecuted = false;
        viewModel.Command = new Command(() =>
        {
            isCommandExecuted = true;
        });

        var control = AnimationReadyHandler.Prepare(new CheckBox());
        control.SetBinding(CheckBox.CheckChangedCommandProperty, new Binding(nameof(TestViewModel.Command)));

        // Act
        control.IsChecked = true;

        // Assert
        isCommandExecuted.ShouldBeTrue();
    }

    [Fact]
    public void CommandParameter_ShouldBePassed_ToSource()
    {
        var viewModel = new TestViewModel();
        viewModel.CommandParameter = "My Custom Parameter";
        object incomingCommandParameter = null;

        viewModel.Command = new Command((parameter) =>
        {
            incomingCommandParameter = parameter;
        });

        var control = AnimationReadyHandler.Prepare(new CheckBox());
        control.SetBinding(CheckBox.CheckChangedCommandProperty, new Binding(nameof(TestViewModel.Command)));
        control.SetBinding(CheckBox.CommandParameterProperty, new Binding(nameof(TestViewModel.Command)));

        // Act
        control.IsChecked = true;

        // Assert
        incomingCommandParameter.ShouldNotBeNull();
        viewModel.CommandParameter.ShouldBe(incomingCommandParameter);
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