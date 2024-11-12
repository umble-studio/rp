using Rp.Core.Systems;

namespace Rp.Core.Managers;

public partial class GameManager : INetworkInitializer.IServer
{
	void INetworkInitializer.IServer.InitializeServer( Connection channel )
	{
		PlayerManager.Instance.InitializeServer( channel );
		CharacterManager.Instance.InitializeServer();
	}
}
