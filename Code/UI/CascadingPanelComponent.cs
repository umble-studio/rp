using System;

namespace Rp.UI;

public partial class CascadingPanelComponent : PanelComponent
{
	private int _hash;
	private bool _isDirty;
	private int _renderCount;

	protected override void OnTreeFirstBuilt()
	{
		DoCascade( true );
		OnAfterRender( true );
	}

	protected override void OnTreeBuilt()
	{
		DoCascade( false );
		OnAfterRender( false );
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
