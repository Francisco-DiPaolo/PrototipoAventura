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
    public GameObject hacha;
    public GameObject hachaUI;
    public GameObject cuadrado;
    public GameObject mesaInv;
    public GameObject mesa;
    public GameObject chozaInv;
    public GameObject choza;
    public bool boolAgarrar;
    public bool boolRecibirDaño;
    public bool boolUsarArco;
    public bool boolUsarHacha;

    public float cooldown;
    public float cooldownObj;
    public float timeObj = 2.0f; 


    private Transform objetosMesa;
    private Transform mesaPrefabLugar;
    private Transform chozaPrefabLugar;

    private int contadorMesa;
    private int contadorChoza;
    private int cantArco;
    private int cantHacha;

    private bool crearBool;
    private bool crearBoolMesa;
    private bool crearBoolChoza;

    private GameObject mesaPrefab;
    private GameObject chozaPrefab;

    private IStatus status;
    private ISound sound;

    float lastShot;


    #region Start ()
    private void Start()
    {
        inventarioImage.SetActive(false);

        arcoUI.SetActive(false);
        //hachaUI.SetActive(false);

        crearBool = false;
        crearBoolMesa = false;
        crearBoolChoza = false;

        contadorMesa = 0;
        contadorChoza = 0;
        cantArco = 0;
        cantHacha = 0;

        boolAgarrar = false;
        boolRecibirDaño = false;
    }
    #endregion

    #region Update ()
    public void Update()
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
                if (hit.collider.CompareTag("Recursos"))
                {
                    IRecursos recursosRecoger = hit.collider.GetComponent<IRecursos>();

                    if (boolRecibirDaño) recursosRecoger.recibirDañoAumentado();
                    else recursosRecoger.recibirDaño();

                    if (boolAgarrar)
                    {
                        Add(obj);
                        recursosRecoger.CaidaRecursos();

                        boolAgarrar = false;
                    }
                }
                else
                {
                    Add(obj);

                    sound.SeleccionAudio(1, 5f);
                }

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

        if (boolUsarHacha)
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                hachaUI.SetActive(true);
                arcoUI.SetActive(false);
            }

        if (boolUsarArco)
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                hachaUI.SetActive(false);
                arcoUI.SetActive(true);
            }
        

        UsarArco();
        UsarHacha();

        //CrearCuadrado();

        Mesa();
        Choza();

        status = FindObjectOfType<IStatus>();
        sound = FindObjectOfType<ISound>();

        HachaActivada();

        Win();
    }
    #endregion

    #region Add ()
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

        StartCoroutine(ActivarTree(item));
    }
    #endregion Add ()

    #region ActivarTree ()
    IEnumerator ActivarTree(IObject item)
    {
        IRecursos recursos = item.gameObject.GetComponent<IRecursos>();

        if (recursos != null)
        {
            item.gameObject.SetActive(false);

            yield return new WaitForSeconds(timeObj);

            item.gameObject.SetActive(true);

            recursos.hp = 3;
        }
        else
        {
            Destroy(item.gameObject);
        }
    }
    #endregion

    #region Find ()
    public ISlot Find(string name) => inventory.Find((item) => item.name == name);
    #endregion

    #region Remove ()
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
    #endregion

    #region UpdateUI ()
    public void UpdateUI() 
    {
        foreach (Transform child in inventoryUI) if (child.gameObject != slotUIPrefab.gameObject) Destroy(child.gameObject);
        foreach (var item in inventory)
        {
            ISlotUI slot = Instantiate(slotUIPrefab.gameObject, inventoryUI).GetComponent<ISlotUI>();
            slot.itemName.text = item.name + " x " + item.quantity;
            slot.image.sprite = item.Image2D;
            slot.delete.onClick.AddListener(() => Remove(item.name, item.quantity));
            if (item.name == "Coco")
            slot.removeOne.onClick.AddListener(() => Consumible());
            else
            slot.removeOne.onClick.AddListener(() => Remove(item.name, 1));
            slot.crear.onClick.AddListener(() => Crear());
            slot.crearHacha.onClick.AddListener(() => CrearHacha());
            slot.mesaInvCrear.onClick.AddListener(() => MesaInv());
            slot.chozaInvCrear.onClick.AddListener(() => ChozaInv());
            slot.gameObject.SetActive(true);
        }
    }
    #endregion

    #region CrearCuadrado ()
    /*public void CrearCuadrado()
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
    }*/
    #endregion

    #region OnTriggerEnter ()
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

        if (other.CompareTag("ChozaInv"))
        {
            crearBoolChoza = true;
        }
    }
    #endregion

    #region OnTriggerExit ()
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

        if (other.CompareTag("ChozaInv"))
        {
            crearBoolChoza = false;
        }
    }
    #endregion

    #region Crear ()
    public void Crear()
    {
        if (!crearBool) return;

        if (cantArco == 0)
        {
            if (BuscarMat("madera", 10) && (BuscarMat("Rocas", 6)))
            {
                Remove("madera", 10);
                Remove("Rocas", 6);

                UpdateUI();

                Instantiate(arco, objetosMesa.position, Quaternion.identity);
                cantArco = 1;

                sound.SeleccionAudio(2, 4f);
            }
        }
    }
    #endregion

    #region CrearHacha ()
    public void CrearHacha()
    {
        if (!crearBool) return;

        if (cantHacha == 0)
        {
            if (BuscarMat("madera", 10) && (BuscarMat("Rocas", 6)))
            {
                Remove("madera", 10);
                Remove("Rocas", 6);

                UpdateUI();

                Instantiate(hacha, objetosMesa.position, Quaternion.identity);
                cantHacha = 1;

                sound.SeleccionAudio(2, 4f);
            }
        }
    }
    #endregion

    #region BuscarMat ()
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
    #endregion

    #region UsarArco ()
    public void UsarArco()
    {
        if(BuscarMat("Arco", 1))
        {
            boolUsarArco = true;
        }
    }
    #endregion

    #region UsarHacha ()
    public void UsarHacha()
    {
        if (BuscarMat("Hacha", 1))
        {
            boolUsarHacha = true;
        }
    }
    #endregion

    #region Mesa ()
    public void Mesa()
    {
        if (!crearBoolMesa) return;

        if (Input.GetKeyDown("e"))
        {
            if (BuscarMat("madera", 20))
            {
                Remove("madera", 20);

                mesaPrefabLugar = mesaPrefab.transform;

                Instantiate(mesa, mesaPrefabLugar.position, Quaternion.identity);

                sound.SeleccionAudio(2, 4f);

                Destroy(mesaPrefab);
                crearBoolMesa = false;

                contadorMesa = 0;

                UpdateUI();
                //mesaInv.GetComponent<Renderer>().material = material2;
            }
        }
    }
    #endregion

    #region MesaInv()
    public void MesaInv()
    {
        if(contadorMesa == 0)
        {
            mesaPrefab = Instantiate(mesaInv, transform.position + (transform.forward * 3), transform.rotation);

            contadorMesa = 1;
        }
    }
    #endregion

    #region Choza()
    public void Choza()
    {
        if (!crearBoolChoza) return;

        if (Input.GetKeyDown("e"))
        {
            if (BuscarMat("madera", 10) && BuscarMat ("Hojas", 20))
            {
                Remove("madera", 10);
                Remove("Hojas", 20);

                chozaPrefabLugar = chozaPrefab.transform;

                Instantiate(choza, chozaPrefabLugar.position, Quaternion.identity);

                sound.SeleccionAudio(2, 4f);

                Destroy(chozaPrefab);
                contadorChoza = 0;

                UpdateUI();
            }
        }
    }
    #endregion

    #region ChozaInv ()
    public void ChozaInv()
    {
        if (contadorChoza == 0)
        {
            chozaPrefab = Instantiate(chozaInv, transform.position + (transform.forward * 5.5f), transform.rotation);
            contadorChoza = 1;
        }
    }
    #endregion

    #region Comsumible ()
    public void Consumible()
    {
        {
            Remove("Coco", 1);

            status.hunger += 20;
            status.health += 10;

            if (status.hunger >= 100) status.hunger = 100;
            if (status.health >= 100) status.health = 100;
        }
    }
    #endregion

    #region HachaActivada ()
    private void HachaActivada()
    {
        if (hachaUI.activeInHierarchy == true)
            boolRecibirDaño = true;
    }
    #endregion

    #region Win ()
    public void Win()
    {
        if (BuscarMat("Book", 1))
        {
            SceneManager.LoadScene("End");
        }
    }
    #endregion

    #region ISlot
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
    #endregion
}