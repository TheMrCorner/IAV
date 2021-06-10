using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;

/// <summary>
/// Este script controla los diferentes estados del fantasma. Además, sirve como 
/// método de comunicación entre el árbol de comportamientos y el resto de objetos
/// y comportamientos de la escena y personajes. 
/// </summary>
public class ComportamientoFantasma : MonoBehaviour
{
    public NavMeshAgent fantasma;

    /// <summary>
    /// GameObject que indica dónde está la sala musica, para establecer el objetivo
    /// y que el fantasma se pueda mover hacia allí al establecerlo como destino. 
    /// </summary>
    public GameObject destinoMusica;

    /// <summary>
    /// GameObject que indica dónde está la sala musica, para establecer el objetivo
    /// y que el fantasma se pueda mover hacia allí al establecerlo como destino. 
    /// </summary>
    public GameObject destinoMusica2;

    /// <summary>
    /// GameObject que indica dónde está la celda, para establecer el objetivo
    /// y que el fantasma se pueda mover hacia allí al establecerlo como destino. 
    /// </summary>
    public GameObject destinoCelda;

    /// <summary>
    /// GameObject que indica dónde están la palanca 1, para establecer el 
    /// objetivo y que pueda ir a bambalinas cuando entre en crisis. 
    /// </summary>
    public GameObject destinoPalanca1;

    /// <summary>
    /// GameObject que indica dónde están la palanca 2, para establecer el 
    /// objetivo y que pueda ir a bambalinas cuando entre en crisis. 
    /// </summary>
    public GameObject destinoPalanca2;

    /// <summary>
    /// GameObject que indica dónde está el escenario, para establecer el objetivo
    /// y que el fantasma se pueda mover hacia allí al establecerlo como destino. 
    /// </summary>
    public GameObject destinoEscenario;

    /// <summary>
    /// GameObject que indica dónde están las bambalinas, para establecer el 
    /// objetivo y que pueda ir a bambalinas cuando entre en crisis. 
    /// </summary>
    public GameObject destinoBambalinas;

    /// <summary>
    /// GameObject de la cantante
    /// </summary>
    public GameObject cantante;

    /// <summary>
    /// Variable que guarda el árbol de comportamiento del fantasma para manejar algunas
    /// de las variables. 
    /// </summary>
    public BehaviorTree arbolDeComportamiento;

    /// <summary>
    /// Booleano que marca si hemos atrapado a la cantante
    /// </summary>
    private bool cantanteAtrapada = false;

    /// <summary>
    /// Booleano que marca si hemos encarcelado a la cantante
    /// </summary>
    private bool cantanteApresada = false;

    /// <summary>
    /// Tiempo de espera del fantasma para volver a comprobar que ve a la cantante
    /// </summary>
    private float tiempoEsperaBuscar = 10;

    /// <summary>
    /// Flag para saber si hay público.
    /// </summary>
    private bool hayPublico = true;


    /// <summary>
    /// Aquí inicializamos todas las variables necesarias para que el 
    /// fantasma funcione correctamente. Algunas de ellas están colocadas
    /// e inicializadas aquí para evitar posibles errores y que sea más fácil 
    /// depurar en el caso de que surja alguno. 
    /// </summary>
    void Start()
    {
        SharedVector3 temp;

        temp = destinoMusica.transform.position;

        arbolDeComportamiento.SetVariable("DestinoMusica", temp);

        temp = destinoCelda.transform.position;

        arbolDeComportamiento.SetVariable("DestinoCelda", temp);

        temp = destinoMusica2.transform.position;

        arbolDeComportamiento.SetVariable("DestinoMusica2", temp);

        temp = destinoPalanca1.transform.position;

        arbolDeComportamiento.SetVariable("DestinoPalanca1", temp);

        temp = destinoPalanca2.transform.position;

        arbolDeComportamiento.SetVariable("DestinoPalanca2", temp);

        temp = destinoEscenario.transform.position;

        arbolDeComportamiento.SetVariable("DestinoEscenario", temp);

        temp = destinoBambalinas.transform.position;

        arbolDeComportamiento.SetVariable("DestinoBambalinas", temp);

    }

