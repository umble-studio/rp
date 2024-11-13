using RoverDB;

namespace Rp.Core.Managers;

public partial class CharacterManager
{
	[Broadcast( NetPermission.Anyone )]
	private void CreateCharacterRpcRequest( CharacterData character )
	{
		if ( !Networking.IsHost ) return;

		Log.Info( "Insert character: " + character.CharacterId );
		RoverDatabase.Instance.Insert( character );
	}
	
	[Broadcast( NetPermission.Anyone )]
	private void LoadCharactersRpcRequest( ulong steamId )
	{
		if ( !Networking.IsHost ) return;

		var player = RoverDatabase.Instance.SelectOne<PlayerData>( x => x.Owner == steamId );
		if ( player is null ) return;

		var characterIds = player.Characters;
		var characters = new List<CharacterData>();

		foreach ( var characterId in characterIds )
		{
			var character = RoverDatabase.Instance.SelectOne<CharacterData>( x => x.CharacterId == characterId );
			if ( character is null ) continue;

			characters.Add( character );
		}
		
		using ( Rpc.FilterInclude( x => x == Rpc.Caller ) )
		{
			LoadCharactersRpcResponse( characters );
		}
	}
}
