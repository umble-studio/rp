using System;
using Sandbox.UI;

namespace Rp.Phone.UI.Components;

public sealed partial class NotificationCenter : Panel
{
	protected override int BuildHash() => HashCode.Combine( Rp.Phone.Phone.Current.Notification.PendingNotifications.Count );
}
