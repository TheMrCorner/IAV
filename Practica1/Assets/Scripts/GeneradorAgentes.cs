using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCM.IAV.Movimiento
{
    public class GeneradorAgentes : MonoBehaviour
    {   
        /// <summary>
        /// Prefab a generar
        /// </summary>
        public Agente varPrefab;
        /// <summary>
        /// Objetivo que marcar en el comportamiento agente del agente a generar
        /// </summary>
        public Agente objetivo;
        /// <summary>
        /// Numero de agentes a generar
        /// </summary>
        public int numAgentes;

        // Start is called before the first frame update
        void Start()
        {
            varPrefab.GetComponent<ComportamientoAgente>().objetivo = objetivo;
            Generar(); // LLamamos al método generar
        }

        /// <summary>
        /// Metodo que genera X numero de agentes y añade a esos agentes un array con los otros agentes generados.
        /// </summary>
        void Generar()
        {
            ArrayList agentesGenerados = new ArrayList();

            for (int i = 0; i < numAgentes; i++)
            {
                float aleatorioX, aleatorioZ;

                // Punto random al rededor del jugador
                aleatorioX = Random.Range(objetivo.transform.position.x - 5, objetivo.transform.position.x + 5);
                aleatorioZ = Random.Range(objetivo.transform.position.z - 5, objetivo.transform.position.x + 5);

                Agente clon = Instantiate(varPrefab, new Vector3(aleatorioX, transform.position.y, aleatorioZ), Quaternion.identity); // Creacion de la copia del agente

                agentesGenerados.Add(clon); // Añadir ese nuevo clon al array de agentes generados
            }

            //Añadir a cada uno de los agentes clonados el resto de agentes clonados
            for (int i = 0; i < numAgentes; i++)
            {
                for(int j = 0; j < numAgentes; j++)
                {
                    if(i != j)
                    {
                        Agente agenteActual = (Agente) agentesGenerados[i], otroAgente = (Agente) agentesGenerados[j];
                        agenteActual.GetComponent<ComportamientoAgente>().meterAgente(otroAgente);
                    }
                }
            }
        }
    }
}
