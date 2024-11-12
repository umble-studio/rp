using System;
using Sandbox.Razor;
using Sandbox.UI;

namespace Rp.Phone.Apps.Messages.Components;

public sealed partial class MessageBar : Panel
{
	public RenderFragment Content { get; set; } = null!;

	public new Action? OnBack { get; set; }
	
	private void Back()
	{
		OnBack?.Invoke();
	}
}
