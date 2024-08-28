using sistema_coord.Models;
using System.Data.Entity;

namespace sistema_coord.DB
{


    public class SistemaCoordenadasEntities : DbContext
    {
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        // Constructor para especificar la cadena de conexión (si es necesario)
        public SistemaCoordenadasEntities() : base("name=SistemaCoordenadasEntities")
        {
        }
    }

}
