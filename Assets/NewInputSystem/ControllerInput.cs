using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * PlayerInput represents a virtual input layer for each player so that the character controllers don't need to couple to the editor-set input axes. 
 * This allows us to use the same control code for each player, as well as emulate more advanced input controls like double taps in the future.
 */
public class ControllerInput : MonoBehaviour {

    //This sets which player this captures the input for [Player indexes are 1 through 4]
    [SerializeField]
	private int controllerNumber;

    // Instead of having to write a bunch of methods for each thing you can do with each button, It's easier to just let controller code access
    // A virtual button for each button with our desired information stored in it.
	[SerializeField]
    public InputButton attack1Button;
	[SerializeField]
    public InputButton attack2Button;
	[SerializeField]
    public InputButton jumpButton;
	[SerializeField]
	public InputButton blockButton;
	[SerializeField]
	public InputButton submitButton;
	[SerializeField]
	public InputButton backButton;
	private InputButton[] buttons;

    // The axes input methods in this class are just handled here instead of another class like InputButton, so we store their identifiers computed at awake here.
    private string horizontalAxisIdentifier;
    private string verticalAxisIdentifier;

    private void Awake()
    {
		SetControllerNumber (controllerNumber);
		buttons = new InputButton[]{ attack1Button, attack2Button, jumpButton, blockButton, submitButton, backButton };
    }

	//Set the controller index value, and then update all input identifiers to use that controller index
	public void SetControllerNumber(int controllerNumber){
		this.controllerNumber = controllerNumber;
		//To get the controllers working on OS X and Windows at the same time (each OS maps button codes differently - sigh). 
		string platformSuffix = (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer) ? "-MacOS" : "-Win";
		attack1Button.SetIdentifier("C"+controllerNumber+"-Attack1" + platformSuffix);
		attack2Button.SetIdentifier("C" + controllerNumber + "-Attack2" + platformSuffix);
		jumpButton.SetIdentifier("C" + controllerNumber + "-Jump" + platformSuffix);
		blockButton.SetIdentifier("C" + controllerNumber + "-Block" + platformSuffix);
		submitButton.SetIdentifier("C" + controllerNumber + "-Submit" + platformSuffix);
		backButton.SetIdentifier("C" + controllerNumber + "-Back" + platformSuffix);
		horizontalAxisIdentifier = "C" + controllerNumber + "-Horizontal" + platformSuffix;
		verticalAxisIdentifier = "C" + controllerNumber + "-Vertical" + platformSuffix;
	}
	//On update, update each button so that they will invoke press, hold, and release events
	void Update(){
		foreach (InputButton button in buttons) {
			button.Update ();
		}
	}

    //Give access to Axis values for the player movement component
    public float GetHorizontalAxis()
    {
        return Input.GetAxis(horizontalAxisIdentifier);
    }

    //Give access to Axis values for the player movement component
    public float GetVerticalAxis()
    {
        return Input.GetAxis(verticalAxisIdentifier);
    }
}
