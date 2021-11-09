using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IInventory : MonoBehaviour
{
    public List<ISlot> inventory;
    public Camera camaraPrimeraPersona;

    private void Update()
    {
        // RAYCASTING TO PICKUP OBJECT
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camaraPrimeraPersona.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.TryGetComponent(out IObject obj))
            {
                Add(obj);
            }
        }

        if (Input.GetKeyDown("e"))
        {
            Remove("Oro", 1);
        }
    }

    public void Add(IObject item)
    {
        if (Find(item.name) == null || !Find(item.name).stackable)
        {
            var newSlot = new ISlot(item.name, item.quantity, item.stackable);
            inventory.Add(newSlot);
        }
        else
        {
            Find(item.name).quantity += item.quantity;
        }

        Destroy(item.gameObject);
    }

    public ISlot Find(string name) => inventory.Find((item) => item.name == name);

    public void Remove(string obj, int cant)
    {
        ISlot objeto = Find(obj);  

        //inventory.Remove(obj);
        objeto.quantity -= cant;

        if (objeto.quantity <= 0)
        {
            inventory.Remove(objeto);
        }
    }

    [System.Serializable]
    public class ISlot
    {
        public string name;
        public int quantity;
        public bool stackable;

        public ISlot(string name, int quantity, bool stackable)
        {
            this.name = name;
            this.quantity = quantity;
            this.stackable = stackable;
        }
    }
}