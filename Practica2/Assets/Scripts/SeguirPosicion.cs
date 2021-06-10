using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeguirPosicion : MonoBehaviour
{
    GameObject perseo = null;

    // Update is called once per frame
    void Update()
    {
        if (perseo != null)
        {
            // Mantenemos la posición de Perseo.
            this.transform.position = new Vector3(perseo.transform.position.x, this.transform.position.y, perseo.transform.position.z);
        }
    }

    /// <summary>
    /// Busca a Perseo en la escena y lo guarda para poder seguirlo.
    /// </summary>
    public void AsignaPerseo()
    {
        perseo = GameObject.FindGameObjectWithTag("Perseo");
    }
}
