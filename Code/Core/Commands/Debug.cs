using Sandbox.Utility;

namespace Rp.Core.Commands;

public static class Debug
{
	#region Commands

	[ConCmd( "steam_id" )]
	private static void DisplaySteamId()
	{
		Log.Info( "SteamId: " + Steam.SteamId );
	}

	#endregion
}
