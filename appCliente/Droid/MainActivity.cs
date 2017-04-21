using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Android.Content;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
namespace appCliente.Droid
{
	[Activity(Label = "appCliente", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		//int count = 1;
		public Button pedir;
		public EditText telefono,edtPizzaId2;
		public int pizza;
		public long telefono2;
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
			edtPizzaId2 = FindViewById<EditText>(Resource.Id.edtPizzaId);
			telefono = FindViewById<EditText>(Resource.Id.edtTelefono);
			List<string> nombres = new List<string>();
			pedir.Click += pedirMetodo;
			List<PizzaModel> json = await LeerApi();
			for (int i = 0; i < json.Count; i++)
			{
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
			pizza = int.Parse(edtPizzaId2.Text);
			telefono2 = long.Parse(telefono.Text);
			contenedor.PutInt("id", pizza);
			contenedor.PutLong("telefono", telefono2);
			intento.PutExtra("bundle", contenedor);
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
}

