using System;
using RoverDB;
using Rp.UI.Extensions;

namespace Rp.Phone.Apps.Messages.Services;

public partial class ConversationService
{
	[Broadcast( NetPermission.Anyone )]
	private void CreateConversationRpcRequest( PhoneContact creator, PhoneContact target )
	{
		// if ( !Networking.IsHost ) return;
		if ( ServerConversationExists( creator, target ) ) return;

		// var simCards = RoverDatabase.Instance.Select<SimCardData>();
		// Connection connectionTarget = null!;
		//
		// var connections = Connection.All.Where( x => x.IsActive ).ToList();
		// Log.Info( "Connections: " + connections.Count );
		//
		// var player = RoverDatabase.Instance.Select<PlayerData>( x => x.Owner == caller.SteamId );
		// if ( player is null ) return;
		//
		// foreach ( var connection in connections )
		// {
		// 	var simCardExists =
		// 		simCards.Exists( x => x.Owner == connection.SteamId && x.PhoneNumber == target.ContactNumber );
		//
		// 	if ( !simCardExists ) continue;
		//
		// 	connectionTarget = connection;
		// 	break;
		// }

		// Log.Info( "Info: " + string.Join( ", ", creator.ContactNumber, target.ContactNumber ) );
		//
		// if ( connectionTarget is null )
		// {
		// 	Log.Error( "Failed to find connection target" );
		// 	return;
		// }

		Log.Info( "Creator: " + creator );
		Log.Info( "Target: " + target );

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

		RoverDatabase.Instance.Insert( conversation );
		Log.Info( "Created conversation: " + conversation.Id );

		if ( Phone.Current.LocalContact.ContactNumber == creator.ContactNumber ||
		     Phone.Current.LocalContact.ContactNumber == target.ContactNumber )
		{
			if ( Phone.Current.SimCard is null ) return;

			_conversations.Add( conversation );
			Log.Info( "Add conversation: " + conversation.Id );
		}

		// using ( Rpc.FilterInclude( x => x == Rpc.Caller ) )
		// {
		// 	CreateConversationRpcResponse( conversation );
		// }

		// using ( Rpc.FilterInclude( x => x == connectionTarget ) )
		// {
		// 	CreateConversationRpcResponse( conversation );
		// }
	}

	[Broadcast]
	private void LoadConversationsRpcRequest( PhoneNumber phoneNumber )
	{
		if ( !Networking.IsHost ) return;

		Log.Info( "LoadConversationsRpcRequest: " + phoneNumber );

		var conversations = RoverDatabase.Instance.Select<ConversationData>( x =>
			x.Participants.Any( p => p.PhoneNumber == phoneNumber ) );

		Log.Info( "Count: " + conversations.Count );

		using ( Rpc.FilterInclude( x => x == Rpc.Caller ) )
		{
			LoadConversationsRpcResponse( conversations );
		}
	}

	[Broadcast( NetPermission.Anyone )]
	public void SendMessageRpcRequest( PhoneNumber sender, Guid conversationId, MessageData message )
	{
		if ( !Networking.IsHost ) return;

		var conversation =
			RoverDatabase.Instance.SelectOne<ConversationData>( x => x.Id == conversationId );

		if ( conversation is null )
		{
			Log.Error( "Failed to find conversation with id: " + conversationId );
			return;
		}

		Log.Info( "Add message to conversation: " + conversationId );

		// TODO - Currently, we send the message to all connected clients
		// We should only send the message to the conversation participants
		SendMessageRpcResponse( conversationId, message );
	}

	[Broadcast( NetPermission.HostOnly )]
	private void SendMessageRpcResponse( Guid conversationId, MessageData message )
	{
		var conversation = Conversations.FirstOrDefault( x => x.Id == conversationId );

		if ( conversation is null )
		{
			Log.Error( "Failed to find conversation with id: " + conversationId );
			return;
		}

		conversation.Messages.Add( message );

		Scene.RunEvent<IMessageEvent>( x => x.OnMessageReceived( message ), true );

		var app = Phone.Current.GetApp<MessagesApp>();
		if ( app is null ) return;

		var notification = new AppNotificationBuilder( app )
			.WithTitle( "New Message: " + message.Author.PhoneNumber )
			.WithMessage( message.Content )
			.Build();
		
		Phone.Current.Notification.CreateNotification( notification );
	}
}
