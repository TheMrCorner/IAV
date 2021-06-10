using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UCM.IAV.Navegacion
{
    public class GestorEscena : MonoBehaviour
    {
        /// <summary>
        /// Flag para hacer las cosas en la escena de suavizado.
        /// </summary>
        public bool suavizado = false;

        /// <summary>
        /// Variable para modificar desde la UI si se quieren salas o no.
        /// </summary>
        public bool salas = false;

        /// <summary>
        /// Variable para modificar desde la UI el tamaño que se va a dar al laberinto.
        /// </summary>
        public int dimension = 33;

        /// <summary>
        /// Tamaño que van a tener las salas del laberinto.
        /// </summary>
        public int dimensionSala = 6;

        /// <summary>
        /// Instancia del controlador de la cuadrícula para settear los datos pertinentes. 
        /// </summary>
        public ControladorCuadricula cc;

        /// <summary>
        /// Variable para controlar que no se cambie de escena al mantener la tecla pulsada.
        /// </summary>
        private bool cambioEscena = true;

        /// <summary>
        /// Guardamos lo que muestra los controles para activarlo y desactivarlo.
        /// </summary>
        public GameObject controles;

        /// <summary>
        /// Guardamos lo que muestra los datos para activarlo y desactivarlo.
        /// </summary>
        public GameObject datos;

        /// <summary>
        /// Guardamos el panel de personalización para activarlo y desactivarlo.
        /// </summary>
        public GameObject panelInicio;

        /// <summary>
        /// Guardamos el botón de reseteo para reiniciar la escena.
        /// </summary>
        public GameObject botonReinicio;

        /// <summary>
        /// Slider de la dimension del laberinto, para poder obtener su valor.
        /// </summary>
        public Slider sliderDimension;

        /// <summary>
        /// Slider del tamaño de las salas, para modificar algunos de sus valores
        /// en ejecución.
        /// </summary>
        public Slider sliderSalas;

        /// <summary>
        /// Texto que usaremos para mostrar el valor del slider de los laberintos.
        /// </summary>
        public Text textoLab;

        /// <summary>
        /// Texto que usaremos para mostrar el valor del slider de las salas.
        /// </summary>
        public Text textoSalas;

        // Awake is called only once
        private void Awake()
        {
            if (!suavizado)
            {
                controles.SetActive(false);
                datos.SetActive(false);
                botonReinicio.SetActive(false);
                panelInicio.SetActive(true);

                // Establecemos el valor por defecto del laberinto
                sliderDimension.value = dimension;
                sliderDimension.minValue = 9; // Establecemos un valor mínimo
                sliderDimension.maxValue = 101;
                textoLab.text = "" + sliderDimension.value;

                // Establecemos el valor por defecto de las salas
                sliderSalas.maxValue = dimension / 4;
                sliderSalas.minValue = 2; // Ponemos un tamaño mínimo de 2
                textoSalas.text = "" + sliderSalas.value;
            }
            else
            {
                cc.IniciarLaberinto(dimension, false, 0);
            }
        }

        // Update is called once per frame
        void Update()
        {
            GestionInput();
        }

        /// <summary>
        /// Función que desactiva y activa las cosas necesarias de la interfaz para
        /// generar la escena. Llama a la función que inicia todo el proceso de 
        /// construcción del laberinto.
        /// </summary>
        public void IniciarEscena()
        {
            panelInicio.SetActive(false);
            controles.SetActive(true);
            datos.SetActive(true);
            botonReinicio.SetActive(true);

            cc.IniciarLaberinto(dimension, salas, dimensionSala);
        }

        /// <summary>
        /// FUnción para recargar la escena y regenerar el laberinto.
        /// </summary>
        public void RecargarEscena()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        ///////////////// Edición de los laberintos y generación de escena //////////////////

        /// <summary>
        /// Función que establece las dimensiones del laberinto. Lee el valor del
        /// slider que hay en la interfaz y lo pasa a las variables.
        /// </summary>
        public void EstablecerDimension()
        {
            // Si el numero fuera par, lo hacemos impar
            if ((sliderDimension.value % 2) == 0)
            {
                sliderDimension.value += 1;
            }

            string value = "" + sliderDimension.value;
            dimension = System.Int32.Parse(value);

            textoLab.text = "" + sliderDimension.value;

            // Modificamos el tamaño máximo de las salas para que no de errores
            sliderSalas.maxValue = dimension / 4;

            textoSalas.text = "" + sliderSalas.value;
        }
        
        /// <summary>
        /// FUnción que establece si se generan salas.
        /// </summary>
        /// <param name="g">True si se generan</param>
        public void GenerarSalas(bool g)
        {
            salas = g;
        }

        /// <summary>
        /// Función que establece las dimensiones de las salas a meter en el laberinto. 
        /// Saca el valor del slider y se lo pasa al constructor tras hacer una transformación.
        /// </summary>
        public void DimensionSala()
        {
            string value = "" + sliderSalas.value;
            dimensionSala = System.Int32.Parse(value);

            textoSalas.text = "" + sliderSalas.value;
        }

        ///////////////// Gestion del cambio de escena //////////////////////////////////////

        /// <summary>
        /// Función que se usa para gestionar el Input y realizar los cambios necesarios.
        /// </summary>
        void GestionInput()
        {
            // cambio de escena
            if (Input.GetKeyDown(KeyCode.O) && cambioEscena)
            {
                cambioEscena = false;
                CambioEscena();
            }

            // Desbloqueamos el cambio de escena
            if (Input.GetKeyUp(KeyCode.O) && !cambioEscena)
            {
                cambioEscena = true;
            }

            // Salimos de la aplicacion
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SalirAplicacion();
            }
        }

        /// <summary>
        /// Función para salir de la aplicación. Está separada por si se quiere usar un botón.
        /// </summary>
        public void SalirAplicacion()
        {
            Application.Quit();
        }

        /// <summary>
        /// Función para cambiar de escena.
        /// </summary>
        public void CambioEscena()
        {
            if (SceneManager.GetActiveScene().name == "Suavizado")
                SceneManager.LoadScene("GraphGrid");
            else
                SceneManager.LoadScene("Suavizado");
        }
    }
}
