using System;
using Rp.UI.Extensions;
using Sandbox.UI;

namespace Rp.UI;

public abstract class NavigationHost : Panel
{
	private readonly List<Type> _pages = new();
	private readonly List<NavigationPage> _instances = new();
	private NavigationPage? _currentPage;

	protected abstract Panel Container { get; set; }
	protected Type? DefaultPage { get; set; } = null!;

	protected override void OnAfterTreeRender( bool firstTime )
	{
		Container.Style.FlexGrow = 1;

		foreach ( var page in _pages )
		{
			if ( GetPage( page ) is not null ) continue;

			var instance = TypeLibrary.Create<INavigationPage>( page );

			var nav = instance as NavigationPage;
			nav!.Host = this;

			_instances.Add( nav );
			Container.AddChild( nav );
		}

		if ( !firstTime ) return;
		if ( DefaultPage is null ) return;

		Navigate( DefaultPage );
	}

	protected void RegisterPage<T>() where T : INavigationPage, new()
	{
		if ( IsRegistered<T>() ) return;
		_pages.Add( typeof(T) );
	}

	public void Navigate( Type type, params object[] args )
	{
		var page = GetPage( type );
		if ( page is null ) return;

		if ( _currentPage is not null )
		{
			Scene.RunEvent<INavigationEvent>( x => x.OnNavigationClose( _currentPage ), true );
			
			_currentPage.Style.ZIndex = 0;
			_currentPage.Hide();
		}

		_currentPage = page;
		_currentPage.Show();
		_currentPage.Style.ZIndex = 10;

		Scene.RunEvent<INavigationEvent>( x => x.OnNavigationOpen( _currentPage, args ), true );
	}

	public void Navigate<T>( params object[] args ) where T : INavigationPage, new()
	{
		Navigate( typeof(T), args );
	}

	private NavigationPage? GetPage( Type type )
	{
		return _instances.FirstOrDefault( x => x.GetType() == type );
	}

	private NavigationPage? GetPage<T>() where T : INavigationPage => GetPage( typeof(T) );

	private bool IsRegistered( TypeDescription type )
	{
		return _pages.Exists( x => x.Name == type.Name );
	}

	private bool IsRegistered<T>() where T : INavigationPage, new()
	{
		var type = TypeLibrary.GetType<T>();
		return IsRegistered( type );
	}

	protected override int BuildHash() => HashCode.Combine( _currentPage, _pages.Count, _instances.Count, Container );
}
