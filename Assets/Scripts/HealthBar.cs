using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public bool takeDamage;
    public float currentDamage;
    private float damageAmt;
    private float maxHealth;
    public bool connect;
    // [1]
        // PlayerController passes true takeDamage bool,
        // takeDamage set to false so if statement doesn't repeat


    public GameObject playerObj;
    public PlayerController my_PlayerController_script;
    // Start is called before the first frame update
    private void Start()
    {
        my_PlayerController_script = playerObj.GetComponent<PlayerController>();
        damageAmt = my_PlayerController_script.damageAmt;
        maxHealth = my_PlayerController_script.maxHealth;
        slider = gameObject.GetComponent<Slider>();
        //Debug.Log("damageAmt = " + my_PlayerController_script.damageAmt + " maxHealth = " + my_PlayerController_script.maxHealth);
        //Debug.Log("damageAmt = " + damageAmt + " maxHealth = " + maxHealth);
        //Debug.Log("Formula: " + 100 * (damageAmt/maxHealth));
        currentDamage = 100 * (damageAmt/maxHealth);
        //Debug.Log("currentDamage = " + currentDamage);
        my_PlayerController_script.connect = true;
        if (connect)
        {
            Debug.Log("PlayerController connected to Healthbar"); 
        }
        
    }

    private void Update()
    {
        // [1]{
        if (takeDamage)
        {
            Debug.Log("Damage recieved");
            takeDamage = false;
            slider.value -= currentDamage;
        }
        //}
    }

    /*
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }
    public void SetHealth(int health)
    {
        slider.value = health;
    }
    */
}
