using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class CIN : MonoBehaviour
{
    /////////////////////////////////////
    /////////// INPUT ACTIONS ///////////
    /////////////////////////////////////
    
    [Header("Input Actions")]
    protected PlayerInput _inputActions;

    // List of input actions in action map
    public static InputAction _action_move, _action_mouse, _action_scroll, _action_primary, _action_aim,
        _action_secondary, _action_sprint, _action_escape, _action_stats, _action_autofire, _action_clickthru,
        _action_leftDpad, _action_rightDpad, _action_leftButton, _action_rightButton, _action_topButton, 
        _action_burn, _action_upDpad, _action_bottomDpad;
    
    // List of keybinds currently being pressed
    private List<InputAction> _keybindActivity = new List<InputAction>();

    // Toggles true if action maps are enabled
    protected bool _actionMappingEnabled = false;

    public bool debug;

    // Grab required components in start
    public void Start()
    {
        // Set input actions
        _inputActions = new PlayerInput();
        ToggleActionMap(true);
    }

    // Calculate inputs each frame
    public void Update()
    {
        if (_actionMappingEnabled)
            CalculateInputs();
    }

    // On disable, deactivate all input listeners
    private void OnDisable()
    {
        ToggleActionMap(false);
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

        ///////////////////////////////////////////
        ////// Calculate escape action input //////
        ///////////////////////////////////////////
        if (_action_clickthru.IsPressed())
        {
            if (!_keybindActivity.Contains(_action_clickthru))
            {
                InputEvents.Instance.onClickThruPressed.Invoke();
                _keybindActivity.Add(_action_clickthru);
                if (debug) Debug.Log("[CONTROLLER] Click thru keybind pressed!");
            }
        }
        else if (_keybindActivity.Contains(_action_clickthru))
        {
            _keybindActivity.Remove(_action_clickthru);
        }

        ///////////////////////////////////////////
        ////// Calculate escape action input //////
        ///////////////////////////////////////////
        if (_action_leftDpad.IsPressed())
        {
            if (!_keybindActivity.Contains(_action_leftDpad))
            {
                InputEvents.Instance.onLeftDPad.Invoke();
                _keybindActivity.Add(_action_leftDpad);
                if (debug) Debug.Log("[CONTROLLER] Left dpad keybind pressed!");
            }
        }
        else if (_keybindActivity.Contains(_action_leftDpad))
        {
            _keybindActivity.Remove(_action_leftDpad);
        }

        ///////////////////////////////////////////
        ////// Calculate escape action input //////
        ///////////////////////////////////////////
        if (_action_burn.IsPressed())
        {
            if (!_keybindActivity.Contains(_action_burn))
            {
                InputEvents.Instance.onBurnPressed.Invoke();
                _keybindActivity.Add(_action_burn);
                if (debug) Debug.Log("[CONTROLLER] Burn keybind pressed!");
            }
        }
        else if (_keybindActivity.Contains(_action_burn))
        {
            _keybindActivity.Remove(_action_burn);
        }

        ///////////////////////////////////////////
        ////// Calculate escape action input //////
        ///////////////////////////////////////////
        if (_action_upDpad.IsPressed())
        {
            if (!_keybindActivity.Contains(_action_upDpad))
            {
                InputEvents.Instance.onTopDPad.Invoke();
                _keybindActivity.Add(_action_upDpad);
                if (debug) Debug.Log("[CONTROLLER] Up dpad keybind pressed!");
            }
        }
        else if (_keybindActivity.Contains(_action_upDpad))
        {
            _keybindActivity.Remove(_action_upDpad);
        }

        ///////////////////////////////////////////
        ////// Calculate escape action input //////
        ///////////////////////////////////////////
        if (_action_rightDpad.IsPressed())
        {
            if (!_keybindActivity.Contains(_action_rightDpad))
            {
                InputEvents.Instance.onRightDPad.Invoke();
                _keybindActivity.Add(_action_rightDpad);
                if (debug) Debug.Log("[CONTROLLER] Right dpad keybind pressed!");
            }
        }
        else if (_keybindActivity.Contains(_action_rightDpad))
        {
            _keybindActivity.Remove(_action_rightDpad);
        }

        ///////////////////////////////////////////
        ////// Calculate escape action input //////
        ///////////////////////////////////////////
        if (_action_bottomDpad.IsPressed())
        {
            if (!_keybindActivity.Contains(_action_bottomDpad))
            {
                InputEvents.Instance.onBottomDPad.Invoke();
                _keybindActivity.Add(_action_bottomDpad);
                if (debug) Debug.Log("[CONTROLLER] Bottom dpad keybind pressed!");
            }
        }
        else if (_keybindActivity.Contains(_action_bottomDpad))
        {
            _keybindActivity.Remove(_action_bottomDpad);
        }

        ///////////////////////////////////////////
        ////// Calculate escape action input //////
        ///////////////////////////////////////////
        if (_action_leftButton.IsPressed())
        {
            if (!_keybindActivity.Contains(_action_leftButton))
            {
                InputEvents.Instance.onLeftButton.Invoke();
                _keybindActivity.Add(_action_leftButton);
                if (debug) Debug.Log("[CONTROLLER] Left button keybind pressed!");
            }
        }
        else if (_keybindActivity.Contains(_action_leftButton))
        {
            _keybindActivity.Remove(_action_leftButton);
        }

        ///////////////////////////////////////////
        ////// Calculate escape action input //////
        ///////////////////////////////////////////
        if (_action_rightButton.IsPressed())
        {
            if (!_keybindActivity.Contains(_action_rightButton))
            {
                InputEvents.Instance.onRightButton.Invoke();
                _keybindActivity.Add(_action_rightButton);
                if (debug) Debug.Log("[CONTROLLER] Right button keybind pressed!");
            }
        }
        else if (_keybindActivity.Contains(_action_rightButton))
        {
            _keybindActivity.Remove(_action_rightButton);
        }

        ///////////////////////////////////////////
        ////// Calculate escape action input //////
        ///////////////////////////////////////////
        if (_action_topButton.IsPressed())
        {
            if (!_keybindActivity.Contains(_action_topButton))
            {
                InputEvents.Instance.onTopButton.Invoke();
                _keybindActivity.Add(_action_topButton);
                if (debug) Debug.Log("[CONTROLLER] Top button keybind pressed!");
            }
        }
        else if (_keybindActivity.Contains(_action_topButton))
        {
            _keybindActivity.Remove(_action_topButton);
        }
    }

    /// <summary>
    /// Enables or disables listening to the action map
    /// </summary>
    public void ToggleActionMap(bool toggle)
    {
        // Set action mapping toggle
        _actionMappingEnabled = toggle;

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
            _action_clickthru = _inputActions.Player.ClickThru;
            _action_upDpad = _inputActions.Player.TopDPad;
            _action_rightDpad = _inputActions.Player.RightDPad;
            _action_bottomDpad = _inputActions.Player.BottomDPad;
            _action_leftDpad = _inputActions.Player.LeftDPad;
            _action_leftButton = _inputActions.Player.LeftButton;
            _action_rightButton = _inputActions.Player.RightButton;
            _action_topButton = _inputActions.Player.TopButton;
            _action_burn = _inputActions.Player.Burn;

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
            _action_clickthru.Enable();
            _action_upDpad.Enable();
            _action_rightDpad.Enable();
            _action_bottomDpad.Enable();
            _action_leftDpad.Enable();
            _action_leftButton.Enable();
            _action_rightButton.Enable();
            _action_topButton.Enable();
            _action_burn.Enable();
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
            _action_clickthru.Disable();
            _action_upDpad.Disable();
            _action_rightDpad.Disable();
            _action_bottomDpad.Disable();
            _action_leftDpad.Disable();
            _action_leftButton.Disable();
            _action_rightButton.Disable();
            _action_topButton.Disable();
            _action_burn.Disable();
        }
    }
}
