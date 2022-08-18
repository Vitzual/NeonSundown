using HeathenEngineering.SteamworksIntegration.API;
using HeathenEngineering.SteamworksIntegration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

[DefaultExecutionOrder(-1)]
public class Authenticator : MonoBehaviour
{
    // Authentication
    private static bool userAuthenticated = false;
    public static bool UserAuthenticated => userAuthenticated;

    /// <summary>
    /// Logs a user in
    /// </summary>
    public static void Login()
    {
        if (!UserAuthenticated)
        {
            Authentication.GetAuthSessionTicket((ticket, IOError) =>
            {
                if (!IOError)
                {
                    AuthenticatUser(ticket.data, UserData.Me);
                }
                else
                {
                    userAuthenticated = false;
                    Events.active.FinishAuthentication();
                }
            });
        }
        else Events.active.FinishAuthentication();
    }
    
    /// <summary>
    /// Logs the user out
    /// </summary>
    public static void Logout()
    {
        Authentication.EndAuthSession(UserData.Me);
    }

    /// <summary>
    /// Authenticates a user before entering the game
    /// </summary>
    /// <param name="ticket"></param>
    /// <param name="user"></param>
    private static void AuthenticatUser(byte[] ticket, UserData user)
    {
        var responce = Authentication.BeginAuthSession(ticket, user, result =>
        {
            switch (result.responce)
            {
                case EAuthSessionResponse.k_EAuthSessionResponseOK:
                    Debug.Log("[STEAM] Authentication Successful");
                    userAuthenticated = true;
                    break;
                default:
                    Debug.Log("[STEAM] Authentication Failed");
                    userAuthenticated = false;
                    break;
            }
            Events.active.FinishAuthentication();
        });
    }
}
