using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    // UI
    [SerializeField] Image staminaUI;
    // Value
    [SerializeField] float maxStamina = 50.0f;
    [SerializeField] float stamina = 0.0f;
    [SerializeField] float recoverTime = 0.0f;
    [SerializeField] float decreaseVal = 20.0f;
    public void StatusSetting()
    {
        maxStamina = 50.0f;
        stamina = maxStamina;
    }

    public void DecreaseStamina()
    {
        stamina -= (decreaseVal * Time.deltaTime);
        recoverTime = 0.0f;
    }

    public void UpdateStamina()
    {
        recoverTime += Time.deltaTime;
        if (recoverTime >= 3.0f)
        {
            recoverTime = 3.0f;
            stamina += Time.deltaTime;
        }

        if (stamina >= maxStamina) stamina = maxStamina;
        if (stamina < 0.0f) stamina = 0.0f;
        staminaUI.fillAmount = stamina / maxStamina;
    }

    public float GetStatmina()
    {
        return stamina;
    }
}
