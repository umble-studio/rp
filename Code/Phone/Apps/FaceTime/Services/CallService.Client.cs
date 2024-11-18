using System;

namespace Rp.Phone.Apps.FaceTime.Services;

public partial class CallService
{
	private SoundHandle? _callingSound;

	public void StartCall( PhoneNumber target )
	{
		if ( !CanCall ) return;

		// The player cannot call itself...
		if ( Phone.Local.SimCard?.PhoneNumber == target ) return;

		var me = Phone.Local.SimCard?.PhoneNumber;
		if ( me is null ) return;

		using var _ = Rpc.FilterInclude( x => x.IsHost );

		// var meParticipant = new Participant
		// {
		// 	Contact = me, IsMuted = false, IsSpeakerEnabled = false, IsVideoEnabled = false
		// };

		var incomingCallInfo = new IncomingCallRequest
		{
			CallId = Guid.NewGuid(), Caller = me.Value, Target = target, CreatedAt = DateTime.Now
		};

		_callingSound = Sound.Play( "sounds/phone/call_to_us.sound" );
		StartCallRpcRequest( incomingCallInfo );
	}

	public void EndCall()
	{
		if ( CurrentCallId is null || CurrentCallId == Guid.Empty )
		{
			Log.Error("CurrentCallId is null or empty");
			return;
		}

		using var _ = Rpc.FilterInclude( x => x.IsHost );
		EndCallRpcRequest( CurrentCallId.Value );
	}
}
