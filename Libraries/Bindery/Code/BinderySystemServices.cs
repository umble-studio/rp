using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Sandbox;
using Component = Sandbox.Component;

namespace Bindery;

public class BinderySystemServices : GameObjectSystem<BinderySystemServices>
{
	private readonly ServiceResolver _serviceResolver = new();
	private readonly ServiceProvider _serviceProvider;

	public BinderySystemServices( Scene scene ) : base( scene )
	{
		// var collection = _serviceResolver.BuildServices();
		// _serviceProvider = new ServiceProvider( collection );

		// Listen( Stage.SceneLoaded, -1, OnStart, nameof(BinderySystemServices) );
	}

	private void OnStart()
	{
	}
}
