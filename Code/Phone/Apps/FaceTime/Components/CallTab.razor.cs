using System;
using Rp.Phone.Apps.FaceTime.Services;
using Rp.Phone.UI.Components;
using Rp.UI;
using Sandbox.UI;

namespace Rp.Phone.Apps.FaceTime.Components;

public sealed partial class CallTab : PhoneNavigationPage, INavigationEvent
{
	private PhoneContact? _currentContact;
	private string _speakerIcon = "fluent:speaker-off-28-filled";
	private string _muteIcon = "fluent:mic-28-filled";
	private bool _isMuted;
	private bool _isSpeaker;

	public override string PageName => "Call";

	private void OnSpeakerClicked( bool toggle )
	{
		_isSpeaker = toggle;
		_speakerIcon = toggle ? "fluent:speaker-28-filled" : "fluent:speaker-off-28-filled";
	}

	private void OnMuteClicked( bool toggle )
	{
		_isMuted = toggle;
		_muteIcon = toggle ? "fluent:mic-off-28-filled" : "fluent:mic-28-filled";

		if ( _isMuted ) Sound.Play( "sounds/phone/call_mute.sound" );
		else Sound.Play( "sounds/phone/call_unmute.sound" );
	}

	private void OnKeypad()
	{
		Host.Navigate<KeypadTab>();
	}

	private void OnBack( PanelEvent e )
	{
		Host.Navigate<FavoriteTab>();
	}

	private void OnContactInfo()
	{
	}

	private void EndCall()
	{
		var callService = Phone.Local.GetService<CallService>();

		if ( callService.IsOccupied )
		{
			Log.Info( "End call" );
			callService.EndCall();
		}
	}

	public void OnNavigationOpen( INavigationPage page, params object[] args )
	{
		if ( page is not CallTab ) return;

		if ( args[0] is PhoneContact contact )
		{
			_currentContact = contact;
		}

		Phone.StatusBar.TextPhoneTheme = PhoneTheme.Light;
		Phone.StatusBar.BackgroundPhoneTheme = PhoneTheme.Light;
	}

	protected override int ShouldRender() =>
		HashCode.Combine( base.ShouldRender(), _currentContact, _isSpeaker,
			_isMuted );

	public enum View
	{
		IncomingCall,
		OutgoingCall
	}
}
