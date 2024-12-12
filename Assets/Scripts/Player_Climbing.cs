using TMPro;
using UnityEngine;

public class Player_Climbing : MonoBehaviour
{
    //references
    [SerializeField] Rigidbody rb;
    public static Player_Climbing instance;
   
    [SerializeField] TextMeshProUGUI stamina_Text;

    //casting for climbing
    public bool canClimb;
    public bool climbing;
    [SerializeField] float sphereCastRadius;
    RaycastHit frontWallHit;
    [SerializeField] float sphereCastLength;
    [SerializeField] LayerMask LM_wall;
    [SerializeField] Vector3 sphereCastOffset;
    bool wallThere;

    //climbing movement
    [SerializeField] float climbSpeed;

    private void Start()
    {
        instance = this;
    }

    private void Update()
    {
        //ui
        stamina_Text.text = "Stamina: " +(int) Player_Statts.instance.stamina;

        //space to exit climbing
        if(climbing && Input.GetKeyDown(KeyCode.Space))
        {
            StopClimbing();
            Player_Movement.instance.CanWallCheck = false;
        }

        if (Player_Statts.instance.stamina <= 0)
        {
            StopClimbing();
        }
 
    }

    public void StopClimbing()
    {
        canClimb = false;
        climbing = false;
        rb.useGravity = true;
        Player_Movement.instance.ResetCanWallCheck();

        //for stamina
        Player_Statts.instance.canRechargeStamina = true;

    }

    private void StartClimbing()
    {
        if (canClimb)
        {
            climbing = true;
            Player_Statts.instance.canRechargeStamina = false;
        }
        else
        {
            climbing = false;
        }

        //upward movement
        if (canClimb && Input.GetKey(KeyCode.W))
        {
            if (Player_Statts.instance.stamina > 0)
            {
                
                transform.position += new Vector3(0, climbSpeed, 0) * Time.deltaTime;
            }
        }
        //downward movement
        if (canClimb && Input.GetKey(KeyCode.S))
        {
            if (Player_Statts.instance.stamina > 0)
            {
                climbing = true;
                transform.position += new Vector3(0, -climbSpeed, 0) * Time.deltaTime;
            }
        }
        //right movement
        if(canClimb && Input.GetKey(KeyCode.D))
        {
            if (Player_Statts.instance.stamina > 0)
            {
                climbing = true;
                transform.position += new Vector3(climbSpeed, 0, 0) * Time.deltaTime;
            }
        }
        //left movement
        if (canClimb && Input.GetKey(KeyCode.A))
        {
            if (Player_Statts.instance.stamina > 0)
            {
                climbing = true;
                transform.position += new Vector3(-climbSpeed, 0, 0) * Time.deltaTime;
            }
        }


        //updating stamina
        if (climbing)
        {
            rb.linearVelocity = Vector3.zero; 
            rb.angularVelocity = Vector3.zero;
            Player_Statts.instance.stamina -= Time.deltaTime;
            rb.useGravity = false;
            
        }
        else
        {
            StopClimbing();
        }
    }

    public void CheckForWalls()
    {
        canClimb = Physics.SphereCast
                    (transform.position + sphereCastOffset, sphereCastRadius, Vector3.forward, out frontWallHit, sphereCastLength, LM_wall);
        if(canClimb)
        {
            StartClimbing();
        }
        else
        {
            StopClimbing();
        }
    }

    //debuging
    private void OnDrawGizmos()
    {
       

        Gizmos.DrawSphere(transform.position+sphereCastOffset, sphereCastRadius);
   

    }
}
