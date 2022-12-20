using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(-1)]
public class InputEvents : Singleton<InputEvents>
{
    // Hotbar keybinds
    [HideInInspector] public UnityEvent<int> onHotbarPressed;

    // Primary keybinds
    [HideInInspector] public UnityEvent onPrimaryPressed;
    [HideInInspector] public UnityEvent onPrimaryHeld;
    [HideInInspector] public UnityEvent onPrimaryReleased;

    // Secondary keybinds
    [HideInInspector] public UnityEvent onSecondaryPressed;
    [HideInInspector] public UnityEvent onSecondaryHeld;
    [HideInInspector] public UnityEvent onSecondaryReleased;

    // Escape keybinds
    [HideInInspector] public UnityEvent onClickThruPressed;
    [HideInInspector] public UnityEvent onEscapePressed;
    [HideInInspector] public UnityEvent onSpacePressed;

    // Sprint keybinds
    [HideInInspector] public UnityEvent onSprintPressed;
    [HideInInspector] public UnityEvent onSprintHeld;
    [HideInInspector] public UnityEvent onSprintReleased;

    // Pipette keybinds
    [HideInInspector] public UnityEvent onPipettePressed;

    // Pause keybinds
    [HideInInspector] public UnityEvent onPausePressed;

    // Toggle HUD keybinds
    [HideInInspector] public UnityEvent onToggleHudPressed;
    [HideInInspector] public UnityEvent onToggleHudReleased;

    // Card control options
    [HideInInspector] public UnityEvent onLeftDPad;
    [HideInInspector] public UnityEvent onRightDPad;
    [HideInInspector] public UnityEvent onRightButton;
    [HideInInspector] public UnityEvent onLeftButton;
}