using System.Collections.Generic;
using System.Linq;
using System.IO;

/// <summary>
/// Generador de laberintos aleatorios. 
/// 
/// Este script es una implementación del algoritmo de Prim en C# para generar un 
/// laberinto aleatorio pero interesante. El algoritmo de Prim crea un árbol que
/// cubre por completo un grafo, por lo que siempre hay un camino que une todo el 
/// laberinto. 
/// 
/// Empezamos con una matriz de muros, habitaciones y pilares (las habitaciones 
/// serían los nodos del grafo por así decirlo). Cada uno de los muros empieza 
/// cerrado, para hacer todo el proceso más rápido. Una de las habitaciones se 
/// escoge como comienzo del camino. 
/// 
/// Después el algoritmo funciona de la siguiente manera: 
/// - Intenta seguir el camino por cada uno de los muros que rodean la habitación. 
/// - Comprueba que se pueda continuar el camino por cada uno de los muros.
/// - Comprueba si la siguiente habitación está ya en el camino y si no, la añade al camino.
/// - Repite con cada habitación del laberinto y cada muro. 
/// 
/// Esto se puede extender a cualquier generación de laberintos.
/// </summary>
namespace UCM.IAV.Navegacion
{

    public class GeneradorLabs
    {
        /// <summary>
        /// Struct que guarda las coordenadas de las diferentes celdas.
        /// </summary>
        struct Coord
        {
            /**
             * Internal value of the coordinates. Accesible from outside to
             * make things easier. 
             * Valor interno de las coordenadas. Accesible desde fuera para 
             * hacer las cosas un poco más sencillas.
             */
            public int y;
            public int x;

            /**
             * Constructor of the struct. Receives the position. 
             * Constructora del Sctruct. Recibe una posición
             * @param newY Type: int 
             * @param newX Type: int
             */
            public Coord(int newY, int newX)
            {
                x = newX;
                y = newY;
            }
        }

        //-------------- VARIABLES -----------------
        // Matriz con la información del laberinto.
        int[,] mapa;

        // Valores del laberinto TODO: revisar estos nombres
        int alturaTotal;
        int anchuraTotal;

        //  Leyenda para entender cómo se codifica el laberinto
        //  0 = Habitación inicio
        //  1 = Habitación
        //  2 = Pilar
        //  3 = Muro Cerrado
        //  4 = Muro Abierto
        //  5 = Muro Salida
        //  6 = Minotauro e inicio
        //  7 = Final Minotauro

        // Todas estas variables se usan para generar el laberinto, cálculos
        int numeroDeHab;
        Coord inicio;
        List<Coord> camino = new List<Coord>(); // Todas las habitaciones del laberinto en un camino
        List<Coord> listaMuros = new List<Coord>();

        // Generador de las habitaciones en el laberinto
        GeneradorSalas genSalas; 

        /// <summary>
        /// Función que convierte un número par en impar para que caiga en una habitación.
        /// </summary>
        /// <param name="num">Número a convertir</param>
        /// <returns>Número impar</returns>
        void HacerImpar(ref int num, int limite)
        {
            int temp = num;

            if ((temp + 1) == (limite - 1))
            {
                num--;
            }
            else
            {
                num++;
            }
        }

        /// <summary>
        /// Función que crea un laberinto en blanco, todos los muros están cerrados y tenemos una malla
        /// de habitaciones y muros. 
        /// </summary>
        /// <returns>Devuelve la matriz con los datos generados</returns>
        int[,] CreaLaberintoEnBlanco()
        {
            // Variables
            int[,] tempMapa = new int[alturaTotal, anchuraTotal];

            // Función
            for (int i = 0; i < alturaTotal; i++)
            {
                for (int j = 0; j < anchuraTotal; j++)
                {
                    // Es necesario diferenciar lineas de habitaciones y lineas de pilares.
                    // Van intercaladas.
                    // Tal como se generaría un laberinto, las lineas pares tienen pilares 
                    // y viceversa. 

                    if ((i % 2) == 0) // Fila con pilares
                    {
                        // Los muros y los pilares también se alternan
                        if ((j % 2) == 0) // Posición par, pilar
                        {
                            tempMapa[i, j] = 2;
                        } // if
                        else // Posición impar, muro cerrado
                        {
                            tempMapa[i, j] = 3;
                        } // else
                    } // if
                    else // Linea con habitaciones
                    {
                        // Alternamos entre muros y habitaciones
                        if ((j % 2) == 0) // Posición par, muro cerrado
                        {
                            tempMapa[i, j] = 3;
                        } // if
                        else // Posición impar habitación
                        {
                            tempMapa[i, j] = 1;
                        } // else
                    } // else
                } // for
            } // for

            return tempMapa;
        } // CreaLaberintoEnBlanco()
        
