using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorCamara : MonoBehaviour
{
    /// <summary>
    /// Esta variable ajusta la sensibilidad del ratón. 
    /// </summary>
    public float sensibilidadRaton = 100f;

    /// <summary>
    /// Objetivo entorno al cual girará la cámara. Estará 
    /// apuntando todo el rato a él. 
    /// </summary>
    public Transform objetivo;

    /// <summary>
    /// Agente que será el objetivo de la cámara, está aquí para
    /// poder girarlo en función de hacia donde mira.
    /// </summary>
    public Transform agente;

    /// <summary>
    /// Flag que gestiona si el agente al que sigue la camara
    /// deber ser girado o no. 
    /// </summary>
    public bool rotarAgente;

    /// <summary>
    /// Valores del movimiento del ratón en sus ejes. 
    /// </summary>
    float ratonX, ratonY;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        ControlCam();
    }

    /// <summary>
    /// Controla la camara a través del ratón
    /// </summary>
    void ControlCam()
    {
        ratonX += Input.GetAxis("Mouse X") * sensibilidadRaton * Time.deltaTime;
        ratonY -= Input.GetAxis("Mouse Y") * sensibilidadRaton * Time.deltaTime;

        ratonY = Mathf.Clamp(ratonY, -35, 60);

        transform.LookAt(objetivo);

        objetivo.rotation = Quaternion.Euler(ratonY, ratonX, 0);

        if (rotarAgente)
        {
            agente.rotation = Quaternion.Euler(0, ratonX, 0);
        }
    }
}
