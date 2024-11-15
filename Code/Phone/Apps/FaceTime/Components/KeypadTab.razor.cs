using System;
using Rp.UI;

namespace Rp.Phone.Apps.FaceTime.Components;

public sealed partial class KeypadTab : NavigationPage
{
	private string _phoneNumber = string.Empty;

	public override string PageName => "Keypad";

	private string PhoneNumberFormat()
	{
		if ( string.IsNullOrEmpty( _phoneNumber ) )
			return string.Empty;
		
		if(_phoneNumber.Length < 4)
			return $"{int.Parse( _phoneNumber ):###}";

		var firstPart = _phoneNumber.AsSpan( 0, 3 );
		var secondPart = _phoneNumber.AsSpan( 3 );
		
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
	}

	protected override int BuildHash()
	{
		return HashCode.Combine( base.BuildHash(), _phoneNumber );
	}
}
