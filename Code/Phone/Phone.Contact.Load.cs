using System;
using RoverDB;

namespace Rp.Phone;

public partial class Phone
{
	private void LoadContacts()
	{
		Contacts.Clear();
		
		if ( Current.SimCard is null )
		{
			Log.Info( "Unable to load contacts when sim card is null" );
			return;
		}

		var localContact = new PhoneContact
		{
			Owner = Current.SimCard.Id,
			ContactAvatar = null,
			ContactName = "You",
			ContactNumber = Current.SimCard.PhoneNumber
		};

		Contacts.AddContact( localContact );

		Log.Warning( "Load contacts.. " + Current.SimCard.Id );
		LoadContactsClientRpc( Current.SimCard.Id.ToString() );
	}

	#region Commands

	[ConCmd( "phone_reload_contacts" )]
	private static void ReloadContactsCmd()
	{
		Log.Info( "Reloading contacts.." );
		Current.LoadContacts();
	}

	#endregion

	#region RPC

	[Broadcast( NetPermission.Anyone )]
	private void LoadContactsClientRpc( string simCardId )
	{
		if ( !Networking.IsHost ) return;

		var contacts = RoverDatabase.Instance.Select<PhoneContact>( x => x.Owner == Guid.Parse( simCardId ) );

		using ( Rpc.FilterInclude( x => x == Rpc.Caller ) )
		{
			LoadContactsServerRpc( contacts );
		}
	}

	[Broadcast( NetPermission.HostOnly )]
	private void LoadContactsServerRpc( List<PhoneContact> contacts )
	{
		foreach ( var contact in contacts )
			Contacts.AddContact( contact );

		Log.Info( $"Load {Contacts.Count} contacts" );
	}

	#endregion
}
