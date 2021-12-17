using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAnimation : MonoBehaviour
{
    public Animator anim;
    public float abilityCooldown;

    float lastShot;

    private IInventory inventory;

    #region Update ()
    private void Update()
    {
        inventory = FindObjectOfType<IInventory>();

        if (inventory.hachaUI.activeInHierarchy == true)
        {
            Pegar();
        }
    }
    #endregion

    #region Pegar ()
    public void Pegar()
    {
        if (Input.GetKeyDown("e"))
        {
            if (Time.time - lastShot < abilityCooldown)
            {
                return;
            }
            lastShot = Time.time;

            anim.SetBool("boolPegar", true);

        } else anim.SetBool("boolPegar", false);
    }
    #endregion
}
