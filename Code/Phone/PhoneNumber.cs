using System;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Rp.Phone;

public readonly partial struct PhoneNumber : IEquatable<PhoneNumber>
{
	public int Value { get; init; }

	public PhoneNumber( int value )
	{
		var valueLength = value.ToString().Length;

		if ( valueLength > 7 )
			throw new ArgumentException( "The phone number is too long. The maximum length is 6 digits." );

		Value = value;
	}

	public bool Equals( PhoneNumber other )
	{
		return Value == other.Value;
	}

	public override bool Equals( object? obj )
	{
		return obj is PhoneNumber other && Equals( other );
	}

	public override int GetHashCode()
	{
		return Value;
	}

	public override string ToString()
	{
		return Value.ToString( "000_0000" );
	}

	public static implicit operator PhoneNumber( int number ) => new(number);
	public static implicit operator int( PhoneNumber number ) => number.Value;

	public static bool operator ==( PhoneNumber left, PhoneNumber right )
	{
		return left.Equals( right );
	}

	public static bool operator !=( PhoneNumber left, PhoneNumber right )
	{
		return !(left == right);
	}

	public static PhoneNumber Parse( string value ) => new(int.Parse( value ));
}
