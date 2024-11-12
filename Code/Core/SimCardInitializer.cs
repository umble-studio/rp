using System;
using RoverDB;
using Rp.Phone;

namespace Rp.Core;

// public sealed class SimCardInitializer : IComponentInitializer
// {
// 	public void OnActive( Connection channel )
// 	{
// 		// Check if simcard exists for the player
// 		Log.Warning( "SimCardInitializer: OnActive" );
//
// 		var simCards = RoverDatabase.Instance.Select<SimCardData>();
//
// 		var simCardExist = simCards.Exists( x => x.Owner == channel.SteamId );
// 		if ( simCardExist ) return;
//
// 		var phoneNumber = PhoneNumber.Generate();
//
// 		// Re-generate the phone number if it already exists
// 		while ( simCards.Exists( x => x.PhoneNumber == phoneNumber ) )
// 			phoneNumber = PhoneNumber.Generate();
//
// 		var newSimCard = new SimCardData { Owner = SteamId.Local, PhoneNumber = phoneNumber, CharacterId = Guid.Empty };
// 	}
// }
