using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestorSalaMusica : MonoBehaviour
{

    /// <summary>
    /// Gestiona cuando el fantasma llega a la sala de musica
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        ComportamientoFantasma comportamiento = other.GetComponent<ComportamientoFantasma>();
        if (comportamiento != null && comportamiento.GetCantanteApresada())
        {
            comportamiento.EnSalaDeMusica(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        ComportamientoFantasma comportamiento = other.GetComponent<ComportamientoFantasma>();
        if (comportamiento != null)
        {
            comportamiento.EnSalaDeMusica(false);
        }
    }
}
