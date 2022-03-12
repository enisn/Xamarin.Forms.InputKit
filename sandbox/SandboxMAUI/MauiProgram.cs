using InputKit.Handlers;

namespace SandboxMAUI;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.ConfigureMauiHandlers(handlers =>
		{
			handlers.AddInputKitHandlers();
		});

		return builder.Build();
	}
}
