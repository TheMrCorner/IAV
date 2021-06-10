
namespace UCM.IAV.Navegacion
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// clase que define el comportamiento de Perseo
    /// </summary>
    public class ComportamientoPerseo : ComportamientoAgente
    {

        public struct LineDrawer
        {
            private LineRenderer lineRenderer;
            private float lineSize;

            public LineDrawer(float lineSize = 0.2f)
            {
                GameObject lineObj = new GameObject("LineObj");
                lineRenderer = lineObj.AddComponent<LineRenderer>();
                //Particles/Additive
                lineRenderer.material = new Material(Shader.Find("Hidden/Internal-Colored"));

                this.lineSize = lineSize;
            }

            private void init(float lineSize = 0.2f)
            {
                if (lineRenderer == null)
                {
                    GameObject lineObj = new GameObject("LineObj");
                    lineRenderer = lineObj.AddComponent<LineRenderer>();
                    //Particles/Additive
                    lineRenderer.material = new Material(Shader.Find("Hidden/Internal-Colored"));

                    this.lineSize = lineSize;
                }
            }

            //Draws lines through the provided vertices
            public void DrawLineInGameView(Vector3 start, Vector3 end, Color color)
            {
                if (lineRenderer == null)
                {
                    init(0.2f);
                }

                //Set color
                lineRenderer.startColor = color;
                lineRenderer.endColor = color;

                //Set width
                lineRenderer.startWidth = lineSize;
                lineRenderer.endWidth = lineSize;

                //Set line count which is 2
                lineRenderer.positionCount = 2;

                //Set the postion of both two lines
                lineRenderer.SetPosition(0, start);
                lineRenderer.SetPosition(1, end);
            }

            public void Destroy()
            {
                if (lineRenderer != null)
                {
                    UnityEngine.Object.Destroy(lineRenderer.gameObject);
                }
            }
        }

        private Direccion dir = new Direccion();

        private bool movAutomatico = false;

        private bool pPulsada = false;
        private bool espacioPulsado = false;
        private bool oPulsado = false;
        private ArrayList drawers = new ArrayList();


        void Start()
        {
            inicio = this.gameObject;
            fin = GameObject.FindGameObjectWithTag("Salida");
        }

        void Update()
        {
            controlarInput();

            if (!SalidaAlcanzada() && movAutomatico)
                MovimientoAutomatico();
            
            agente.SetDireccion(dir);
        }

        private void actualizarUI()
        {
            GameObject.FindGameObjectWithTag("TextoSuavizado").GetComponent<Text>().text = "Suavizado: " + suavizando;
            GameObject.FindGameObjectWithTag("TextoMovimiento").GetComponent<Text>().text = "Movimiento Automatico: " + movAutomatico;
            GameObject.FindGameObjectWithTag("TextoNodos").GetComponent<Text>().text = "Nodos Explorados: " + aEstrella.nodosExplorados;
            GameObject.FindGameObjectWithTag("TextoTiempo").GetComponent<Text>().text = "Tiempo de Calculo: " + aEstrella.tiempoCalculo;
        }

        /// <summary>
        /// controla la llegada de perseo a la salida
        /// </summary>
        private bool SalidaAlcanzada()
        {
            bool b = ControladorCuadricula.instancia.CogerIndexCelda(this.transform.position) == ControladorCuadricula.instancia.CogerIndexCelda(fin.transform.position);

            if (b && movAutomatico)
            {
                movAutomatico = false;
                camino = null;

                dir.lineal = new Vector3(0, 0, 0);
                destroyLine();
                Debug.Log("SALIDA ALCANZADA");
            }

            return b;
        }

        /// <summary>
        /// controla el movimiento automatico de perseo
        /// </summary>
        private void MovimientoAutomatico()
        {
            if (camino != null)
            {
                dir.lineal = ((Nodo)camino[siguienteNodoIndex]).posicion - this.transform.position;
                dir.lineal.y = 0;
                dir.lineal.Normalize();
                dir.lineal *= agente.aceleracionMax;
                
                if (ControladorCuadricula.instancia.CogerIndexCelda(this.transform.position) == ControladorCuadricula.instancia.CogerIndexCelda(((Nodo)camino[siguienteNodoIndex]).posicion))
                    if (siguienteNodoIndex < camino.Count - 1)
                    {
                        destroyLine();
                        drawLine();
                        siguienteNodoIndex++;
                    }
            }
        }

        /// <summary>
        /// maneja el input y sus consecuencias
        /// </summary>
        private void controlarInput()
        {
            if (!movAutomatico)
            {
                dir.lineal.x = Input.GetAxis("Horizontal");
                dir.lineal.z = Input.GetAxis("Vertical");
                dir.lineal.Normalize();
                dir.lineal *= agente.aceleracionMax;
            }


            // activar o desactivar suavizado
            if (Input.GetKeyDown(KeyCode.P) && !pPulsada)
            {
                pPulsada = true;
                suavizando = !suavizando;
                EncontrarCamino();
                actualizarUI();
            }
            // buscar camino nuevo
            else if (Input.GetKeyDown(KeyCode.Space) && !espacioPulsado)
            {
                espacioPulsado = true;
                movAutomatico = !movAutomatico;
                EncontrarCamino();
                actualizarUI();
            }

            // evitar mantener pulsado
            if (Input.GetKeyUp(KeyCode.P) && pPulsada)
                pPulsada = false;
            else if (Input.GetKeyUp(KeyCode.Space) && espacioPulsado)
                espacioPulsado = false;

        }



        /// <summary>
        /// dibuja el camino de perseo en blanco
        /// </summary>
        void drawLine()
        {
            if (camino == null)
                return;
            if (camino.Count > 0)
            {
                int index = 1;
                for (int i = siguienteNodoIndex; i < camino.Count - 1; i++)
                {
                    Nodo siguiente = (Nodo)camino[i + 1];
                    LineDrawer drawer = new LineDrawer();
                    drawer.DrawLineInGameView(((Nodo)camino[i]).posicion, siguiente.posicion, Color.white);
                    drawers.Add(drawer);
                    index++;
                    
                }
            }
        }

        void destroyLine()
        {
            int size = drawers.Count;

            for(int i = 0; i < size; i++)
            {
                ((LineDrawer)drawers[i]).Destroy();
            }
            drawers.Clear();
        }

    }
}
