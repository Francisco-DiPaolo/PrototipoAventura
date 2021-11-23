using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IInventory : MonoBehaviour
{
    public List<ISlot> inventory;
    public Camera camaraPrimeraPersona;
    public Transform inventoryUI;
    public ISlotUI slotUIPrefab;
    public GameObject inventarioImage;
    public GameObject arco;
    public GameObject arcoUI;
    public GameObject cuadrado;

    public Transform arcoPosicion;

    public float cooldown;

    private bool crearBool;

    float lastShot;

    private void Start()
    {
        inventarioImage.SetActive(false);

        arcoUI.SetActive(false);

        crearBool = false;
    }
    private void Update()
    {
        // RAYCASTING TO PICKUP OBJECT
        if (Input.GetKeyDown("e"))
        {
            if (Time.time - lastShot < cooldown)
            {
                return;
            }
            lastShot = Time.time;

            Ray ray = camaraPrimeraPersona.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.TryGetComponent(out IObject obj))
            {
                Add(obj);
            }
        }

        /*if (Input.GetKeyDown("f"))
        {
            Remove("Oro", 1);
        }*/

        if (Input.GetKeyDown("i"))
        {
            if (inventarioImage.activeInHierarchy == false)
            {
                inventarioImage.SetActive(true);
            }
            else
            {
                inventarioImage.SetActive(false);
            }
        }

        UsarArco();

        CrearCuadrado();
    }

    public void Add(IObject item)
    {
        if (Find(item.name) == null || !Find(item.name).stackable)
        {
            var newSlot = new ISlot(item.name, item.quantity, item.stackable);
            inventory.Add(newSlot);

            UpdateUI();
        }
        else
        {
            Find(item.name).quantity += item.quantity;
        }

        Destroy(item.gameObject);
    }

    public ISlot Find(string name) => inventory.Find((item) => item.name == name);

    public void Remove(string name, int quantity)
    {
        var slot = Find(name);

        if (slot.quantity - quantity <= 0)
            inventory.Remove(Find(name));
        else
            slot.quantity -= quantity;

        UpdateUI();

        /*ISlot objeto = Find(obj);  

        //inventory.Remove(obj);
        objeto.quantity -= cant;

        if (objeto.quantity <= 0)
        {
            inventory.Remove(objeto);
        }*/
    }

    public void UpdateUI()
    {
        foreach (Transform child in inventoryUI) if (child.gameObject != slotUIPrefab.gameObject) Destroy(child.gameObject);
        foreach (var item in inventory)
        {
            ISlotUI slot = Instantiate(slotUIPrefab.gameObject, inventoryUI).GetComponent<ISlotUI>();
            slot.itemName.text = item.name + " x " + item.quantity;
            slot.delete.onClick.AddListener(() => Remove(item.name, item.quantity));
            slot.removeOne.onClick.AddListener(() => Remove(item.name, 1));
            slot.crear.onClick.AddListener(() => Crear());
            slot.gameObject.SetActive(true);
        }
    }

    public void CrearCuadrado()
    {
        if (BuscarMat("metal", 2) && (BuscarMat("Oro", 1)))
        {
            if (Input.GetKeyDown("q"))
            {
                Instantiate(cuadrado, transform.position + (transform.forward * 2), transform.rotation);

                Remove("metal", 2);
                Remove("Oro", 1);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mesa"))
        {
            crearBool = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Mesa"))
        {
            crearBool = false;
        }
    }

    public void Crear()
    {
        if (!crearBool) return;

        if (BuscarMat("madera", 5) && (BuscarMat("metal", 10)))
        {
            Remove("madera", 5);
            Remove("metal", 10);

            UpdateUI();

            Instantiate(arco, arcoPosicion/*transform.forward * 5 + new Vector3(0, transform.position.y, 0), transform.rotation*/);
        }
    }

    public bool BuscarMat(string name, int quantity)
    {
        ISlot item = Find(name);

        if (item == null)
        {
            return false;
        }
        else
        {
            if (item.quantity >= quantity)
            {
                return true;
            }
            return false;
        }
    }

    public void UsarArco()
    {
        if(BuscarMat("Arco", 1))
        {
            arcoUI.SetActive(true);
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