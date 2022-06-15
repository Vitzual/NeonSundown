using MK.Glow.URP;
using UnityEngine;
using UnityEngine.Rendering;

public class Effects : MonoBehaviour
{
    // Render profiles for menu and main scene
    public VolumeProfile _menuVolumeProfile, _mainVolumeProfile;
    public static VolumeProfile menuVolumeProfile, mainVolumeProfile;

    // Index of rendering features
    public int _mkGlowIndex = 0, _menuGlitchIndex = 1, _mainGlitchIndex = 1;
    private static int mkGlowIndex = 0, menuGlitchIndex = 1, mainGlitchIndex = 1;

    // On start, setup effects
    public void Awake()
    {
        menuVolumeProfile = _menuVolumeProfile;
        mainVolumeProfile = _mainVolumeProfile;
        mkGlowIndex = _mkGlowIndex;
        menuGlitchIndex = _menuGlitchIndex;
        mainGlitchIndex = _mainGlitchIndex;
    }
    
    /// <summary>
    /// Sets the glow amount of the shapes
    /// </summary>
    public static void SetGlowAmount(float amount)
    {
        // Set menu glow component
        MKGlowLite mkg = (MKGlowLite)menuVolumeProfile.components[mkGlowIndex];
        if (mkg != null) mkg.bloomIntensity.value = amount;

        // Set main glow component
        mkg = (MKGlowLite)mainVolumeProfile.components[mkGlowIndex];
        if (mkg != null) mkg.bloomIntensity.value = amount;
    }

    /// <summary>
    /// Toggles the menu glitch effect
    /// </summary>
    public static void ToggleMenuGlitchEffect(bool toggle)
    {
        Limitless_Glitch3 glitchEffect = (Limitless_Glitch3)menuVolumeProfile.components[menuGlitchIndex];
        if (glitchEffect.enable.value != toggle) glitchEffect.enable.value = toggle;
    }

    /// <summary>
    /// Toggles the main glitch effect
    /// </summary>
    public static void TogglMainGlitchEffect(bool toggle)
    {
        Limitless_Glitch3 glitchEffect = (Limitless_Glitch3)mainVolumeProfile.components[mainGlitchIndex];
        if (glitchEffect.enable.value != toggle) glitchEffect.enable.value = toggle;
    }
}
