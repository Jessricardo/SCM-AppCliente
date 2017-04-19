
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace appCliente.Droid
{
	[Activity(Label = "estado")]
	public class estado : Activity
	{
		public TextView estadoPedido, distancia;
		public Button actualizar;
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.pedido);
			// Create your application here
			estadoPedido = FindViewById<EditText>(Resource.Id.txtEstado);
			distancia = FindViewById<EditText>(Resource.Id.txtDistancia);
			actualizar = FindViewById<Button>(Resource.Id.actualizar);
			actualizar.Click += actualizarMetodo;
		}

		public void actualizarMetodo(object sender, EventArgs e)
		{

		}
	}
}
