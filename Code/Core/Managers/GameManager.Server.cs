namespace Rp.Core.Managers;

public partial class GameManager : Component.INetworkListener
{
	public void OnActive( Connection channel )
	{
		PlayerManager.Instance.OnActive( channel );
		CharacterManager.Instance.OnActive( channel );
	}
}
