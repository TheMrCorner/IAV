using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UCM.IAV.Navegacion
{
    public class AEstrella : MonoBehaviour
    {
        /// <summary>
        /// cola de prioridad de nodos abiertos
        /// </summary>
        public static ColaPrioridad listaAbiertos;

        /// <summary>
        /// lista de nodos cerrados
        /// </summary>
        public static HashSet<Nodo> listaCerrados;

        public int nodosExplorados = 0;
        public double tiempoCalculo = 0;

        /// <summary>
        /// devuelve el coste heuristico estimado de ir del actual al destino
        /// </summary>
        private static float CosteHeuristicoEstimado(Nodo actual, Nodo destino)
        {
            Vector3 vecCost = actual.posicion - destino.posicion;
            return vecCost.magnitude;
        }

        /// <summary>
        /// encuentra el camino utilizando AEstrella
        /// </summary>
        public ArrayList EncontrarCamino(Nodo inicio, Nodo destino)
        {
            double ultimoTiempo = Time.time;

            nodosExplorados = 0;

            inicio.costeTotal = 0.0f;
            inicio.costeEstimado = CosteHeuristicoEstimado(inicio, destino);

            listaAbiertos = new ColaPrioridad();
            listaAbiertos.Push(inicio);
            listaCerrados = new HashSet<Nodo>();

            Nodo nodo = null;

            // Mientras quede algun nodo en la lista de abiertos, se sigue buscando
            while (listaAbiertos.Length != 0)
            {
                nodosExplorados++;

                nodo = listaAbiertos.First();

                // Comprobamos si el nodo ya es el destino, si lo es, terminamos la búsqueda
                if (nodo.posicion == destino.posicion)
                {
                    tiempoCalculo = (double) Time.time - ultimoTiempo;
                    return CalculatePath(nodo);
                }

                // Cogemos los vecinos del nodo y los recorremos
                ArrayList vecinos = new ArrayList();
                ControladorCuadricula.instancia.CogerVecinos(nodo, vecinos);

                int tamVecinos = vecinos.Count;
                for (int i = 0; i < tamVecinos; i++)
                {
                    Nodo vecinoNodo = (Nodo)vecinos[i];

                    // si el vecino no se encuentra en la lista de cerrados, se sigue
                    if (!listaCerrados.Contains(vecinoNodo))
                    {
                        float coste = CosteHeuristicoEstimado(nodo, vecinoNodo);
                        float costeTotal = nodo.costeTotal + coste + vecinoNodo.coste;
                        float costeEstimadoNodo = CosteHeuristicoEstimado(vecinoNodo, destino);

                        vecinoNodo.costeTotal = costeTotal;
                        vecinoNodo.anterior = nodo;
                        vecinoNodo.costeEstimado = costeTotal + costeEstimadoNodo;

                        // si la lista de abiertos no contiene al vecino, se le mete
                        if (!listaAbiertos.Contains(vecinoNodo))
                            listaAbiertos.Push(vecinoNodo);
                    }
                }

                // Se añade el nodo a la lista de cerrados
                listaCerrados.Add(nodo);
                // Se quita el nodo de la lista de abiertos
                listaAbiertos.Remove(nodo);
            }

            // Si el ultimo nodo de la vuelta es distinto al destino, no se ha podido encontrar un camino, salta un error y termina el proceso
            if (nodo.posicion != destino.posicion)
            {
                Debug.LogError("Destino no encontrado.");
                tiempoCalculo = (double)Time.time - ultimoTiempo;
                return null;
            }

            tiempoCalculo = (double)Time.time - ultimoTiempo;

            return CalculatePath(nodo);
        }

        /// <summary>
        /// calcula el camino hasta el nodo
        /// </summary>
        private static ArrayList CalculatePath(Nodo nodo)
        {
            ArrayList camino = new ArrayList();

            while (nodo != null)
            {
                camino.Add(nodo);
                nodo = nodo.anterior;
            }

            camino.Reverse();
            return camino;
        }
    }

}
