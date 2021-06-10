using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Este script gestionará las cámaras que harán de camara principal y 
/// mostrarán las diferentes posiciones.
/// </summary>
public class GestorCamaras : MonoBehaviour
{
    /// <summary>
    /// Array con todas las cámaras disponibles en la escena.
    /// </summary>
    public Camera[] camaras;

    /// <summary>
    /// Indice que guarda la cámara que está en funcionamiento. 
    /// </summary>
    private int indiceCamaraActual;

    // Start is called before the first frame update
    void Start()
    {
        // Establecemos la primera como principal
        indiceCamaraActual = 0;

        // Activamos la principal
        camaras[indiceCamaraActual].gameObject.SetActive(true);

        // Desactivamos las demás 
        for (int i = 1; i < camaras.Length; i++)
        {
            camaras[i].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        GestionInput();
    }

    /// <summary>
    /// Función que gestiona el input para hacer el cambio
    /// entre cámaras.
    /// </summary>
    void GestionInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            indiceCamaraActual= 0;
            // Cambiamos la cámara que está activa
            CambioDeCamara();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            indiceCamaraActual = 1;
            // Cambiamos la cámara que está activa
            CambioDeCamara();

        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            indiceCamaraActual = 2;
            // Cambiamos la cámara que está activa
            CambioDeCamara();
        }
    }

    /// <summary>
    /// Función que gestiona el cambio de cámaras, activando y
    /// desactivando según sea conveniente. 
    /// </summary>
    void CambioDeCamara()
    {
        // Si estamos en la principal, desactivamos la última
        if(indiceCamaraActual == 0)
        {
            camaras[camaras.Length - 1].gameObject.SetActive(false);
        }
        // Si no, desactivamos la anterior
        else
        {
            camaras[indiceCamaraActual - 1].gameObject.SetActive(false);
        }

        // Activamos la actual
        camaras[indiceCamaraActual].gameObject.SetActive(true);
    }
}
