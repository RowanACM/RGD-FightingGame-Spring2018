using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {

    public PlayerInput input;

    public float horizAcceleration;

    public float horizDecceleration;

    public float airControlModifier;

    public float maxSpeed;

    public float maxAirSpeed;

    public float jumpSpeed;

    public float gravityModifier;

    public int jumpCount;

    private int currentJumps;

    public bool velocitySnap;

    public float velocitySnapTiming;

    [SerializeField]
    private Rigidbody rigid;
	// Use this for initialization
	void Start () {
        currentJumps = 0;
	}
	
    public bool IsGrounded()
    {
        return Mathf.Abs(rigid.velocity.y) == 0f;
    }

	// Update is called once per frame
	void Update () {
        float horizInput = input.GetHorizontalAxis();
        //check for velocity snap - keep speed but flip direction
        if (velocitySnap)
        {
            //if want to move in opposite direction of movement, invert velocity (snapping)
            //for handling impact forces, we will need to add locking logic to this behaviour to turn off snapping
            if (Mathf.Sign(horizInput) != Mathf.Sign(rigid.velocity.x))
            {
                rigid.velocity = new Vector3(-rigid.velocity.x, rigid.velocity.y, rigid.velocity.z);
            }
        }
        
        float velocityRatioDiff = (rigid.velocity.x / maxSpeed) - horizInput;
        if (Mathf.Abs(velocityRatioDiff) > 0.01f)
        {
            float accelerationValue = 0f;
            //if we want to deccelerate
            if (Mathf.Abs(rigid.velocity.x / maxSpeed) > Mathf.Abs(horizInput))
            {
                accelerationValue = horizDecceleration;
            }
            else
            {
                accelerationValue = horizAcceleration;
            }
            rigid.AddForce(Vector3.left * Mathf.Sign(velocityRatioDiff) * accelerationValue * (IsGrounded() ? 1f : airControlModifier), ForceMode.Acceleration);
        }

        if (IsGrounded())
        {
            currentJumps = 0;
            Debug.Log("Grounded");
        }

        if (input.jumpButton.GetPress())
        {
            if (IsGrounded() || currentJumps < jumpCount)
            {
                currentJumps++;
                float neededJumpVelocity = Mathf.Max(jumpSpeed-rigid.velocity.y, 0f);
                rigid.AddForce(Vector3.up * neededJumpVelocity, ForceMode.VelocityChange);
                
            }
        }
	}
}
