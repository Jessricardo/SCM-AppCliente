
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Newtonsoft.Json;
using Plugin.Geolocator;

namespace appCliente.Droid
{
	[Activity(Label = "estado")]
	public class estado : Activity
	{
		public TextView estadoPedido, distancia,txtEstado,txtDistancia;
		public Button actualizar;
		public int id;
		protected async override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.pedido);
			// Create your application here
			txtDistancia = FindViewById<TextView>(Resource.Id.txtDistancia);
			txtEstado = FindViewById<TextView>(Resource.Id.txtEstado);
			estadoPedido = FindViewById<TextView>(Resource.Id.txtvwEstado);
			distancia = FindViewById<TextView>(Resource.Id.txtvwDistancia);
			actualizar = FindViewById<Button>(Resource.Id.actualizar);
			actualizar.Click += actualizarMetodo;
			estadoPedido.Visibility = Android.Views.ViewStates.Gone;
			txtEstado.Visibility = Android.Views.ViewStates.Gone;
			txtDistancia.Visibility = Android.Views.ViewStates.Gone;
			distancia.Visibility = Android.Views.ViewStates.Gone;
			actualizar.Visibility = Android.Views.ViewStates.Gone;

			Bundle paquete = Intent.GetBundleExtra("bundle");
			PedidoModel item = new PedidoModel();
			item.pizza = paquete.GetInt("id");
			item.telefono = paquete.GetLong("telefono");
			item.id = Int32.Parse(item.telefono.ToString().Substring(item.telefono.ToString().Length - 2)+(new Random()).Next(0,9)+ (new Random()).Next(0, 9));
			id = item.id;
			item.nombrerepartidor = "unknown";
			item.nombre = "unknown";
			item.estado = "Pedido";
			item.fecha = "unknown";
			item.hora = "unknown";
			ProgressDialog progress = new ProgressDialog(this);
			progress.Indeterminate = true;
			progress.SetProgressStyle(ProgressDialogStyle.Spinner);
			progress.SetMessage("Realizando pedido...");
			progress.SetCancelable(false);
			progress.Show();
			item.latitud = await latitude();
			item.longitud = await longitud();
			if (Postear(item))
			{
				estadoPedido.Text = "Pedido";
				distancia.Text = "No disponible";
				progress.Hide();
				estadoPedido.Visibility = Android.Views.ViewStates.Visible;
				distancia.Visibility = Android.Views.ViewStates.Visible;
				actualizar.Visibility = Android.Views.ViewStates.Visible;
				txtEstado.Visibility = Android.Views.ViewStates.Visible;
				txtDistancia.Visibility = Android.Views.ViewStates.Visible;
				Toast.MakeText(ApplicationContext, "Pedido #"+item.id.ToString(), ToastLength.Long).Show();
			}
			else
			{
				progress.Hide();
				Toast.MakeText(ApplicationContext, "¡Error al pedir la pizza!", ToastLength.Long).Show();
			}

		}

		public async Task<Double> latitude()
		{
			var locator = CrossGeolocator.Current;
			locator.DesiredAccuracy = 50;
			var position = await locator.GetPositionAsync(10000);
			return position.Latitude;
		}
		public async Task<Double> longitud()
		{
			var locator = CrossGeolocator.Current;
			locator.DesiredAccuracy = 50;
			var position = await locator.GetPositionAsync(10000);
			return position.Longitude;
		}

		public void actualizarMetodo(object sender, EventArgs e)
		{
			string baseurl = "http://scmrocket.azurewebsites.net/api/pedidos/"+id.ToString();
			var Client = new HttpClient();
			Client.MaxResponseContentBufferSize = 256000;
			var uril = new Uri(baseurl);
			var response = Client.GetAsync(uril).Result;
			if (response.IsSuccessStatusCode)
			{
				var content = response.Content.ReadAsStringAsync().Result;
				var items = JsonConvert.DeserializeObject<PedidoModel>(content);
				estadoPedido.Text = items.estado;
				Android.Locations.Location puntoa = new Android.Locations.Location("punto A");
				Android.Locations.Location puntob = new Android.Locations.Location("punto B");
				puntoa.Latitude=items.latitud;
				puntoa.Longitude = items.longitud;
				puntob.Latitude = items.latitudRep;
				puntob.Longitude = items.longitudRep;
				distancia.Text = puntoa.DistanceTo(puntob).ToString();
			}
		}
		public bool Postear(PedidoModel item)
		{
			string baseurl = "http://scmrocket.azurewebsites.net/api/pedidos";
			var Client = new HttpClient();
			Client.MaxResponseContentBufferSize = 256000;
			var uril = new Uri(baseurl);
			var json = JsonConvert.SerializeObject(item);
			StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
			var response = Client.PostAsync(uril, content).Result;
			if (response.IsSuccessStatusCode)
			{
				return true;
			}
			else
			{
				return false;
			}
		}


	}
}
