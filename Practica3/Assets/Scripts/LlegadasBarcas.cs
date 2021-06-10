using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script que controla las colisiones e interacciones con la barca
/// </summary>
public class LlegadasBarcas : MonoBehaviour
{
    /// <summary>
    /// Script del gestor de la barca asociada.
    /// </summary>
    public GestorBarca gestor;

    /// <summary>
    /// Mueve la barca al hacer colision y no es el vizconde
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        gestor.LlegadaPunto(this.gameObject, other);
    }
}
