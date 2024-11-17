using System;
using Rp.UI;
using Sandbox.UI;

namespace Rp.Phone.UI.Components;

public sealed partial class AppIcon : PhoneWidget
{
	public IPhoneApp App { get; set; } = null!;
	public bool ShowTitle { get; set; } = true;

	// private int Notifications =>
	// 	(App as IAppNotifiable)?.GetPendingNotifications( App ).Count() ?? 0;

	// private int Notifications =>
	// 	App.Phone.Notification.GetPendingNotifications( App ).Count();

	private int Notifications { get; set; }

	protected override void OnAfterRender( bool firstRender )
	{
		Notifications = App.Phone.Notification.GetPendingNotifications( App ).Count();
	}

	protected override void OnClick( MousePanelEvent e )
	{
		App.Phone.SwitchToApp( App );
	}

	protected override int ShouldRender() =>
		HashCode.Combine( base.ShouldRender(), App, Notifications, ShowTitle );
}
