namespace Rp.Core.Systems;

public sealed class NetworkInitializer : GameObjectSystem, Component.INetworkListener
{
	private bool _isInitialized;
	private readonly List<INetworkInitializer.IClient> _clientInitializers = new();
	private readonly List<INetworkInitializer.IServer> _serverInitializers = new();

	public NetworkInitializer( Scene scene ) : base( scene )
	{
		Listen( Stage.StartUpdate, -1, Initialize, nameof(NetworkInitializer) );
	}

	private void Initialize()
	{
		// We can't use the State.SceneLoaded because it's only called on the host (because the scene is loaded on the host/server)
		// So we need to call it in StartUpdate to be sure it's called on call clients, but only one time.
		if ( _isInitialized ) return;
		_isInitialized = true;

		if ( Networking.IsHost )
			LoadServerInitializers();
		
		if ( Application.IsEditor || Networking.IsClient )
		{
			LoadClientInitializers();

			foreach ( var initializer in _clientInitializers )
				initializer.InitializeClient();
		}
	}

	public void OnActive( Connection channel )
	{
		if ( !Networking.IsHost ) return;

		foreach ( var initializer in _serverInitializers )
			initializer.InitializeServer( channel );
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
