using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCM.IAV.Movimiento
{
    public class Huir : MonoBehaviour
    {
        /// <summary>
        /// Metodo que permite a un agente huir de un objetivo.
        /// </summary>
        /// <param name="objetivoPos"></param>
        /// <param name="agente"></param>
        public Direccion huir(Vector3 objetivoPos, Agente agente)
        {
            Direccion direccion = new Direccion();
            direccion.lineal = transform.position - objetivoPos;
            direccion.lineal.Normalize();
            direccion.lineal = direccion.lineal * agente.aceleracionMax;

            return direccion;
        }
    }
}
