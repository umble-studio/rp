namespace Rp.Core.Managers;

public partial class CharacterManager
{
	// private readonly Dictionary<SteamId, CharacterId> _currentSelectedCharacters = new();

	internal void OnActive( Connection connection )
	{
		var currentPlayerCharacter = PlayerManager.ServerPlayers[connection.SteamId];
		// var simcard = RoverDatabase.Instance.Select<SimCardData>(x => x.)
	}

	// public void OnActive( Connection channel )
	// {
	// 	var simcard = RoverDatabase.Instance.Select<CharacterData>( x => x. );
	// 	// RoverDatabase.Instance.Select<PhoneContact>(x => x.)
	// }
}
