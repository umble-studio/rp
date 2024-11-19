using System;
using Rp.UI;

namespace Rp.Phone.Apps.Messages.Components;

public sealed partial class NavHost : NavigationHost
{
	private const string PreviousTabCookieKey = "messages:current_tab";

	public NavHost()
	{
		RegisterPage<ChatTab>();
		RegisterPage<UserConversationsTab>();
	}

	protected override void OnNavigationReady()
	{
		if ( PhoneCookie.TryGetCookie<Type>( PreviousTabCookieKey, out var type ) )
		{
			Navigate( type );
			return;
		}

		GoToUserConversations();
	}

	public override INavigationPage? Navigate( Type type, params object[] args )
	{
		PhoneCookie.SetCookie( PreviousTabCookieKey, type );
		return base.Navigate( type, args );
	}

	public void GoToChat( ConversationData conversationData )
	{
		Navigate<ChatTab>( conversationData );
	}

	public void GoToUserConversations()
	{
		Navigate<UserConversationsTab>();
	}
}
