using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    [SerializeField]
    private int playerIndex;

    //Use the InputButton class for our buttons so that we don't have to repeat implementing functionality such as double press, long hold, etc. for all buttons.
    public InputButton fastAttackButton;
    public InputButton strongAttackButton;
    public InputButton jumpButton;

    private string horizontalAxisIdentifier;
    private string verticalAxisIdentifier;

    //TODO: Use the Facade design pattern and make this class have UnityEvent objects for attack slots (like Up-Attack, Left-Attack, Right-Attack or other simultaneous presses)

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
