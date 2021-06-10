using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Distintos estados por los que pasa una lámpara.
/// </summary>
public enum LamparaEstado
{
    COLGADA,
    CAIDA
}

/// <summary>
/// Este script controla los diferentes estados de una lámpara.
/// </summary>
public class ComportamientoLampara : MonoBehaviour
{
    /// <summary>
    /// Sonido de la lampara rota
    /// </summary>
    public AudioClip roto;
    /// <summary>
    /// Sonido de la lampara arreglada
    /// </summary>
    public AudioClip arreglo;
    /// <summary>
    /// Manejador del sonido
    /// </summary>
    private AudioSource audio;
    /// <summary>
    /// Estado actual de la lampara
    /// </summary>
    private LamparaEstado estado = LamparaEstado.COLGADA;

    private void Start()
    {
        audio = gameObject.AddComponent<AudioSource>();
    }

    /// <summary>
    /// Si el vizconde interactua con la lampara, esta se restaura pasando al estado COLGADA.
    /// </summary>
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E) && other.gameObject.tag == "Vizconde")
            this.cambiarEstado(LamparaEstado.COLGADA);
    }

    /// <summary>
    /// Devuelve el estado de la lampara
    /// </summary>
    public LamparaEstado verEstado() { return this.estado; }

    /// <summary>
    /// Metodo publico que establece un nuevo estado para la lampara, realizando el cambio pertinente en la misma
    /// </summary>
    public void cambiarEstado(LamparaEstado est) 
    {
        this.estado = est;

        switch(est)
        {
            case LamparaEstado.COLGADA:
                audio.clip = arreglo;
                audio.Play();
                this.transform.parent.GetComponent<Rigidbody>().useGravity = false;
                this.transform.parent.transform.localPosition = new Vector3(transform.parent.transform.localPosition.x, 0, transform.parent.transform.localPosition.z);
                break;
            case LamparaEstado.CAIDA:
                audio.clip = roto;
                audio.Play();
                this.transform.parent.GetComponent<Rigidbody>().useGravity = true;
                break;
            default:
                break;
        }
    }
}
