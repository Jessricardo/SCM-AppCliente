using System;
using System.Collections.Generic;

namespace appCliente.Droid
{
	public class PizzaModel
	{
		public string Nombre { get; set; }
		public int Id { get; set; }
		public List<string> Ingredientes { get; set; } = new List<string>();
		public int Caliicacion { get; set; }
		public int TiempoDePreparacion { get; set; }
		public Decimal Precio { get; set; }
	}
}
