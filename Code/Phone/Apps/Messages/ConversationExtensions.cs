namespace Rp.Phone.Apps.Messages;

public static class ConversationExtensions
{
	/// <summary>
	/// Checks if the message was sent by the current user
	/// </summary>
	/// <param name="message"></param>
	/// <returns></returns>
	public static bool IsMe( this MessageData message, PhoneNumber phoneNumber ) =>
		message.Author.PhoneNumber == phoneNumber;
	// public static bool IsMe( this MessageData message ) =>
	// 	message.Author.PhoneNumber == Phone.Current.SimCard.PhoneNumber;
	
	/// <summary>
	/// Checks if the participant is the current user
	/// </summary>
	/// <param name="participant"></param>
	/// <returns></returns>
	public static bool IsMe( this ConversationParticipant participant, PhoneNumber phoneNumber ) =>
		participant.PhoneNumber == phoneNumber;
	
	/// <summary>
	/// Returns the latest message of the conversation
	/// </summary>
	/// <param name="conversation"></param>
	/// <returns></returns>
	public static MessageData? GetLatestMessage( this ConversationData conversation )
	{
		return conversation.Messages.OrderBy( x => x.Date ).LastOrDefault();
	}

	/// <summary>
	/// Returns the participant from an author
	/// </summary>
	/// <param name="conversation"></param>
	/// <param name="author"></param>
	/// <returns></returns>
	public static ConversationParticipant GetParticipant( this ConversationData conversation, MessageAuthor author )
	{
		return conversation.Participants.First( x => x.PhoneNumber == author.PhoneNumber );
	}
}
