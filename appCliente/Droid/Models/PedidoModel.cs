using System;
namespace appCliente.Droid
{
	public class PedidoModel
	{
		public int id { get; set; }
		public string hora { get; set; }
		public string fecha { get; set; }
		public long telefono { get; set; }
		public string nombre { get; set; }
		public int pizza { get; set; }
		public double latitud { get; set; }
		public double longitud { get; set; }
		public string estado { get; set; }
		public string nombrerepartidor { get; set; }
		public double latitudRep { get; set; }
		public double longitudRep { get; set; }
	}
}
