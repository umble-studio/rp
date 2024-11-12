using System;
using RoverDB;

namespace Rp.Core.Managers;

public partial class CharacterManager
{
	[ConCmd( "character_create" )]
	private static void ServerCreateCharacterCmd( int characterId, string firstname, string lastname )
	{
		if ( !Networking.IsHost ) return;

		var player = RoverDatabase.Instance.SelectOne<PlayerData>( x => x.Owner == SteamId.Local );
		if ( player is null ) return;

		var character = new CharacterData
		{
			CharacterId = new CharacterId( SteamId.Local, (ushort)characterId ), Firstname = firstname, Lastname = lastname,
		};

		player.Characters.Add( character.CharacterId );
		player.CurrentCharacter = character.CharacterId;

		Log.Info( "Creating character: " + character.CharacterId );
		Instance.CreateCharacterRpcRequest( character );
	}
}
