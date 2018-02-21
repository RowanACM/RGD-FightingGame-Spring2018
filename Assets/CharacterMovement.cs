using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {

    public PlayerInput input;

    public float horizAcceleration;

    public float horizDecceleration;

    public float airControlModifier;

    public float maxSpeed;

    public float jumpSpeed;

    public float secondaryJumpSpeed;

    public int jumpCount;

    private int currentJumps;

    public float downSpeed;

    private bool downState;

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
            downState = false;
        } else
        {
            if(input.GetVerticalAxis() == 1f && !downState)
            {
                rigid.AddForce(Vector2.down * downSpeed, ForceMode.VelocityChange);
                downState = true;
            }
        }

        if (input.jumpButton.GetPress())
        {
            if (IsGrounded() || currentJumps < jumpCount)
            {
                currentJumps++;
                float neededJumpVelocity = 0f;
                if (currentJumps == 1)
                {
                    neededJumpVelocity = Mathf.Max(jumpSpeed - rigid.velocity.y, 0f);
                }
                else
                {
                    neededJumpVelocity = Mathf.Max(secondaryJumpSpeed - rigid.velocity.y, 0f);
                }
                rigid.AddForce(Vector3.up * neededJumpVelocity, ForceMode.VelocityChange);
                downState = false;
            }
        }
	}
}
