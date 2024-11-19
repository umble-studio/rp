namespace Rp.Phone.Apps.Messages.Services;

[Category( "Phone" )]
public sealed partial class ConversationService : Component, IPhoneService
{
	private Phone Phone { get; set; } = null!;

	protected override void OnStart()
	{
		Phone = GameObject.GetComponent<Phone>();
	}
}
