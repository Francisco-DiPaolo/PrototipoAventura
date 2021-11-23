using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IStatus : MonoBehaviour
{
    public float health, oxygen, hunger, thirst;
    public float healthTick, oxygenTick, hungerTick, thirstTick;
    public float healthMax, oxygenMax, hungerMax, thirstMax;

    public Slider healthUI;
    public Slider thirstUI;
    public Slider hungerUI;

    private void Start()
    {
        healthUI.value = health;
        thirstUI.value = thirst;
        hungerUI.value = hunger;
    }
    private void Update()
    {
        if (health > healthMax) health -= Time.deltaTime / healthTick;
        healthUI.value = health;

        //if (oxygen > 0) oxygen -= Time.deltaTime / oxygenTick;
        if (hunger > hungerMax) hunger -= Time.deltaTime / hungerTick;
        hungerUI.value = hunger;

        if (thirst > thirstMax) thirst -= Time.deltaTime / thirstTick;
        thirstUI.value = thirst;

        //hunger = Mathf.Clamp(hunger += Time.deltaTime / hungerTick, 0, hungerMax);
    }
}
