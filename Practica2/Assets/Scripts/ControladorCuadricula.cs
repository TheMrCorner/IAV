namespace UCM.IAV.Navegacion
{
    using UnityEngine;
    using System.Collections;


    /// <summary>
    /// clase que permite el control de la cuadricula
    /// </summary>
    public class ControladorCuadricula : MonoBehaviour
    {
        private static ControladorCuadricula s_Instancia = null;
        public static ControladorCuadricula instancia
        {
            get
            {
                if (s_Instancia == null)
                {
                    s_Instancia = FindObjectOfType(typeof(ControladorCuadricula)) as ControladorCuadricula;

                    if (s_Instancia == null)
                        Debug.Log("No se puede localizar un objeto ControladorCuadricula. \n Necesitas tener exactamente un ControladorCuadricula en la escena.");
                }
                return s_Instancia;
            }
        }

        /// <summary>
        /// numero de filas y columnas que contendra la cuadricula
        /// </summary>
        private int numFilas;
        private int numColumnas;
        public int dimension = 33;
        /// <summary>
        /// tamaño de las celdas de la cuadricula
        /// </summary>
        public float tamCelda;

        /// <summary>
        /// permite mostrar o no como debug la cuadricula y/o los obstaculos
        /// </summary>
        public bool mostrarCuadricula = true;
        public bool mostrarBloquesObstaculos = true;

        /// <summary>
        /// array de nodos de la cuadricula
        /// </summary>
        public Nodo[,] nodos { get; set; }

        /// <summary>
        /// Variable para determinar si el laberinto tendrá salas
        /// </summary>
        public bool salas = false;

        /// <summary>
        /// 
        /// </summary>
        public bool creaLaberinto;

        /// <summary>
        /// Variable para determinar el tamaño de las salas
        /// </summary>
        public int tamSalas;

        /// <summary>
        /// array de los obstaculos de la cuadricula
        /// </summary>
        private GameObject[] listaObstaculos;

        /// <summary>
        /// Instancia del generador de Laberintos. 
        /// </summary>
        private GeneradorLabs genLab;

        /// <summary>
        /// Constructor del laberinto en la escena. Se modifica desde editor.
        /// </summary>
        public ConstructorLaberinto constructorLab;

        private Vector3 origen = new Vector3();
        public Vector3 Origen
        {
            get { return origen; }
        }

        /// <summary>
        /// Función que sirve para iniciar el laberinto. Llama al Generador de laberintos y gestiona 
        /// los diferentes obstáculos que hay en la escena. Prepara la Grid para poder hacer el 
        /// A* de forma correcta.
        /// </summary>
        /// <param name="nuevaDimension"> Dimensión del laberinto. </param>
        /// <param name="genSalas">Marca si se generan salas o no.</param>
        /// <param name="dimSalas">Dimensión de las salas que va a tener.</param>
        public void IniciarLaberinto(int nuevaDimension, bool genSalas, int dimSalas)
        {
            // Establecemos las variables personalizadas
            dimension = nuevaDimension;
            salas = genSalas;
            tamSalas = dimSalas;

            numFilas = dimension;
            numColumnas = dimension;

            if (creaLaberinto)
            {
                genLab = new GeneradorLabs();
                
                // Creamos el laberinto primero 
                int[,] mapa;
                mapa = genLab.GenLab(numFilas, numColumnas, salas, tamSalas);

                if (mapa == null)
                {
                    Debug.LogError("Mapa mal generado, numeros pares introducidos");
                }

                // Montar laberinto
                if ((constructorLab != null) && (mapa != null))
                    constructorLab.ConstruyeLaberinto(mapa);
            }

            listaObstaculos = GameObject.FindGameObjectsWithTag("Obstaculo");
            CalcularObstaculos();
        }

        /// <summary>
        /// encuentra los obstaculos dentro de la cuadricula
        /// </summary>
        void CalcularObstaculos()
        {
            nodos = new Nodo[numColumnas, numFilas];

            // crea los nodos
            int index = 0;
            for (int i = 0; i < numColumnas; i++)
            {
                for (int j = 0; j < numFilas; j++)
                {
                    Vector3 posCelda = CogerCentroCelda(index);
                    Nodo nodo = new Nodo(posCelda);
                    nodos[i, j] = nodo;
                    index++;
                }
            }

            // marca los obstaculos
            if (listaObstaculos != null && listaObstaculos.Length > 0)
            {
                //For each obstacle found on the map, record it in our list
                foreach (GameObject data in listaObstaculos)
                {
                    int indexCelda = CogerIndexCelda(data.transform.position);
                    int col = CogerColumna(indexCelda);
                    int fil = CogerFila(indexCelda);
                    nodos[fil, col].MarcarComoObstaculo();
                }
            }
        }

        /// <summary>
        /// devuelve el centro de la celda
        /// </summary>
        public Nodo CogerCelda(Vector3 pos)
        {
            int col = (int)(pos.x / tamCelda);
            int fil = (int)(pos.z / tamCelda);

            return nodos[fil, col];
        }

        /// <summary>
        /// devuelve el centro de la celda
        /// </summary>
        public Vector3 CogerCentroCelda(int index)
        {
            Vector3 celdaPosition = CogerPosicionCelda(index);
            celdaPosition.x += (tamCelda / 2.0f);
            celdaPosition.z += (tamCelda / 2.0f);

            return celdaPosition;
        }

        /// <summary>
        /// devuelve la posicion de la celda
        /// </summary>
        public Vector3 CogerPosicionCelda(int index)
        {
            int fil = CogerFila(index);
            int col = CogerColumna(index);
            float xPos = col * tamCelda;
            float zPos = fil * tamCelda;

            return Origen + new Vector3(xPos, 0.0f, zPos);
        }

        /// <summary>
        /// devuelva el identificador de la celda
        /// </summary>
        public int CogerIndexCelda(Vector3 pos)
        {
            if (!EstaEnLimite(pos))
                return -1;

            pos -= Origen;
            int col = (int)(pos.x / tamCelda);
            int fil = (int)(pos.z / tamCelda);

            return (fil * numColumnas + col);
        }

        /// <summary>
        /// devuelve si la celda esta dentro de la cuadricula
        /// </summary>
        public bool EstaEnLimite(Vector3 pos)
        {
            float ancho = numColumnas * tamCelda;
            float largo = numFilas * tamCelda;

            return (pos.x >= Origen.x && pos.x <= Origen.x + ancho && pos.x <= Origen.z + largo && pos.z >= Origen.z);
        }

        /// <summary>
        /// devuelve la fila de la celda
        /// </summary>
        public int CogerFila(int index)
        {
            int fil = index / numColumnas;

            return fil;
        }

        /// <summary>
        /// devuelve la columna de la celda
        /// </summary>
        public int CogerColumna(int index)
        {
            int col = index % numColumnas;

            return col;
        }

        /// <summary>
        /// devuelve los vecinos del nodo
        /// </summary>
        public void CogerVecinos(Nodo nodo, ArrayList vecinos)
        {
            Vector3 vecinoPos = nodo.posicion;
            int vecinoIndex = CogerIndexCelda(vecinoPos);
            int fila = CogerFila(vecinoIndex);
            int columna = CogerColumna(vecinoIndex);

            //Bottom
            int filaNodoIzq = fila - 1;
            int columnaNodoIzq = columna;
            AsignarVecino(filaNodoIzq, columnaNodoIzq, vecinos);

            //Top
            filaNodoIzq = fila + 1;
            columnaNodoIzq = columna;
            AsignarVecino(filaNodoIzq, columnaNodoIzq, vecinos);

            //Right
            filaNodoIzq = fila;
            columnaNodoIzq = columna + 1;
            AsignarVecino(filaNodoIzq, columnaNodoIzq, vecinos);

            //Left
            filaNodoIzq = fila;
            columnaNodoIzq = columna - 1;
            AsignarVecino(filaNodoIzq, columnaNodoIzq, vecinos);
        }

        /// <summary>
        /// asigna un vecino al nodo
        /// </summary>
        void AsignarVecino(int fila, int columna, ArrayList vecinos)
        {
            if (fila != -1 && columna != -1 &&
            fila < numFilas && columna < numColumnas)
            {
                Nodo nuevoNodo = nodos[fila, columna];
                if (!nuevoNodo.bObstaculo)
                {
                    vecinos.Add(nuevoNodo);
                }
            }
        }

        void OnDrawGizmos()
        {
            if (mostrarCuadricula)
                DebugDibujarCuadricula(transform.position, numFilas, numColumnas, tamCelda, Color.blue);

            Gizmos.DrawSphere(transform.position, 0.5f);

            if (mostrarBloquesObstaculos)
            {
                Vector3 tamCelda = new Vector3(this.tamCelda, 1.0f, this.tamCelda);
                if (listaObstaculos != null && listaObstaculos.Length > 0)
                {
                    foreach (GameObject data in listaObstaculos)
                    {
                        Gizmos.DrawCube(CogerCentroCelda(
                        CogerIndexCelda(data.transform.position)), tamCelda);
                    }
                }
            }
        }

        /// <summary>
        /// dibuja como debug la cuadricula
        /// </summary>
        public void DebugDibujarCuadricula(Vector3 origen, int numFilas, int numColumnas, float tamCelda, Color color)
        {
            float ancho = (numColumnas * tamCelda);
            float largo = (numFilas * tamCelda);

            // Draw the horizontal grid lines
            for (int i = 0; i < numFilas + 1; i++)
            {
                Vector3 iniPos = origen + i * tamCelda * new Vector3(0.0f, 0.0f, 1.0f);
                Vector3 finPos = iniPos + ancho * new Vector3(1.0f, 0.0f, 0.0f);
                Debug.DrawLine(iniPos, finPos, color);
            }

            // Draw the vertical grid lines
            for (int i = 0; i < numColumnas + 1; i++)
            {
                Vector3 iniPos = origen + i * tamCelda * new Vector3(1.0f, 0.0f, 0.0f);
                Vector3 finPos = iniPos + largo * new Vector3(0.0f, 0.0f, 1.0f);
                Debug.DrawLine(iniPos, finPos, color);
            }
        }
    }
}

