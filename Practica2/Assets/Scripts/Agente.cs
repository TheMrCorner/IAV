
namespace UCM.IAV.Navegacion
{

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine; 

/// <summary>
/// La clase Agente es responsable de modelar los agentes y gestionar todos los comportamientos asociados para combinarlos (si es posible) 
/// </summary>
    public class Agente : MonoBehaviour 
    {
        /// <summary>
        /// Velocidad m�xima
        /// </summary>
        [Tooltip("Velocidad m�xima.")]
        public float velocidadMax;
        /// <summary>
        /// Aceleraci�n m�xima
        /// </summary>
        [Tooltip("Aceleraci�n m�xima.")]
        public float aceleracionMax;
        /// <summary>
        /// Rotaci�n m�xima
        /// </summary>
        [Tooltip("Rotaci�n m�xima.")]
        public float rotacionMax;
        /// <summary>
        /// Aceleraci�n angular m�xima
        /// </summary>
        [Tooltip("Aceleraci�n angular m�xima.")]
        public float aceleracionAngularMax;
        /// <summary>
        /// Orientacion (es como la velocidad angular)
        /// </summary>
        [Tooltip("Orientaci�n.")]
        public float orientacion;
        /// <summary>
        /// Rotatci�n (valor que puede variar, como la velocidad, para cambiar la orientaci�n)
        /// </summary>
        [Tooltip("Rotaci�n.")]
        public float rotacion;
        /// <summary>
        /// Velocidad
        /// </summary>
        [Tooltip("Velocidad.")]
        public Vector3 velocidad;
        /// <summary>
        /// Valor de direcci�n / direccionamiento
        /// </summary>
        [Tooltip("Direcci�n.")]
        protected Direccion direccion;
        /// <summary>
        /// Grupos de direcciones, agrupados por valor de prioridad
        /// </summary>
        [Tooltip("Grupos de direcciones.")]
        private Dictionary<int, List<Direccion>> grupos;
        /// <summary>
        /// Componente de cuerpo r�gido
        /// </summary>
        [Tooltip("Cuerpo r�gido.")]
        private Rigidbody cuerpoRigido;

        /// <summary>
        /// Al comienzo, se inicialian algunas variables
        /// </summary>
        void Start()
        {
            velocidad = Vector3.zero;
            direccion = new Direccion();
            grupos = new Dictionary<int, List<Direccion>>();
            cuerpoRigido = GetComponent<Rigidbody>(); // Cojo el cuerpo r�gido
        }

        /// <summary>
        /// En cada tick fijo, si hay cuerpo r�gido, uso el simulador f�sico aplicando fuerzas o no
        /// </summary>
        public virtual void FixedUpdate()
        {
            if (cuerpoRigido == null)
                return;

            Vector3 displacement = velocidad * Time.deltaTime;
            orientacion += rotacion * Time.deltaTime;
            // Necesitamos "constre�ir" inteligentemente la orientaci�n al rango (0, 360)
            if (orientacion < 0.0f)
                orientacion += 360.0f;
            else if (orientacion > 360.0f)
                orientacion -= 360.0f;
            // El ForceMode depender� de lo que quieras conseguir
            // Estamos usando VelocityChange s�lo con prop�sitos ilustrativos
            cuerpoRigido.AddForce(displacement, ForceMode.VelocityChange);
            Vector3 orientationVector = OriToVec(orientacion);
            cuerpoRigido.rotation = Quaternion.LookRotation(orientationVector, Vector3.up);
        }

        /// <summary>
        /// En cada tick, hace lo b�sico del movimiento del agente
        /// </summary>
        public virtual void Update()
        {
            if (cuerpoRigido != null)
                return;
            // ... c�digo previo

            Vector3 desplazamiento = velocidad * Time.deltaTime;
            orientacion += rotacion * Time.deltaTime;
            // Necesitamos "constre�ir" inteligentemente la orientaci�n al rango (0, 360)
            if (orientacion < 0.0f)
                orientacion += 360.0f;
            else if (orientacion > 360.0f)
                orientacion -= 360.0f;
            transform.Translate(desplazamiento, Space.World);
            // Restaura la rotaci�n al punto inicial antes de rotar el objeto nuestro valor
            transform.rotation = new Quaternion();
            transform.Rotate(Vector3.up, orientacion);
        }

        /// <summary>
        /// En cada parte tard�a del tick, hace tareas de correcci�n num�rica (ajustar a los m�ximos, la combinaci�n etc.
        /// </summary>
        public virtual void LateUpdate()
        {
            velocidad += direccion.lineal * Time.deltaTime;
            rotacion += direccion.angular * Time.deltaTime;

            if (velocidad.magnitude > velocidadMax)
            {
                velocidad.Normalize();
                velocidad = velocidad * velocidadMax;
            }

            if (rotacion > rotacionMax)
            {
                rotacion = rotacionMax;
            }

            if (direccion.angular == 0.0f)
            {
                rotacion = 0.0f;
            }

            if (direccion.lineal.sqrMagnitude == 0.0f)
            {
                velocidad = Vector3.zero;
            }

            // En realidad si se quiere cambiar la orientaci�n lo suyo es hacerlo con un comportamiento, no as�:
            transform.LookAt(transform.position + velocidad);

            // Se limpia el steering de cara al pr�ximo tick
            direccion = new Direccion();
        }

        /// <summary>
        /// Establece la direcci�n tal cual
        /// </summary>
        public void SetDireccion(Direccion direccion)
        {
            this.direccion = direccion;
        }

        /// <summary>
        /// Devuelve la actual direccion del agente
        /// </summary>
        public Direccion GetDireccion()
        {
            return this.direccion;
        }

        /// <summary>
        /// Calculates el Vector3 dado un cierto valor de orientaci�n
        /// </summary>
        /// <param name="orientacion"></param>
        /// <returns></returns>
        public Vector3 OriToVec(float prientacion)
        {
            Vector3 vector = Vector3.zero;
            vector.x = Mathf.Sin(prientacion * Mathf.Deg2Rad) * 1.0f;
            vector.z = Mathf.Cos(prientacion * Mathf.Deg2Rad) * 1.0f;
            return vector.normalized;
        }
    }
}
