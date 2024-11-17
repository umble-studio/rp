using Rp.Core;

namespace Rp.Phone;

public partial class Phone
{
	[ConCmd( "phone_reload_contacts" )]
	private void ReloadContactsCmd()
	{
		Log.Info( "Reloading contacts.." );
		
		LoadContacts();
	}

	[ConCmd( "phone_create_contact" )]
	private void CreateContactCmd( string phoneNumber, string name, string? avatar = null )
	{
		if ( SimCard is null )
		{
			Log.Warning( "Unable to create contact when sim card is null" );
			return;
		}

		var contact = new PhoneContact
		{
			Owner = SimCard.Id,
			ContactName = name,
			ContactAvatar = avatar,
			ContactNumber = PhoneNumber.Parse( phoneNumber )
		};

		CreateContactRpc( contact );
	}

	[ConCmd( "phone_create_simcard" )]
	private void CreateSimCardCmd( ulong steamId, int characterId, int phoneNumber )
	{
		var simCard = new SimCardData
		{
			Owner = new CharacterId( steamId, (ushort)characterId ), PhoneNumber = phoneNumber
		};

		CreateSimCardRpc( simCard );
	}
}
