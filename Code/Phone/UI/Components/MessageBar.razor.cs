using System;
using Sandbox.Razor;
using Sandbox.UI;

namespace Rp.Phone.UI.Components;

public sealed partial class MessageBar : Panel
{
	public RenderFragment Content { get; set; } = null!;

	public new Action? OnBack { get; set; }
	
	private void Back()
	{
		OnBack?.Invoke();
	}
}
