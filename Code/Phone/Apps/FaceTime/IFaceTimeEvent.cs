using System;
using Rp.Phone.Apps.FaceTime.Services;

namespace Rp.Phone.Apps.FaceTime;

public interface IFaceTimeEvent : ISceneEvent<IFaceTimeEvent>
{
	void OnCallStarted( IncomingCallRequest callId ) { }
	void OnCallEnded( CallResult callResult ) { }
	void OnCallAccepted( IncomingCallRequest incomingCallInfo ) { }
	void OnCallRejected( CallResult callResult ) { }
	void OnCallFailed( Guid callId ) { }
}