    /// <summary>
    /// Se gestionan los distintos timers del fantasma y algunas comprobaciones que no se pueden hacer desde el arbol
    /// </summary>
    void Update()
    {
        //Establece si se el fantasma puede pasar o no por el escenario cambiando su areaMask
        if(hayPublico != ((SharedBool)arbolDeComportamiento.GetVariable("PublicoPresente")).Value)
        {
            hayPublico = ((SharedBool)arbolDeComportamiento.GetVariable("PublicoPresente")).Value;
            PasarEscenario();
        }

        bool buscandoCantante = ((SharedBool)arbolDeComportamiento.GetVariable("YendoACantante")).Value;
        if (!buscandoCantante) 
        {

            //Si no encuentra camino hasta las bambalinas significa que por como estan las barcas no puede llegar asi que se avisa al arbol para que deje de intentar 
            //ir hacia las bambalinas
            if (((SharedBool)arbolDeComportamiento.GetVariable("ComprobarBambalinas")).Value)
            {
                NavMeshPath path = new NavMeshPath();
                this.GetComponent<NavMeshAgent>().CalculatePath(destinoBambalinas.transform.position, path);
                if (path.corners.Length > 0)
                {

                    Vector3 compare = new Vector3(path.corners[path.corners.Length - 1].x, path.corners[path.corners.Length - 1].y, path.corners[path.corners.Length - 1].z);

                    if (!((compare.x >= destinoBambalinas.transform.position.x - 0.2f && compare.x <= destinoBambalinas.transform.position.x + 0.2f)
                        && (compare.y >= destinoBambalinas.transform.position.y - 0.2f && compare.y <= destinoBambalinas.transform.position.y + 0.2f)
                        && (compare.z >= destinoBambalinas.transform.position.z - 0.2f && compare.z <= destinoBambalinas.transform.position.z + 0.2f)))
                        arbolDeComportamiento.SetVariable("ComprobarBambalinas", (SharedBool)false);
                }
            }
            //Si no encuentra camino hasta la Palanca1 significa lo mismo que antes, asi que se avisa al arbol para que vaya a la palanca2
            else if (((SharedBool)arbolDeComportamiento.GetVariable("PublicoPresente")).Value)
            {
                Vector3 currentDest = ((SharedVector3)arbolDeComportamiento.GetVariable("DestinoPalanca1")).Value;

                if(currentDest == destinoPalanca1.transform.position)
                {
                    NavMeshPath path = new NavMeshPath();
                    this.GetComponent<NavMeshAgent>().CalculatePath(destinoPalanca1.transform.position, path);

                    if (path.corners.Length > 0)
                    {
                        Vector3 compare = new Vector3(path.corners[path.corners.Length - 1].x, path.corners[path.corners.Length - 1].y, path.corners[path.corners.Length - 1].z);

                        if (!((compare.x >= destinoPalanca1.transform.position.x - 0.2f && compare.x <= destinoPalanca1.transform.position.x + 0.2f)
                            && (compare.y >= destinoPalanca1.transform.position.y - 0.2f && compare.y <= destinoPalanca1.transform.position.y + 0.2f)
                            && (compare.z >= destinoPalanca1.transform.position.z - 0.2f && compare.z <= destinoPalanca1.transform.position.z + 0.2f)))
                            arbolDeComportamiento.SetVariable("DestinoPalanca1", (SharedVector3)destinoPalanca2.transform.position);
                    }
                }
                //Si no encuentra camino hasta la Palanca2 significa lo mismo que antes, asi que se avisa al arbol para que vaya a la palanca1
                else
                {
                    NavMeshPath path = new NavMeshPath();
                    this.GetComponent<NavMeshAgent>().CalculatePath(destinoPalanca2.transform.position, path);

                    Vector3 compare = new Vector3(path.corners[path.corners.Length - 1].x, path.corners[path.corners.Length - 1].y, path.corners[path.corners.Length - 1].z);
                    if (path.corners.Length > 0)
                    {
                        if (!((compare.x >= destinoPalanca2.transform.position.x - 0.2f && compare.x <= destinoPalanca2.transform.position.x + 0.2f)
                        && (compare.y >= destinoPalanca2.transform.position.y - 0.2f && compare.y <= destinoPalanca2.transform.position.y + 0.2f)
                        && (compare.z >= destinoPalanca2.transform.position.z - 0.2f && compare.z <= destinoPalanca2.transform.position.z + 0.2f)))
                            arbolDeComportamiento.SetVariable("DestinoPalanca1", (SharedVector3)destinoPalanca1.transform.position);
                    }
                }

            }
        }
        //Timer por si el fantasma pierde de vista a la cantante mientras la persigue para que deje de perseguirla despues de un tiempo y no instantaneamente
        bool veoCantante = ((SharedBool)arbolDeComportamiento.GetVariable("VeoCantante")).Value;
        if (veoCantante)
        {
            if (tiempoEsperaBuscar <= 0)
            {
                SharedBool temp = false;
                arbolDeComportamiento.SetVariable("YendoACantante", temp);
                tiempoEsperaBuscar = 10;
            }
            else
            {
                SharedVector3 temp;
                temp = cantante.transform.position;
                arbolDeComportamiento.SetVariable("PosicionCantante", temp);

                tiempoEsperaBuscar -= Time.deltaTime;
            }
        }
    }
    /// <summary>
    /// Marca si el fantasma puede pasar por el escenario o no
    /// </summary>
    void PasarEscenario()
    {
        // Esto va por bits, así que hay que hacer malabares 
        if (hayPublico && (fantasma.areaMask == 15))
        {
            fantasma.areaMask = 7;
        }
        else if (!hayPublico && (fantasma.areaMask == 7))
        {
            fantasma.areaMask = 15;
        }
    }
    /// <summary>
    /// Marca si suena ruido en la sala de musica o no
    /// </summary>
    /// <param name="ruido">Valor al que cambiará el estado de ruido en la sala</param>
    public void SuenaRuido(bool ruido)
    {
        if (ruido)
            cantante.GetComponent<ComportamientoCantante>().Liberar();

        SharedBool temp = ruido;
        arbolDeComportamiento.SetVariable("RuidoSalaMusica", temp);
    }
    /// <summary>
    /// Marca si suena la cantante ha sido atrapada
    /// </summary>
    /// <param name="atrapada">Valor que marca si la cantante esta atrapada</param>
    public void CantanteAtrapada(bool atrapada)
    {
        cantanteAtrapada = atrapada;
        SharedBool temp = atrapada;
        arbolDeComportamiento.SetVariable("CantanteAtrapada", temp);
    }
    /// <summary>
    /// devuelve si la cantante esta atrapada
    /// </summary>
    public bool GetCantanteAtrapada()
    {
        return cantanteAtrapada;
    }
    /// <summary>
    /// Cambia booleanos si la cantante esta apresada
    /// </summary>
    public void ApresarCantante()
    {
        cantanteApresada = true;
        cantanteAtrapada = false;

        SharedBool temp = false;
        arbolDeComportamiento.SetVariable("CantanteAtrapada", temp);

        temp = true;
        arbolDeComportamiento.SetVariable("CantanteApresada", temp);

        cantante.GetComponent<ComportamientoCantante>().CambiaAtrapada(false);
        cantante.GetComponent<ComportamientoCantante>().CambiaApresada(true);
    }

