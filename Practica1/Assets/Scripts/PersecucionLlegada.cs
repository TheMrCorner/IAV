
namespace UCM.IAV.Movimiento
{
    using System;
    using UnityEngine;

    public class PersecucionLlegada : SeguirLlegada
    {
        /// <summary>
        /// Cantidad de prediccion
        /// </summary>
        public float maxPredicccion = 1;

        /// <summary>
        /// Metodo que permite a un agente perseguir a un objetivo, parandose cuando esta cerca de él.
        /// </summary>
        /// <param name="objetivo"></param>
        /// <param name="agente"></param>
        public Direccion persecucionLlegada(Agente objetivo, Agente agente)
        {
            Direccion direccion = new Direccion();
            direccion.lineal = objetivo.transform.position - transform.position;

            float distancia = Math.Abs(direccion.lineal.magnitude);
            float agenteVel = Math.Abs(agente.velocidad.magnitude);

            float prediccion;
            if (agenteVel <= distancia / maxPredicccion)
                prediccion = maxPredicccion;
            else
                prediccion = distancia / agenteVel;

            Vector3 objPos = objetivo.GetComponent<Transform>().position;
            objPos += objetivo.velocidad * prediccion;

            return seguirLlegada(objPos, agente);
        }
    }
}
