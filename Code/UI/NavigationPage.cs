using System;
using Sandbox.UI;

namespace Rp.UI;

public abstract class NavigationPage : Panel, INavigationPage
{
	private bool _isOpen;

	public abstract string PageName { get; }
	public NavigationHost Host { get; internal set; } = null!;

	public override void Tick()
	{
		BindClass( "hidden", () => !_isOpen );
	}

	internal void Show()
	{
		_isOpen = true;
	}

	internal void Hide()
	{
		_isOpen = false;
	}

	protected override int BuildHash() => HashCode.Combine( _isOpen );
}
