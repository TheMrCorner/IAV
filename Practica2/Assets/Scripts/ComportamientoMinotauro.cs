
namespace UCM.IAV.Navegacion
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// clase que define el comportamiento del Minotauro
    /// </summary>
    public class ComportamientoMinotauro : ComportamientoAgente
    {
        private bool volviendo = true;
        public bool debug = true;

        private Direccion dir = new Direccion();

        /// <summary>
        /// nodo actual
        /// </summary>
        public Nodo nActual { get; set; }

        void Start()
        {
            // se pone al reves porque con CambiarSentido() se pone bien
            fin = GameObject.FindGameObjectWithTag("InicioMinotauro");
            inicio = GameObject.FindGameObjectWithTag("FinMinotauro");

            CambiarSentido();

            this.transform.position = new Vector3(inicio.transform.position.x, this.transform.position.y, inicio.transform.position.z);

            nActual = ControladorCuadricula.instancia.CogerCelda(this.transform.position);
            nActual.coste = 100;
        }

        void Update()
        {
            Movimiento();

            if (ControladorCuadricula.instancia.CogerIndexCelda(this.transform.position) == ControladorCuadricula.instancia.CogerIndexCelda(fin.transform.position))
                CambiarSentido();

            agente.SetDireccion(dir);
        }

        /// <summary>
        /// controla el movimiento del minotauro
        /// </summary>
        private void Movimiento()
        {
            if (camino != null)
            {
                dir.lineal = ((Nodo)camino[siguienteNodoIndex]).posicion - this.transform.position;
                dir.lineal.Normalize();
                dir.lineal *= agente.aceleracionMax;

                Nodo nNuevoNodo = ControladorCuadricula.instancia.CogerCelda(this.transform.position);
                if(nNuevoNodo != nActual)
                {
                    nActual.coste = 1;
                    nNuevoNodo.coste = 100;
                    nActual = nNuevoNodo;
                }

                if (ControladorCuadricula.instancia.CogerIndexCelda(this.transform.position) == ControladorCuadricula.instancia.CogerIndexCelda(((Nodo)camino[siguienteNodoIndex]).posicion))
                    if (siguienteNodoIndex < camino.Count - 1)
                        siguienteNodoIndex++;
            }
        }

        /// <summary>
        /// cambia el sentido de la patrulla del minotauro
        /// </summary>
        private void CambiarSentido()
        {
            GameObject tmp = fin;
            fin = inicio;
            inicio = tmp;

            dir.lineal = new Vector3(0, 0, 0);

            EncontrarCamino();
        }

        /// <summary>
        /// en caso de necesidad, dibuja el camino del minotauro
        /// </summary>
        void OnDrawGizmos()
        {
            if (debug)
            {
                if (camino == null)
                    return;
                if (camino.Count > 0)
                {
                    int index = 1;
                    foreach (Nodo node in camino)
                    {
                        if (index < camino.Count)
                        {
                            Nodo siguiente = (Nodo)camino[index];
                            Debug.DrawLine(node.posicion, siguiente.posicion, Color.red);
                            index++;
                        }
                    }
                }
            }
        }
    }
}