        /// <summary>
        /// Función que comprueba si una habitación está en el camino del laberinto.
        /// </summary>
        /// <param name="habitacion"></param>
        /// <returns>Devuelve "True" si una habitación está ya en el camino</returns>
        bool BuscaHabitacion(Coord habitacion)
        {   
            // Variables
            bool estaEnCamino = false;
            int i = 0; // Used to iterate 

            // Función
            // Busca en el array con las habitaciones
            while (i < camino.Count() && !estaEnCamino)
            {
                if ((camino[i].x == habitacion.x) && (camino[i].y == habitacion.y))
                {
                    estaEnCamino = true;
                } // if
                i++;
            } // while

            return estaEnCamino;
        } // BuscaHabitacion

        /// <summary>
        /// Comprueba si los muros alrededor de la habitación están abiertos. Si
        /// alguno de los muros está abierto, significa que la habitación está 
        /// conectada con el camino.
        /// </summary>
        /// <param name="habitacion">Coordenadas de la habitacion</param>
        /// <returns>Devuelve "True" cuando la habitación está conectada</returns>
        bool HabitacionConectada(Coord habitacion)
        {
            // Comprueba los muros que rodean la habitación uno por uno
            if (mapa[habitacion.y - 1, habitacion.x] == 4)
            {
                return true;
            } // if

            else if (mapa[habitacion.y + 1, habitacion.x] == 4)
            {
                return true;
            } // else if

            else if (mapa[habitacion.y, habitacion.x - 1] == 4)
            {
                return true;
            } // else if

            else if (mapa[habitacion.y, habitacion.x + 1] == 4)
            {
                return true;
            } // else if

            else
            {
                return false;
            } // else
        } // HabitacionConectada

        /// <summary>
        /// Función que añade los muros de una habitación a la lista de muros para revisar.
        /// 
        /// Disclaimer: El nombre se queda en inglés porque meter ñ's es demasiado y no sé si el compilador se puede rayar
        /// </summary>
        /// <param name="habitacion">Habitación de la que se van a añadir los muros</param>
        void AñadirMuroALaLista(Coord habitacion)
        {
            // Comprobamos si un muro está abierto, si es así, no lo añadimos a la lista
            if (mapa[habitacion.y - 1, habitacion.x] == 3 || mapa[habitacion.y - 1, habitacion.x] == 4)
            {
                if (!listaMuros.Contains(new Coord(habitacion.y - 1, habitacion.x)) && (mapa[habitacion.y - 1, habitacion.x] == 3))
                {
                    listaMuros.Add(new Coord(habitacion.y - 1, habitacion.x));
                } // if 
            } // if 

            if (mapa[habitacion.y + 1, habitacion.x] == 3 || mapa[habitacion.y + 1, habitacion.x] == 4)
            {
                if (!listaMuros.Contains(new Coord(habitacion.y + 1, habitacion.x)) && (mapa[habitacion.y + 1, habitacion.x] == 3))
                {
                    listaMuros.Add(new Coord(habitacion.y + 1, habitacion.x));
                } // if 
            } // if 

            if (mapa[habitacion.y, habitacion.x - 1] == 3 || mapa[habitacion.y, habitacion.x - 1] == 4)
            {
                if (!listaMuros.Contains(new Coord(habitacion.y, habitacion.x - 1)) && (mapa[habitacion.y, habitacion.x - 1] == 3))
                {
                    listaMuros.Add(new Coord(habitacion.y, habitacion.x - 1));
                } // if
            } // if

            if (mapa[habitacion.y, habitacion.x + 1] == 3 || mapa[habitacion.y, habitacion.x + 1] == 4)
            {
                if (!listaMuros.Contains(new Coord(habitacion.y, habitacion.x + 1)) && (mapa[habitacion.y, habitacion.x + 1] == 3))
                {
                    listaMuros.Add(new Coord(habitacion.y, habitacion.x + 1));
                } // if
            } // if
        } // AddWallsToList
        
