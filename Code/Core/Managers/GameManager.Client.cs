namespace Rp.Core.Managers;

public partial class GameManager
{
	protected override void OnStart()
	{
		Task.RunInThreadAsync( async () =>
		{
			PlayerManager.Instance.InitializeClient();
			await PlayerManager.Instance.WaitForPlayerInitialization();

			// Initialize the character manager when the player is loaded
			CharacterManager.Instance.OnActive();
		} );
	}
}
