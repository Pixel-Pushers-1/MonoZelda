using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

namespace MonoZelda.Sound;

public static class SoundManager
{
    private static ContentManager contentManager;
    private static Dictionary<string, SoundEffectInstance> soundEffects;
    private static bool Muted;

    // Static initialization method to set up ContentManager and other resources
    public static void Initialize(ContentManager content)
    {
        contentManager = content;
        soundEffects = new Dictionary<string, SoundEffectInstance>();
        Muted = false;  
    }

    public static void PlaySound(string soundName, bool Looped)
    {
        if (!soundEffects.ContainsKey(soundName))
        {
            // load sound
            string soundFilePath = "Sound/" + soundName;
            SoundEffect soundEffect = contentManager.Load<SoundEffect>(soundFilePath);
            SoundEffectInstance soundInstance = soundEffect.CreateInstance();
            soundInstance.IsLooped = Looped;

            // Add sound instance to the dictionary
            soundEffects.Add(soundName, soundInstance);

            // play sound
            soundInstance.Play();
        }
        else
        {
            soundEffects[soundName].Play();
        }
    }

    public static void StopSound(string soundName)
    {
        if (soundEffects.ContainsKey(soundName))
        {
            SoundEffectInstance soundInstance = soundEffects[soundName];
            soundInstance.Stop();
            soundEffects.Remove(soundName);
        }
    }

    public static void ChangeMuteState()
    {
        if (Muted == false)
        {
            foreach (SoundEffectInstance soundEffect in soundEffects.Values)
            {
                soundEffect.Volume = 0;
            }
            Muted = true;
        }
        else
        {
            foreach (SoundEffectInstance soundEffect in soundEffects.Values)
            {
                soundEffect.Volume = 1;
            }
            Muted = false;
        }
    }

    public static void ClearSoundDictionary()
    {
        soundEffects.Clear();
    }
}
