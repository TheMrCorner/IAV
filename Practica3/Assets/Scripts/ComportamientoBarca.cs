using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Script que controla el movimiento de la barca. 
/// </summary>
public class ComportamientoBarca : MonoBehaviour
{
    /// <summary>
    /// Puntos de posicion a los que tiene que ir la barca.
    /// </summary>
    public Transform puntoA, puntoB;

    /// <summary>
    /// Velocidad de la barca. 
    /// </summary>
    public float velocidad;

    /// <summary>
    /// Marca si la barca se está moviendo o no.
    /// </summary>
    private bool moviendose = false;

    /// <summary>
    /// El componente de cuerpo rígido
    /// </summary>
    private Rigidbody cuerpoRigido;

    private void Start()
    {
        cuerpoRigido = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (moviendose)
        {
            if (HaLlegado())
            {
                PararBarca();
                
            }
            else
                MovimientoBarca();
        }
    }

    /// <summary>
    /// Devuelve true si la barca ha llegado al destino puntoB, false si no.
    /// </summary>
    public bool HaLlegado() 
    {
        return (transform.position.x <= puntoB.position.x + 0.5 && transform.position.x >= puntoB.position.x - 0.5) && (transform.position.z <= puntoB.position.z + 0.5 && transform.position.z >= puntoB.position.z - 0.5);
    }
    /// <summary>
    /// Realiza el movimiento de la barca.
    /// </summary>
    public void MovimientoBarca()
    {
        Vector3 dir = puntoB.position - puntoA.position;
        dir.Normalize();
        dir *= velocidad;
        cuerpoRigido.AddForce(dir * Time.deltaTime, ForceMode.VelocityChange);
    }
    /// <summary>
    /// Esta función sirve para decirle a la barca que se ponga en movimiento. 
    /// </summary>
    public void MoverBarca()
    {
        moviendose = true;
        MovimientoBarca();
    }
    /// <summary>
    /// Con esta función se puede parar la barca cuando llegue a su destino y cambiar sus puntos de destino.
    /// </summary>
    public void PararBarca()
    {
        moviendose = false;

        Transform temp = puntoA;
        puntoA = puntoB;
        puntoB = temp;

        cuerpoRigido.velocity = new Vector3(0, 0, 0);
    }
}