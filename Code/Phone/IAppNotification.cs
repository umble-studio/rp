namespace Rp.Phone;

public interface IAppNotifiable
{
	// IEnumerable<AppNotification> GetPendingNotifications( IPhoneApp app )
	// {
	// 	return Phone.Current.Notification.GetPendingNotifications( app );
	// }
}

public interface IAppNotifiable<T> where T : IPhoneApp, IAppNotifiable
{
	// IEnumerable<AppNotification> GetPendingNotifications()
	// {
	// 	return Phone.Current.Notification.GetPendingNotifications<T>();
	// }
}
