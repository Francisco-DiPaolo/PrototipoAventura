using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IShot : MonoBehaviour
{
    public Camera camaraPrimeraPersona;
    public GameObject proyectil;

    public float abilityCooldown;

    float lastShot;

    public IInventory inventory;

    private void Update()
    {
        inventory = FindObjectOfType<IInventory>();

        if (inventory.arcoUI.activeInHierarchy == true)
        {
            Shot();
        }
    }

    public void Shot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time - lastShot < abilityCooldown)
            {
                return;
            }
            lastShot = Time.time;

            Ray ray = camaraPrimeraPersona.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            GameObject pro;

            pro = Instantiate(proyectil, ray.origin, transform.rotation);

            Rigidbody rb = pro.GetComponent<Rigidbody>();

            rb.AddForce(camaraPrimeraPersona.transform.forward * 30, ForceMode.Impulse);

            Destroy(pro, 10);

            if ((Physics.Raycast(ray, out hit) == true) && hit.distance < 5)
            {
                Debug.Log("El rayo tocó al objeto: " + hit.collider.name);

                if (hit.collider.name.Substring(0, 3) == "Ene")
                {
                    GameObject objetoTocado = GameObject.Find(hit.transform.name);
                    IEnemy scriptObjetoTocado = (IEnemy)objetoTocado.GetComponent(typeof(IEnemy));

                    if (scriptObjetoTocado != null)
                    {
                        scriptObjetoTocado.recibirDaño();
                    }
                }
            }
        }
    }
}