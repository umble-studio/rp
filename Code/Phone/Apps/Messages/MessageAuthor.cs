using System;

namespace Rp.Phone.Apps.Messages;

public readonly struct MessageAuthor : IPhoneNumber, IEquatable<MessageAuthor>
{
	public PhoneNumber PhoneNumber { get; init; }

	public bool Equals(MessageAuthor other)
	{
		return PhoneNumber.Equals(other.PhoneNumber);
	}

	public override bool Equals(object? obj)
	{
		return obj is MessageAuthor other && Equals(other);
	}

	public override int GetHashCode()
	{
		return PhoneNumber.GetHashCode();
	}

	public static bool operator ==( MessageAuthor left, MessageAuthor right )
	{
		return left.Equals( right );
	}

	public static bool operator !=( MessageAuthor left, MessageAuthor right )
	{
		return !(left == right);
	}
}
