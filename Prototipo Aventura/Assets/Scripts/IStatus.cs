using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IStatus : MonoBehaviour
{
    public float health, oxygen, hunger, thirst;
    public float healthTick, oxygenTick, hungerTick, thirstTick;
    public float healthMax, oxygenMax, hungerMax, thirstMax;

    private void Update()
    {
        if (health < healthMax) health += Time.deltaTime / healthTick;
        if (oxygen > 0) oxygen -= Time.deltaTime / oxygenTick;
        if (hunger < hungerMax) hunger += Time.deltaTime / hungerTick;
        if (thirst < thirstMax) thirst += Time.deltaTime / thirstTick;

        //hunger = Mathf.Clamp(hunger += Time.deltaTime / hungerTick, 0, hungerMax);
    }
}
