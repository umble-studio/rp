using System.Threading.Tasks;

namespace Rp.Core.Managers;

public partial class PlayerManager
{
	private bool _isPlayerLoaded;

	internal void InitializeClient()
	{
		LoadPlayerRpcRequest( SteamId.Local );
	}

	public async Task WaitForPlayerInitialization()
	{
		while ( !_isPlayerLoaded )
			await GameTask.Delay( 100 );
	}
}
