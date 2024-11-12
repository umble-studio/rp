using System;
using RoverDB;

namespace Rp.Core.Managers;

public partial class PlayerManager
{
	[Broadcast( NetPermission.Anyone )]
	private void LoadPlayerRpcRequest( ulong steamId )
	{
		if ( !Networking.IsHost ) return;

		var player = RoverDatabase.Instance.SelectOne<PlayerData>( x => x.Owner == steamId );
		if ( player is null ) return;

		using ( Rpc.FilterInclude( x => x == Rpc.Caller ) )
		{
			LoadPlayerRpcResponse( player );
		}
	}

	[Broadcast( NetPermission.Anyone )]
	public void AddPlayerCharacterRpcRequest( ulong steamId, Guid characterId )
	{
		if ( !Networking.IsHost ) return;

		var player = RoverDatabase.Instance.SelectOne<PlayerData>( x => x.Owner == steamId );
		player?.Characters.Add( characterId );
	}
}
