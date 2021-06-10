using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCM.IAV.Movimiento
{
    public class Separacion : MonoBehaviour
    {
        /// <summary>
        /// Coeficiente de deterioro
        /// </summary>
        public float coeficienteDeterioro = 1;

        /// <summary>
        /// Radio de detencion
        /// </summary>
        public float radio = 1;

        /// <summary>
        /// Metodo que compara al agente con otros agentes relacionados, devolviendo una dirección que le permita separarse de esos
        /// otros agentes si se encuentran cerca de él.
        /// </summary>
        /// <param name="otrosAgentes"></param>
        public Direccion separar(ArrayList otrosAgentes)
        {
            Direccion resultado = new Direccion();

            int numAgentes = otrosAgentes.Count;
            for (int i = 0; i < numAgentes; i++)
            {
                Agente agenteObjetivo = (Agente)otrosAgentes[i];

                Vector3 direccion = this.GetComponent<Transform>().position - agenteObjetivo.GetComponent<Transform>().position;
                float distancia = Mathf.Abs(direccion.magnitude);

                if (distancia < radio)
                {
                    float fuerza = Mathf.Min(coeficienteDeterioro / (distancia * distancia), this.GetComponent<Agente>().aceleracionMax);

                    direccion.Normalize();
                    resultado.lineal += fuerza * direccion;
                }
            }

            return resultado;
        }
    }
}
