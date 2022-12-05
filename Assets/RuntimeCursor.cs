using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeCursor : MonoBehaviour
{
    // Crosshair flag
    private bool crosshairEnabled = false;
    public Texture2D crosshair, mouse;
    public bool isMenu = false;
    private bool cursorSet = false;

    public void Start()
    {
        /*if (isMenu)
        {
            crosshairEnabled = false;
            Cursor.SetCursor(mouse, Vector2.zero, CursorMode.ForceSoftware);
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            crosshairEnabled = true;
            Cursor.SetCursor(crosshair, Vector2.zero, CursorMode.ForceSoftware);
            Cursor.lockState = CursorLockMode.Confined;
        }*/
    }
    
    public void Update()
    {
        /*if (!isMenu)
        {
            if (Dealer.isOpen && crosshairEnabled)
            {
                crosshairEnabled = false;
                Cursor.SetCursor(mouse, Vector2.zero, CursorMode.ForceSoftware);
                Cursor.lockState = CursorLockMode.None;
            }
            else if (!Dealer.isOpen && !crosshairEnabled)
            {
                crosshairEnabled = true;
                Cursor.SetCursor(crosshair, Vector2.zero, CursorMode.ForceSoftware);
                Cursor.lockState = CursorLockMode.Confined;
            }
        }*/
    }

    public void OnMouseEnter()
    {
        /*if (isMenu)
        {
            crosshairEnabled = false;
            Cursor.SetCursor(mouse, Vector2.zero, CursorMode.ForceSoftware);
            Cursor.lockState = CursorLockMode.None;
        }*/
    }
}
