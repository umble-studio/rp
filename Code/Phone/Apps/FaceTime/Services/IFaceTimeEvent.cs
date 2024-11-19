using System;

namespace Rp.Phone.Apps.FaceTime.Services;

public interface IFaceTimeEvent : ISceneEvent<IFaceTimeEvent>
{
	void OnCallStarted( Guid callId ) { }
	void OnCallEnded( CallResult callResult ) { }
	void OnCallAccepted( IncomingCallRequest incomingCallInfo ) { }
	void OnCallRejected( CallResult callResult ) { }
	void OnCallFailed( Guid callId ) { }
}
