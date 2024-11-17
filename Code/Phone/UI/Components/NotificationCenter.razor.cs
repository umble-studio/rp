using System;
using Rp.UI;
using Sandbox.UI;

namespace Rp.Phone.UI.Components;

public sealed partial class NotificationCenter : PhoneWidget
{
	private List<AppNotification> Notifications => Phone?.Notification.PendingNotifications ?? new List<AppNotification>();
	
	protected override int ShouldRender() => HashCode.Combine( Phone.Notification.PendingNotifications.Count );
}
