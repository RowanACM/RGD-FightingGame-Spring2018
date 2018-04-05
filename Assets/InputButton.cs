using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/**
 * InputButton encapsulates handling logic for all of the signals a button receives.
 * The simple Getters for Press and Hold just reference the static Input class.
 */
[System.Serializable]
public class InputButton {

	[SerializeField]
	private UnityEvent OnPress;
	[SerializeField]
	private UnityEvent OnHold;
	[SerializeField]
	private UnityEvent OnRelease;

	private string identifier;

	public void SetIdentifier(string identifier){
		this.identifier = identifier;
	}

	//Needs to be called from client class
	public void Update(){
		if (GetPress ()) {
			OnPress.Invoke ();
		} else if (GetHold ()) {
			OnHold.Invoke ();
		} else if (GetRelease ()) {
			OnRelease.Invoke ();
		}
	}

    public bool GetPress()
    {
        return Input.GetButtonDown(this.identifier);
    }

    public bool GetHold()
    {
        return Input.GetButton(this.identifier);
    }

	public bool GetRelease(){
		return Input.GetButtonUp(this.identifier);
	}
}
