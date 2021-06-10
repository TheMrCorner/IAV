using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCM.IAV.Movimiento
{
    public class Encarar : Alineamiento
    {
        /// <summary>
        /// Metodo que permite a un agente encarar a un objetivo.
        /// </summary>
        /// <param name="agente"></param>
        public Direccion encarar(Agente agente)
        {
            Vector3 direccion = _objetivo.transform.position - agente.transform.position;

            // Comprobamos que esa dirección no sea nula
            if (direccion.magnitude == 0)
                return null; // No hacemos cambios, un resultado nulo por así decirlo
            
            // Y ahora delegamos en Alineamiento 
            _objetivo.orientacion = Mathf.Atan2(-direccion.x, direccion.z);
            return alinear(agente);
        }
    }
}
