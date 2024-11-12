namespace Rp.Phone.Apps.Messages;

public interface IMessageEvent : ISceneEvent<IMessageEvent>
{
	void OnMessageSent( MessageData messageData ) { }
	void OnMessageReceived( MessageData messageData ) { }
}
