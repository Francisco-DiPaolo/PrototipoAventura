using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public Material material;
    public Material material2;
    public GameObject mesaInv;
    public GameObject mesa;

    public float cooldown;

    private Transform objetosMesa;
    private Transform mesaPrefabLugar;

    private int contadorMesa;

    private bool crearBool;
    private bool crearBoolMesa;

    private GameObject mesaPrefab;
    private IStatus status;

    float lastShot;

    private void Start()
    {
        inventarioImage.SetActive(false);

        arcoUI.SetActive(false);

        crearBool = false;
        crearBoolMesa = false;

        contadorMesa = 0;
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

            if (Physics.Raycast(ray, out hit, 4f) && hit.collider.gameObject.TryGetComponent(out IObject obj))
            {
                Add(obj);
                UpdateUI();
            }

            if (Physics.Raycast(ray, out hit, 3f) && hit.collider.gameObject.CompareTag("Oasis"))
            {
                if (status.thirst < 100)
                    status.thirst += 20;

                if (status.thirst >= 100)
                    status.thirst = 100;
            }
        }

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

        Mesa();

        status = FindObjectOfType<IStatus>();

        Win();
    }

    public void Add(IObject item)
    {
        if (Find(item.name) == null || !Find(item.name).stackable)
        {
            var newSlot = new ISlot(item.name, item.quantity, item.stackable, item.Imagen2D);
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
            slot.image.sprite = item.Image2D;
            slot.delete.onClick.AddListener(() => Remove(item.name, item.quantity));
            slot.removeOne.onClick.AddListener(() => Consumible());
            slot.removeOne.onClick.AddListener(() => Remove(item.name, 1));
            slot.crear.onClick.AddListener(() => Crear());
            slot.mesaInvCrear.onClick.AddListener(() => MesaInv());
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
            objetosMesa = other.transform.GetChild(0).gameObject.transform;
        }

        if (other.CompareTag("MesaInv"))
        {
            crearBoolMesa = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Mesa"))
        {
            crearBool = false;
        }

        if (other.CompareTag("MesaInv"))
        {
            crearBoolMesa = false;
        }
    }

    public void Crear()
    {
        if (!crearBool) return;

        if (BuscarMat("madera", 10) && (BuscarMat("Rocas", 6)))
        {
            Remove("madera", 10);
            Remove("Rocas", 6);

            UpdateUI();

            Instantiate(arco, objetosMesa.position, Quaternion.identity);
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

    public void Mesa()
    {
        if (!crearBoolMesa) return;

        if (Input.GetKeyDown("e"))
        {
            if (BuscarMat("madera", 20))
            {
                Remove("madera", 20);

                UpdateUI();

                mesaPrefabLugar = mesaPrefab.transform;

                Instantiate(mesa, mesaPrefabLugar.position, Quaternion.identity);

                Destroy(mesaPrefab);

                contadorMesa = 0;
                //mesaInv.GetComponent<Renderer>().material = material2;
            }
        }
    }

    public void MesaInv()
    {
        if(contadorMesa == 0)
        {
            mesaPrefab = Instantiate(mesaInv, transform.position + (transform.forward * 3), transform.rotation);

            contadorMesa = 1;
        }
    }

    public void Consumible()
    {
        if (BuscarMat("Coco", 1))
        {
            status.hunger += 20;
        }
    }

    public void Win()
    {
        if (BuscarMat("Book", 1))
        {
            SceneManager.LoadScene("End");
        }
    }

    [System.Serializable]
    public class ISlot
    {
        public string name;
        public int quantity;
        public bool stackable;

        public Sprite Image2D;

        public ISlot(string name, int quantity, bool stackable, Sprite Image2D)
        {
            this.name = name;
            this.quantity = quantity;
            this.stackable = stackable;

            this.Image2D = Image2D;
    }
    }
}