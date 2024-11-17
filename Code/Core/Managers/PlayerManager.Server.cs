using RoverDB;

namespace Rp.Core.Managers;

public partial class PlayerManager
{
	/// <summary>
	/// List of connected players (Server only)
	/// </summary>
	public readonly List<PlayerData> Players = new();

	internal void InitializeServer( Connection channel )
	{
		// Clear all occurrences of the player, then catch it
		Players.RemoveAll( x => x.Owner == channel.SteamId );

		PlayerData? player;

		if ( ServerPlayerExists( channel.SteamId ) )
		{
			player = ServerLoadPlayer( channel );
		}
		else
		{
			player = ServerCreatePlayer( channel );
		}

		if ( player is null ) return;
		Players.Add( player );
	}

	private static PlayerData? ServerLoadPlayer( Connection channel )
	{
		return RoverDatabase.Instance.SelectOne<PlayerData>( x => x.Owner == channel.SteamId );
	}

	private static PlayerData? ServerCreatePlayer( Connection channel )
	{
		var player = new PlayerData { Owner = channel.SteamId };
		RoverDatabase.Instance.Insert( player );

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
