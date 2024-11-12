using System;

namespace Rp.Core.Managers;

public partial class CharacterManager
{
	[ConCmd( "character_create" )]
	private static void ServerCreateCharacterCmd( string firstname, string lastname )
	{
		var character = new CharacterData { CharacterId = Guid.NewGuid(), Firstname = firstname, Lastname = lastname, };

		Log.Info( "Creating character: " + character.CharacterId );
		Instance.CreateCharacterRpcRequest( character );
	}
}