        /// <summary>
        /// Selecciona una habitacion aleatoria de todas las posibles en el laberinto. 
        /// </summary>
        /// <returns>Variable tipo Coord, coordenadas de una habitacion del laberinto</returns>
        Coord HabitacionAleatoria() //This only works for Rooms
        {
            // Variables
            System.Random numeroAleatorio = new System.Random();
            Coord primeraHab = new Coord(1, 1); // La primera habitación siempre está en la posicion (1, 1)
            int y, x;
            bool terminado = false;

            // Función
            while (!terminado)
            {
                // Genera coordenadas random entre 1 y los máximos respectivos
                y = numeroAleatorio.Next(1, alturaTotal - 1);
                x = numeroAleatorio.Next(1, anchuraTotal - 1);

                if (y >= 1 && x >= 1) // Las habitaciones siempre son números impares y no están fuera del laberinto
                {
                    if ((y % 2) == 0) // Cambiamos de par a impar
                    {
                        // De aquí se puede sacar una función
                        HacerImpar(ref y, alturaTotal);
                    } // if

                    primeraHab.y = y;

                    if ((x % 2) == 0) // Las mismas condiciones para la coordenada X
                    {
                        HacerImpar(ref x, anchuraTotal);
                    } // if 

                    primeraHab.x = x;

                    terminado = true; 
                } // if 
            } // while

            return primeraHab;
        } // HabitacionAleatoria

        /// <summary>
        /// Funcion que busca una habitación adyacente a un muro
        /// </summary>
        /// <param name="muro">Muro del que vamos a buscar adyacencias</param>
        /// <returns>Devuelve "True" si ha encontrado una habitación adyacente</returns>
        bool BuscaHabitacionAdyacenteMuro(Coord muro)
        {
            // Variables
            int numeroDeHab = 0;
            bool addedRoom = false; // De nuevo, queda más claro added y nos evitamos posibles errores

            // Funcion
            if (((muro.y - 1) > 0) && mapa[muro.y - 1, muro.x] == 1)
            {
                if (!BuscaHabitacion(new Coord(muro.y - 1, muro.x)))
                {
                    camino.Add(new Coord(muro.y - 1, muro.x));
                    AñadirMuroALaLista(new Coord(muro.y - 1, muro.x));
                    addedRoom = true;
                } // if
                numeroDeHab++;
            } // if 

            if (((muro.y + 1) < alturaTotal) && mapa[muro.y + 1, muro.x] == 1)
            {
                if (!BuscaHabitacion(new Coord(muro.y + 1, muro.x)))
                {
                    camino.Add(new Coord(muro.y + 1, muro.x));
                    AñadirMuroALaLista(new Coord(muro.y + 1, muro.x));
                    addedRoom = true;
                } // if
                numeroDeHab++;
            } // if 

            if (((muro.x - 1) > 0) && mapa[muro.y, muro.x - 1] == 1)
            {
                if (!BuscaHabitacion(new Coord(muro.y, muro.x - 1)))
                {
                    camino.Add(new Coord(muro.y, muro.x - 1));
                    AñadirMuroALaLista(new Coord(muro.y, muro.x - 1));
                    addedRoom = true;
                } // if
                numeroDeHab++;
            } // if 

            if (((muro.x + 1) < anchuraTotal) && mapa[muro.y, muro.x + 1] == 1)
            {
                if (!BuscaHabitacion(new Coord(muro.y, muro.x + 1)))
                {
                    camino.Add(new Coord(muro.y, muro.x + 1));
                    AñadirMuroALaLista(new Coord(muro.y, muro.x + 1));
                    addedRoom = true;
                } // if
                numeroDeHab++;
            } // if 

            return (addedRoom && numeroDeHab == 2);
        } // BuscaHabitacionAdyacenteMuro

