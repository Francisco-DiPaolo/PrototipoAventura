using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IEnemy : MonoBehaviour
{
    private int hp;

    public Slider Vida;

    public float cooldown;

    float lastShot;


    void Start()
    {
        hp = 100;
        Vida.value = hp;
    }

    public void recibirDaño()
    {
        if (Time.time - lastShot < cooldown)
        {
            return;
        }
        lastShot = Time.time;

        hp -= 50;
        Vida.value = hp;

        if (hp <= 0)
            desaparecer();
    }

    public void desaparecer()
    {

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Proyectil"))
            recibirDaño();
    }
}
