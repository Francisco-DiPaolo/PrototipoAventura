using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class IEnemy : MonoBehaviour
{
    private int hp;
    private Transform destination;
    private IStatus status;

    public float cooldown;
    public Slider Vida;
    public NavMeshAgent navMeshAgent;
    private IPersonaje pj;
    float lastShot;

    #region Start ()
    void Start()
    {
        hp = 100;
        Vida.value = hp;

        destination = GameObject.FindGameObjectWithTag("Player").transform;
    }
    #endregion

    #region Update ()
    private void Update()
    {
        status = FindObjectOfType<IStatus>();

        pj = FindObjectOfType<IPersonaje>();

        if (pj.boolTemple) InvokeRepeating(nameof(SetDestination), 3f, 0.5f);
    }
    #endregion

    #region SetDestination ()
    public void SetDestination()
    {
        navMeshAgent.destination = destination.position;
    }
    #endregion

    #region recibirDaño ()
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
    #endregion

    #region desaparecer ()
    public void desaparecer()
    {
        Destroy(gameObject);
    }
    #endregion

    #region OnCollisionEnter ()
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Proyectil"))
            recibirDaño();
    }
    #endregion

    #region OnCollisionStay ()
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            recibirDañoPersonaje();
    }
    #endregion

    #region recibirDañoPersonaje ()
    public void recibirDañoPersonaje()
    {
        if (Time.time - lastShot < cooldown)
        {
            return;
        }
        lastShot = Time.time;

        status.health -= 20;
    }
    #endregion
}
