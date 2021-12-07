using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IStatus : MonoBehaviour
{
    public float health, hunger, thirst, hot;
    public float healthTick, hungerTick, thirstTick, hotTick;
    public float healthMax, hungerMax, thirstMax, hotMax;

    public Slider healthUI;
    public Slider thirstUI;
    public Slider hungerUI;
    public Image hotUI;

    private bool oasis;

    private void Start()
    {
        healthUI.value = health;
        thirstUI.value = thirst;
        hungerUI.value = hunger;
        hotUI.fillAmount = hot / hotMax;

        oasis = false;
    }
    private void Update()
    {
        //health -= Time.deltaTime / healthTick;
        if (health > healthMax)
        {
            if (thirst <= 0 && hot >= 100 && hunger <= 0)
            {
                health -= Time.deltaTime / healthTick * 6;
            }
            else if (thirst <= 0 && hot >= 100 || thirst <= 0 && hunger <= 0 || hot >= 100 && hunger <= 0)
            {
                health -= Time.deltaTime / healthTick * 3;
            }
            else if (thirst <= 0 || hot >= 100 || hunger <= 0)
            {
                health -= Time.deltaTime / healthTick;
            }

            healthUI.value = health;
        }

        if (hunger > hungerMax) hunger -= Time.deltaTime / hungerTick;
        hungerUI.value = hunger;

        if (thirst > thirstMax) thirst -= Time.deltaTime / thirstTick;
        thirstUI.value = thirst;

        if (hot < hotMax)
        { 
            hot += Time.deltaTime / hotTick;
            hotUI.fillAmount = hot / hotMax;
        }

        if (oasis == true && hot > 0)
        {
            hot -= Time.deltaTime / hotTick * 15;
            hotUI.fillAmount = hot / hotMax;
        }

        ColorChanger();
    }

    void ColorChanger()
    {
        if (hot >= 80)
        {
            hotUI.color = Color.red;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Oasis"))
        {
            oasis = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Oasis"))
        {
            oasis = false;
        }
    }
}
