using System;

namespace Rp.Phone;

public readonly partial struct PhoneNumber
{
	public static PhoneNumber Generate()
	{
		var random = new Random();
		var value = random.Next( 100_0000, 999_9999 );

		return new PhoneNumber( value );
	}
}
