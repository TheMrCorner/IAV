using UnityEngine;

/// <summary>
/// Este script controla las colisiones con el botón de producir ruido de la sala de música.
/// </summary>
public class ComportamientoMusica : MonoBehaviour
{
    /// <summary>
    /// Gameobject del fantasma
    /// </summary>
    public GameObject fantasma;
    /// <summary>
    /// Sonido del ruido
    /// </summary>
    public AudioClip ruido;
    /// <summary>
    /// Manejador del sonido
    /// </summary>
    private AudioSource audio;

    private void Start()
    {
        audio = gameObject.AddComponent<AudioSource>();
        audio.clip = ruido;
        audio.loop = true;
    }

    /// <summary>
    /// Si el vizconde interactua con el boton, informa al fantasma de que hay musica sonando en la sala de musica.
    /// Si el fantasma colisiona con el boton se para la musica.
    /// </summary>
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E) && other.gameObject.tag == "Vizconde")
        {
            fantasma.GetComponent<ComportamientoFantasma>().SuenaRuido(true);
            audio.Play();
        }

        else if (other.gameObject.tag == "Fantasma" && audio.isPlaying)
        {
            fantasma.GetComponent<ComportamientoFantasma>().SuenaRuido(false);
            audio.Stop();
        }
        
    }
}
