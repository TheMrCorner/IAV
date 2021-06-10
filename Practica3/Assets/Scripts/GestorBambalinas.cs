using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

/// <summary>
/// Este script sirve para gestionar si la cantante está o no en bambalinas. De esta manera
/// abstraemos conocer esta información de la propia cantante. Esto es necesario sobre todo 
/// porque el escenario es muy estrecho para que la cantante quede centrada. 
/// </summary>
public class GestorBambalinas : MonoBehaviour
{
    /// <summary>
    /// Funcion que controla si algún collider entra dentro del collider del escenario. 
    /// Comprueba si es la cantante y si es así le indica que ha entrado en el escenario. 
    /// </summary>
    /// <param name="other">Collider del objeto que ha entrado</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Cantante")
        {
            other.GetComponent<ComportamientoCantante>().EntrarBambalinas();
        }
        else if (other.gameObject.tag == "Fantasma")
        {
            if (!((SharedBool)other.GetComponent<ComportamientoFantasma>().GetComponent<BehaviorTree>().GetVariable("YendoACantante")).Value)
                other.GetComponent<ComportamientoFantasma>().EnBambalinas(false);
        }
    }

    /// <summary>
    /// Funcion que controla si algún collider sale del collider del escenario. 
    /// Comprueba si es la cantante y si es así le indica que ha salido del escenario. 
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Cantante")
        {
            other.GetComponent<ComportamientoCantante>().SalirBambalinas();
        }
    }
}
