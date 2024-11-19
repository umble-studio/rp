using System;
using Rp.UI;
using Sandbox.UI;

namespace Rp.Phone.UI.Components;

public sealed partial class NotificationCenter : Panel
{
	private List<AppNotification> Notifications => Phone.Local.Notification.PendingNotifications;
	
	protected override int BuildHash() => HashCode.Combine( Notifications.Count );
}
