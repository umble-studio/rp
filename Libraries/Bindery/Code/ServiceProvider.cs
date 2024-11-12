using System.Linq;

namespace Bindery;

public sealed class ServiceProvider : IServiceProvider
{
	private readonly ServiceCollection _services;

	public IServiceCollection Services => _services;
	
	public ServiceProvider( ServiceCollection collection )
	{
		_services = collection;
	}

	public T? GetService<T>()
	{
		return _services.OfType<T>().FirstOrDefault();
	}

	public bool TryGetService<T>( out T service )
	{
		var svc = GetService<T>();

		if ( svc is null )
		{
			service = default!;
			return false;
		}

		service = svc;
		return true;
	}
}
