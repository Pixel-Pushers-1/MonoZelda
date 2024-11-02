using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

namespace MonoZelda.Sound;

public static class SoundManager
{    
    private static ContentManager contentManager;
    private static Dictionary<string, SoundEffectInstance> loopedSoundEffects;

    // Static initialization method to set up ContentManager and other resources
    public static void Initialize(ContentManager content)
    {
        contentManager = content;
        loopedSoundEffects = new Dictionary<string, SoundEffectInstance>();
    }

    public static void PlaySound(string soundName, bool Looped)
    {
        if (!loopedSoundEffects.ContainsKey(soundName))
        {
            //load sound
            string soundFilePath = "Sound/" + soundName;
            SoundEffect soundEffect = contentManager.Load<SoundEffect>(soundFilePath);
            SoundEffectInstance soundInstance = soundEffect.CreateInstance();

            soundInstance.IsLooped = Looped;
            if (Looped == true)
            {
                loopedSoundEffects.Add(soundName, soundInstance);
            }

            // play sound
            soundInstance.Play();
        }
    }

    public static void StopSound(string soundName)
    {
        if (loopedSoundEffects.ContainsKey(soundName))
        {
            SoundEffectInstance soundInstance = loopedSoundEffects[soundName];
            soundInstance.Stop();
        }
    }
}
