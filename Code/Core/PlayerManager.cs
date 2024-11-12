using Bindery;

namespace Rp.Core;

public sealed partial class PlayerManager : Singleton<PlayerManager>
{
	public PlayerData Current { get; private set; } = null!;
}
