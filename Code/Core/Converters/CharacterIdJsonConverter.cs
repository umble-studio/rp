using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Rp.Core.Converters;

public sealed class CharacterIdJsonConverter : JsonConverter<CharacterId>
{
	public override CharacterId Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
	{
		var stringValue = reader.GetString();

		if ( stringValue is null )
			throw new JsonException(
				$"Unable to deserialize {nameof(CharacterId)} because the input string was null." );

		return stringValue;
	}

	public override void Write( Utf8JsonWriter writer, CharacterId value, JsonSerializerOptions options )
	{
		writer.WriteStringValue( value.ToString() );
	}
}
