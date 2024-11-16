using System;
using Rp.Phone.UI.Components;
using Rp.UI;
using Sandbox.UI;

namespace Rp.Phone.Apps.FaceTime.Components;

public sealed partial class CallTab : NavigationPage, INavigationEvent
{
	private PhoneContact? _phoneContact;
	private string _speakerIcon = "fluent:speaker-off-28-filled";
	private string _muteIcon = "fluent:mic-28-filled";
	private bool _isMuted;
	private bool _isSpeaker;

	public override string PageName => "Call";

	public FaceTimeApp App { get; set; } = null!;

	protected override void OnAfterTreeRender( bool firstTime )
	{
		if ( !firstTime ) return;
	}

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
		// App.SwitchToContacts();
		Host.Navigate<FavoriteTab>();
	}

	private void OnContactInfo( PanelEvent e )
	{
	}

	public void OnNavigationOpen( INavigationPage page, params object[] args )
	{
		if ( page is not CallTab ) return;
		_phoneContact = args[0] as PhoneContact;

		Phone.Current.StatusBar.TextPhoneTheme = PhoneTheme.Light;
		Phone.Current.StatusBar.BackgroundPhoneTheme = PhoneTheme.Light;
	}

	protected override int BuildHash() =>
		HashCode.Combine( _phoneContact, _isSpeaker, _isMuted );
}
