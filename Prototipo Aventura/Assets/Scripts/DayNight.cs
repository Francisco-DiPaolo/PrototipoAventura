using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNight : MonoBehaviour
{
    public GameObject diaNoche;
    public float velocidad;
    public Light mesaLight;

    void Start()
    {
        //mesaLight.enabled = false;
    }
    void Update()
    {
        diaNoche.transform.Rotate(velocidad, 0, 0);

        /*if (mesaLight.rotate.y == 90)
        {
            mesaLight.enabled = true;
        }*/
    }
}
