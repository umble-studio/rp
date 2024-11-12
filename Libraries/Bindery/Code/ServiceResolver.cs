using System.Linq;
using Bindery.Helpers;
using Sandbox;

namespace Bindery;

public sealed class ServiceResolver : IServiceResolver
{
	private readonly ServiceCollection _services = new();

	public ServiceResolver()
	{
		Init();
	}

	private void Init()
	{
		var services = InternalServiceHelper.GetInternalServiceTypes().ToList();
		Log.Info( "Found " + services.Count + " services" );

		foreach ( var service in services )
		{
			RegisterService( service );
		}
	}

	private void RegisterService( TypeDescription service )
	{
		var instance = CreateServiceInstance( service );

		if ( instance is null )
		{
			Log.Error( $"Could not create service {service.TargetType.Name}" );
			return;
		}

		_services.Add( instance );
	}

	private static Service? CreateServiceInstance( TypeDescription serviceType )
	{
		return InternalServiceHelper.CreateInternalService( serviceType );
	}

	internal ServiceCollection BuildServices()
	{
		return _services;
	}
}
