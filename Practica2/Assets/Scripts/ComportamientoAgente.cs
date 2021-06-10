
namespace UCM.IAV.Navegacion
{
    using System.Collections;
    using System.Collections.Generic;
    using System;
    using UnityEngine;

    /// <summary>
    /// Clase abstracta que funciona como plantilla para todos los comportamientos de agente
    /// </summary>
    public class ComportamientoAgente : MonoBehaviour
    {
        /// <summary>
        /// Objetivo (para aplicar o representar el comportamiento, depende del comportamiento que sea)
        /// </summary>
        protected Agente objetivo;
        /// <summary>
        /// Agente que hace uso del comportamiento
        /// </summary>
        protected Agente agente;
        /// <summary>
        /// camino a seguir
        /// </summary>
        protected GameObject inicio, fin;
        /// <summary>
        /// camino a seguir
        /// </summary>
        public Nodo nInicio { get; set; }
        /// <summary>
        /// camino a seguir
        /// </summary>
        public Nodo nFin { get; set; }
        /// <summary>
        /// camino a seguir
        /// </summary>
        protected ArrayList camino = null;
        /// <summary>
        /// Index al sigueinte nodo del camino
        /// </summary>
        protected int siguienteNodoIndex = 0;
        /// <summary>
        /// Agente que hace uso del comportamiento
        /// </summary>
        protected bool suavizando = false;
        /// <summary>
        /// instancia de A*
        /// </summary>
        protected AEstrella aEstrella = new AEstrella();
        /// <summary>
        /// instancia de suavizado
        /// </summary>
        protected Suavizado suavizado = new Suavizado();


        /// <summary>
        /// Al despertar, establecer el agente (script) que hará uso del comportamiento
        /// </summary>
        public virtual void Awake()
        {
            agente = this.GetComponent<Agente>();
        }

        /// <summary>
        /// En cada tick, establecer la dirección que corresponde al agente, con peso o prioridad si se están usando
        /// </summary>
        public virtual void Update()
        {
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
        /// calcula el camino entre dos puntos
        /// </summary>
        protected void EncontrarCamino()
        {
            nInicio = new Nodo(ControladorCuadricula.instancia.CogerCentroCelda(ControladorCuadricula.instancia.CogerIndexCelda(inicio.transform.position)));
            nFin = new Nodo(ControladorCuadricula.instancia.CogerCentroCelda(ControladorCuadricula.instancia.CogerIndexCelda(fin.transform.position)));

            camino = aEstrella.EncontrarCamino(nInicio, nFin);
            if (suavizando)
                camino = suavizado.SuavizarCamino(camino);

            siguienteNodoIndex = 0;
        }
    }
}
