using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class IEnemy : MonoBehaviour
{
    private int hp;
    private Transform destination;

    public float cooldown;
    public Slider Vida;
    public NavMeshAgent navMeshAgent;

    float lastShot;


    void Start()
    {
        hp = 100;
        Vida.value = hp;

        //destination = GameObject.FindGameObjectWithTag("Player").transform;
        //InvokeRepeating(nameof(SetDestination), 3f, 0.5f);
    }

    private void SetDestination()
    {
        //navMeshAgent.destination = destination.position;
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
