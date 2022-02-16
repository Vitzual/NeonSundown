using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keybinds
{
    public enum Key
    {
        move_up,
        move_left,
        move_down,
        move_right,
        dash,
        shoot,
        escape
    }

    public static KeyCode move_up { get; set; }
    public static KeyCode move_left { get; set; }
    public static KeyCode move_down { get; set; }
    public static KeyCode move_right { get; set; }
    public static KeyCode dash { get; set; }
    public static KeyCode shoot { get; set; }
    public static KeyCode ability { get; set; }
    public static KeyCode escape { get; set; }
    public static KeyCode debug { get; set; }

    public static void SetKeybind(Key key, KeyCode value)
    {
        switch (key)
        {
            case Key.move_up:
                move_up = value;
                break;

            case Key.move_left:
                move_left = value;
                break;

            case Key.move_down:
                move_down = value;
                break;

            case Key.move_right:
                move_right = value;
                break;

            case Key.dash:
                dash = value;
                break;

            case Key.shoot:
                shoot = value;
                break;

            case Key.escape:
                escape = value;
                break;
        }
    }

    public static KeyCode GetKeybind(Key key)
    {
        switch (key)
        {
            case Key.move_up:
                return move_up;

            case Key.move_left:
                return move_left;

            case Key.move_down:
                return move_down;

            case Key.move_right:
                return move_right;

            case Key.dash:
                return dash;

            case Key.shoot:
                return shoot;

            case Key.escape:
                return escape;

            default:
                return move_up;
        }
    }

    public static void SetDefaultKeybinds()
    {
        move_up = KeyCode.W;
        move_left = KeyCode.A;
        move_down = KeyCode.S;
        move_right = KeyCode.D;
        dash = KeyCode.LeftShift;
        shoot = KeyCode.Mouse0;
        ability = KeyCode.Mouse1;
        escape = KeyCode.Escape;
        debug = KeyCode.Q;
    }
}
