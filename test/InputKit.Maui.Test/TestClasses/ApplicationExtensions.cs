namespace InputKit.Maui.Test.TestClasses;

static class ApplicationExtensions
{
	public static Window LoadPage(this Application app, Page page)
	{
		app.MainPage = page;

		return ((IApplication)app).CreateWindow(null) as Window;
	}

	public static void CreateAndSetMockApplication()
	{
		var appBuilder = MauiApp.CreateBuilder()
								.UseMauiApp<MockApplication>();
		var mauiApp = appBuilder.Build();
		var application = mauiApp.Services.GetRequiredService<IApplication>();
		application.Handler = new ApplicationHandlerStub();
		application.Handler.SetMauiContext(new HandlersContextStub(mauiApp.Services));
	}

}