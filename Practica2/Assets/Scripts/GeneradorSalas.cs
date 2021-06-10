using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UCM.IAV.Navegacion
{

    /// <summary>
    /// Clase que servirá sólo para colocar salas en el laberinto. Comprueba el máximo. 
    /// </summary>
    public class GeneradorSalas
    {
        // Guardamos anchura y altura para gestionar laberintos rectangulares.
        int alturaTotal;
        int anchuraTotal;
        int ladoSalasGeneral; 
        
        /// <summary>
        /// Sitúa una sala de tamaño ladoSalasGeneral desde la posición (initX, initY)
        /// proporcionada comprobando que no se sale por los laterales.
        /// </summary>
        /// <param name="mapa">Laberinto donde meter la sala</param>
        /// <param name="initX">Coordenada X donde empieza la sala</param>
        /// <param name="initY">Coordenada Y donde empieza la sala</param>
        private void SituaSala(ref int[,] mapa, int initX, int initY)
        {
            for (int i = 0; i < ladoSalasGeneral; i++)
            {
                for (int j = 0; j < ladoSalasGeneral; j++)
                {
                    // Comprobamos que no nos salimos del laberinto
                    if (initX + i < anchuraTotal - 1)
                    {
                        if (initY + j < alturaTotal - 1)
                        {
                            // Colocamos la sala
                            if (mapa[initY + j, initX + i] != 1)
                            {
                                mapa[initY + j, initX + i] = 1;
                            } // if
                        } // if
                    } // if
                } // for
            } // for
        } // SituaSala

        /// <summary>
        /// Función que genera las salas en el laberinto. Divide el mismo en cuatro cuadrantes
        /// y sitúa las habitaciones en ellos, asegurando de esta manera que no se salgan por 
        /// los límites y que queden lo más centradas posible.
        /// </summary>
        /// <param name="mapa">Laberinto en el que meter las salas.</param>
        /// <param name="ladoSalas">Tamaño de las salas.</param>
        public void CreaSalas(ref int[,] mapa, int ladoSalas)
        {
            alturaTotal = mapa.GetLength(0);
            anchuraTotal = mapa.GetLength(1);
            ladoSalasGeneral = ladoSalas;

            int initX = 0;
            int initY = 0;

            // Números aleatorios para colocar las salas
            System.Random rnd = new System.Random();

            // Primer cuadrante
            initX = rnd.Next(2, anchuraTotal / 2);
            initY = rnd.Next(2, alturaTotal / 2);

            SituaSala(ref mapa, initX, initY);

            // Segundo cuadrante
            initX = rnd.Next(anchuraTotal / 2, anchuraTotal - (ladoSalas + 1));
            initY = rnd.Next(2, alturaTotal / 2);

            SituaSala(ref mapa, initX, initY);

            // Tercer cuadrante
            initX = rnd.Next(2, anchuraTotal / 2);
            initY = rnd.Next(alturaTotal / 2, alturaTotal - (ladoSalas + 1));

            SituaSala(ref mapa, initX, initY);

            // Cuarto cuadrante
            initX = rnd.Next(anchuraTotal / 2, anchuraTotal - (ladoSalas + 1));
            initY = rnd.Next(alturaTotal / 2, alturaTotal - (ladoSalas + 1));

            SituaSala(ref mapa, initX, initY);
        }
    }
}
