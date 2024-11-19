using System;
using Rp.Phone.Apps.FaceTime.Services;
using Rp.UI;

namespace Rp.Phone.Apps.FaceTime.Components;

public sealed partial class KeypadTab : PhoneNavigationPage
{
	private string _phoneNumber = string.Empty;

	public override string PageName => "Keypad";

	private string PhoneNumberFormat()
	{
		if ( _phoneNumber.Length < 4 )
			return _phoneNumber;

		var firstPart = _phoneNumber[..3];
		var secondPart = _phoneNumber[3..];

		return $"{firstPart}-{secondPart}";
	}

	private void AddNumber( string number )
	{
		if ( _phoneNumber.Length + 1 > 7 ) return;
		_phoneNumber += number;
	}

	private void NumberOne() => AddNumber( "1" );
	private void NumberTwo() => AddNumber( "2" );
	private void NumberThree() => AddNumber( "3" );
	private void NumberFour() => AddNumber( "4" );
	private void NumberFive() => AddNumber( "5" );
	private void NumberSix() => AddNumber( "6" );
	private void NumberSeven() => AddNumber( "7" );
	private void NumberEight() => AddNumber( "8" );
	private void NumberNine() => AddNumber( "9" );
	private void NumberZero() => AddNumber( "0" );

	private void Star()
	{
	}

	private void Hash()
	{
	}

	private void Backspace()
	{
		if ( _phoneNumber.Length > 0 )
			_phoneNumber = _phoneNumber[..^1];
	}

	private void Call()
	{
		// TODO - Do a phone screen shake or something like that
		if ( string.IsNullOrEmpty( _phoneNumber ) ) return;
		if ( !int.TryParse( _phoneNumber, out var number ) ) return;
		
		// Don't call if the number is less than 7 digits
		if ( number.ToString().Length < 7 ) return;

		var phoneNumber = (PhoneNumber)number;
		var callService = Phone.Local.GetService<CallService>();

		var canCall = callService.StartOutgoingCall( phoneNumber );
		if ( !canCall ) return;

		var app = Phone.Local.GetApp<FaceTimeApp>();
		var tab = app.NavHost.Navigate<CallTab>();

		var fakeContact = new PhoneContact { ContactNumber = phoneNumber };
		tab.ShowPendingCallView( fakeContact );
	}

	protected override int ShouldRender()
	{
		return HashCode.Combine( base.ShouldRender(), PhoneNumberFormat() );
	}
}
