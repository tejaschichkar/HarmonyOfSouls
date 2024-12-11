using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    
    [SerializeField] float moveSpeed;
    [SerializeField] Rigidbody rb;
    Vector3 moveDirection;

    private void Update()
    {
        float movex = Input.GetAxis("Horizontal");
        float movez = Input.GetAxis("Vertical");
        moveDirection = new Vector3(movex, 0, movez);
        moveDirection.Normalize();
        if(!(Player_Climbing.instance.climbing))
        {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
    }
}
