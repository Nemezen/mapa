﻿using System;

namespace sistema_coord.Models
{
    public class Proveedor
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string Direccion { get; set; }
        public string Colonia { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
