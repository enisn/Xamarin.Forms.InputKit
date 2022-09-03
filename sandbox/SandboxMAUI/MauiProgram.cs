﻿using InputKit.Handlers;
using InputKit.Shared.Controls;

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
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("MaterialIconsTwoTone-Regular.otf", "MaterialIcon");
			});

		builder.ConfigureMauiHandlers(handlers =>
		{
			handlers.AddInputKitHandlers();
		});

		SelectionView.GlobalSetting.CornerRadius = 0;

		return builder.Build();
	}
}
