namespace Rp.Core;

public sealed partial class GameManager : Bindery.Singleton<GameManager>, Component.INetworkListener
{
	public void OnActive( Connection channel )
	{
		// Start the initialization when the client is active
		Initialize( channel );
	}
	
	private void Initialize( Connection channel )
	{
		if ( Networking.IsHost )
		{
			InitializeServer( channel );
		}

		if ( Application.IsEditor || Networking.IsClient )
		{
			InitializeClient();
		}
	}
}
