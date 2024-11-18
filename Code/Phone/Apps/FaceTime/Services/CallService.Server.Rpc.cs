using System;

namespace Rp.Phone.Apps.FaceTime.Services;

public partial class CallService
{
	[Broadcast( NetPermission.Anyone )]
	private void StartCallRpcRequest( IncomingCallRequest incomingCallInfo )
	{
		if ( !Networking.IsHost ) return;

		// Log.Info( "StartCallRpcRequest: " + me + ", " + target );

		var isReadyToCall = ServerIsParticipantReadyToCall( incomingCallInfo.Caller );
		Log.Info( "isReadyToCall: " + isReadyToCall );

		var isTargetReadyToCall = ServerIsParticipantReadyToCall( incomingCallInfo.Target );
		Log.Info( "isTargetReadyToCall: " + isTargetReadyToCall );

		if ( isReadyToCall && !isTargetReadyToCall )
		{
			Log.Info( "Cancel call: " + incomingCallInfo.Caller + ", " + incomingCallInfo.Target );
			CancelCallRpcResponse();
			return;
		}

		var phones = Scene.GetAllComponents<Phone>()
			.Where( x => x.Network.Active && x.SimCard is not null )
			.ToList();

		var firstPhone = phones.FirstOrDefault( x => x.SimCard!.PhoneNumber == incomingCallInfo.Caller );
		var secondPhone = phones.FirstOrDefault( x => x.SimCard!.PhoneNumber == incomingCallInfo.Target );

		if ( firstPhone is null || secondPhone is null )
		{
			Log.Info( "One of the phones is null: " + (firstPhone is null) + ", " + (secondPhone is null) );
			return;
		}

		// Log.Info("firstPhone: " + (firstPhone is null) + ", " + firstPhone.Network.Owner);
		// Log.Info("secondPhone: " + (secondPhone is null) + ", " + secondPhone.Network.Owner);

		// Send a request to the other phone to start the call

		// var incomingCall = new IncomingCallInfo() { CallId = Guid.NewGuid(), Caller = me, };

		// var callService = secondPhone.GetService<CallService>();
		// callService.IncomingCall = incomingCall;

		_pendingIncomingCallsRequests[incomingCallInfo.CallId] = incomingCallInfo;

		// The other phone is now waiting for the call to be accepted
		var callService = firstPhone.GetService<CallService>();
		callService.CurrentCallId = incomingCallInfo.CallId;
		callService.IsOutgoingCallCallPending = true;

		// We tell the other phone that we are waiting for a call response
		callService = secondPhone.GetService<CallService>();
		callService.IsIncomingCallPending = true;

		// Sending the incoming call request to the target phone
		{
			var targetConnection = secondPhone.Network.Owner;

			using ( Rpc.FilterInclude( x => x == targetConnection ) )
				ShowIncomingCallTabRpcRequest( incomingCallInfo );
		}
	}

	[Broadcast( NetPermission.Anyone )]
	public void AcceptIncomingCallRpcRequest( Guid callId )
	{
		if ( !Networking.IsHost ) return;

		if ( !_pendingIncomingCallsRequests.Remove( callId, out var incomingCallRequest ) )
		{
			Log.Info( "Call not found: " + callId );
			return;
		}
		
		var phones = Scene.GetAllComponents<Phone>()
			.Where( x => x.Network.Active && x.SimCard is not null )
			.ToList();
		
		var firstPhone = phones.FirstOrDefault( x => x.SimCard!.PhoneNumber == incomingCallRequest.Caller );
		var secondPhone = phones.FirstOrDefault( x => x.SimCard!.PhoneNumber == incomingCallRequest.Target );

		if ( firstPhone is null || secondPhone is null )
		{
			Log.Info( "One of the phones is null: " + (firstPhone is null) + ", " + (secondPhone is null) );
			return;
		}
		
		// We set the current call state as occupied
		{
			var callService = firstPhone.GetService<CallService>();
			callService.CurrentCallId = callId;
			callService.IsOutgoingCallCallPending = false;
			callService.IsOccupied = true;

			callService = secondPhone.GetService<CallService>();
			callService.CurrentCallId = callId;
			callService.IsIncomingCallPending = false;
			callService.IsOccupied = true;
		}
	}

	[Broadcast( NetPermission.Anyone )]
	public void RejectIncomingCallRpcRequest( Guid callId )
	{
		if ( !Networking.IsHost ) return;
		_pendingIncomingCallsRequests.Remove( callId );
	}

	[Broadcast( NetPermission.Anyone )]
	private void CancelOutgoingCallCallRpcRequest( Guid callId )
	{
		if ( !Networking.IsHost ) return;
		_pendingIncomingCallsRequests.Remove( callId );
	}

	[Broadcast( NetPermission.Anyone )]
	private void EndCallRpcRequest( Guid callId )
	{
	}
}
