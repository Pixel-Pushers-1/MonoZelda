using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace MonoZelda.Sound;

public static class SoundManager
{
    private static ContentManager contentManager;
    private static Dictionary<string, SoundEffectInstance> gameSoundEffects;
    private static bool Paused;
    private static bool Muted;

    // Static initialization method to set up ContentManager and other resources
    public static void Initialize(ContentManager content)
    {
        contentManager = content;
        gameSoundEffects = new Dictionary<string, SoundEffectInstance>();
        Muted = false;
        Paused = false;
    }

    public static void PlaySound(string soundName, bool Looped)
    {
        if (!gameSoundEffects.ContainsKey(soundName))
        {
            // load sound
            string soundFilePath = "Sound/" + soundName;
            SoundEffect soundEffect = contentManager.Load<SoundEffect>(soundFilePath);
            SoundEffectInstance soundInstance = soundEffect.CreateInstance();
            soundInstance.IsLooped = Looped;

            // Add sound instance to the dictionary
            soundInstance.Volume = Muted ? 0 : 1;
            gameSoundEffects.Add(soundName, soundInstance);

            // play sound
            soundInstance.Play();
        }
        else
        {
            gameSoundEffects[soundName].Play();
        }
    }

    public static void StopSound(string soundName)
    {
        if (gameSoundEffects.ContainsKey(soundName))
        {
            SoundEffectInstance soundInstance = gameSoundEffects[soundName];
            soundInstance.Stop();
        }
    }

    public static void Pause(string soundName)
    {
        SoundEffectInstance soundEffect = gameSoundEffects[soundName];
        soundEffect.Pause();
    }

    public static void ChangeMuteState()
    {
        Muted = !Muted;
        foreach (SoundEffectInstance soundEffect in gameSoundEffects.Values)
        {
            soundEffect.Volume = Muted ? 0 : 1;
        }
    }

    public static void ClearSoundDictionary()
    {
        foreach (SoundEffectInstance soundEffect in gameSoundEffects.Values)
        {
            soundEffect.Dispose();
        }
        gameSoundEffects.Clear();
    }
}
