using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;

/// <summary>
/// Este script controla los diferentes estados de la cantante. Además, sirve como 
/// método de comunicación entre el árbol de comportamientos y el resto de objetos
/// y comportamientos de la escena y personajes. 
/// </summary>
public class ComportamientoCantante : MonoBehaviour
{
    /// <summary>
    /// GameObject que indica dónde está el escenario, para establecer el objetivo
    /// y que la cantante se pueda mover hacia allí al establecerlo como destino. 
    /// </summary>
    public GameObject destinoEscenario;

    /// <summary>
    /// GameObject que indica dónde están las bambalinas, para establecer el 
    /// objetivo y que pueda ir a bambalinas cuando entre en crisis. 
    /// </summary>
    public GameObject destinoBambalinas;

    public GameObject fantasma;

    /// <summary>
    /// Variable que guarda el árbol de comportamiento de la cantante para manejar algunas
    /// de las variables y quitar peso del árbol en sí. 
    /// </summary>
    public BehaviorTree arbolDeComportamiento;
    /// <summary>
    /// Sonido de enfado del fantasma
    /// </summary>
    public AudioClip enfadoFantasma;
    /// <summary>
    /// Sonido del llorar de la cantante
    /// </summary>
    public AudioClip lloro;
    /// <summary>
    /// Sonido del grito
    /// </summary>
    public AudioClip grito;
    /// <summary>
    /// Sonido del canto
    /// </summary>
    public AudioClip canto;
    /// <summary>
    /// Manejador del sonido
    /// </summary>
    private AudioSource audio;
    /// <summary>
    /// Variable interna que controla si la cantante está o no en el escenario, para de esta
    /// manera evitar que pueda entrar en crisis si no está en el escenario. 
    /// </summary>
    private bool estaEnEscenario;
    /// <summary>
    /// Variable interna que controla si la cantante ha llegado a las bambalinas. Esta sirve
    /// para cuando entra en crisis. 
    /// </summary>
    private bool estaEnBambalinas;
    /// <summary>
    /// Variable interna que controla si la cantante esta atrapada.
    /// </summary>
    private bool estoyAtrapada = false;
    /// <summary>
    /// Variable interna que controla si la cantante esta atrapada.
    /// </summary>
    private bool estoyApresada = false;
    /// <summary>
    /// Bool que marca si hay publico
    /// </summary>
    private bool hayPublico = false;


    /// <summary>
    /// Aquí inicializamos todas las variables necesarias para que la 
    /// cantante funcione correctamente. Algunas de ellas están colocadas
    /// e inicializadas aquí para evitar posibles errores y que sea más fácil 
    /// depurar en el caso de que surja alguno. 
    /// </summary>
    void Start()
    {
        audio = gameObject.AddComponent<AudioSource>();

        this.GetComponent<NavMeshAgent>().areaMask = 15;

        SharedVector3 temp;

        estaEnBambalinas = true;
        estaEnEscenario = false;

        // Por si los valores no coinciden, los establecemos desde aquí también
        CambiaBambalinas();
        CambiaEscenario();

        // Establecemos la posicion del escenario
        temp = destinoEscenario.transform.position;

        arbolDeComportamiento.SetVariable("Escenario", temp);

        // Establecemos la posición de las bambalinas
        temp = destinoBambalinas.transform.position;

        arbolDeComportamiento.SetVariable("Bambalinas", temp);
    }


    /// <summary>
    /// En este Update sólo se gestiona si la cantante entra en crisis o 
    /// no, que depende de si está en el escenario, ya que podría entrar
    /// en crisis en medio del camino al escenario. Por ello, sólo puede
    /// entrar en crisis si está en el escenario. 
    /// </summary>
    void Update()
    {
        bool seguirFantasma = ((SharedBool)arbolDeComportamiento.GetVariable("Atrapada")).Value;
        if (seguirFantasma)
        {
            SharedVector3 temp;
            temp = fantasma.transform.position;
            arbolDeComportamiento.SetVariable("PosicionFantasma", temp);

        }
    }

    /// <summary>
    /// Esta función es la que se encarga de cambiar el valor de la variable "Crisis" del árbol 
    /// de comportamiento. De esta manera limitamos el acceso al árbol a sólo esta función, al 
    /// menos para modificar valores. 
    /// </summary>
    /// <param name="estadoCrisis"></param>
    void CambiaCrisis(bool estadoCrisis)
    {
        if (estadoCrisis)
        {
            audio.Stop();
            audio.clip = lloro;
            audio.loop = true;
            audio.Play();
        }
        else if (audio.clip == lloro)
            audio.Stop();

        SharedBool value = estadoCrisis;
        arbolDeComportamiento.SetVariable("Crisis", value);
    }

    /// <summary>
    /// Cambia el estado de la cantante entre atrapada y no.
    /// </summary>
    /// <param name="estadoAtrapada">Valor al que cambiará el estado</param>
    public void CambiaAtrapada(bool estadoAtrapada)
    {
        if (estadoAtrapada)
        {
            this.GetComponent<CapsuleCollider>().enabled = false;
            this.GetComponent<NavMeshAgent>().enabled = false;
        }
        else
        {
            this.GetComponent<CapsuleCollider>().enabled = true;
            this.GetComponent<NavMeshAgent>().enabled = true;
        }
        estoyAtrapada = estadoAtrapada;
        SharedBool value = estadoAtrapada;
        arbolDeComportamiento.SetVariable("Atrapada", value);
    }

