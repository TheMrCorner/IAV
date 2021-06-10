using UnityEngine;
using System.Collections;
using System;

namespace UCM.IAV.Navegacion
{
    /// <summary>
    /// clase que representa a los nodos de la cuadricula
    /// </summary>
    public class Nodo : IComparable
    {
        /// <summary>
        /// coste del nodo
        /// </summary>
        public float coste;

        /// <summary>
        /// coste total del nodo
        /// </summary>
        public float costeTotal;

        /// <summary>
        /// coste estimado del nodo
        /// </summary>
        public float costeEstimado;

        /// <summary>
        /// booleano que especifica si es obstaculo o no
        /// </summary>
        public bool bObstaculo;

        /// <summary>
        /// nodo del que procede este
        /// </summary>
        public Nodo anterior;

        /// <summary>
        /// posicion del nodo
        /// </summary>
        public Vector3 posicion;

        /// <summary>
        /// contructores
        /// </summary>
        public Nodo()
        {
            this.coste = 1.0f;
            this.costeEstimado = 0.0f;
            this.costeTotal = 1.0f;
            this.bObstaculo = false;
            this.anterior = null;
        }

        public Nodo(Vector3 pos)
        {
            this.coste = 1.0f;
            this.costeEstimado = 0.0f;
            this.costeTotal = 1.0f;
            this.bObstaculo = false;
            this.anterior = null;
            this.posicion = pos;
        }

        /// <summary>
        /// marca el nodo como obstaculo
        /// </summary>
        public void MarcarComoObstaculo()
        {
            this.bObstaculo = true;
        }

        /// <summary>
        /// compara el nodo con el objeto
        /// </summary>
        public int CompareTo(object obj)
        {
            Nodo node = (Nodo)obj;
            //Negative value means object comes before this in the sort
            //order.
            if (this.costeEstimado < node.costeEstimado)
                return -1;
            //Positive value means object comes after this in the sort
            //order.
            if (this.costeEstimado > node.costeEstimado) return 1;
            return 0;
        }
    }
}