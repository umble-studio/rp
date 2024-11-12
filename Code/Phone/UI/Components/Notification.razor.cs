using System;
using Sandbox.UI;

namespace Rp.Phone.UI.Components;

public sealed partial class Notification : Panel
{
	public AppNotification Pending { get; set; } = default;

	private void LaunchApp()
	{
		Rp.Phone.Phone.Current.SwitchToApp( Pending.App );
	}

	protected override int BuildHash() => HashCode.Combine( Pending );
}
