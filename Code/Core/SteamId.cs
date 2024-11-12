using Sandbox.Utility;

namespace Rp.Core;

public record struct SteamId
{
	public ulong Value { get; init; }

	public static SteamId Local => Steam.SteamId;

	private SteamId( ulong value ) => Value = value;

	public static SteamId Parse( string value ) => ulong.Parse( value );

	public static implicit operator ulong( SteamId id ) => id.Value;
	public static implicit operator SteamId( ulong id ) => new(id);
}
