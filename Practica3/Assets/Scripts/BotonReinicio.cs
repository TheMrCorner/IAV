using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Maneja el input de reinicio
/// </summary>
public class BotonReinicio : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
            SceneManager.LoadScene("Decisiones");
    }
}
