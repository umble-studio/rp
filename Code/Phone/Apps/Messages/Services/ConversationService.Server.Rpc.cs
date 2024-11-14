using System;
using RoverDB;
using Rp.UI.Extensions;

namespace Rp.Phone.Apps.Messages.Services;

public partial class ConversationService
{
	[Broadcast( NetPermission.Anyone )]
	private void CreateConversationRpcRequest( PhoneContact creator, PhoneContact target )
	{
		if ( !Networking.IsHost ) return;
		if ( ServerConversationExists( creator, target ) ) return;

		var simCards = RoverDatabase.Instance.Select<SimCardData>();
		Connection connectionTarget = null!;

		var connections = Connection.All.Where( x => x.IsActive ).ToList();

		foreach ( var connection in connections )
		{
			var simCardExists =
				simCards.Exists( x => x.Owner.SteamId == connection.SteamId && x.PhoneNumber == target.ContactNumber );
			if ( !simCardExists ) continue;

			connectionTarget = connection;
			break;
		}

		if ( connectionTarget is null )
		{
			Log.Error( "Failed to find connection target" );
			return;
		}

		var participants = new List<ConversationParticipant>
		{
			creator.ToConversationParticipant(), target.ToConversationParticipant()
		};

		var conversation = new ConversationData
		{
			Participants = participants, Messages = new List<MessageData>(), CreatedAt = DateTime.Now,
		};

		RoverDatabase.Instance.Insert( conversation );
		Log.Info( "Created conversation: " + conversation.Id );

		var targets = new List<ulong> { Rpc.Caller.SteamId, connectionTarget.SteamId };

		using ( Rpc.FilterInclude( x => targets.Contains( x.SteamId ) ) )
		{
			CreateConversationRpcResponse( conversation );
		}
	}

	[Broadcast( NetPermission.Anyone )]
	private void LoadConversationsRpcRequest( PhoneNumber phoneNumber )
	{
		if ( !Networking.IsHost ) return;

		var conversations = RoverDatabase.Instance.Select<ConversationData>( x =>
			x.Participants.Any( p => p.PhoneNumber == phoneNumber ) );

		using ( Rpc.FilterInclude( x => x == Rpc.Caller ) )
		{
			LoadConversationsRpcResponse( conversations );
		}
	}

	[Broadcast( NetPermission.Anyone )]
	public void SendMessageRpcRequest( Guid conversationId, MessageData message )
	{
		if ( !Networking.IsHost ) return;

		if ( ServerTryGetConversation( conversationId, out var conversation ) )
		{
			Log.Error( "Failed to find conversation with id: " + conversationId );
			return;
		}

		Log.Info( "Add message to conversation: " + conversationId );

		var targets = new List<ulong>();

		foreach ( var participant in conversation.Participants )
		{
			var simcard =
				RoverDatabase.Instance.SelectOne<SimCardData>( x => x.PhoneNumber == participant.PhoneNumber );
			if ( simcard is null ) continue;

			targets.Add( simcard.Owner.SteamId );
		}

		using ( Rpc.FilterInclude( x => targets.Contains( x.SteamId ) ) )
		{
			SendMessageRpcResponse( conversationId, message );
		}
	}

	[Broadcast( NetPermission.HostOnly )]
	private void SendMessageRpcResponse( Guid conversationId, MessageData message )
	{
		var conversation = Conversations.FirstOrDefault( x => x.Id == conversationId );

		// If one of the participant didn't have opened there phone
		// The conversation will be null because conversation are loaded when the phone is opened
		if ( conversation is null )
		{
			// We load conversations to be sure that the conversation is loaded
			// Maybe need to wait a bit before getting the conversation,
			// Because the conversation might not be inserted in the database yet
			LoadConversations();

			conversation = Conversations.FirstOrDefault( x => x.Id == conversationId );
			if ( conversation is null ) return;
		}

		conversation.Messages.Add( message );
		Scene.RunEvent<IMessageEvent>( x => x.OnMessageReceived( message ), true );
	}
}
