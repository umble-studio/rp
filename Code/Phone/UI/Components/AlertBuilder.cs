using Sandbox.UI;

namespace Rp.Phone.UI.Components;

public sealed class AlertBuilder
{
	private Alert _alert = null!;
	private string? _title;
	private string? _message;
	private readonly Panel _parent;
	private readonly List<Button> _buttons = new();

	public AlertBuilder( Panel parent )
	{
		_parent = parent;
	}

	public AlertBuilder WithTitle( string title )
	{
		_title = title;
		return this;
	}

	public AlertBuilder WithMessage( string message )
	{
		_message = message;
		return this;
	}

	public AlertBuilder WithButton( Button button )
	{
		_buttons.Add( button );
		return this;
	}

	public Alert Build()
	{
		_alert = new Alert( _title!, _message!, _buttons ) { Parent = _parent };
		return _alert;
	}
}
