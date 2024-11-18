using System;
using Rp.Phone.Apps.FaceTime.Services;
using Rp.Phone.UI.Components;
using Rp.UI;
using Sandbox.UI;

namespace Rp.Phone.Apps.FaceTime.Components;

public sealed partial class IncomingCallTab : PhoneNavigationPage, INavigationEvent
{
	private CallService.IncomingCallRequest? _incomingCallRequest;

	public override string PageName => "Incoming Call";

	private void AcceptCall()
	{
		var callService = Phone.Local.GetService<CallService>();

		if ( callService.IsOccupied ) return;
		if ( _incomingCallRequest is null ) return;

		Log.Info( "Accept incoming call from phone: " + _incomingCallRequest.Value.Caller );
		callService.AcceptIncomingCallRpcRequest( _incomingCallRequest.Value.CallId );
	}

	private void RejectCall()
	{
		var callService = Phone.Local.GetService<CallService>();
		if ( _incomingCallRequest is null ) return;

		callService.RejectIncomingCallRpcRequest( _incomingCallRequest.Value.CallId );
	}

	public void OnNavigationOpen( INavigationPage page, params object[] args )
	{
		if ( page is not IncomingCallTab ) return;

		if ( args[0] is CallService.IncomingCallRequest request )
		{
			_incomingCallRequest = request;
		}

		Phone.Local.StatusBar.TextPhoneTheme = PhoneTheme.Light;
		Phone.Local.StatusBar.BackgroundPhoneTheme = PhoneTheme.Light;
	}

	protected override int ShouldRender() =>
		HashCode.Combine( base.ShouldRender(), _incomingCallRequest );

	public enum View
	{
		IncomingCall,
		OutgoingCall
	}
}
