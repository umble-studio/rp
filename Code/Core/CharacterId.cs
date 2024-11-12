using System;
using System.Text.Json.Serialization;

namespace Rp.Core;

public readonly struct CharacterId : IEquatable<CharacterId>
{
	public ulong SteamId { get; }
	public ushort Id { get; }

	[JsonConstructor]
	public CharacterId( ulong steamId, ushort id )
	{
		SteamId = steamId;
		Id = id;
	}

	public bool Equals( CharacterId other )
	{
		return SteamId == other.SteamId && Id == other.Id;
	}

	public override bool Equals( object? obj )
	{
		return obj is CharacterId other && Equals( other );
	}

	public override int GetHashCode()
	{
		return HashCode.Combine( SteamId, Id );
	}

	public override string ToString() => $"{SteamId}_{Id}";

	public static implicit operator CharacterId( string id )
	{
		var parts = id.Split( '_' );

		if ( parts.Length is not 2 )
			throw new Exception( "CharacterId format is invalid." );

		var steamId = ulong.Parse( parts[0] );
		var characterId = ushort.Parse( parts[1] );

		return new CharacterId( steamId, characterId );
	}

	public static bool operator ==( CharacterId left, CharacterId right )
	{
		return left.Equals( right );
	}

	public static bool operator !=( CharacterId left, CharacterId right )
	{
		return !(left == right);
	}
}
