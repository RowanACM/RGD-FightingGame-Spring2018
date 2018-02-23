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
        //for now, a hack to get the controllers working on OS X and Windows at the same time (each OS maps button codes differently - sigh). 
        // We manually configure different sets of input axes for each platform, and just change the name of the axis to include a suffix if on MacOS/OS X.
        // Is this a good solution? NO. Is this a one-ish line solution that lets me show a prototype tomorrow? Yes. We'll need to replace this with one of many good solutions.
        // Probably what we'll do is write a script that generates these input axes on the fly or in-editor. Should help with the duplicate P1-4 player prefixes to the axes :P
        string platformSuffix = (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer) ? "-MacOS" : "";
        fastAttackButton = new InputButton("P"+playerIndex+"-Fast" + platformSuffix);
        strongAttackButton = new InputButton("P" + playerIndex + "-Strong" + platformSuffix);
        jumpButton = new InputButton("P" + playerIndex + "-Jump" + platformSuffix);
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
