using sistema_coord.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace sistema_coord.DB
{
    public class SistemaCoordenadasRepository
    {
        private readonly SistemaCoordenadasEntities dbContext;
        public SistemaCoordenadasRepository()
        {
            Console.WriteLine(dbContext);
            dbContext = new SistemaCoordenadasEntities();
        }

        #region Operaciones CRUD para Clientes

        public void AgregarCliente(Cliente cliente)
        {
            dbContext.Clientes.Add(cliente);
            dbContext.SaveChanges();
        }

        public Cliente ObtenerClientePorId(int id)
        {
            return dbContext.Clientes.FirstOrDefault(c => c.Id == id);
        }

        public List<Cliente> ObtenerTodosLosClientes()
        {
            return dbContext.Clientes.ToList();
        }

        public void ActualizarCliente(Cliente cliente)
        {
            var clienteExistente = dbContext.Clientes.FirstOrDefault(c => c.Id == cliente.Id);

            if (clienteExistente != null)
            {
                clienteExistente.Nombre = cliente.Nombre;
                clienteExistente.Latitud = cliente.Latitud;
                clienteExistente.Longitud = cliente.Longitud;
                clienteExistente.Direccion = cliente.Direccion;
                clienteExistente.Colonia = cliente.Colonia;
                // Actualizar otros campos según sea necesario

                dbContext.SaveChanges();
            }
            // Manejar el caso en que el cliente no exista si es necesario
        }

        public void EliminarCliente(int id)
        {
            var cliente = dbContext.Clientes.FirstOrDefault(c => c.Id == id);

            if (cliente != null)
            {
                dbContext.Clientes.Remove(cliente);
                dbContext.SaveChanges();
            }
            // Manejar el caso en que el cliente no exista si es necesario
        }

        #endregion

        #region Operaciones CRUD para Empleados

        public void AgregarEmpleado(Empleado empleado)
        {
            dbContext.Empleados.Add(empleado);
            dbContext.SaveChanges();
        }

        public Empleado ObtenerEmpleadoPorId(int id)
        {
            return dbContext.Empleados.FirstOrDefault(emp => emp.Id == id);
        }

        public List<Empleado> ObtenerTodosLosEmpleados()
        {
            return dbContext.Empleados.ToList();
        }

        public void ActualizarEmpleado(Empleado empleado)
        {
            var empleadoExistente = dbContext.Empleados.FirstOrDefault(emp => emp.Id == empleado.Id);

            if (empleadoExistente != null)
            {
                empleadoExistente.Nombre = empleado.Nombre;
                empleadoExistente.Latitud = empleado.Latitud;
                empleadoExistente.Longitud = empleado.Longitud;
                empleadoExistente.Direccion = empleado.Direccion;
                empleadoExistente.Colonia = empleado.Colonia;
                empleadoExistente.FechaRegistro = empleado.FechaRegistro;
                // Actualizar otros campos según sea necesario

                dbContext.SaveChanges();
            }
            // Manejar el caso en que el empleado no exista si es necesario
        }

        public void EliminarEmpleado(int id)
        {
            var empleado = dbContext.Empleados.FirstOrDefault(emp => emp.Id == id);

            if (empleado != null)
            {
                dbContext.Empleados.Remove(empleado);
                dbContext.SaveChanges();
            }
            // Manejar el caso en que el empleado no exista si es necesario
        }

        #endregion

        #region Operaciones CRUD para Proveedores

        public void AgregarProveedor(Proveedor proveedor)
        {
            dbContext.Proveedores.Add(proveedor);
            dbContext.SaveChanges();
        }

        public Proveedor ObtenerProveedorPorId(int id)
        {
            return dbContext.Proveedores.FirstOrDefault(prov => prov.Id == id);
        }

        public List<Proveedor> ObtenerTodosLosProveedores()
        {
            return dbContext.Proveedores.ToList();
        }

        public void ActualizarProveedor(Proveedor proveedor)
        {
            var proveedorExistente = dbContext.Proveedores.FirstOrDefault(prov => prov.Id == proveedor.Id);

            if (proveedorExistente != null)
            {
                proveedorExistente.Nombre = proveedor.Nombre;
                proveedorExistente.Latitud = proveedor.Latitud;
                proveedorExistente.Longitud = proveedor.Longitud;
                proveedorExistente.Direccion = proveedor.Direccion;
                proveedorExistente.Colonia = proveedor.Colonia;
                proveedorExistente.FechaRegistro = proveedor.FechaRegistro;
                // Actualizar otros campos según sea necesario

                dbContext.SaveChanges();
            }
            // Manejar el caso en que el proveedor no exista si es necesario
        }

        public void EliminarProveedor(int id)
        {
            var proveedor = dbContext.Proveedores.FirstOrDefault(prov => prov.Id == id);

            if (proveedor != null)
            {
                dbContext.Proveedores.Remove(proveedor);
                dbContext.SaveChanges();
            }
            // Manejar el caso en que el proveedor no exista si es necesario
        }

        #endregion



    }
}
