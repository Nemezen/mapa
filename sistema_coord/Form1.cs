using System;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using sistema_coord.DB;
using sistema_coord.Models;

namespace sistema_coord
{
    public partial class Form1 : Form
    {
        private readonly SistemaCoordenadasEntities dbContext;
        private GMarkerGoogle marcador;
        private double lat, lon;
        private bool mapaArrastrandose = false;

        public Form1()
        {
            InitializeComponent();
            dbContext = new SistemaCoordenadasEntities();
        }

        private void btnBuscar_Click_1(object sender, EventArgs e)
        {
            if (int.TryParse(txtID.Text, out int id))
            {
                using (var dbContext = new SistemaCoordenadasEntities())
                {
                    // Ejemplo de consulta
                    
                    // Puedes realizar operaciones adicionales con la base de datos aquí
                }
                // Buscar en la base de datos por ID
                var cliente = dbContext.Clientes.FirstOrDefault(c => c.Id == id);
                /* var empleado = dbContext.Empleados.FirstOrDefault(emp => emp.Id == id);
                 var proveedor = dbContext.Proveedores.FirstOrDefault(prov => prov.Id == id);*/

                // Mostrar en el mapa
                MostrarEnMapa(cliente?.Latitud, cliente?.Longitud, cliente?.Direccion);
                /*                MostrarEnMapa(empleado?.Latitud, empleado?.Longitud, empleado?.Direccion);
                                MostrarEnMapa(proveedor?.Latitud, proveedor?.Longitud, proveedor?.Direccion);*/
            }
            else
            {
                MessageBox.Show("Ingrese un ID válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarEnMapa(double? latitud, double? longitud, string direccion)
        {
            if (latitud.HasValue && longitud.HasValue)
            {
                // Limpiar marcadores anteriores
                gMapControl1.Overlays.Clear();

                // Agregar un marcador
                GMapOverlay markersOverlay = new GMapOverlay("Marcadores");
                GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(latitud.Value, longitud.Value), GMarkerGoogleType.red);
                markersOverlay.Markers.Add(marker);
                gMapControl1.Overlays.Add(markersOverlay);

                // Centrar el mapa en las coordenadas
                gMapControl1.Position = new PointLatLng(latitud.Value, longitud.Value);
                gMapControl1.Zoom = 14;

                // Mostrar la dirección en un cuadro de texto
                txtDireccion.Text = direccion;
            }
            else
            {
                MessageBox.Show("No se encontraron coordenadas para el ID proporcionado.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void gMapControl1_MouseClick(object sender, MouseEventArgs e)
        {
            if (mapaArrastrandose)
            {
                this.lat = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lat;
                this.lon = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lng;

                // Mostrar el marcador en el mapa
                MostrarMarcador(this.lat, this.lon);

                // También puedes mostrar las coordenadas en algún lugar, como un cuadro de texto
                txtDireccion.Text = $"Latitud: {this.lat}, Longitud: {this.lon}";
            }

            // Restablecer la bandera después de que se completa el clic
            mapaArrastrandose = false;
        }


        private void gMapControl1_Load(object sender, EventArgs e)
        {

        }

        private void gMapControl1_OnMapDrag()
        {
            // Establecer la bandera cuando se inicia el arrastre del mapa
            mapaArrastrandose = true;
        }


        private void Form1_Load_1(object sender, EventArgs e)
        {
            try
            {
                // Configurar el control GMap para centrarlo en México
                gMapControl1.MapProvider = GMapProviders.GoogleMap;
                gMapControl1.CanDragMap = true;
                gMapControl1.DragButton = MouseButtons.Left;
                gMapControl1.Position = new PointLatLng(23.6345, -102.5528);
                gMapControl1.MinZoom = 1;
                gMapControl1.MaxZoom = 24;
                gMapControl1.Zoom = 9;
                gMapControl1.AutoScroll = true;
                gMapControl1.MouseClick += gMapControl1_MouseClick;

                gMapControl1.OnMapDrag += gMapControl1_OnMapDrag;
                // Establecer el nivel de zoom
                gMapControl1.Zoom = 6;

                gMapControl1.MouseClick += gMapControl1_MouseClick;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al configurar el mapa: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guardarCoord_Click(object sender, EventArgs e)
        {

            // Mostrar el marcador en el mapa
            GuardarCoordenadas(this.lat, this.lon);
        }
        private void MostrarMarcador(double latitud, double longitud)
        {
            // Limpiar marcadores anteriores
            gMapControl1.Overlays.Clear();

            // Agregar un marcador
            GMapOverlay markersOverlay = new GMapOverlay("Marcadores");
            marcador = new GMarkerGoogle(new PointLatLng(latitud, longitud), GMarkerGoogleType.red);
            markersOverlay.Markers.Add(marcador);
            gMapControl1.Overlays.Add(markersOverlay);

            // Centrar el mapa en las coordenadas del marcador
            gMapControl1.Position = new PointLatLng(latitud, longitud);
        }
        private void GuardarCoordenadas(double latitud, double longitud)
        {
            // Guardar en la base de datos (ejemplo para Clientes)
            Cliente nuevoCliente = new Cliente
            {
                Latitud = latitud,
                Longitud = longitud,
                // Otros campos de Cliente si los hay
            };

            dbContext.Clientes.Add(nuevoCliente);
            dbContext.SaveChanges();
        }

    }
}