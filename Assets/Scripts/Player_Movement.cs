using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float sprintSpeed = 6f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] float jumpForce;

    private Vector3 moveDirection;
    private bool isGrounded;
    private Vector3 velocity;

    bool falling=false;
    private void Update()
    {
        // Ground Check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0f; // Reset vertical velocity when grounded
        }

        // Movement Input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        moveDirection = new Vector3(x, 0, z).normalized;

        // Sprinting Check
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;

        //checking is player in climbing and then addingg movement
        if (!(Player_Climbing.instance.climbing))
        {
            Vector3 move = transform.TransformDirection(moveDirection) * currentSpeed;
            rb.MovePosition(transform.position + move * Time.deltaTime);
        }

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            if(falling)
            {
                falling = false;
                Player_Climbing.instance.StopClimbing();
            }
            else
            {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            falling = true;
            }
        }

        //enetering and exiting climbing states using isgrounded
        if(!isGrounded )
        {
            Player_Climbing.instance.CheckForWalls();
        }
        else
        {
            Player_Climbing.instance.StopClimbing();
            falling = false;
        }
        
        // Apply Gravity if player is not in climbing state
        if (!(Player_Climbing.instance.climbing))
        {
            velocity.y += gravity * Time.deltaTime;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, velocity.y, rb.linearVelocity.z);
        }

    }
}
