/*    
   Copyright (C) 2020 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/
namespace UCM.IAV.Movimiento
{
	using System;
	using UnityEngine;

    public class Seguir : MonoBehaviour
    {
        /// <summary>
        /// Metodo que permite a un agente moverse hacia un objetivo.
        /// </summary>
        /// <param name="objetivoPos"></param>
        /// <param name="agente"></param>
        public Direccion seguir(Vector3 objetivoPos, Agente agente)
        {
            Direccion direccion = new Direccion();
            direccion.lineal = objetivoPos - transform.position;
            direccion.lineal.Normalize();
            direccion.lineal = direccion.lineal * agente.aceleracionMax;
            return direccion;
        }
    }
}
