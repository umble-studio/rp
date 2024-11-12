namespace Rp.Core.Helpers;

public static class SteamIdUtility
{
	public static SteamId Parse( string value )
	{
		return ulong.Parse( value );
	}
}
