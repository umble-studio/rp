using System.Threading.Tasks;

namespace Rp.Core.Managers;

public partial class CharacterManager
{
	private bool _isCharacterLoaded;

	internal void OnActive()
	{
		LoadCharacters();
		
		if ( !Panel.CharacterSelected )
			Panel.Show();
	}

	private void LoadCharacters()
	{
		if ( _isCharacterLoaded ) return;
		LoadCharactersRpcRequest( SteamId.Local );
	}

	public async Task WaitForCharacterInitialization()
	{
		while ( !_isCharacterLoaded )
			await GameTask.Delay( 100 );
	}
}
