using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ISound : MonoBehaviour
{
    [SerializeField] AudioClip[] audios;

    public AudioSource Sonido;
    //public Slider ControlVolumen;

    private DayNight dayNight;

    #region Awake ()
    private void Awake()
    {
        Sonido = GetComponent<AudioSource>();

        //ControlVolumen.value = PlayerPrefs.GetFloat("Volumen");
    }
    #endregion

    #region Update ()
    private void Update()
    {
        //PlayerPrefs.SetFloat("Volumen", ControlVolumen.value);
        //Sonido.volume = ControlVolumen.value;

        dayNight = FindObjectOfType<DayNight>();

        if (dayNight.diaNoche.transform.rotation.eulerAngles.x < 180)
            Sonido.volume = 0.04f;

        if (dayNight.diaNoche.transform.rotation.eulerAngles.x >= 200)
            Sonido.volume += Time.deltaTime / 800;
    }
    #endregion

    #region SeleccionAudio ()
    public void SeleccionAudio(int indice, float volumen)
    {
        Sonido.PlayOneShot(audios[indice], volumen);
    }
    #endregion
}
