using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Animations;

namespace InputKit.Maui.Test.TestClasses;

class HandlersContextStub : IMauiContext
{
	public HandlersContextStub(IServiceProvider services)
	{
		Services = services;
		Handlers = Services.GetRequiredService<IMauiHandlersFactory>();
		AnimationManager = Services.GetRequiredService<IAnimationManager>();
	}

	public IServiceProvider Services { get; }

	public IMauiHandlersFactory Handlers { get; }

	public IAnimationManager AnimationManager { get; }
}