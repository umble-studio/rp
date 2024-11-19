using System;
using Sandbox.Audio;

namespace Rp.Phone.Apps.FaceTime.Services;

internal static class PhoneCallMixerHelper
{
	public static void CreateVoiceCallMixer( this Voice voice, Guid callId )
	{
		var mixerName = $"phone-{callId}";
		var mixer = Mixer.FindMixerByName( mixerName );

		if ( mixer is not null )
		{
			Log.Info( "Skipping mixer creation: " + mixerName );
			return;
		}

		mixer = Mixer.Master.AddChild();
		mixer.Name = mixerName;
		mixer.Occlusion = 0;
		mixer.Spacializing = 0;
		mixer.AirAbsorption = 0;
		mixer.DistanceAttenuation = 0;
		mixer.Solo = false;
		mixer.Mute = false;

		voice.TargetMixer = mixer;
		voice.IsListening = true;
	}

	public static void DestroyVoiceCallMixer( this Voice voice, Guid callId )
	{
		voice.IsListening = false;

		var mixer = Mixer.FindMixerByName( $"phone-{callId}" );
		mixer?.Destroy();
	}
}
