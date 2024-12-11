using TMPro;
using UnityEngine;

public class Player_Climbing : MonoBehaviour
{
    //references
    [SerializeField] Rigidbody rb;
    public static Player_Climbing instance;
    public float stamina=100;
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
        stamina_Text.text = "Stamina: " +(int) stamina;


        //CheckForWalls();
        //updating climbing
        if (!climbing && stamina < 100)
        {
            stamina += Time.deltaTime;
        }

        if (stamina <= 0)
        {
            StopClimbing();
        }
 
    }

    public void StopClimbing()
    {
        canClimb = false;
        climbing = false;
        rb.useGravity = true;
    }

    private void StartClimbing()
    {
        if (canClimb)
        {
            climbing = true;
        }
        else
        {
            climbing = false;
        }

        //upward movement
        if (canClimb && Input.GetKey(KeyCode.W))
        {
            if (stamina > 0)
            {
                
                transform.position += new Vector3(0, climbSpeed, 0) * Time.deltaTime;
            }
        }
        //downward movement
        if (canClimb && Input.GetKey(KeyCode.S))
        {
            if (stamina > 0)
            {
                climbing = true;
                transform.position += new Vector3(0, -climbSpeed, 0) * Time.deltaTime;
            }
        }
        //right movement
        if(canClimb && Input.GetKey(KeyCode.D))
        {
            if (stamina > 0)
            {
                Debug.Log("running");
                climbing = true;
                transform.position += new Vector3(climbSpeed, 0, 0) * Time.deltaTime;
            }
        }
        //left movement
        if (canClimb && Input.GetKey(KeyCode.A))
        {
            if (stamina > 0)
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
            stamina -= Time.deltaTime;
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
