using System;

namespace Rp.UI.Extensions;

public static class SceneExtensions
{
	private static void RunEventInPanel<T>( this PanelComponent component, Action<T> action )
	{
		var panels = component.Panel.Descendants.OfType<T>().ToList();

		for ( var i = 0; i < panels.Count; i++ )
		{
			var c = panels[i];

			try
			{
				action( c );
			}
			catch ( Exception e )
			{
				Log.Warning( e, e.Message );
			}
		}
	}

	public static void RunEvent<T>( this Scene scene, Action<T> action, bool includePanels )
	{
		var components = scene.Components.GetAll( FindMode.EnabledInSelfAndDescendants ).ToList();

		for ( var i = 0; i < components.Count; i++ )
		{
			var c = components[i];

			try
			{
				if ( c is T t )
				{
					action( t );
				}

				if ( includePanels && c is PanelComponent component )
					component.RunEventInPanel( action );
			}
			catch ( Exception e )
			{
				Log.Warning( e, e.Message );
				throw;
			}
		}
	}
}
