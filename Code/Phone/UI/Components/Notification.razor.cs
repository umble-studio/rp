using System;
using Rp.UI;
using Sandbox.UI;

namespace Rp.Phone.UI.Components;

public sealed partial class Notification : PhoneWidget
{
	public AppNotification Pending { get; set; } = default;

	private void LaunchApp()
	{
		Phone.SwitchToApp( Pending.App );
	}

	protected override int ShouldRender() => HashCode.Combine( Pending );
}
