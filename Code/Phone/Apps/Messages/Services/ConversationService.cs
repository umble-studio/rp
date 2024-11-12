namespace Rp.Phone.Apps.Messages.Services;

public sealed partial class ConversationService : Bindery.Singleton<ConversationService>, IPhoneService, IMessageEvent
{
	/// <summary>
	/// Load all conversations when the service	starts
	/// </summary>
	protected override void OnStart()
	{
		LoadConversations();
	}

	void IMessageEvent.OnMessageSent( MessageData messageData )
	{
	}

	void IMessageEvent.OnMessageReceived( PhoneNumber sender, MessageData messageData )
	{
	}
}
