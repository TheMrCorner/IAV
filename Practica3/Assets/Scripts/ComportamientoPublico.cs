using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PublicoEstado
{
    HUYENDO,
    VOLVIENDO,
    FUERA,
    DENTRO
}
public class ComportamientoPublico : MonoBehaviour
{
    /// <summary>
    /// El componente de cuerpo rígido
    /// </summary>
    private Rigidbody cuerpoRigido;
    /// <summary>
    /// estado actual de la publico
    /// </summary>
    private PublicoEstado estado = PublicoEstado.DENTRO;
    /// <summary>
    /// direccion en la que se mueven
    /// </summary>
    private Vector3 velocidad = new Vector3(0, 0, 0);
    /// <summary>
    /// array de lamparas
    /// </summary>
    private GameObject[] lamparas;
    /// <summary>
    /// nodo al que huir cuando esten huyendo fuera y cuando vuelvan a la sala
    /// </summary>
    public GameObject dentro, fuera;
    /// <summary>
    /// booleanos para controlar la colision con los triger de dentro y de fuera
    /// </summary>
    private bool colisionoDentro = true, colisionoFuera = false;

    /// <summary>
    /// Se establecen los objetos lampara, el rigidbody y los colliders de dentro y de fuera
    /// </summary>
    private void Start()
    {
        lamparas = GameObject.FindGameObjectsWithTag("Lampara");
        cuerpoRigido = GetComponent<Rigidbody>();
        dentro = GameObject.FindGameObjectWithTag("Dentro");
        fuera = GameObject.FindGameObjectWithTag("Fuera");
    }

    /// <summary>
    /// Se actualiza cada tick el estado del publico segun el estado de las lamparas
    /// </summary>
    void Update()
    {
        comprobarLamparas();

        if(estado == PublicoEstado.DENTRO && !colisionoDentro)
        {
            velocidad = dentro.GetComponent<Transform>().position - transform.position;
            velocidad.Normalize();

        }
        else if(estado == PublicoEstado.FUERA && !colisionoFuera)
        {
            velocidad = fuera.GetComponent<Transform>().position - transform.position;
            velocidad.Normalize();
        }
        else
        {
            velocidad = new Vector3(0, 0, 0);
            cuerpoRigido.velocity = new Vector3(0, 0, 0);
        }


        cuerpoRigido.AddRelativeForce(velocidad * Time.deltaTime, ForceMode.VelocityChange);
    }

    /// <summary>
    /// Cambia los booleanos para controlar la colision con los triger de dentro y de fuera al salir de la colision
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Dentro")
        {
            colisionoDentro = false;
        }
        else if (other.gameObject.tag == "Fuera")
        {
            colisionoFuera = false;
        }
    }

    /// <summary>
    /// Cambia los booleanos para controlar la colision con los triger de dentro y de fuera al entrar en la colision
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Dentro")
        {
            colisionoDentro = true;
        }
        else if (other.gameObject.tag == "Fuera")
        {
            colisionoFuera = true;
        }
    }

    /// <summary>
    /// Comprobar si hay lamparas caidas o no. Cambia el estado del público según el estado de las lamparas.
    /// Además avisa al fantasma de si hay público dentro o no
    /// </summary>
    private void comprobarLamparas()
    {
        int lamparasColgadas = 0;
        int numLamparas = lamparas.Length;
        foreach(GameObject lampara in lamparas)
        {
            if ((estado == PublicoEstado.DENTRO || estado == PublicoEstado.VOLVIENDO) && lampara.GetComponentInChildren<ComportamientoLampara>().verEstado() == LamparaEstado.CAIDA)
            {
                cambiarEstado(PublicoEstado.FUERA);
                GameObject.FindGameObjectWithTag("Fantasma").GetComponent<ComportamientoFantasma>().PublicoPresente(false);
                GameObject.FindGameObjectWithTag("Cantante").GetComponent<ComportamientoCantante>().cambioPublico(false);
                break;
            }
            else if ((estado == PublicoEstado.FUERA || estado == PublicoEstado.HUYENDO) && lampara.GetComponentInChildren<ComportamientoLampara>().verEstado() == LamparaEstado.COLGADA)
                lamparasColgadas++;
        }

        if (lamparasColgadas == numLamparas)
        {
            cambiarEstado(PublicoEstado.DENTRO);
            GameObject.FindGameObjectWithTag("Fantasma").GetComponent<ComportamientoFantasma>().PublicoPresente(true);
            GameObject.FindGameObjectWithTag("Cantante").GetComponent<ComportamientoCantante>().cambioPublico(true);
        }
    }

    /// <summary>
    /// metodo de ayuda para comprobar si estamos en la posicion de los nodos
    /// </summary>
    private bool estoyEnSuPosicion(Vector3 pos)
    {
        return (transform.position.x <= pos.x + 0.1 && transform.position.x >= pos.x - 0.1) && (transform.position.z <= pos.z + 0.1 && transform.position.z >= pos.z - 0.1);
    }

    /// <summary>
    /// devuelve el estado del publico
    /// </summary>
    public PublicoEstado verEstado() 
    { 
        return estado; 
    }

    /// <summary>
    /// cambia el estado del publico
    /// </summary>
    public void cambiarEstado(PublicoEstado est)
    {
        estado = est;
    }
}
