using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCM.IAV.Navegacion
{
    public class ConstructorLaberinto : MonoBehaviour
    {
        public GameObject prefabMuro;
        public GameObject prefabSalida;
        public GameObject prefabPerseo;
        public GameObject prefabMinotauro;
        public GameObject prefabInitMinotauro;
        public GameObject prefabFinMinotauro;

        // Se puede modificar en el editor por si fuera necesario
        public Vector3 initPos = new Vector3(1.0f, 0.0f, 1.0f); // Posición inicial desde la que se generará el laberinto
        
        /// <summary>
        /// Función que genera el laberinto en la escena. Coloca los diferentes muros y personajes leyendo de una matriz con la información. 
        /// Básicamente traduce la matriz generada en el Generador de Laberintos a una escena de Unity.
        /// </summary>
        /// <param name="mapa"> Laberinto que hay que crear. </param>
        public void ConstruyeLaberinto(int[,] mapa)
        {
            // Para que tenga más coherencia con el generador, tomaremos esta posición inicial como la esquina superior izda, no afectaría para nada
            for (int i = 0; i < mapa.GetLength(0); i++)
            {
                for (int j = 0; j < mapa.GetLength(1); j++)
                {
                    // Instanciamos los diferentes objetos en función del valor de la matriz
                    if(mapa[i, j] == 0)
                    {
                        Instantiate(prefabPerseo, new Vector3(1.0f + j * 2.0f, 0.0f, 1.0f + i * 2.0f), transform.rotation).transform.parent = transform;
                    }
                    else if (mapa[i, j] == 2 || mapa[i, j] == 3)
                    {
                        Instantiate(prefabMuro, prefabMuro.transform.position + new Vector3(1.0f + j * 2.0f, 0.0f, 1.0f + i * 2.0f), transform.rotation).transform.parent = transform;
                    }
                    else if (mapa[i, j] == 5)
                    {
                        Instantiate(prefabSalida, new Vector3(1.0f + j * 2.0f, 0.0f, 1.0f + i * 2.0f), transform.rotation).transform.parent = transform;
                    }
                    else if (mapa[i, j] == 6)
                    {
                        Instantiate(prefabInitMinotauro, new Vector3(1.0f + j * 2.0f, 0.0f, 1.0f + i * 2.0f), transform.rotation).transform.parent = transform;
                        Instantiate(prefabMinotauro, new Vector3(1.0f + j * 2.0f, 0.0f, 1.0f + i * 2.0f), transform.rotation).transform.parent = transform;
                    }
                    else if(mapa[i, j] == 7)
                    {
                        Instantiate(prefabFinMinotauro, new Vector3(1.0f + j * 2.0f, 0.0f, 1.0f + i * 2.0f), transform.rotation).transform.parent = transform;
                    }
                }
            }
        }
    }
}
