using System;
using System.Text.Json.Serialization;

namespace Rp.Core;

public readonly struct CharacterId
{
	[JsonPropertyName( "steamId" )] public ulong SteamId { get; }
	[JsonPropertyName( "id" )] public ushort Id { get; }

	[JsonConstructor]
	public CharacterId( ulong steamId, ushort id )
	{
		SteamId = steamId;
		Id = id;
	}

	public static implicit operator CharacterId( string id )
	{
		var parts = id.Split( '_' );
		
		if ( parts.Length is not 2 )
			throw new Exception( "CharacterId format is invalid." );
		
		var steamId = ulong.Parse( parts[0] );
		var characterId = ushort.Parse( parts[1] );

		return new CharacterId( steamId, characterId );
	}

	public override string ToString() => $"{SteamId}_{Id}";
}