    /// <summary>
    /// Cambia booleanos si la cantante es liberada
    /// </summary>
    public void CantanteLiberada()
    {
        cantanteAtrapada = false;
        cantanteApresada = false;
        SharedBool temp = false;
        arbolDeComportamiento.SetVariable("CantanteAtrapada", temp);
        arbolDeComportamiento.SetVariable("CantanteApresada", temp);
    }
    /// <summary>
    /// Devuelve si la cantante es apresada
    /// </summary>
    public bool GetCantanteApresada()
    {
        return cantanteApresada;
    }
    /// <summary>
    /// Cambia booleanos si entra en la sala de musica
    /// </summary>
    /// <param name="enSala">Valor que marca si está el fantasma en la sala de musica o no</param>
    public void EnSalaDeMusica(bool enSala)
    {
        SharedBool temp = enSala;
        arbolDeComportamiento.SetVariable("EnSalaDeMusica", temp);
    }

    /// <summary>
    /// Marca si la cantante esta subida al escenario
    /// </summary>
    /// <param name="enEscenario">Valor que marca si está la cantante o no</param>
    public void CantanteEnEscenario(bool enEscenario)
    {
        SharedBool temp = enEscenario;
        arbolDeComportamiento.SetVariable("CantanteEnEscenario", temp);
    }

    /// <summary>
    /// Marca si el público esta mirando o no
    /// </summary>
    /// <param name="publicoPresente">Valor que marca si el público esta presente</param>
    public void PublicoPresente(bool publicoPresente)
    {
        SharedBool temp;
        if (!publicoPresente)
        {
            temp = true;
            arbolDeComportamiento.SetVariable("ComprobarBambalinas", temp);
        }
        temp = publicoPresente;
        arbolDeComportamiento.SetVariable("PublicoPresente", temp);
    }

    /// <summary>
    /// Marca si llegas al escenario 
    /// </summary>
    /// <param name="enEscenario">Valor que marca si el público esta presente</param>
    public void EnEscenario(bool enEscenario)
    {
        SharedBool temp = enEscenario;
        arbolDeComportamiento.SetVariable("ComprobarEscenario", temp);
    }

    /// <summary>
    /// Marca si llegas al escenario 
    /// </summary>
    /// <param name="enBambalinas">Valor que marca si el público esta presente</param>
    public void EnBambalinas(bool enBambalinas)
    {
        SharedBool temp = enBambalinas;
        arbolDeComportamiento.SetVariable("ComprobarBambalinas", temp);
    }
}