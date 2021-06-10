
namespace UCM.IAV.Movimiento
{
    using System;
    using UnityEngine;

    public class Evadir : Huir
    {
        /// <summary>
        /// Cantidad de prediccion
        /// </summary>
        public float maxPredicccion = 1;

        /// <summary>
        /// Metodo que permite a un agente evadir a un objetivo.
        /// </summary>
        /// <param name="objetivo"></param>
        /// <param name="agente"></param>
        public Direccion evadir(Agente objetivo, Agente agente)
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
            return huir(objPos, agente);
        }
    }
}
