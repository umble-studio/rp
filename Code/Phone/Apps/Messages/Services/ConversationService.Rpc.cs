using System;
using RoverDB;
using Sandbox.Network;

namespace Rp.Phone.Apps.Messages.Services;

public partial class ConversationService
{
	#region Create Conversation

	[Broadcast( NetPermission.Anyone )]
	private void CreateConversationClientRpc( PhoneContact creator, PhoneContact target )
	{
		if ( !Networking.IsHost ) return;

		// The first participant is always the user who created the conversation, the second is the contact
		// Both contacts must be in the conversation
		var conversationExists = RoverDatabase.Instance.Exists<ConversationData>( x =>
			x.Participants.Any( p => p.PhoneNumber == creator.ContactNumber ) &&
			x.Participants.Any( p => p.PhoneNumber == target.ContactNumber ) );

		if ( conversationExists )
		{
			Log.Info( "Conversation already exists" );
			return;
		}

		var simCards = RoverDatabase.Instance.Select<SimCardData>();
		Connection connectionTarget = null!;

		var connections = Connection.All.Where( x => x.IsActive ).ToList();
		Log.Info( "Connections: " + connections.Count );

		foreach ( var connection in connections )
		{
			var simCardExists =
				simCards.Exists( x => x.Owner == connection.SteamId && x.PhoneNumber == target.ContactNumber );
			if ( !simCardExists ) continue;

			connectionTarget = connection;
			break;
		}

		Log.Info("Info: " + string.Join(", ", creator.ContactNumber, target.ContactNumber));
		
		if ( connectionTarget is null )
		{
			Log.Error( "Failed to find connection target" );
			return;
		}

		var participants = new List<ConversationParticipant>
		{
			new()
			{
				Avatar = creator.ContactAvatar, Name = creator.ContactName, PhoneNumber = creator.ContactNumber
			},
			new() { Avatar = target.ContactAvatar, Name = target.ContactName, PhoneNumber = target.ContactNumber }
		};

		var conversation = new ConversationData
		{
			Participants = participants, Messages = new List<MessageData>(), CreatedAt = DateTime.Now,
		};

		using ( Rpc.FilterInclude( x => x == Rpc.Caller ) )
		{
			CreateConversationServerRpc( conversation );
		}

		using ( Rpc.FilterInclude( x => x == connectionTarget ) )
		{
			CreateConversationServerRpc( conversation );
		}
	}

	[Broadcast( NetPermission.HostOnly )]
	private void CreateConversationServerRpc( ConversationData conversationData )
	{
		// Only the 2 participants can create the conversation

		// var isMe = conversationData.Participants.Exists(x => x.PhoneNumber == Phone.Current.SimCard.PhoneNumber);
		// if ( !isMe ) return;
		//
		// if ( Phone.Current.SimCard.PhoneNumber == firstParticipant.PhoneNumber &&
		//      Phone.Current.SimCard.PhoneNumber == secondParticipant.PhoneNumber )

		_conversations.Add( conversationData );
		Log.Info( "New conversation created and added" );
	}

	#endregion

	#region Load Conversations

	[Broadcast( NetPermission.Anyone )]
	private void LoadPhoneConversationsClientRpc( PhoneNumber phoneNumber )
	{
		if ( !Networking.IsHost ) return;

		Log.Info( "Load conversations for: " + phoneNumber );

		var conversations = RoverDatabase.Instance.Select<ConversationData>( x =>
			x.Participants.Any( p => p.PhoneNumber == phoneNumber ) );

		using ( Rpc.FilterInclude( x => x == Rpc.Caller ) )
		{
			LoadPhoneConversationsServerRpc( conversations );
		}
	}

	[Broadcast( NetPermission.HostOnly )]
	private void LoadPhoneConversationsServerRpc( List<ConversationData> conversations )
	{
		_conversations = conversations;
		Log.Info( "Conversations: " + conversations.Count );
	}

	#endregion

	#region Send Message

	[Broadcast( NetPermission.Anyone )]
	private void SendMessageRpc( PhoneNumber sender, PhoneNumber receiver, Guid conversationId, MessageData message )
	{
		if ( Networking.IsHost )
		{
			Log.Info( "Im the host !!!" );

			var conversation =
				RoverDatabase.Instance.SelectOne<ConversationData>( x => x.Id == conversationId );

			if ( conversation is null )
			{
				Log.Error( "Failed to find conversation with id: " + conversationId );
				return;
			}

			Log.Info( "Add message to conversation: " + conversationId );
			conversation.Messages.Add( message );
		}

		// Executed on the client
		if ( receiver == Phone.Current.SimCard!.PhoneNumber )
		{
			Log.Info( "Message was sent to me!" );

			var app = Phone.Current.GetApp<MessagesApp>();
			if ( app is null ) return;

			var notification = new AppNotificationBuilder( app )
				.WithTitle( "New Message: " + message.Author.PhoneNumber )
				.WithMessage( message.Content )
				.Build();

			Scene.RunEvent<IMessageEvent>( x => x.OnMessageReceived( sender, message ) );
			var conversation = Conversations.FirstOrDefault( x => x.Id == conversationId );

			if ( conversation is null )
			{
				Log.Error( "Failed to find conversation with id: " + conversationId );
				return;
			}

			conversation.Messages.Add( message );
			Phone.Current.Notification.CreateNotification( notification );
		}
	}

	#endregion
}
