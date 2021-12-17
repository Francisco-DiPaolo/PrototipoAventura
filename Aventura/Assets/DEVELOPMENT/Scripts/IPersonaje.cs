using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IPersonaje : MonoBehaviour
{
    public bool boolTemple;

    #region Start ()
    private void Start()
    {
        boolTemple = false;
    }
    #endregion

    #region OnTriggerStay
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Temple"))
        {
            boolTemple = true;
        }
    }
    #endregion
}
