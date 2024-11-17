using Rp.Core.UI;

namespace Rp.Core.Managers;

public sealed partial class CharacterManager : Bindery.Singleton<CharacterManager>
{
	[Property] public CharacterSelection Panel { get; init; } = null!;

	public CharacterData Current { get; internal set; } = null!;
	public List<CharacterData> Characters { get; private set; } = new();
}
