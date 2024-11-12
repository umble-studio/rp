using System;
using Sandbox.UI;

namespace Rp.Phone.UI.Components;

public sealed partial class AppIcon : Panel
{
	public IPhoneApp App { get; set; } = null!;
	public bool ShowTitle { get; set; } = true;

	private int Notifications =>
		(App as IAppNotifiable)?.GetPendingNotifications( App ).Count() ?? 0;

	protected override void OnClick( MousePanelEvent e )
	{
		Rp.Phone.Phone.Current.SwitchToApp( App );
	}

	protected override int BuildHash() => HashCode.Combine( App, Notifications, ShowTitle );
}