        /// <summary>
        /// Función que genera el laberinto implementando el algoritmo de Prim
        /// </summary>
        void CreaLaberinto()
        {
            // Variables
            // Primera habitación del camino
            Coord primeraHab = HabitacionAleatoria();
            camino.Add(primeraHab); // Empezamos el camino
            int i = 0;

            // Función
            AñadirMuroALaLista(primeraHab); // Añadimos los muros de la primera habitación

            // Procesamos todos los muros de la matriz 
            while (listaMuros.Count() > 0) 
            {
                // Buscamos habitaciones adyacentes
                if (BuscaHabitacionAdyacenteMuro(listaMuros[i]))
                {
                    mapa[listaMuros[i].y, listaMuros[i].x] = 4;
                    listaMuros.Remove(listaMuros[i]);
                } // if
                // Si no hay, descartamos este muro
                else
                {
                    listaMuros.Remove(listaMuros[i]);
                } // else

                if (i < listaMuros.Count - 1)
                {
                    i++;
                } // if
                else
                {
                    i = 0;
                } // else
            } // while 
        } // CreaLaberinto

        /// <summary>
        /// Método para establecer el punto de inicio en el laberinto.
        /// </summary>
        void EstablecerInicio()
        {
            // Seleccionamos una habitacion aleatoria
            inicio = HabitacionAleatoria();

            // La establecemos como inicio
            mapa[inicio.y, inicio.x] = 0;
        }

        /// <summary>
        /// Este método concreto establece al minotauro en el laberinto,
        /// evitando que se sitúe en la misma casilla que el inicio. 
        /// </summary>
        void EstablecerMinotauro()
        {
            // Generamos una habitación aleatoria 
            Coord aleatoria;
            bool colocado = false;

            while (!colocado)
            {
                aleatoria = HabitacionAleatoria();

                // Comprobamos que no esté en el inicio
                if ((aleatoria.y != inicio.x) && (aleatoria.y != inicio.y))
                {
                    // Establecemos la casilla del minotauro
                    mapa[aleatoria.y, aleatoria.x] = 6;
                    System.Random rnd = new System.Random();
                    int coordY;

                    // Establecemos la casilla final del recorrido del minotauro
                    // Mitad izda del laberinto
                    if (aleatoria.x < anchuraTotal / 2)
                    {
                        // Parte superior del laberinto 
                        if (aleatoria.y < alturaTotal / 2)
                        {
                            coordY = rnd.Next(alturaTotal / 2, alturaTotal);
                        }
                        else
                        {
                            coordY = rnd.Next(1, alturaTotal / 2);
                        }

                        if ((coordY % 2) == 0) // Cambiamos de par a impar
                        {
                            HacerImpar(ref coordY, alturaTotal);
                        } // if

                        mapa[coordY, anchuraTotal - 2] = 7;

                        colocado = true;
                    } // if
                     
                    // Mitad dcha del laberinto
                    else
                    {
                        if (aleatoria.y < alturaTotal / 2)
                        {
                            coordY = rnd.Next(alturaTotal / 2, alturaTotal);
                        }
                        else
                        {
                            coordY = rnd.Next(2, alturaTotal / 2);
                        }

                        if ((coordY % 2) == 0) // Cambiamos de par a impar
                        {
                            HacerImpar(ref coordY, alturaTotal);
                        } // if

                        mapa[coordY, 1] = 7;
                    } // else

                    colocado = true;
                } // if
            } // while
        } // EstablecerMinotauro

        /// <summary>
        /// Método para establecer la salida del laberinto. Estableceremos la salida
        /// de manera que esté lejos del inicio. 
        /// 
        /// Para ello dividimos el laberinto en cuadrantes y en función de en qué 
        /// cuadrante esté la casilla inicial, estableceremos la salida en uno de los 
        /// otros tres. 
        /// </summary>
        void EstablecerSalida()
        {
            System.Random rnd = new System.Random();
            int coordY = 0;

            // Mitad izda del laberinto
            if(inicio.x < anchuraTotal / 2)
            {
                // Parte superior del laberinto 
                if(inicio.y < alturaTotal / 2)
                {
                    coordY = rnd.Next(alturaTotal / 2, alturaTotal);
                }
                else
                {
                    coordY = rnd.Next(1, alturaTotal / 2);
                }

                if ((coordY % 2) == 0) // Cambiamos de par a impar
                {
                    HacerImpar(ref coordY, alturaTotal);
                } // if

                mapa[coordY, anchuraTotal - 1] = 5;
            } // if
            // Mitad dcha del laberinto
            else 
            {
                if (inicio.y < alturaTotal / 2)
                {
                    coordY = rnd.Next(alturaTotal / 2, alturaTotal);
                }
                else
                {
                    coordY = rnd.Next(1, alturaTotal / 2);
                }

                if ((coordY % 2) == 0) // Cambiamos de par a impar
                {
                    HacerImpar(ref coordY, alturaTotal);
                } // if

                mapa[coordY, 0] = 5;
            } // else
        } // EstablecerSalida

