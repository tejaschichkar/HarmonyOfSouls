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
    bool canClimb;
    public bool climbing;
    [SerializeField] float sphereCastRadius;
    RaycastHit frontWallHit;
    [SerializeField] float sphereCastLength;
    [SerializeField] LayerMask LM_wall;
    [SerializeField] Vector3 sphereCastOffset;

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
        CheckForWalls();
        StartClimbing();
        StopClimbing();

    }

    private void StopClimbing()
    {
        if (stamina <= 0)
        {
            climbing = false;
            rb.useGravity = true;
        }

        //updating climbing
        if (!climbing && stamina < 100)
        {
            stamina += Time.deltaTime;
        }
    }

    private void StartClimbing()
    {
        //upward movement
        if (canClimb && Input.GetKey(KeyCode.W))
        {
            if (stamina > 0)
            {
                climbing = true;
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
        //updating stamina
        if (climbing)
        {
            stamina -= Time.deltaTime;
            rb.useGravity = false;
        }
    }

    private void CheckForWalls()
    {
        canClimb = Physics.SphereCast
                    (transform.position + sphereCastOffset, sphereCastRadius, Vector3.forward, out frontWallHit, sphereCastLength, LM_wall);
    }

    //debuging
    private void OnDrawGizmos()
    {
       

        Gizmos.DrawSphere(transform.position+sphereCastOffset, sphereCastRadius);
   

    }
}
