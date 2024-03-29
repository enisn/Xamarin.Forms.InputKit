﻿using Microsoft.Maui.Handlers;

namespace InputKit.Maui.Test.TestClasses;

class WindowHandlerStub : ElementHandler<IWindow, object>
{
	public static IPropertyMapper<IWindow, WindowHandlerStub> Mapper =
		new PropertyMapper<IWindow, WindowHandlerStub>(ElementMapper)
		{
		};

	public static CommandMapper<IWindow, WindowHandlerStub> CommandMapper =
		new CommandMapper<IWindow, WindowHandlerStub>(ElementCommandMapper)
		{
		};

	public WindowHandlerStub(IPropertyMapper mapper = null, CommandMapper commandMapper = null)
		: base(mapper ?? Mapper, commandMapper ?? CommandMapper)
	{
	}

	protected override object CreatePlatformElement() => default;
}
