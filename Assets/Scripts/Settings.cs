using UnityEngine;

public class Settings
{
    public float _masterVolume;
    public float _musicVolume;
    public float _soundVolume;

    public static float music = 0.5f;
    public static float sound = 0.5f;

    public void UpdateVolume()
    {
        music = _musicVolume * _masterVolume;
        sound = _soundVolume * _masterVolume;
    }
}
