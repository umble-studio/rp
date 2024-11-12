namespace Rp.Phone;

public interface IAppNotificationEvent : ISceneEvent<IAppNotificationEvent>
{
	void OnNotify( AppNotification notification );
}
