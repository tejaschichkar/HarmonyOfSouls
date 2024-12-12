using UnityEngine;

public class Player_Statts : MonoBehaviour
{
    public static Player_Statts instance;


    //stats
    public float stamina = 100;
    public bool canRechargeStamina = true;

    private void Start()
    {
        instance = this;
    }

    public void Update()
    {
        IncreaseStamina();
    }
    public void IncreaseStamina(float extra=0)
    {
        if(stamina<100 && canRechargeStamina)
        {
        stamina += Time.deltaTime+extra;
        }
    }
}
