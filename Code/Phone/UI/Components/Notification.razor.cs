using System;
using Rp.UI;
using Sandbox.UI;

namespace Rp.Phone.UI.Components;

public sealed partial class Notification : Panel
{
	public AppNotification Pending { get; set; } = default;

	private void LaunchApp()
	{
		Phone.Local.SwitchToApp( Pending.App );
	}

	protected override int BuildHash() => HashCode.Combine( Pending );
}
