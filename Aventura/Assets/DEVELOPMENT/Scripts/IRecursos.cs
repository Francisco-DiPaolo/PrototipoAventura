using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IRecursos : MonoBehaviour
{
    public int hp;
    public GameObject objeto1;

    public Transform pos1;
    public Transform pos2;
    public Transform pos3;

    private IInventory inventory;
    private ISound sound;

    #region Update ()
    void Update()
    {
        inventory = FindObjectOfType<IInventory>();
        sound = FindObjectOfType<ISound>();
    }
    #endregion

    #region recibirDaño ()
    public void recibirDaño()
    {
        hp -= 1;

        if (hp <= 0) inventory.boolAgarrar = true;

        sound.SeleccionAudio(0, 6f);
    }
    #endregion

    #region recibirDañoAumentado ()
    public void recibirDañoAumentado()
    {
        hp -= 2;

        if (hp <= 0) inventory.boolAgarrar = true;

        sound.SeleccionAudio(0, 6f);
    }
    #endregion

    #region CaidaRecursos ()
    public void CaidaRecursos()
    {
        Instantiate(objeto1, pos1.position, Quaternion.identity);
        Instantiate(objeto1, pos2.position, Quaternion.identity);
        Instantiate(objeto1, pos3.position, Quaternion.identity);
    }
    #endregion
}