using System.Collections;
using System.Collections.Generic;
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

    /// <summary>
    /// Clase abstracta que funciona como plantilla para todos los comportamientos de agente
    /// </summary>
    public class ComportamientoAgente : MonoBehaviour
    {
        /// <summary>
        /// Peso
        /// </summary>
        public float peso = 1.0f;
        /// <summary>
        /// Prioridad
        /// </summary>
        public int prioridad = 1;
        /// <summary>
        /// Objetivo (para aplicar o representar el comportamiento, depende del comportamiento que sea)
        /// </summary>
        public Agente objetivo;
        /// <summary>
        /// Agente que hace uso del comportamiento
        /// </summary>
        protected Agente agente;
        /// <summary>
        /// Bool que ocntrola si la flauta esta sonando
        /// </summary>
        protected bool flautaSonando = false;
        /// <summary>
        /// Array de otros agentes relacionados con este agente
        /// </summary>
        protected ArrayList otrosAgentes = new ArrayList();

        /// <summary>
        /// Meter agente en el array de agentes
        /// </summary>
        public void meterAgente(Agente agente)
        {
            otrosAgentes.Add(agente);
        }

        /// <summary>
        /// Coger el array de otros agentes desde fuera
        /// </summary>
        public ArrayList cogerAgentes()
        {
            return otrosAgentes;
        }
        /// <summary>
        /// Al despertar, establecer el agente (script) que hará uso del comportamiento
        /// </summary>
        public virtual void Awake()
        {
            agente = gameObject.GetComponent<Agente>();
        }

        /// <summary>
        /// En cada tick, establecer la dirección que corresponde al agente, con peso o prioridad si se están usando
        /// </summary>
        public virtual void Update()
        {
            if (agente.mezclarPorPeso)
                agente.SetDireccion(GetDireccion(), peso);
            else if (agente.mezclarPorPrioridad)
                agente.SetDireccion(GetDireccion(), prioridad);
            else
                agente.SetDireccion(GetDireccion());
        }

        /// <summary>
        /// Devuelve la direccion calculada
        /// </summary>
        /// <returns></returns>
        public virtual Direccion GetDireccion()
        {
            return new Direccion();
        }

        /// <summary>
        /// Asocia la rotación al rango de 360 grados
        /// </summary>
        /// <param name="rotacion"></param>
        /// <returns></returns>
        public float RadianesAGrados(float rotacion)
        {
            rotacion %= 360.0f;
            if (Mathf.Abs(rotacion) > 180.0f)
            {
                if (rotacion < 0.0f)
                    rotacion += 360.0f;
                else
                    rotacion -= 360.0f;
            }
            return rotacion;
        }

        /// <summary>
        /// Cambia el valor real de la orientación a un Vector3 
        /// </summary>
        /// <param name="orientacion"></param>
        /// <returns></returns>
        public Vector3 OriToVec(float orientacion)
        {
            Vector3 vector = Vector3.zero;
            vector.x = Mathf.Sin(orientacion * Mathf.Deg2Rad) * 1.0f;
            vector.z = Mathf.Cos(orientacion * Mathf.Deg2Rad) * 1.0f;
            return vector.normalized;
        }

        /// <summary>
        /// Cambia el valor del bool flautaSonando segun el Input
        /// </summary>
        /// <returns></returns>
        protected void detectarInputFlauta()
        {
            if (Input.GetKey(KeyCode.Space) == true && !flautaSonando)
                flautaSonando = true;
            else if (Input.GetKey(KeyCode.Space) == false && flautaSonando)
                flautaSonando = false;
            else if (Input.GetKey(KeyCode.Escape))
                Application.Quit();
        }
    }
}
