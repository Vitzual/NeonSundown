using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class CIN : MonoBehaviour
{
    public enum InputType
    {
        KMB,
        Controller
    }
    public static InputType inputType;

    /////////////////////////////////////
    /////////// INPUT ACTIONS ///////////
    /////////////////////////////////////

    [Header("Input Actions")]
    protected PlayerInput _inputActions;

    // List of input actions in action map
    public static InputAction _action_move, _action_mouse, _action_scroll, _action_primary, _action_aim,
        _action_secondary, _action_sprint, _action_escape, _action_stats, _action_autofire;

    // List of keybinds currently being pressed
    private List<InputAction> _keybindActivity = new List<InputAction>();

    public bool debug;

    // Grab required components in start
    public void Start()
    {
        // Set input actions
        _inputActions = new PlayerInput();
        ToggleActionMap(true);

        InputUser.onChange += ChangeInputDevice;
    }

    // On disable, deactivate all input listeners
    private void OnDisable()
    {
        ToggleActionMap(false);
    }

    public void ChangeInputDevice(InputUser user, InputUserChange change, InputDevice device)
    {
        if (user.controlScheme == _inputActions.GamepadScheme)
        {
            Debug.Log("[INPUT] Controller device changed to controller!");
            inputType = InputType.Controller;
        }
        else
        {
            Debug.Log("[INPUT] Controller device changed to KMB!");
            inputType = InputType.KMB;
        }
    }

    /// <summary>
    /// Calculates any other inputs not related to movement or zoom
    /// </summary>
    public virtual void CalculateInputs()
    {
        //////////////////////////////////////////
        ///// Calculate primray action input /////
        //////////////////////////////////////////
        if (_action_primary.IsPressed())
        {
            if (!_keybindActivity.Contains(_action_primary))
            {
                InputEvents.Instance.onPrimaryPressed.Invoke();
                _keybindActivity.Add(_action_primary);
                if (debug) Debug.Log("[CONTROLLER] Primary keybind pressed!");
            }
            else
            {
                InputEvents.Instance.onPrimaryHeld.Invoke();
                if (debug) Debug.Log("[CONTROLLER] Primary keybind being held!");
            }
        }
        else if (_keybindActivity.Contains(_action_primary))
        {
            InputEvents.Instance.onPrimaryReleased.Invoke();
            _keybindActivity.Remove(_action_primary);
            if (debug) Debug.Log("[CONTROLLER] Primary keybind released!");
        }

        //////////////////////////////////////////
        //// Calculate secondary action input ////
        //////////////////////////////////////////
        if (_action_secondary.IsPressed())
        {
            if (!_keybindActivity.Contains(_action_secondary))
            {
                InputEvents.Instance.onSecondaryPressed.Invoke();
                _keybindActivity.Add(_action_secondary);
                if (debug) Debug.Log("[CONTROLLER] Secondary keybind pressed!");
            }
            else
            {
                InputEvents.Instance.onSecondaryHeld.Invoke();
                if (debug) Debug.Log("[CONTROLLER] Secondary keybind being held!");
            }
        }
        else if (_keybindActivity.Contains(_action_secondary))
        {
            InputEvents.Instance.onSecondaryReleased.Invoke();
            _keybindActivity.Remove(_action_secondary);
            if (debug) Debug.Log("[CONTROLLER] Secondary keybind released!");
        }

        ///////////////////////////////////////////
        ////// Calculate escape action input //////
        ///////////////////////////////////////////
        if (_action_escape.IsPressed())
        {
            if (!_keybindActivity.Contains(_action_escape))
            {
                InputEvents.Instance.onEscapePressed.Invoke();
                _keybindActivity.Add(_action_escape);
                if (debug) Debug.Log("[CONTROLLER] Escape keybind pressed!");
            }
        }
        else if (_keybindActivity.Contains(_action_escape))
        {
            _keybindActivity.Remove(_action_escape);
        }

        ///////////////////////////////////////////
        /////// Calculate space action input //////
        ///////////////////////////////////////////
        if (_action_autofire.IsPressed())
        {
            if (!_keybindActivity.Contains(_action_autofire))
            {
                InputEvents.Instance.onSpacePressed.Invoke();
                _keybindActivity.Add(_action_autofire);
                if (debug) Debug.Log("[CONTROLLER] Auto fire keybind pressed!");
            }
        }
        else if (_keybindActivity.Contains(_action_autofire))
        {
            _keybindActivity.Remove(_action_autofire);
        }

        ///////////////////////////////////////////
        ////// Calculate sprint action input //////
        ///////////////////////////////////////////
        if (_action_sprint.IsPressed())
        {
            if (!_keybindActivity.Contains(_action_sprint))
            {
                InputEvents.Instance.onSprintPressed.Invoke();
                _keybindActivity.Add(_action_sprint);
                if (debug) Debug.Log("[CONTROLLER] Sprint keybind pressed!");
            }
            else
            {
                InputEvents.Instance.onSprintHeld.Invoke();
                if (debug) Debug.Log("[CONTROLLER] Sprint keybind being held!");
            }
        }
        else if (_keybindActivity.Contains(_action_sprint))
        {
            InputEvents.Instance.onSprintReleased.Invoke();
            _keybindActivity.Remove(_action_sprint);
            if (debug) Debug.Log("[CONTROLLER] Sprint keybind released!");
        }
    }

    /// <summary>
    /// Enables or disables listening to the action map
    /// </summary>
    public void ToggleActionMap(bool toggle)
    {
        if (toggle)
        {
            // Debug action map is enabled
            Debug.Log("[CONTROLLER] Enabling action mapping on " + transform.name);

            // Set input action references
            _action_move = _inputActions.Player.Move;
            _action_mouse = _inputActions.Player.Mouse;
            _action_aim = _inputActions.Player.Aim;
            _action_scroll = _inputActions.Player.Scroll;
            _action_primary = _inputActions.Player.Primary;
            _action_secondary = _inputActions.Player.Secondary;
            _action_sprint = _inputActions.Player.Sprint;
            _action_escape = _inputActions.Player.Escape;
            _action_stats = _inputActions.Player.Stats;
            _action_autofire = _inputActions.Player.AutoFire;

            // Enable all input actions
            _action_move.Enable();
            _action_mouse.Enable();
            _action_scroll.Enable();
            _action_primary.Enable();
            _action_secondary.Enable();
            _action_sprint.Enable();
            _action_escape.Enable();
            _action_stats.Enable();
            _action_autofire.Enable();
            _action_aim.Enable();
        }
        else
        {
            // Debug action map is enabled
            Debug.Log("[CONTROLLER] Disabling action mapping on " + transform.name);

            // Disable all input acitons
            _action_move.Disable();
            _action_mouse.Disable();
            _action_scroll.Disable();
            _action_primary.Disable();
            _action_secondary.Disable();
            _action_sprint.Disable();
            _action_escape.Disable();
            _action_stats.Disable();
            _action_autofire.Disable();
            _action_aim.Disable();
        }
    }
}
