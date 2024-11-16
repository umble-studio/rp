using System;

namespace Rp.UI;

public partial class CascadingPanel : Sandbox.UI.Panel
{
	private int _hash;
	private bool _isDirty;
	private int _renderCount;
	
	protected override void OnAfterTreeRender( bool firstTime )
	{
		DoCascade( firstTime );
		OnAfterRender( firstTime );
	}
	
	public new void StateHasChanged()
	{
		_renderCount++;
		base.StateHasChanged();
	}
	
	protected override int BuildHash()
	{
		var newHash = HashCode.Combine( ShouldRender(), _renderCount );
		if ( _hash == newHash ) return newHash;

		_hash = newHash;
		_isDirty = true;

		return newHash;
	}
	
	protected virtual void OnAfterRender( bool firstRender )
	{
	}

	protected virtual int ShouldRender() => 0;
}
