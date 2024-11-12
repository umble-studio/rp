using System;

namespace Rp.Core.Managers;

public partial class CharacterManager
{
	[Broadcast( NetPermission.HostOnly )]
	private void LoadCharactersRpcResponse( List<CharacterData> characters )
	{
		Characters = characters;
		
		var player = PlayerManager.Instance.Current;
		
		// Don't load the character if the player doesn't have one
		if ( player.CurrentCharacter == Guid.Empty ) return;
		
		Log.Info( "Current player: " + player );
		var character = characters.FirstOrDefault( x => x.CharacterId == player.CurrentCharacter );

		if ( character is null )
		{
			Log.Error( "Failed to load current character: " + player.CurrentCharacter );
			return;
		}

		Current = character;
		_isCharacterLoaded = true;
		Log.Warning( $"CharacterManager | Loaded: {Characters.Count} characters" );
	}
}
