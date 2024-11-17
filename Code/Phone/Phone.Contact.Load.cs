using System;
using RoverDB;

namespace Rp.Phone;

public partial class Phone
{
	private void LoadContacts()
	{
		Contacts.Clear();
		
		if ( SimCard is null )
		{
			Log.Info( "Unable to load contacts when sim card is null" );
			return;
		}

		var localContact = new PhoneContact
		{
			Owner = SimCard.Id,
			ContactAvatar = null,
			ContactName = "You",
			ContactNumber = SimCard.PhoneNumber
		};

		Contacts.AddContact( localContact );

		Log.Warning( "Load contacts.. " + SimCard.Id );
		LoadContactsClientRpc( SimCard.Id.ToString() );
	}

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
