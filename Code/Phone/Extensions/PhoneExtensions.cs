using System;

namespace Rp.Phone.Extensions;

public static class PhoneExtensions
{
	public static IEnumerable<TypeDescription> GetApps()
	{
		return TypeLibrary.GetTypes<IPhoneApp>()
			.Where( x => x is { IsAbstract: false, IsInterface: false } );
	}

	public static IPhoneApp CreateAppInstance( Type type )
	{
		var instance = TypeLibrary.Create( type.Name, type );

		if ( instance is not IPhoneApp app )
		{
			throw new Exception( $"Could not create app instance. The app should implement {nameof(IPhoneApp)}." );
		}

		return app;
	}

	public static bool CreateAppInstance<T>( out IPhoneApp app ) where T : IPhoneApp
	{
		var instance = TypeLibrary.Create<T>();

		if ( instance is null )
		{
			app = default!;
			return false;
		}

		app = instance;
		return true;
	}
}
