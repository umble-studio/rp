using System;

namespace Rp.Phone;

public sealed class AppNotificationBuilder
{
	private readonly IPhoneApp _app;
	private string _title = string.Empty;
	private string _message = string.Empty;

	public AppNotificationBuilder( IPhoneApp app )
	{
		_app = app;
	}

	public AppNotificationBuilder WithTitle( string title )
	{
		_title = title;
		return this;
	}

	public AppNotificationBuilder WithMessage( string message )
	{
		_message = message;
		return this;
	}

	public AppNotification Build() => new()
	{
		App = _app,
		Title = _title,
		Message = _message,
		Date = DateTime.Now
	};
}
