using System;

namespace Rp.Phone.Apps.Messages.Services;

public partial class ConversationService : IMessageEvent
{
	private bool _conversationCreator;
	private List<ConversationData> _conversations = new();

	public IReadOnlyList<ConversationData> Conversations => _conversations;

	public void LoadConversations()
	{
		Log.Info( "LoadConversations: " + string.Join( ", ", Phone.Current.SimCard ) );
		LoadConversationsRpcRequest( Phone.Current.SimCard!.PhoneNumber );
	}

	public void AddConversation( ConversationData conversation )
	{
		if ( ConversationExists( conversation.Id ) ) return;
		_conversations.Add( conversation );
	}

	public void RemoveConversation( ConversationData conversation )
	{
		_conversations.RemoveAll( x => x.Id == conversation.Id );
	}

	private bool ConversationExists( Guid conversationId )
	{
		return _conversations.Exists( x => x.Id == conversationId );
	}

	public void ClearConversations() => _conversations.Clear();

	public IList<ConversationData> GetConversations( PhoneNumber phoneNumber )
	{
		return _conversations.Where( c => c.Participants.Any( p => p.PhoneNumber == phoneNumber ) ).ToList();
	}

	public ConversationData? GetConversation( PhoneNumber myNumber, PhoneNumber contactNumber )
	{
		return GetConversations( myNumber )
			.FirstOrDefault( x => x.Participants.Any( p => p.PhoneNumber == contactNumber ) );
	}

	public void CreateConversation( PhoneContact target )
	{
		Log.Info( "Create conversation with: " + string.Join( ", ", Phone.Current.LocalContact, target ) );

		_conversationCreator = true;
		CreateConversationRpcRequest( Phone.Current.LocalContact, target );
	}

	#region Message Events

	void IMessageEvent.OnConversationCreated( ConversationData conversationData )
	{
		// Only switch to the conversation if we are the creator
		if ( !_conversationCreator ) return;
		_conversationCreator = false;

		var app = Phone.Current.GetApp<MessagesApp>();
		app.SwitchToChat( conversationData );
	}

	void IMessageEvent.OnMessageReceived( MessageData message )
	{
		var app = Phone.Current.GetApp<MessagesApp>();

		var notification = new AppNotificationBuilder( app )
			.WithTitle( "New Message: " + message.Author.PhoneNumber )
			.WithMessage( message.Content )
			.Build();

		Phone.Current.Notification.CreateNotification( notification );
	}

	#endregion
}
