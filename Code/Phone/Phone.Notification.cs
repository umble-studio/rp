using Rp.Phone.UI.Components;
using Rp.UI.Extensions;

namespace Rp.Phone;

public partial class Phone
{
	public NotificationCenter Notification { get; private set; } = null!;

	public sealed class NotificationCenter
	{
		public List<AppNotification> PendingNotifications { get; } = new();

		public void AddPendingNotification( AppNotification notification )
		{
			PendingNotifications.Push( notification );
		}

		public void RemovePendingNotification( AppNotification notification )
		{
			PendingNotifications.Remove( notification );
		}
		
		public void ClearPendingNotifications<T>() where T : IPhoneApp
		{
			PendingNotifications.RemoveAll( x => x.App is T );
		}

		public void ClearPendingNotifications() => PendingNotifications.Clear();
		
		public void CreateNotification( AppNotification notification )
		{
			AddPendingNotification( notification );
			Current.Scene.RunEvent<IAppNotificationEvent>( x => x.OnNotify( notification ), true );
		}

		public void CreateNotification<T>( AppNotification notification ) where T : IAppNotifiable, IPhoneApp
		{
			var app = Current.GetApp<T>();
			if ( app is null ) return;

			notification.App = app;
			CreateNotification( notification );
		}

		public IEnumerable<AppNotification> GetPendingNotifications<T>() where T : IPhoneApp =>
			PendingNotifications.Where( x => x.App is T );

		public IEnumerable<AppNotification> GetPendingNotifications( IPhoneApp app ) =>
			PendingNotifications.Where( x => x.App.AppName == app.AppName );
	}
}