    /// <summary>
    /// Cambia el estado de la cantante entre apresada y no.
    /// </summary>
    /// <param name="estadoApresada">Valor al que cambiará el estado</param>
    public void CambiaApresada(bool estadoApresada)
    {
        estoyApresada = estadoApresada;
        SharedBool value = estadoApresada;
        arbolDeComportamiento.SetVariable("Apresada", value);
    }

    /// <summary>
    /// Gestiona si la cantante está o no en el escenario
    /// </summary>
    void CambiaEscenario()
    {
        SharedBool value = estaEnEscenario;
        arbolDeComportamiento.SetVariable("EstaEscenario", value);
    }

    /// <summary>
    /// Gestiona si la cantante ha llegado a las bambalinas. Esto se 
    /// hace porque si no se queda parada nada más salir del collider
    /// del escenario. 
    /// </summary>
    void CambiaBambalinas()
    {
        SharedBool value = estaEnBambalinas;
        arbolDeComportamiento.SetVariable("EstaBambalinas", value);
    }

    /// <summary>
    /// Función para activar que ha entrado al escenario.
    /// </summary>
    public void EntrarEscenario()
    {
        audio.Stop();
        audio.clip = canto;
        audio.loop = true;
        audio.Play();

        estaEnEscenario = true;
        CambiaEscenario();
    }

    /// <summary>
    /// Función para activar que ha salido del escenario. 
    /// </summary>
    public void SalirEscenario()
    {
        if(audio.clip == canto)
            audio.Stop();

        estaEnEscenario = false;
        CambiaEscenario();
    }

    /// <summary>
    /// Función para activar que ha llegado a las bambalinas
    /// </summary>
    public void EntrarBambalinas()
    {
        estaEnBambalinas = true;
        CambiaBambalinas();
    }

    /// <summary>
    /// Función para activar que ha salido de las bambalinas.
    /// </summary>
    public void SalirBambalinas()
    {
        estaEnBambalinas = false;
        CambiaBambalinas();
    }

    /// <summary>
    /// Esta función es necesaria para que el fantasma pueda 
    /// capturar a la cantante. Cambia el estado de crisis a 
    /// true y el estado de atrapada también. 
    /// </summary>
    public void Capturar()
    {
        CambiaCrisis(true);
        CambiaAtrapada(true);

        audio.Stop();
        audio.clip = grito;
        audio.loop = false;
        audio.Play();
    }

    /// <summary>
    /// Esta función será llamada por el Vizconde cuando libere a la 
    /// cantante o cuando el fantasma la deje en la celda. 
    /// </summary>
    public void Liberar()
    {
        fantasma.GetComponent<ComportamientoFantasma>().CantanteLiberada();
        CambiaApresada(false);
        CambiaAtrapada(false);
    }

    ///////----------------------Modificar y gestionar la crisis------------------------///////

    /// <summary>
    /// Esta funcion sólo es llamada si la cantante entra en crisis. Esta se puede llamar
    /// cuando la cantante entra en crisis por sí sola o cuando las lámparas caen. 
    /// </summary>
    public void EntraEnCrisis()
    {
        CambiaCrisis(true);
    }

    /// <summary>
    /// Esta funcion sólo se llama cuando el vizconde anima a la cantante para que siga
    /// cantando. 
    /// </summary>
    public void EsAnimada()
    {
        //if(hayPublico)
            CambiaCrisis(false);
    }
    /// <summary>
    /// Esta funcion sólo se llama cuando el publico abandona o entra 
    /// cantando. 
    /// </summary>
    public void cambioPublico(bool hay)
    {
        if (!hay)
            EntraEnCrisis();
        hayPublico = hay;
    }

    ///////----------------------------------------------------------------------------///////
    /// <summary>
    /// Si colisiona con el vizconde la anima si no esta atrapada por el fantasma
    /// Si colisiona con el fantasma la atrapa
    /// cantando. 
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Vizconde")
        {
            if(!estoyAtrapada)
            {
                EsAnimada();
                if (estoyApresada)
                {
                    audio.Stop();
                    audio.clip = enfadoFantasma;
                    audio.loop = false;
                    audio.Play();
                    Liberar();
                }
            }

        }

        else if (collision.gameObject.tag == "Fantasma" && !(bool)(collision.gameObject.GetComponent<BehaviorTree>().GetVariable("RuidoSalaMusica").GetValue()) && !estoyAtrapada && !estoyApresada)
        {
            Capturar();
            collision.gameObject.GetComponent<ComportamientoFantasma>().CantanteAtrapada(true);
        }
    }

    /// <summary>
    /// Devuelve si esta atrapada
    /// cantando. 
    /// </summary>
    public bool getEstoyAtrapada()
    {
        return estoyAtrapada;
    }

}