        //----------------- Salvado del laberinto -------------------

        /// <summary>
        /// Función que sirve para guardar el laberinto en un archivo .txt Está pensado para añadirse a otro juego
        /// </summary>
        /// <param name="contador">Contador de laberintos generados</param>
        /// <param name="juego">Contador para la partida en la que se generó</param>
        void SalvarLaberinto(int contador, int juego) 
        {
            //Atributes
            StreamWriter mapaWriter;
            string nombreDirectorio = "juego " + juego;
            string nombreArchivo = "Labyrinth" + contador + ".txt";
            string DirectorioPadre = @"Assets/Labyrinths";
            string pathSaver = System.IO.Path.Combine(DirectorioPadre, nombreDirectorio);

            //Function
            if (!Directory.Exists(pathSaver))
            {
                System.IO.Directory.CreateDirectory(pathSaver);
            } // if 

            pathSaver = System.IO.Path.Combine(pathSaver, nombreArchivo);

            File.WriteAllText(pathSaver, "Number of Labyrinth: " + contador);

            mapaWriter = File.AppendText(pathSaver);

            if (mapa != null)
            {
                mapaWriter.WriteLine(alturaTotal);
                mapaWriter.WriteLine(anchuraTotal);

                for (int i = 0; i < alturaTotal; i++)
                {
                    for (int j = 0; j < anchuraTotal; j++)
                    {
                        mapaWriter.Write(mapa[i, j]);
                    } // for 
                    mapaWriter.WriteLine();
                } // for
            } // if

            mapaWriter.Close();
        } // SalvarLaberinto

        //----------------- Generador de Laberinto -------------------

        /// <summary>
        /// Función que genera el laberinto aleatorio dependiendo de unos parámetros concretos que se
        /// definen en la llamada. Primero crea el laberinto y luego devuelve una matriz(?) habría que
        /// ver cómo funciona el A*.
        /// </summary>
        /// <param name="altura">Altura total del laberinto</param>
        /// <param name="anchura">Anchura total del laberinto</param>
        /// <returns>Devuelve una matriz con el laberinto generado</returns>
        public int[,] GenLab(int altura, int anchura, bool salas, int ladoSalas)
        {
            // Comprobamos que los datos del laberinto son impares 
            if(altura%2 == 0 || anchura%2 == 0)
            {
                return null;
            }

            // Asignamos los valores al laberinto
            alturaTotal = altura;
            anchuraTotal = anchura;

            // Generamos el laberinto, primero una matriz vacía con los datos de anchura y altura
            // y después rellenamos en blanco y generamos el laberinto.
            mapa = new int[alturaTotal, anchuraTotal];
            mapa = CreaLaberintoEnBlanco();
            CreaLaberinto();

            // Ahora colocamos las salas, antes de colocar todo lo demás para que no se solapen
            // como es opcional el generar salas, nos ahorramos crear la instancia de la clase si
            // no se van a colocar.
            if (salas)
            {
                genSalas = new GeneradorSalas();

                // Si se quieren salas pero no se ha dado un valor, sacamos un número aleatorio entre
                // el mínimo y el máximo de las salas.
                if(ladoSalas == 0)
                {
                    System.Random rnd = new System.Random();

                    ladoSalas = rnd.Next(2, alturaTotal / 6);
                }

                // Creamos las salas
                genSalas.CreaSalas(ref mapa, ladoSalas);              
            }

            // Establecemos las cosas necesarias
            EstablecerInicio();
            EstablecerSalida();
            EstablecerMinotauro();

            // Si todo funciona bien, devuelve true
            return mapa;
        } //GenLab
    } // clase GeneradorLabs
}
