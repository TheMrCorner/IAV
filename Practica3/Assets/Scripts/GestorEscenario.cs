using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

/// <summary>
/// Este script gestiona si la cantante ha llegado al escenario o no. Esto se usa para 
/// controlar el movimiento de este personaje y pararlo cuando sea necesario. 
/// </summary>
public class GestorEscenario : MonoBehaviour
{
    /// <summary>
    /// Funcion que controla si algún collider entra dentro del collider del escenario. 
    /// Comprueba si es la cantante y si es así le indica que ha entrado en el escenario. 
    /// Además le indica al fantasma que la cantante esta en el escenario
    /// </summary>
    /// <param name="other">Collider del objeto que ha entrado</param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Cantante")
        {
            other.GetComponent<ComportamientoCantante>().EntrarEscenario();
            GameObject.FindGameObjectWithTag("Fantasma").GetComponent<ComportamientoFantasma>().CantanteEnEscenario(true);
        }
        else if(other.gameObject.tag == "Fantasma")
        {
            if (!((SharedBool)other.GetComponent<ComportamientoFantasma>().GetComponent<BehaviorTree>().GetVariable("YendoACantante")).Value)
                other.GetComponent<ComportamientoFantasma>().EnEscenario(false);
        }
    }

    /// <summary>
    /// Funcion que controla si algún collider sale del collider del escenario. 
    /// Comprueba si es la cantante y si es así le indica que ha salido del escenario.
    /// Además le indica al fantasma que la cantante no esta en el escenario
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Cantante")
        {
            other.GetComponent<ComportamientoCantante>().SalirEscenario();
            GameObject.FindGameObjectWithTag("Fantasma").GetComponent<ComportamientoFantasma>().CantanteEnEscenario(false);
        }
    }
}
