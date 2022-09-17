using Microsoft.Maui.Handlers;

namespace InputKit.Maui.Test.TestClasses;

class ApplicationHandlerStub : ElementHandler<IApplication, object>
{
	public static IPropertyMapper<IApplication, ApplicationHandlerStub> Mapper = new PropertyMapper<IApplication, ApplicationHandlerStub>(ElementMapper)
	{
	};
	public ApplicationHandlerStub()
		: base(Mapper)
	{
	}


	protected override object CreatePlatformElement() => default;
}
