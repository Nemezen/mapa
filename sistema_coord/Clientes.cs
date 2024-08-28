using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using sistema_coord.DB;
using sistema_coord.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace sistema_coord
{
    public partial class Clientes : Form
    {
        private readonly SistemaCoordenadasEntities dbContext;
        private GMarkerGoogle marcador;
        private double lat, lon;
        private bool mapaArrastrandose = false;
        private bool editar = false;
        private string idProducto = null;


        public Clientes()
        {

            InitializeComponent();
            dbContext = new SistemaCoordenadasEntities();
        }

        private void Clientes_Load(object sender, EventArgs e)
        {

            mostrarClientes();
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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al configurar el mapa: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            MostrarEnMapa(latitud, longitud);
        }

        private void EliminarClienteSeleccionado()
        {
            try
            {
                // Verificar si hay una fila seleccionada en el DataGridView
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    // Obtener el Id del cliente seleccionado
                    int idCliente = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Id"].Value);

                    // Buscar el cliente en la base de datos
                    Cliente clienteAEliminar = dbContext.Clientes.FirstOrDefault(c => c.Id == idCliente);

                    // Verificar si se encontró el cliente
                    if (clienteAEliminar != null)
                    {
                        // Eliminar el cliente del contexto y guardar los cambios
                        dbContext.Clientes.Remove(clienteAEliminar);
                        dbContext.SaveChanges();

                        // Mostrar los clientes actualizados en el DataGridView
                        mostrarClientes();

                        MessageBox.Show("Cliente eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("No se encontró el cliente seleccionado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Seleccione una fila para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar el cliente: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EditarTodo(string nombre, string direccion, string colonia, double latitud, double longitud)
        {
            if (!string.IsNullOrEmpty(idProducto))
            {
                // Obtener el Cliente existente por el Id
                int id = Convert.ToInt32(idProducto);
                Cliente clienteExistente = dbContext.Clientes.FirstOrDefault(c => c.Id == id);

                if (clienteExistente != null)
                {
                    // Actualizar los campos del Cliente existente
                    clienteExistente.Nombre = nombre;
                    clienteExistente.Direccion = direccion;
                    clienteExistente.Colonia = colonia;
                    clienteExistente.Latitud = latitud;
                    clienteExistente.Longitud = longitud;

                    // Guardar los cambios en la base de datos
                    dbContext.SaveChanges();
                    mostrarClientes();
                }
                else
                {
                    MessageBox.Show("No se encontró el cliente con el ID proporcionado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar un cliente para editar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GuardarTodo(string nombre, string direccion, string colonia, double latitud, double longitud)
        {
            try
            {
                // Crear un nuevo Cliente con los datos proporcionados
                Cliente nuevoCliente = new Cliente
                {
                    Nombre = nombre,
                    Direccion = direccion,
                    Colonia = colonia,
                    Latitud = latitud,
                    Longitud = longitud
                };

                // Agregar el nuevo Cliente al contexto y guardar los cambios
                dbContext.Clientes.Add(nuevoCliente);
                dbContext.SaveChanges();

                // Mostrar los clientes actualizados en el DataGridView
                mostrarClientes();

                // Limpiar el formulario después de guardar exitosamente
                limpiarForm();

                MessageBox.Show("Se insertó correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al insertar los datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void gMapControl1_MouseClick(object sender, MouseEventArgs e)
        {
            // Mostrar el marcador solo si no se está arrastrando el mapa
            if (mapaArrastrandose)
            {
                this.lat = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lat;
                this.lon = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lng;

                // Mostrar el marcador en el mapa
                MostrarMarcador(this.lat, this.lon);
            }

            // Restablecer la bandera después de que se completa el clic
            mapaArrastrandose = true;
        }
        private void gMapControl1_OnMapDrag()
        {
            // Establecer la bandera cuando se inicia el arrastre del mapa
            mapaArrastrandose = false;
        }

        private void mostrarClientes()
        {
            // Obtener todos los clientes de la base de datos
            List<Cliente> clientes = dbContext.Clientes.ToList();
            // Asignar la lista de clientes al DataSource del DataGridView
            dataGridView1.DataSource = clientes;
            dataGridView1.Columns["Id"].Width = 30;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {


        }

        private void txtNombre_Enter(object sender, EventArgs e)
        {
            if (txtNombre.Text.Equals("Ingrese su nombre"))
            {
                txtNombre.Text = "";
                txtNombre.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void txtLongitud_Enter(object sender, EventArgs e)
        {
            if (txtLongitud.Text.Equals("Ingrese longitud"))
            {
                txtLongitud.Text = "";
                txtLongitud.ForeColor = System.Drawing.Color.Black;
            }

        }

        private void txtDireccion_Enter(object sender, EventArgs e)
        {

            if (txtDireccion.Text.Equals("Ingrese direccion"))
            {
                txtDireccion.Text = "";
                txtDireccion.ForeColor = System.Drawing.Color.Black;
            }

        }

        private void txtColonia_Enter(object sender, EventArgs e)
        {
            if (txtColonia.Text.Equals("Ingrese colonia"))
            {
                txtColonia.Text = "";
                txtColonia.ForeColor = System.Drawing.Color.Black;
            }

        }

        private void txtLatitud_Enter(object sender, EventArgs e)
        {

            if (txtLatitud.Text.Equals("Ingrese latitud"))
            {
                txtLatitud.Text = "";
                txtLatitud.ForeColor = System.Drawing.Color.Black;
            }

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!editar)
                try
                {
                    GuardarTodo(txtNombre.Text, txtDireccion.Text, txtColonia.Text, Double.Parse(txtLatitud.Text), Double.Parse(txtLongitud.Text));
                    limpiarForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se pudo insertar los datos por: " + ex);
                    limpiarForm();
                }
            else
            {
                try
                {
                    EditarTodo(txtNombre.Text, txtDireccion.Text, txtColonia.Text, Double.Parse(txtLatitud.Text), Double.Parse(txtLongitud.Text));
                    MessageBox.Show("Se editó correctamente");
                    limpiarForm();
                    editar = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se pudo editar los datos por: " + ex);
                }
            }


        }
        private void limpiarForm()
        {
            txtNombre.Clear();
            txtDireccion.Text = "";
            txtColonia.Clear();
            txtLatitud.Clear();
            txtLongitud.Clear();
            txtGmapLatitud.Clear();
            txtGmapLongitud.Clear();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                editar = true;

                txtNombre.Text = dataGridView1.CurrentRow.Cells["Nombre"].Value.ToString();
                txtNombre.ForeColor = System.Drawing.Color.Black;

                txtDireccion.Text = dataGridView1.CurrentRow.Cells["Direccion"].Value.ToString();
                txtDireccion.ForeColor = System.Drawing.Color.Black;

                txtColonia.Text = dataGridView1.CurrentRow.Cells["Colonia"].Value.ToString();
                txtColonia.ForeColor = System.Drawing.Color.Black;

                txtLatitud.Text = dataGridView1.CurrentRow.Cells["Latitud"].Value.ToString();
                txtLatitud.ForeColor = System.Drawing.Color.Black;

                txtLongitud.Text = dataGridView1.CurrentRow.Cells["Longitud"].Value.ToString();
                txtLongitud.ForeColor = System.Drawing.Color.Black;

                idProducto = dataGridView1.CurrentRow.Cells["Id"].Value.ToString();
            }
            else
                MessageBox.Show("seleccione una fila por favor");
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            EliminarClienteSeleccionado();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            // Verificar si el clic fue en una fila y no en los encabezados
            if (e.RowIndex >= 0)
            {
                // Obtener la latitud y longitud de la fila seleccionada
                double latitud = Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells["Latitud"].Value);
                double longitud = Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells["Longitud"].Value);
                // Mostrar el marcador en el mapa
                MostrarMarcador(latitud, longitud);
            }


        }
        private void MostrarEnMapa(double? latitud, double? longitud)
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
                txtGmapLatitud.Text = latitud.Value.ToString();
                txtGmapLongitud.Text = longitud.Value.ToString();
            }
            else
            {
                MessageBox.Show("No se encontraron coordenadas para el ID proporcionado.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
