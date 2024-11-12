namespace Rp.Core.Components;

public class NetworkInitializer : Bindery.Singleton<NetworkInitializer>, Component.INetworkListener
{
	private readonly List<INetworkInitializer.IClient> _clientInitializers = new();
	private readonly List<INetworkInitializer.IServer> _serverInitializers = new();

	protected override void OnAwake()
	{
		if ( !Networking.IsHost ) return;

		LoadClientInitializers();
		LoadServerInitializers();
	}

	public void OnActive( Connection channel )
	{
		// Run the code only on the host
		if ( Networking.IsHost )
		{
			foreach ( var initializer in _serverInitializers )
				initializer.InitializeServer( channel );
		}

		// Run the code only in editor or on the client
		if ( !Application.IsEditor && !Networking.IsClient ) return;

		foreach ( var initializer in _clientInitializers )
			initializer.InitializeClient( channel );
	}
	
	private void LoadClientInitializers()
	{
		var components = TypeLibrary.GetTypes<INetworkInitializer.IClient>()
			.Where( x => x is { IsAbstract: false, IsInterface: false } );

		foreach ( var type in components )
		{
			var initializer = TypeLibrary.Create<INetworkInitializer.IClient>( type.Name );
			_clientInitializers.Add( initializer );
		}
	}

	private void LoadServerInitializers()
	{
		var components = TypeLibrary.GetTypes<INetworkInitializer.IServer>()
			.Where( x => x is { IsAbstract: false, IsInterface: false } );

		foreach ( var type in components )
		{
			var initializer = TypeLibrary.Create<INetworkInitializer.IServer>( type.Name );
			_serverInitializers.Add( initializer );
		}
	}
}
