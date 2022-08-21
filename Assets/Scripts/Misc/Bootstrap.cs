using CloudAPI = HeathenEngineering.SteamworksIntegration.API.RemoteStorage.Client;
using HeathenEngineering.SteamworksIntegration;
using HeathenEngineering.SteamworksIntegration.API;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    // Canvas group
    public SteamworksBehaviour steamworks;
    public CanvasGroup canvasGroup;
    private float cooldown = 0.5f;

    // Start is called before the first frame update
    public void Start()
    {
        // Set steamworks to not be destroyed
        DontDestroyOnLoad(steamworks);

        // Update old save
        SaveSystem.CheckForOldSave();

        // End authentication session
        Authenticator.Logout();

        // Load main
        cooldown = 0.5f;
        LeanTween.alphaCanvas(canvasGroup, 1f, 1f);
        LeanTween.alphaCanvas(canvasGroup, 0f, 1f).setDelay(6f);
    }
    
    // Update method
    public void Update()
    {
        if (cooldown > 0) cooldown -= Time.deltaTime;
        else if (canvasGroup.alpha == 0f) SceneManager.LoadScene("Menu");
    }
}
