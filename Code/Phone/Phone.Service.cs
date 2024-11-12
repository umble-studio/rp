using SteamId = Rp.Core.SteamId;

namespace Rp.Phone;

public partial class Phone
{
	private void RegisterAllServices()
	{
		var types = TypeLibrary.GetTypes<IPhoneService>()
			.Where( x => x is { IsAbstract: false, IsInterface: false } );

		foreach ( var type in types )
		{
			if ( !type.TargetType.IsAssignableTo( typeof(Component) ) )
			{
				Log.Error( "Type is not a component: " + type.TargetType.Name );
				continue;
			}

			Log.Info( "Registering service: " + type.TargetType.Name );
			Components.Create( type );
		}
	}

	private IEnumerable<IPhoneService> GetServices()
	{
		return Components.GetAll<IPhoneService>();
	}

	internal T GetService<T>() where T : IPhoneService
	{
		var services = GetServices();
		return services.OfType<T>().First();
	}

	internal bool TryGetService<T>( out T service ) where T : IPhoneService
	{
		var services = GetServices();
		var svc = services.OfType<T>().FirstOrDefault();

		if ( svc is null )
		{
			service = default!;
			return false;
		}

		service = svc;
		return true;
	}
}
