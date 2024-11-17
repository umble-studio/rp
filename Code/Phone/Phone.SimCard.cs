using System;
using RoverDB;
using RoverDB.Attributes;
using Rp.Core;
using SteamId = Rp.Core.SteamId;

namespace Rp.Phone;

public partial class Phone
{
	private SimCardData? _simCard;
	private bool _isSimCardLoaded;

	public SimCardData? SimCard
	{
		get => _simCard;
		set
		{
			if ( _simCard == value ) return;
			_simCard = value;

			Scene.RunEvent<IPhoneEvent>( x => x.OnPhoneSimCardChanged( value! ) );
		}
	}

	#region Commands

	[ConCmd( "phone_create_simcard" )]
	private void CreateSimCardCmd( ulong steamId, int characterId, int phoneNumber )
	{
		var simCard = new SimCardData
		{
			Owner = new CharacterId( steamId, (ushort)characterId ), PhoneNumber = phoneNumber
		};

		CreateSimCardRpc( simCard );
	}

	#endregion

	private void LoadSimCard( SteamId steamId, CharacterId characterId )
	{
		_isSimCardLoaded = false;
		LoadSimCardsClientRpc( steamId, characterId );
	}

	#region RPC

	[Broadcast( NetPermission.Anyone )]
	private void LoadSimCardsClientRpc( SteamId steamId, CharacterId characterId )
	{
		if ( !Networking.IsHost ) return;

		using ( Rpc.FilterInclude( x => x == Rpc.Caller ) )
		{
			var simCard =
				RoverDatabase.Instance.SelectOne<SimCardData>( x =>
					x.Owner == characterId );

			LoadSimCardsServerRpc( simCard );
		}
	}

	[Broadcast( NetPermission.HostOnly )]
	private void LoadSimCardsServerRpc( SimCardData? simCard )
	{
		if ( simCard is null )
		{
			Log.Warning( "No sim card found for phone" );
			return;
		}

		SimCard = simCard;
		_isSimCardLoaded = true;

		Log.Info( $"Load sim card: {simCard.PhoneNumber} to phone" );
	}

	[Broadcast( NetPermission.Anyone )]
	private void CreateSimCardRpc( SimCardData simCard )
	{
		if ( !Networking.IsHost ) return;

		Log.Info( $"Insert sim card: {simCard.PhoneNumber} to database" );
		RoverDatabase.Instance.Insert( simCard );
	}

	#endregion
}

/// <summary>
/// Represents a sim card for the phone
/// </summary>
[Collection( "phone/sim_cards" )]
public record SimCardData
{
	[Id, Saved] public Guid Id { get; init; }
	[Saved] public CharacterId Owner { get; init; }
	[Saved] public PhoneNumber PhoneNumber { get; init; }

	public override string ToString() => PhoneNumber.ToString();
}
