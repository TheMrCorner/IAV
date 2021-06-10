using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCM.IAV.Movimiento
{
    public class Alineamiento : MonoBehaviour
    {
        /// <summary>
        // Objetivo hacia el cual queremos alinearnos
        /// </summary>
        public Agente _objetivo;

        /// <summary>
        // Maxima aceleracion y rotacion del objetivo al que alinearte
        /// </summary>
        public float maxAceleracionAngularObjetivo;
        public float maxRotacionObjetivo;

        /// <summary>
        // Distancia de llegada
        /// </summary>
        public float radioLlegada = 0.5f;

        /// <summary>
        // Distancia de frenado
        /// </summary>
        public float radioFrenado = 1.5f;

        /// <summary>
        // Tiempo para alcanzar la velocidad
        /// </summary>
        public float timeToTarget = 0.1f;

        /// <summary>
        /// Metodo que permite a un agente alinearse con un objetivo
        /// </summary>
        /// <returns>Nueva Dirección que tendrá que seguir el agente</returns>
        /// <param name="agente"></param>
        public Direccion alinear(Agente agente)
        {
            Direccion resultado = new Direccion();
            float rotacionObjetivo;

            // Dirección al objetivo
            float rotacion = _objetivo.orientacion - agente.orientacion;

            // Cambiar a radianes 
            rotacion = rotacion % 360; // Mapeamos para que sea menor de 360
            rotacion = (rotacion > 180) ? rotacion - 360 : rotacion; // Limitamos a 180 y -180
            rotacion = rotacion * Mathf.Deg2Rad; // Transformamos a radianes

            float tamRotacion = Mathf.Abs(rotacion);  // Sacamos el valor absoluto

            // Si ya estamos alineados con el objetivo, no tocamos nada
            if (tamRotacion < radioLlegada)
            {
                resultado.angular = 0;
                return resultado;
            }

            // Slow radius checking
            if (tamRotacion > radioFrenado)
            {
                rotacionObjetivo = maxRotacionObjetivo;
            }
            // Calcular la rotacion
            else
            {
                rotacionObjetivo = maxRotacionObjetivo * (tamRotacion / radioFrenado);
            }

            rotacionObjetivo *= rotacion / tamRotacion;

            resultado.angular = rotacionObjetivo - agente.rotacion;
            resultado.angular /= timeToTarget;

            // Suavizar
            float angularAcceleration = Mathf.Abs(resultado.angular);
            if (angularAcceleration > maxAceleracionAngularObjetivo)
            {
                resultado.angular /= angularAcceleration;
                resultado.angular *= maxAceleracionAngularObjetivo;
            }

            resultado.lineal = new Vector3(0.0f, 0.0f, 0.0f);

            return resultado;
        }
    }
}