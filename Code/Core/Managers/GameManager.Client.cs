using Rp.Core.Components;

namespace Rp.Core.Managers;

public partial class GameManager : INetworkInitializer.IClient
{
	void INetworkInitializer.IClient.InitializeClient( Connection channel )
	{
		Task.RunInThreadAsync( async () =>
		{
			PlayerManager.Instance.InitializeClient();
			await PlayerManager.Instance.WaitForPlayerInitialization();

			// Initialize the character manager when the player is loaded
			CharacterManager.Instance.InitializeClient();
		} );
	}
}
