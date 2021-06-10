using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCM.IAV.Movimiento
{
	// Hijo de Comportamiento Agente que sobreescribe el Update
	public class ComportamientoPerro : ComportamientoAgente
	{
		/// <summary>
		/// Metodo que permite controlar el comportamiento propio del perro.
		/// En cada tick, establecer la dirección que corresponde al perro, con peso o prioridad si se están usando
		/// </summary>
		public override void Update()
		{
			detectarInputFlauta();

			Direccion dir;

			if (flautaSonando) // Si la flauta esta sonando
				dir = GetComponent<Evadir>().evadir(objetivo, agente);
			else // Si la flauta no esta sonando
				dir = GetComponent<PersecucionLlegada>().persecucionLlegada(objetivo, agente);

			if (agente.mezclarPorPeso)
				agente.SetDireccion(dir, peso);

			else if (agente.mezclarPorPrioridad)
				agente.SetDireccion(dir, prioridad);

			else
				agente.SetDireccion(dir);
		}
	}
}
