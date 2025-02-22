using System;
using Cascade;
using Rp.UI.Extensions;
using Sandbox.UI;

namespace Rp.UI;

public abstract class NavigationHost : CascadingPanel
{
	private readonly List<Type> _pages = new();
	private readonly List<NavigationPage> _instances = new();

	protected Panel Container { get; set; } = null!;
	protected NavigationPage? CurrentPage { get; private set; }

	protected override void OnAfterRender( bool firstTime )
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
		OnNavigationReady();
	}

	protected virtual void OnNavigationReady()
	{
	}

	protected void RegisterPage<T>() where T : INavigationPage, new()
	{
		if ( IsRegistered<T>() ) return;
		_pages.Add( typeof(T) );
	}

	public virtual INavigationPage? Navigate( Type type, params object[] args )
	{
		var page = GetPage( type );

		if ( page is null )
			return null;

		foreach ( var p in _instances )
			p.Style.ZIndex = 0;

		if ( CurrentPage is not null )
		{
			Scene.RunEvent<INavigationEvent>( x => x.OnNavigationClose( CurrentPage ), true );

			CurrentPage.Style.ZIndex = 0;
			CurrentPage.Hide();
		}

		CurrentPage = page;
		CurrentPage.Show();
		CurrentPage.Style.ZIndex = 10;

		Scene.RunEvent<INavigationEvent>( x => x.OnNavigationOpen( CurrentPage, args ), true );
		return CurrentPage;
	}

	public T Navigate<T>( params object[] args ) where T : INavigationPage, new()
	{
		return (T)Navigate( typeof(T), args )!;
	}

	private NavigationPage? GetPage( Type type )
	{
		return _instances.FirstOrDefault( x => x.GetType() == type );
	}

	private bool IsRegistered( TypeDescription type )
	{
		return _pages.Exists( x => x.Name == type.Name );
	}

	private bool IsRegistered<T>() where T : INavigationPage, new()
	{
		var type = TypeLibrary.GetType<T>();
		return IsRegistered( type );
	}

	public bool IsOpen<T>() where T : INavigationPage, new()
	{
		return CurrentPage is T;
	}

	protected override int ShouldRender() =>
		HashCode.Combine( _pages.Count, _instances.Count, Container, CurrentPage );
}
