using Rp.Core.UI;

namespace Rp.Core.Managers;

public sealed partial class CharacterManager : Bindery.Singleton<CharacterManager>
{
	[HostSync, Change( "IdChanged" )] public int Id { get; set; } = 2;

	[Property] public CharacterSelection Panel { get; init; } = null!;

	public CharacterData Current { get; internal set; } = null!;
	public List<CharacterData> Characters { get; private set; } = new();

	// public async Task Initialize()
	// {
	// 	// Wait for player to load before loading characters
	// 	await PlayerManager.Instance.WaitForPlayerInitialization();
	//
	// 	LoadCharacters();
	// 	Log.Info( "Panel character selected: " + Panel.CharacterSelected );
	//
	// 	if ( !Panel.CharacterSelected )
	// 		Panel.Show();
	// }
}
