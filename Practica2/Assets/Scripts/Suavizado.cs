
namespace UCM.IAV.Navegacion
{
    using System.Collections;
    using UnityEngine;

    public class Suavizado : MonoBehaviour
    {
        public ArrayList SuavizarCamino(ArrayList camino)
        {
            // si el camino que nos pasan no tiene ningun paso, te vas
            if (camino.Count == 0)
                return null;

            // si el camino que nos pasan tiene 2 o menos vertices, no se puede suavizar mas
            if (camino.Count < 3)
                return camino;

            // creas el nuevo camino suavizado y añades el primer nodo, que nunca puede sobrar
            ArrayList nuevoCamino = new ArrayList();
            nuevoCamino.Add(camino[0]);

            // se recorre el camino y se crea el nuevo
            int tam = camino.Count;
            for (int i = 0; i < tam - 1;)
            {
                Vector3 origen = ((Nodo) camino[i]).posicion;

                int j;
                for (j = i + 1; j < tam; j++)
                {
                    Vector3 destino = ((Nodo)camino[j]).posicion;

                    Vector3 direccion = destino - origen;
                    float distancia = direccion.magnitude;
                    direccion.Normalize();

                    // haces un raycast para comprobar si el vertice siguiente se encuentra en linea recta o esta bloqueado por un muro
                    Ray ray = new Ray(origen, direccion);
                    RaycastHit[] golpes;
                    golpes = Physics.RaycastAll(ray, distancia);

                    // si esta bloqueado por un muro, paras
                    bool muro = false;
                    foreach (RaycastHit golpe in golpes)
                        if (golpe.collider.gameObject.tag == "Obstaculo" || golpe.collider.gameObject.tag == "Minotauro")
                        {
                            muro = true;
                            break;
                        }

                    // si has salido porque habia un muro, paras el bucle
                    if (muro)
                        break;
                }

                // coges el ultimo nodo "limpio" y lo guardas
                i = j - 1;
                nuevoCamino.Add(camino[i]);
            }

            return nuevoCamino;
        }
    }
}
