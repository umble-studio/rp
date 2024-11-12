namespace Rp.Core;

public partial class GameManager
{
	private void InitializeClient()
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
