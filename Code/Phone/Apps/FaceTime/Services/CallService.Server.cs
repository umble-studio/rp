using System;

namespace Rp.Phone.Apps.FaceTime.Services;

public partial class CallService : Component.INetworkListener
{
	// private readonly Dictionary<Guid, Info> _serverCalls = new();

	private readonly Dictionary<Guid, IncomingCallRequest> _pendingIncomingCallsRequests = new();
	private const int MaxPendingIncomingCallDuration = 11;

	private void CheckForOutdatedIncomingCallsRequests()
	{
		foreach ( var (callId, incomingCall) in _pendingIncomingCallsRequests )
		{
			if ( DateTime.Now - incomingCall.CreatedAt > TimeSpan.FromSeconds( MaxPendingIncomingCallDuration ) )
			{
				Log.Info( "Removing outdated incoming call request: " + callId );
				
				_pendingIncomingCallsRequests.Remove( callId );
				CancelPendingOutcomingCallRpcResponse( callId );
			}
		}
	}

	public bool ServerIsParticipantReadyToCall( PhoneNumber participantNumber )
	{
		var participantPhone = Scene.GetAllComponents<Phone>().FirstOrDefault( x =>
			x.SimCard is not null && x.SimCard.PhoneNumber == participantNumber );

		if ( participantPhone is null )
			return false;

		var callService = participantPhone.GameObject.GetComponent<CallService>();
		return !callService.IsCalling;
	}

	// public void ServerEnqueueCall( Info callInfo ) => _serverCalls[callInfo.CallId] = callInfo;
	// public void ServerDequeueCall( Guid callId ) => _serverCalls.Remove( callId );
	// public bool ServerServerHasCall( Guid callId ) => _serverCalls.ContainsKey( callId );
	// public Info? ServerGetCall( Guid callId ) => CollectionExtensions.GetValueOrDefault( _serverCalls, callId );

	// public bool IsParticipantInCall( Participant participant ) => _serverCalls.Values.Any( x =>
	// 	x.Participants.Exists( a => a.Contact.ContactNumber == participant.Contact.ContactNumber ) );
}
