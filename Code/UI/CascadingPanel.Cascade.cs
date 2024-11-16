using System.Reflection;

namespace Rp.UI;

public partial class CascadingPanel
{
	private void DoCascade( bool firstTime )
	{
		if ( !firstTime )
		{
			if ( !_isDirty ) return;
			_isDirty = false;
		}
		
		IterateChildren( Ancestors );
	}

	private void IterateChildren( IEnumerable<Sandbox.UI.Panel> children )
	{
		foreach ( var child in children )
			Check( child );
	}

	private void Check( Sandbox.UI.Panel panel )
	{
		if ( panel is not CascadingValue cascade ) return;

		var props = GetType()
			.GetProperties( BindingFlags.Public | BindingFlags.Instance )
			.ToList();

		foreach ( var prop in props )
		{
			var found = prop.IsCascadingProperty( cascade.Name, cascade.Value.GetType() );
			if ( !found ) continue;

			prop.SetValue( this, cascade.Value );
			
			// I don't know if this is needed
			// StateHasChanged();
		}
	}
}
