using System;
using Sandbox.UI;

namespace Rp.UI;

public partial class CascadingPanel : Panel
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
		var computedHash = 0;

		foreach ( var prop in _cachedProperties )
		{
			var value = prop.GetValue( this );
			var hash = value?.GetHashCode() ?? 0;
			computedHash = HashCode.Combine( computedHash, hash );
		}

		var newHash = HashCode.Combine( computedHash, ShouldRender(), _renderCount );
		
		if ( _hash == newHash ) 
			return _hash;

		_hash = newHash;
		_isDirty = true;

		return newHash;
	}

	protected virtual void OnAfterRender( bool firstRender )
	{
	}

	protected virtual int ShouldRender() => 0;
}
