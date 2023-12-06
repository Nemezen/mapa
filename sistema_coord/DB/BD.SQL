-- Crear la base de datos
CREATE DATABASE sistema_coordenadas;

-- Utilizar la base de datos
USE sistema_coordenadas;

-- Crear la tabla Clientes
CREATE TABLE Clientes (
    id INT IDENTITY(1,1) PRIMARY KEY,
    nombre NVARCHAR(255) NOT NULL,
    latitud FLOAT NOT NULL,
    longitud FLOAT NOT NULL,
    direccion NVARCHAR(255) NOT NULL,
    colonia NVARCHAR(255) NOT NULL
);

-- Crear la tabla Empleados
CREATE TABLE Empleados (
    id INT IDENTITY(1,1) PRIMARY KEY,
    nombre NVARCHAR(255) NOT NULL,
    latitud FLOAT NOT NULL,
    longitud FLOAT NOT NULL,
    direccion NVARCHAR(255) NOT NULL,
    colonia NVARCHAR(255) NOT NULL,
    fecha_registro DATE NOT NULL
);

-- Crear la tabla Proveedores
CREATE TABLE Proveedores (
    id INT IDENTITY(1,1) PRIMARY KEY,
    nombre NVARCHAR(255) NOT NULL,
    latitud FLOAT NOT NULL,
    longitud FLOAT NOT NULL,
    direccion NVARCHAR(255) NOT NULL,
    colonia NVARCHAR(255) NOT NULL,
    fecha_registro DATE NOT NULL
);