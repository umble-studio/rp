using System;
using Sandbox.Razor;
using Sandbox.UI;

namespace Rp.Phone.UI.Components;

public sealed partial class NavigationBar : Panel
{
	public RenderFragment? Header { get; set; } = null!;
	public RenderFragment? Content { get; set; } = null!;

	public new Action? OnBack { get; set; } 
	
	private void Back()
	{
		OnBack?.Invoke();
	}
}
