namespace Rp.Core;

public partial class GameManager
{
	private void InitializeServer( Connection channel )
	{
		PlayerManager.Instance.InitializeServer( channel );
		CharacterManager.Instance.InitializeServer();
	}
}
