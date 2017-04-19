﻿using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Android.Content;
using Newtonsoft.Json;
using System.Net.Http;
using System.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace appCliente.Droid
{
	[Activity(Label = "appCliente", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		//int count = 1;
		public Button pedir;
		public EditText telefono;
		public 	int pizza;
		public string telefono2;
		public ListView pizzas;
		protected override async void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);
			// Get our button from the layout resource,
			// and attach an event to it
			//Button button = FindViewById<Button>(Resource.Id.myButton);
			//button.Click += delegate { button.Text = $"{count++} clicks!"; };
			pedir = FindViewById<Button>(Resource.Id.pedir);
			pizzas = FindViewById<ListView>(Resource.Id.pizzas);
			telefono = FindViewById<EditText>(Resource.Id.edtTelefono);
			telefono2 = telefono.Text;
			List<string> nombres = new List<string>();
			pedir.Click += pedirMetodo;
			List<PizzaModel> json = await LeerApi();
			for (int i = 0; i < json.Count; i++) {
				PizzaModel a = json.ElementAt(i);
				string nombreCompleto = a.Id + " - " + a.Nombre;
				nombres.Add(nombreCompleto);
			}
			ArrayAdapter ListAdapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, nombres);
			pizzas.SetAdapter(ListAdapter);
			//pizzas.Text = json["title"];
			//par2.Text = json["body"];

		}
		public void pedirMetodo(object sender, EventArgs e)
		{
			Intent intento = new Intent(this, typeof(estado));
			Bundle contenedor = new Bundle();
			contenedor.PutInt("id",pizza);
			contenedor.PutString("telefono",telefono2);
			intento.PutExtra("bundle",contenedor);
			StartActivity(intento);
		}
		public async Task<List<PizzaModel>> LeerApi()
		{
			string baseurl = "http://192.168.0.107/api/allpizzas";
			var Client = new HttpClient();
			Client.MaxResponseContentBufferSize = 256000;
			var uril = new Uri(baseurl);
			var response = await Client.GetAsync(uril);
			if (response.IsSuccessStatusCode)
			{
				var content = await response.Content.ReadAsStringAsync();
				var items = JsonConvert.DeserializeObject<List<PizzaModel>>(content);
				return items;
			}
			else
			{
				return null;
			}
		}
	}

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
