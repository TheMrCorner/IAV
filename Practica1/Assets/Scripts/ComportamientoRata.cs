using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCM.IAV.Movimiento
{
	using System;
	using UnityEngine;

	// Hijo de Comportamiento Agente que sobreescribe el Update
	public class ComportamientoRata : ComportamientoAgente
	{
		/// <summary>
		/// Radio que detecta cuando huir o evadir
		/// </summary>
		public float radius = 1;

		/// <summary>
		/// Metodo que permite controlar el comportamiento propio de la rata.
		/// En cada tick, establecer la dirección que corresponde a la rata, con peso o prioridad si se están usando
		/// </summary>
		public override void Update()
		{
			detectarInputFlauta();

			ArrayList dir = new ArrayList(); // array de direcciones

			if (flautaSonando) // Si la flauta está sonado
			{
				Direccion seguir = GetComponent<PersecucionLlegada>().persecucionLlegada(objetivo, agente);
				seguir.peso = 0.15f;
				dir.Add(seguir);

				Direccion separar = GetComponent<Separacion>().separar(otrosAgentes);
				separar.peso = 0.85f;
				dir.Add(separar);
			}
			else // Si la flauta no está sonando
			{
				Vector3 objetivoPos = objetivo.GetComponent<Transform>().position;
				float dist = Math.Abs((objetivoPos - transform.position).magnitude);

				if (dist < radius)
					dir.Add(GetComponent<Huir>().huir(objetivo.transform.position, agente));
				else
					//dir.Add(new Direccion());
			//	else
					dir.Add(GetComponent<Merodeo>().merodeo(agente));
			}

			int numDir = dir.Count;
			if (numDir > 1) // para limitar que solo de use si hay varias direcciones compitiendo por peso/prioridad
			{
				if (agente.mezclarPorPeso)
					for (int i = 0; i < numDir; i++)
							agente.SetDireccion((Direccion) dir[i], ((Direccion)dir[i]).peso);

				else if (agente.mezclarPorPrioridad)
					for (int i = 0; i < numDir; i++)
						agente.SetDireccion((Direccion)dir[i], ((Direccion)dir[i]).prioridad);
			}
			else
				agente.SetDireccion((Direccion)dir[0]);
		}
	}
}
