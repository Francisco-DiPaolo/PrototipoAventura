using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    private bool choza;

    #region Start ()
    private void Start()
    {
        healthUI.value = health;
        thirstUI.value = thirst;
        hungerUI.value = hunger;
        hotUI.fillAmount = hot / hotMax;

        oasis = false;
        choza = false;
    }
    #endregion

    #region Update ()
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

        if (thirst > thirstMax) 
            thirst -= Time.deltaTime / thirstTick;
            thirstUI.value = thirst;

        if (hot < hotMax)
            hot += Time.deltaTime / hotTick;
            hotUI.fillAmount = hot / hotMax;

        if (hot == 100)
            thirst -= Time.deltaTime / thirstTick * 2;

        if (oasis == true && hot > 0)
            hot -= Time.deltaTime / hotTick * 15;
            hotUI.fillAmount = hot / hotMax;

        if (choza == true && hot > 0)
            hot -= Time.deltaTime / hotTick * 15;
            hotUI.fillAmount = hot / hotMax;

        ColorChanger();

        EscenaJuego();
    }
    #endregion

    #region ColorChanger ()
    void ColorChanger()
    {
        if (hot >= 80) hotUI.color = Color.red;
        if (hot < 80) hotUI.color = Color.yellow;
        if (hot <= 30) hotUI.color = Color.cyan;
    }
    #endregion

    #region OnTriggerEnter ()
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Oasis")) oasis = true;

        if (other.CompareTag("Sombra")) choza = true;
    }
    #endregion

    #region OnTriggerExit ()
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Oasis")) oasis = false;

        if (other.CompareTag("Sombra")) choza = false;
    }
    #endregion

    #region EscenaJuego ()
    public void EscenaJuego()
    {
        if (health <= 0) SceneManager.LoadScene("End");
    }
    #endregion
}
