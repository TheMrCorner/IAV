using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCM.IAV.Movimiento
{
    public class Merodeo : Encarar
    {
        /// <summary>
        // Radio y distancia de separación del círculo de merodeo
        /// </summary>
        public float merodeoOffset;
        public float merodeoRadio;

        /// <summary>
        // Ángulo máximo para el cambio de orientación
        /// </summary>
        public float merodeoAngulo;

        /// <summary>
        // Orientación actual del objetivo
        /// </summary>
        //float merodeoOrientacion = 0.0f;

        /// <summary>
        // Aceleración máxima 
        /// </summary>
        public float maxAcceleracion;

        /// <summary>
        // Contador
        /// </summary>
        public float contadorValor = 5;
        private float contador = 0;

        /// <summary>
        // Tiempo anterior a la ultima resta del contador
        /// </summary>
        private float tiempoAnterior = 0;

        /// <summary>
        // Ultima direccion otorgada
        /// </summary>
        private Direccion ultimaDir;

        /// <summary>
        /// metodo que calcula un binomial aleatorio
        /// </summary>
        private float randomBinomial()
        {
            float rnd1 = Random.Range(-1.0f, 1.0f);
            float rnd2 = Random.Range(-1.0f, 1.0f);

            return rnd1 - rnd2;
        }

        /// <summary>
        /// Metodo que permite a un agente merodear.
        /// </summary>
        /// <param name="agente"></param>
        public Direccion merodeo(Agente agente)
        {
            /************** NO SE USA ***************/
            //// Calculamos el objetivo
            //merodeoOrientacion += randomBinomial() * merodeoAngulo;

            //// Orientación combinada del objetivo
            //float targetOrientation = merodeoOrientacion + agente.orientacion;

            //// Calculamos el centro del círculo de merodeo
            //Vector3 objetivoPosicion = transform.position + (merodeoOffset * agente.OriToVec(agente.orientacion));
            //objetivoPosicion += merodeoRadio * _objetivo.OriToVec(targetOrientation);

            ////_objetivo.transform.position = transform.position + (merodeoOffset * agente.OriToVec(agente.orientacion));
            ////_objetivo.transform.position += merodeoRadio * _objetivo.OriToVec(targetOrientation);

            //Direccion resultado = encarar(agente);

            //resultado.lineal = objetivoPosicion - agente.transform.position;

            ////resultado.lineal = maxAcceleracion * agente.OriToVec(agente.orientacion);
            ////resultado.lineal = _objetivo.transform.position - agente.transform.position;

            //Direccion resultado2 = new Direccion();
            //resultado2.lineal = objetivoPosicion - _objetivo.transform.position;

            //_objetivo.SetDireccion(resultado2);

            /**************************************/

            float tiempoActual = Time.realtimeSinceStartup;
            this.contador -= (tiempoActual - this.tiempoAnterior);
            this.tiempoAnterior = tiempoActual;

            if (this.contador <= 0f)
            { // Reinicia el contador y coge un punto random alrededor del agente y va hacia el.
                this.contador = contadorValor;

                float randomX = Random.Range(-merodeoRadio, merodeoRadio) + agente.transform.position.x;
                float randomZ = Random.Range(-merodeoRadio, merodeoRadio) + agente.transform.position.z;
                Vector3 puntoRandom = new Vector3(randomX, agente.transform.position.y, randomZ);

                Direccion resultado = new Direccion();
                resultado.lineal = puntoRandom - agente.transform.position;

                this.ultimaDir = resultado;

                return resultado;
            }
            else
                return this.ultimaDir;
        }
    }
}
