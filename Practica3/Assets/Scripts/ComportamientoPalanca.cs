using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Este script controla las colisiones con una palanca, modificando el estado de la lampara que tenga asociada.
/// </summary>
public class ComportamientoPalanca : MonoBehaviour
{
    /// <summary>
    /// Lampara asociada a la palanca
    /// </summary>
    public GameObject lampara;

    /// <summary>
    /// Cambia el estado de la lampara cuando el fantasma choque con la palanca o cuando el vizconde la use
    /// </summary>
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Fantasma" ||(Input.GetKeyDown(KeyCode.E) && other.gameObject.tag == "Vizconde"))
            this.lampara.GetComponentInChildren<ComportamientoLampara>().cambiarEstado(LamparaEstado.CAIDA);
    }
}
