using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNight : MonoBehaviour
{
    public GameObject diaNoche;
    public float velocidad;

    #region Update ()
    void Update()
    {
        diaNoche.transform.Rotate(velocidad, 0, 0);
    }
    #endregion
}
