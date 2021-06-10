using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestorCelda : MonoBehaviour
{
    /// <summary>
    /// Gentiona cuando la cantante llegua a la celda y es apresada 
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        ComportamientoFantasma comportamiento = other.GetComponent<ComportamientoFantasma>();
        if (comportamiento != null && comportamiento.GetCantanteAtrapada())
        {
            GameObject.FindGameObjectsWithTag("Fantasma")[0].GetComponent<ComportamientoFantasma>().ApresarCantante();
        }
    }
}
