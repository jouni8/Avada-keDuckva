using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Tooltip("movement stats")]
    public float acceleration = 10;
    public float decelaration = 10;
    public float walkingSpeed = 8;
    public float sprintSpeed = 14;
    public float crouchSpeed = 5;

    [Tooltip("stamina")]
    public float maxStamina = 100;
    public float stamina;
    public float staminaRecoverSpeed = 5;
    public float staminaRecoveryDelay = 5;
    public float sprintStaminaUse = 2;

    float lastStaminaUse;

    public bool movementLocked;

    private void Awake()
    {
        stamina = maxStamina;
    }
    private void FixedUpdate()
    {
        if (lastStaminaUse + staminaRecoveryDelay <= Time.time && stamina != maxStamina)
        {
            RecoverStamina();
        }
    }

    void RecoverStamina()
    {
        if(stamina < maxStamina)
        {
            stamina += staminaRecoverSpeed * Time.deltaTime;
        }
        else
        {
            stamina = maxStamina;
        }
    }

    public void UseStamina(float amount, bool stopRecovery = true)
    {
        if(stamina - amount > 0)
        {            
            stamina -= amount;
        }
        else
        {
            stamina = 0;
        }
        lastStaminaUse = Time.time;
    }
}
