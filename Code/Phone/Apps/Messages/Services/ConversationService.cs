namespace Rp.Phone.Apps.Messages.Services;

public sealed partial class ConversationService : Bindery.Singleton<ConversationService>, IPhoneService
{
	/// <summary>
	/// Load all conversations when the service	starts
	/// </summary>
	// protected override void OnStart()
	// {
	// 	Task.RunInThreadAsync( async () =>
	// 	{
	// 		while(Phone.Current.SimCard is null)
	// 			await GameTask.Delay(100);
	// 	
	// 		LoadConversations();
	// 	} );
	// }
}
