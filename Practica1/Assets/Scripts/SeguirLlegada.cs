
namespace UCM.IAV.Movimiento
{
    using System;
    using UnityEngine;

    public class SeguirLlegada : MonoBehaviour
    {
        /// <summary>
        /// Radio de parada
        /// </summary>
		public float radius = 1;

        /// <summary>
        /// Metodo que permite a un agente seguir a un objetivo y quedarse quieto cuando se encuentre cerca.
        /// </summary>
        /// <param name="objetivoPos"></param>
        /// <param name="agente"></param>
        public Direccion seguirLlegada(Vector3 objetivoPos, Agente agente)
        {
            Direccion direccion = new Direccion();
            direccion.lineal = objetivoPos - transform.position;

            if (Math.Abs(direccion.lineal.magnitude) < radius)
                agente.velocidad = new Vector3(0, 0, 0);
            
            direccion.lineal.Normalize();
            direccion.lineal = direccion.lineal * agente.aceleracionMax;
            return direccion;
        }
    }
}
