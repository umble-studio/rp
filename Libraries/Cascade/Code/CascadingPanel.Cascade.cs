using System.Collections.Generic;
using Sandbox;
using Sandbox.UI;

namespace Cascade;

public partial class CascadingPanel
{
	private readonly List<PropertyDescription> _cachedProperties = new();
	
	private void DoCascade( bool firstTime )
	{
		if ( !firstTime )
		{
			if ( !_isDirty ) return;
			_isDirty = false;
		}
		
		IterateChildren( Ancestors );
	}

	private void IterateChildren( IEnumerable<Panel> children )
	{
		foreach ( var child in children )
			Check( child );
	}

	private void Check( Panel panel )
	{
		if ( panel is not CascadingValue cascade ) return;

		var type = TypeLibrary.GetType( GetType() );
		var props = type.Properties;
		
		foreach ( var prop in props )
		{
			var found = prop.IsCascadingProperty( cascade.Name, cascade.Value.GetType() );
			if ( !found ) continue;

			prop.SetValue( this, cascade.Value );
			
			if ( _cachedProperties.Exists( x => x.Name == prop.Name ) ) 
				continue;
			
			_cachedProperties.Add( prop );

			// I don't know if this is needed
			// StateHasChanged();
		}
	}
}
