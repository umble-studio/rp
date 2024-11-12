using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Rp.Core.Helpers;

namespace Rp.Core.Serializers;

// public sealed class CharacterIdJsonConverter : JsonConverter<CharacterId>
// {
// 	public override CharacterId Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
// 	{
// 		var stringValue = reader.GetString();
//
// 		if ( stringValue is null )
// 			throw new JsonException( "Unable to deserialize CharacterId because the input string was null." );
//
// 		var parts = stringValue.Split( '_' );
//
// 		if ( parts.Length is not 2 || !ushort.TryParse( parts[1], out var id ) )
// 			throw new JsonException( "CharacterId format is invalid." );
//
// 		var steamId = SteamIdUtility.Parse( parts[0] );
// 		return new CharacterId( steamId, id );
// 	}
//
// 	public override void Write( Utf8JsonWriter writer, CharacterId value, JsonSerializerOptions options )
// 	{
// 		writer.WriteStringValue( value.ToString() );
// 	}
// }
