using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * InputButton encapsulates handling logic for all of the signals a button receives.
 * The simple Getters for Press and Hold just reference the static Input class.
 */
public class InputButton {

    private string identifier;

    public InputButton(string identifier)
    {
        this.identifier = identifier;
    }

    public bool GetPress()
    {
        return Input.GetButtonDown(this.identifier);
    }

    public bool GetHold()
    {
        return Input.GetButton(this.identifier);
    }
}
