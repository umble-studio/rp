namespace Rp.Core.Managers;

public partial class PlayerManager
{
	[Broadcast( NetPermission.HostOnly )]
	private void LoadPlayerRpcResponse( PlayerData player )
	{
		Current = player;
		_isPlayerLoaded = true;
		Log.Warning( $"PlayerManager | Loaded: {player.Id} player" );
	}
}
