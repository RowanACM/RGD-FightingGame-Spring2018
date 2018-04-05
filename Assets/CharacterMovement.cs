using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Character Movement encapsulates how to move the character rigidbody (left/right/jump/descend) based on movement control inputs.
 * Effects and Attacks that move the player should disable this component if these controls should be disabled and operate on the Rigidbody themselves
 */
public class CharacterMovement : MonoBehaviour {

    // Virtual Input layer for the player this character represents
    public ControllerInput input;

    // How quickly does this character get up to speed when running or air controlling
    public float horizAcceleration;
    // How quickly does this character slow down when stopping or moving in the opposite direction
    public float horizDecceleration;
    // This is just a multiplier for the acceleration and decceleration when in the air
    public float airControlModifier;
    // Maximum horizontal speed of the character. This will be reached when the joystick is all the way to the left or the right.
    public float maxSpeed;
    // Upward velocity of the character when jumping from the ground
    public float jumpSpeed;
    // Upward velocity of the character when jumping in the air
    public float secondaryJumpSpeed;
    // Number of jumps the character can make (2 would indicate the character can double jump)
    public int jumpCount;
    // The immediate downward velocity the character when descending (Move stick down when in the air)
    public float downSpeed;

    // Current number of jumps while the character is in air
    private int currentJumps;
    // Is the character descending with the descend move (not just gravity) - prevents the player from dropping their character too rapidly
    private bool downState;

    // Rigidbody component to drive for movement
    [SerializeField]
    private Rigidbody rigid;

	// Use this for initialization
	void Start () {
        currentJumps = 0;
	}

    // Obviously this method is borked when they are at the top of their jump but the person who is tasked with movement should read this. 
    // If not, I'd guess they weren't paying much attention.
    public bool IsGrounded()
    {
        return Mathf.Abs(rigid.velocity.y) == 0f;
    }

	// Update is called once per frame
	void Update () {
        // Get our horizontal movement input from input layer
        float horizInput = input.GetHorizontalAxis();
        // Calculate the difference in our current velocity from our target velocity, mapped to the input space [-1,+1]
        float velocityRatioDiff = (rigid.velocity.x / maxSpeed) - horizInput;
        // Simple threshold check. If our velocity is close enough to the target velocity then don't do anything
        if (Mathf.Abs(velocityRatioDiff) > 0.01f)
        {
            float accelerationValue = 0f;
            // Calculate our acceleration magnitude
            // If the magnitude of our current velocity in the input space is greater than the magnitude of our target velocity, then we want to deccellerate (I KNOW i'm spelling decellerate wrong but its too late to change it).
            if (Mathf.Abs(rigid.velocity.x / maxSpeed) > Mathf.Abs(horizInput))
            {
                accelerationValue = horizDecceleration;
            }
            else // accelerate
            {
                accelerationValue = horizAcceleration;
            }
            // Add an acceleration to the left of the rigidbody with the calculated magnitude multiplied by our air control modif. if not grounded, and in the direction we want to move towards to match our input target velocity
            rigid.AddForce(Vector3.left * Mathf.Sign(velocityRatioDiff) * accelerationValue * (IsGrounded() ? 1f : airControlModifier), ForceMode.Acceleration);
        }

        // Aerial state code
        if (IsGrounded())
        {
            // Reset jump and aerial states once we are grounded.
            currentJumps = 0;
            downState = false;
        } else
        {
            // Check for descent control. If activated, we want to immediately add a downward velocity change to the rigidbody to make them descend faster.
            if(input.GetVerticalAxis() == 1f && !downState)
            {
                rigid.AddForce(Vector2.down * downSpeed, ForceMode.VelocityChange);
                downState = true;
            }
        }

        // Jump code
        if (input.jumpButton.GetPress())
        {
            // Check if we can jump
            if (IsGrounded() || currentJumps < jumpCount)
            {
                //increment the jumpCount, then use it for logic
                currentJumps++;
                // Calculate the velocity change necessary to make the rigidbody move upwards at our desired jump speed
                float neededJumpVelocity = 0f;
                if (currentJumps == 1)
                {
                    neededJumpVelocity = Mathf.Max(jumpSpeed - rigid.velocity.y, 0f);
                }
                else
                {
                    neededJumpVelocity = Mathf.Max(secondaryJumpSpeed - rigid.velocity.y, 0f);
                }
                // Add the velocity difference to make the rigidbody move upwards
                rigid.AddForce(Vector3.up * neededJumpVelocity, ForceMode.VelocityChange);
                // Since we are jumping this frame, we exit the descent state if we were in it before.
                downState = false;
            }
        }
	}
}
