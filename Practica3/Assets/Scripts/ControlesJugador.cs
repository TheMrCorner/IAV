using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script que controla el movimiento del jugador. Está simulado 
/// en tercera persona. Decidimos no usar rigidbody y usar Player
/// Controller porque es bastante más preciso y menos lioso. 
/// </summary>
public class ControlesJugador : MonoBehaviour
{
    /// <summary>
    /// Variable que guarda la instancia del componente 
    /// CharacterController. Esta se usa para mover al jugador.
    /// </summary>
    public CharacterController controler;

    /// <summary>
    /// Variable que gestiona la velocidad a la que se moverá 
    /// el jugador por la escena. 
    /// </summary>
    public float velocidad = 12f;

    /// <summary>
    /// Variable que controla la gravedad que vamos a simular en 
    /// este script. 
    /// </summary>
    public float gravedad = -9.81f;


    /// <summary>
    /// Transform que señala a la parte que comprobará si el jugador
    /// está en el suelo, cayendo o saltando. 
    /// </summary>
    public Transform compruebaSuelo;

    /// <summary>
    /// Esto es el radio de la esfera que se generará para comprobar si
    /// el jugador está o no en el suelo. Esto hará que esté más o menos
    /// cerca del objeto que simula el suelo. 
    /// </summary>
    public float distanciaSuelo = 0.4f;

    /// <summary>
    /// LayerMask que indica si el objeto que hay debajo es suelo o no. 
    /// </summary>
    public LayerMask suelo;


    /// <summary>
    /// Esta variable gestiona la velocidad de caida. 
    /// </summary>
    Vector3 vel;

    /// <summary>
    /// Flag para comprobar si el jugador está en el suelo. 
    /// </summary>
    bool estaSuelo;

    /// <summary>
    /// Flag para saber si el vizconde está en una barca y
    /// cancelar su movimiento.
    /// </summary>
    bool enBarca;

    /// <summary>
    /// Variable para controlar que se baja.
    /// </summary>
    bool haBajado;

    /// <summary>
    /// Tranform de la barca para actualizar la posicion sobre la barca. 
    /// </summary>
    Transform barca;


    private void Start()
    {
        enBarca = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Comprobamos primero si estamos en el suelo
        estaSuelo = Physics.CheckSphere(compruebaSuelo.position, distanciaSuelo, suelo);

        if(estaSuelo && vel.y < 0)
        {
            vel.y = -2f;
        }

        // Gestionamos el Input de las teclas de movimiento
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        
        if (haBajado)
        {
            transform.position = barca.position;
            haBajado = false;
        }
        else if (!enBarca)
        {
            controler.Move(move * velocidad * Time.deltaTime);

            // Simulamos la gravedad
            vel.y += gravedad;

            controler.Move(vel * Time.deltaTime);
        }
        else
        {
            transform.position = barca.position;
        }
    }

    /// <summary>
    /// Función que activa o desactiva el flag de la barca para el movimiento.
    /// </summary>
    /// <param name="value">Valor de si entra o no en la barca.</param>
    public void EnBarca(bool value, Transform barc)
    {
        if (enBarca && !value)
        {
            haBajado = true;
        }

        enBarca = value;

        barca = barc;
    }
}
