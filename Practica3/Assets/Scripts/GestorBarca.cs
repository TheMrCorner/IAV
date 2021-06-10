using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Este script gestiona
/// </summary>
public class GestorBarca : MonoBehaviour
{
    /// <summary>
    /// Componente de salto de NavMesh original en las direcciones 
    /// iniciales. 
    /// </summary>
    public OffMeshLink salto;
    /// <summary>
    /// Componente de salto de NavMesh invertido.
    /// </summary>
    public OffMeshLink reverso;

    /// <summary>
    /// Punto de origen.
    /// </summary>
    private GameObject puntoA;
    /// <summary>
    /// Punto de llegada.
    /// </summary>
    private GameObject puntoB;
    /// <summary>
    /// Script de comportamiento de la barca asociada.
    /// </summary>
    private ComportamientoBarca barca;
    /// <summary>
    /// Flag que gestiona si algún agente ha entrado en la barca. 
    /// </summary>
    private bool entrada = false;


    private void Awake()
    {
        puntoA = salto.startTransform.gameObject;
        puntoB = salto.endTransform.gameObject;

        barca = this.gameObject.GetComponentInChildren<ComportamientoBarca>();
    }
    /// <summary>
    /// Mueve la barca de una orilla a la otra, cambiando las orillas cuando termine el viaje
    /// </summary>
    public void LlegadaPunto(GameObject punto, Collider character)
    {
        // orilla desde la que sale la barca
        if(punto == puntoA && !entrada)
        {
            entrada = true;

            barca.MoverBarca();

            if (character.GetComponent<ControlesJugador>())
            {
                character.GetComponent<ControlesJugador>().EnBarca(true, barca.transform.GetChild(0).transform);
            }
        }
        // orilla a la que llega la barca
        else if(entrada && (punto == puntoB))
        {
            if (character.GetComponent<ControlesJugador>())
            {
                character.GetComponent<ControlesJugador>().EnBarca(false, puntoB.transform.GetChild(0).transform);
            }

            SwapJump();

            entrada = false;
        }
    }
    /// <summary>
    /// Activa una direccion del salto u otra dependiendo de cual deba estar activada
    /// </summary>
    public void SwapJump()
    {
        if (salto.activated)
        {
            salto.activated = false;
            reverso.activated = true;

            puntoA = reverso.startTransform.gameObject;
            puntoB = reverso.endTransform.gameObject;
        }
        else
        {
            salto.activated = true;
            reverso.activated = false;

            puntoA = salto.startTransform.gameObject;
            puntoB = salto.endTransform.gameObject;
        }
    }
}
