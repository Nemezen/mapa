using System;
using System.Windows.Forms;

namespace sistema_coord
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }



        private void abrirFormHija(object formhija)
        {
            if (this.panelContenedor.Controls.Count > 0) this.panelContenedor.Controls.RemoveAt(0);
            Form fh = formhija as Form;
            fh.TopLevel = false;
            fh.Dock = DockStyle.Fill;
            this.panelContenedor.Controls.Add(fh);
            this.panelContenedor.Tag = fh;
            fh.Show();
        }
        private void btnCliente_Click(object sender, EventArgs e)
        {
            abrirFormHija(new Clientes());
        }

        private void btnProveedores_Click(object sender, EventArgs e)
        {
            abrirFormHija(new Proveedores());
        }

        private void btnEmpleados_Click(object sender, EventArgs e)
        {
            abrirFormHija(new Empleados());

        }
    }
}