namespace Rp.Core;

public class ComponentInitializer : Bindery.Singleton<ComponentInitializer>, Component.INetworkListener
{
	private readonly List<IComponentInitializer> _initializers = new();

	protected override void OnAwake()
	{
		if ( !Networking.IsHost ) return;

		var components = TypeLibrary.GetTypes<IComponentInitializer>()
			.Where( x => x is { IsAbstract: false, IsInterface: false } );

		foreach ( var type in components )
		{
			// var go = new GameObject( true ) { Parent = _parent };
			// var component = go.Components.Create( type ) as IComponentInitializer;
			// component?.OnActive( channel );

			var initializer = TypeLibrary.Create<IComponentInitializer>( type.Name );
			_initializers.Add( initializer );
		}
	}

	public void OnActive( Connection channel )
	{
		// Call the code only on the host
		if ( !Networking.IsHost ) return;
		
		foreach ( var initializer in _initializers )
			initializer.OnActive( channel );
	}
}
