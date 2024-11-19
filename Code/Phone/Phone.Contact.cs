using System;
using RoverDB;
using RoverDB.Attributes;

namespace Rp.Phone;

public partial class Phone
{
	public PhoneContact LocalContact => Contacts.GetContactByNumber( SimCard!.PhoneNumber )!;

	public PhoneContacts Contacts { get; private set; } = new();

	public sealed class PhoneContacts
	{
		private readonly List<PhoneContact> _values = new();

		public int Count => All.Count;
		public List<PhoneContact> All => _values;

		public void AddContact( PhoneContact contact )
		{
			// The contact already exists
			if ( TryGetContactByNumber( contact.ContactNumber, out _ ) ) return;

			_values.Add( contact );
		}

		public void RemoveContact( PhoneNumber number )
		{
			// The contact doesn't exist, do nothing
			if ( !TryGetContactByNumber( number, out var contact ) ) return;

			_values.Remove( contact );
		}

		public PhoneContact? GetContactByNumber( PhoneNumber number )
		{
			var contact = _values.Find( x => x.ContactNumber == number );
			return contact;
		}

		public bool TryGetContactByNumber( PhoneNumber number, out PhoneContact contact )
		{
			var target = _values.Find( x => x.ContactNumber == number );

			if ( target is null )
			{
				contact = null!;
				return false;
			}

			contact = target;
			return true;
		}

		public void Clear()
		{
			_values.Clear();
		}
	}

	[Broadcast( NetPermission.Anyone )]
	private void CreateContactRpc( PhoneContact contact )
	{
		if ( !Networking.IsHost ) return;

		Log.Info( "Insert contact: " + contact.ContactNumber );
		RoverDatabase.Instance.Insert( contact );
	}
}

[Collection( "phone/contacts" )]
public record PhoneContact
{
	/// <summary>
	/// The sim card id that owns this contact
	/// </summary>
	[Id, Saved]
	public Guid Id { get; init; }

	[Saved] public Guid Owner { get; init; }
	[Saved] public string ContactName { get; init; } = null!;
	[Saved] public string? ContactAvatar { get; init; }
	[Saved] public PhoneNumber ContactNumber { get; init; }
}
