using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputKit.Maui.Test.TestClasses;

public class TestApp : Application
{
	ContentPage _withPage;
	TestWindow _window;

	public TestApp() : base()
	{

	}

	public TestApp(TestWindow window)
	{
		_window = window;
	}

	public TestWindow CreateWindow() =>
		(TestWindow)(this as IApplication).CreateWindow(null);

	protected override Window CreateWindow(IActivationState activationState)
	{
		return _window ?? new TestWindow(_withPage ?? new ContentPage());
	}

	public TestWindow CreateWindow(ContentPage withPage)
	{
		_withPage = withPage;
		return (TestWindow)(this as IApplication).CreateWindow(null);
	}
}
