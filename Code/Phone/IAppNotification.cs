namespace Rp.Phone;

public interface IAppNotifiable
{
	IEnumerable<AppNotification> GetPendingNotifications( IPhoneApp app )
	{
		return Phone.Local.Notification.GetPendingNotifications( app );
	}
}

public interface IAppNotifiable<T> where T : IPhoneApp, IAppNotifiable
{
	IEnumerable<AppNotification> GetPendingNotifications()
	{
		return Phone.Local.Notification.GetPendingNotifications<T>();
	}
}
