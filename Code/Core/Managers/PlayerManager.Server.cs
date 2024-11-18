using RoverDB;

namespace Rp.Core.Managers;

public partial class PlayerManager
{
	public static readonly Dictionary<SteamId, PlayerData> ServerPlayers = new();

	internal void OnActive( Connection channel )
	{
		var player = ServerPlayerExists( channel.SteamId )
			? ServerLoadPlayer( channel )
			: ServerCreatePlayer( channel );

		if ( player is null ) return;
		ServerPlayers[channel.SteamId] = player;
	}

	private PlayerData? ServerLoadPlayer( Connection channel )
	{
		var player = RoverDatabase.Instance.SelectOne<PlayerData>( x => x.Owner == channel.SteamId );

		if ( player is null )
			return null;

		return ServerPlayers[player.Owner] = player;
	}

	private PlayerData? ServerCreatePlayer( Connection channel )
	{
		var player = new PlayerData { Owner = channel.SteamId };
		RoverDatabase.Instance.Insert( player );

		ServerPlayers[player.Owner] = player;
		return player;
	}

	/// <summary>
	/// Check if a player exists in the database
	/// </summary>
	/// <param name="steamId">The SteamId of the player</param>
	/// <returns></returns>
	private static bool ServerPlayerExists( ulong steamId )
	{
		return RoverDatabase.Instance.Exists<PlayerData>( x => x.Owner == steamId );
	}
}
