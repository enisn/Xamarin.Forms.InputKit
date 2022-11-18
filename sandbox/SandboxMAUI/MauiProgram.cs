using InputKit.Handlers;
using InputKit.Shared.Controls;
using UraniumUI;

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
				fonts.AddFont("OpenSansRegular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSansSemibold.ttf", "OpenSansSemibold");
				fonts.AddMaterialIconFonts();
			});

		builder.ConfigureMauiHandlers(handlers =>
		{
			handlers.AddInputKitHandlers();
		});

		SelectionView.GlobalSetting.CornerRadius = 0;

		return builder.Build();
	}
}
