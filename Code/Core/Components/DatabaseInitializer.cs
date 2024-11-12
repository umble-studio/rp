using System;
using System.Text.Json.Nodes;
using RoverDB;
using RoverDB.Attributes;
using Rp.Phone;
using Rp.Phone.Apps.Messages;
using Sandbox.Utility;

namespace Rp.Core.Components;

public sealed class DatabaseInitializer : Component
{
	protected override void OnStart()
	{
		if ( !Networking.IsHost ) return;

#if DEBUG
		// {
		// 	var player = new PlayerData { Owner = Steam.SteamId };
		// 	var characterId = new CharacterId( Steam.SteamId, 0 );
		//
		// 	var character = new CharacterData { CharacterId = characterId, Firstname = "Robert", Lastname = "Smith", };
		//
		// 	if ( !RoverDatabase.Instance.Exists<CharacterData>( x => x.CharacterId == characterId ) )
		// 	{
		// 		RoverDatabase.Instance.Insert( character );
		// 	}
		//
		// 	var c = RoverDatabase.Instance.SelectOne<CharacterData>( x => x.CharacterId == characterId );
		// 	Log.Info( "Character: " + c?.Firstname );
		//
		// 	player.Characters.Add( characterId );
		//
		// 	if ( !RoverDatabase.Instance.Exists<PlayerData>( x => x.Characters.Any( a => a == characterId ) ) )
		// 	{
		// 		RoverDatabase.Instance.Insert( player );
		// 	}
		//
		// 	var simCard = new SimCard { Owner = characterId, PhoneNumber = new PhoneNumber( 555_1111 ) };
		//
		// 	if ( !RoverDatabase.Instance.Exists<SimCard>( x => x.PhoneNumber == simCard.PhoneNumber ) )
		// 	{
		// 		RoverDatabase.Instance.Insert( simCard );
		// 	}
		// }
		//
		// {
		// 	var player = new PlayerData { Owner = Steam.SteamId };
		// 	var characterId = new CharacterId( Steam.SteamId, 1 );
		//
		// 	var character = new CharacterData { CharacterId = characterId, Firstname = "Marry", Lastname = "Jane", };
		//
		// 	if ( !RoverDatabase.Instance.Exists<CharacterData>( x => x.CharacterId == characterId ) )
		// 	{
		// 		RoverDatabase.Instance.Insert( character );
		// 	}
		//
		// 	var c = RoverDatabase.Instance.SelectOne<CharacterData>( x => x.CharacterId == characterId );
		// 	Log.Info( "Character: " + c?.Firstname );
		//
		// 	player.Characters.Add( characterId );
		//
		// 	if ( !RoverDatabase.Instance.Exists<PlayerData>( x => x.Characters.Any( a => a == characterId ) ) )
		// 	{
		// 		RoverDatabase.Instance.Insert( player );
		// 	}
		//
		// 	var simCard = new SimCard { Owner = characterId, PhoneNumber = new PhoneNumber( 555_1111 ) };
		//
		// 	if ( !RoverDatabase.Instance.Exists<SimCard>( x => x.PhoneNumber == simCard.PhoneNumber ) )
		// 	{
		// 		RoverDatabase.Instance.Insert( simCard );
		// 	}
		// }
		//
		// {
		// 	if ( !RoverDatabase.Instance.Exists<ConversationData>( x =>
		// 		    x.Messages.Any( y => y.Author.PhoneNumber == 555_1111 ) ) )
		// 	{
		// 		var conversation = new ConversationData();
		//
		// 		conversation.Messages.Add( new MessageData
		// 		{
		// 			Author = new MessageAuthor
		// 			{
		// 				Name = "Marry",
		// 				Avatar = "textures/ui/phone/avatars/avatar_01.jpg",
		// 				PhoneNumber = 555_2222
		// 			},
		// 			Content = "Hello, how are you?",
		// 			Date = DateTime.Now
		// 		} );
		// 		conversation.Messages.Add( new MessageData
		// 		{
		// 			Author = new MessageAuthor
		// 			{
		// 				Name = "Robert",
		// 				Avatar = "textures/ui/phone/avatars/avatar_02.jpg",
		// 				PhoneNumber = 555_1111
		// 			},
		// 			Content = "Fine, and you?",
		// 			Date = DateTime.Now
		// 		} );
		//
		// 		RoverDatabase.Instance.Insert( conversation );
		// 	}
		//
		// 	var conversations = RoverDatabase.Instance.Select<ConversationData>();
		// 	Log.Info( "Conversations: " + conversations.Count );
		// }
#endif
	}
}
