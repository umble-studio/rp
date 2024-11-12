using System.Collections.Generic;
using System.Linq;
using Sandbox;

namespace Bindery.Helpers;

internal static class InternalServiceHelper
{
	public static Service? CreateInternalService( TypeDescription serviceType )
	{
		return TypeLibrary.Create<Service>( serviceType.TargetType );
	}

	public static IEnumerable<TypeDescription> GetInternalServiceTypes()
	{
		var services = TypeLibrary.GetTypes( typeof(Service) )
			.Where( x => x is { IsAbstract: false } );

		foreach ( var service in services )
			yield return service;
	}
}
