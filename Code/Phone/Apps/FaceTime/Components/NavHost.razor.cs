﻿using System;
using Rp.UI;

namespace Rp.Phone.Apps.FaceTime.Components;

public sealed partial class NavHost : NavigationHost
{
	private const string PreviousTabCookieKey = "facetime:current_tab";
	
	private bool IsFavoriteTab => CurrentPage is FavoriteTab;
	private bool IsRecentTab => CurrentPage is RecentTab;
	private bool IsContactsTab => CurrentPage is ContactsTab;
	private bool IsKeypadTab => CurrentPage is KeypadTab;

	public NavHost()
	{
		RegisterPage<FavoriteTab>();
		RegisterPage<RecentTab>();
		RegisterPage<ContactsTab>();
		RegisterPage<KeypadTab>();
		RegisterPage<IncomingCallTab>();
		RegisterPage<CallTab>();
	}

	protected override void OnNavigationReady()
	{
		if ( PhoneCookie.TryGetCookie<Type>( PreviousTabCookieKey, out var type ) )
		{
			Navigate( type );
			return;
		}
		
		GoToFavorite();
	}

	public override INavigationPage? Navigate( Type type, params object[] args )
	{
		PhoneCookie.SetCookie( PreviousTabCookieKey, type );
		return base.Navigate( type, args );
	}

	public void GoToFavorite()
	{
		Navigate<FavoriteTab>();
	}

	public void GoToRecent()
	{
		Navigate<RecentTab>();
	}

	public void GoToContacts()
	{
		Navigate<ContactsTab>();
	}

	public void GoToKeypad()
	{
		Navigate<KeypadTab>();
	}
}
