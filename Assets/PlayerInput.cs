using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * PlayerInput represents a virtual input layer for each player so that the character controllers don't need to couple to the editor-set input axes. 
 * This allows us to use the same control code for each player, as well as emulate more advanced input controls like double taps in the future.
 */
public class PlayerInput : MonoBehaviour {

    //This sets which player this captures the input for [Player indexes are 1 through 4]
    [SerializeField]
    private int playerIndex;

    // Instead of having to write a bunch of methods for each thing you can do with each button, It's easier to just let controller code access
    // A virtual button for each button with our desired information stored in it.
    public InputButton fastAttackButton;
    public InputButton strongAttackButton;
    public InputButton jumpButton;

    // The axes input methods in this class are just handled here instead of another class like InputButton, so we store their identifiers computed at awake here.
    private string horizontalAxisIdentifier;
    private string verticalAxisIdentifier;

    //Possible TODO: Make the virtual input layer work as a push system using unity events so you can set things in the editor. Right now its a pull system that you interact with through code.

    private void Awake()
    {
        fastAttackButton = new InputButton("P"+playerIndex+"-Fast");
        strongAttackButton = new InputButton("P" + playerIndex + "-Strong");
        jumpButton = new InputButton("P" + playerIndex + "-Jump");
        horizontalAxisIdentifier = "P" + playerIndex + "-Horizontal";
        verticalAxisIdentifier = "P" + playerIndex + "-Vertical";
    }

    //Give access to Axis values for the player movement component
    public float GetHorizontalAxis()
    {
        Debug.Log("Horizontal: " + Input.GetAxis(horizontalAxisIdentifier));
        return Input.GetAxis(horizontalAxisIdentifier);
    }

    //Give access to Axis values for the player movement component
    public float GetVerticalAxis()
    {
        return Input.GetAxis(verticalAxisIdentifier);
    }
}
