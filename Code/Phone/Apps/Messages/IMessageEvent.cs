using System;

namespace Rp.Phone.Apps.Messages;

public interface IMessageEvent : ISceneEvent<IMessageEvent>
{
	void OnMessageSent( MessageData messageData ) { }
	void OnMessageReceived( MessageData messageData ) { }
	void OnConversationCreated( ConversationData conversationData ) { }
	void OnConversationRemoved( Guid conversationId ) { }
}
