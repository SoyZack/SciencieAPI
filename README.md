# API de Descubrimientos Científicos

Proyecto Full-Stack desarrollado para la gestión de científicos, sus especialidades, descubrimientos e investigaciones. El sistema se divide en una API RESTful construida con .NET y un cliente web independiente basado en tecnologías estándar.

## 🛠️ Tecnologías y Herramientas Utilizadas
- **Backend:** C#, .NET 10, Entity Framework Core, AutoMapper, Swagger.
- **Frontend:** HTML5, CSS3, JavaScript (Fetch API), Bootstrap 5.3.
- **Base de Datos:** SQL Server.

## 📁 Estructura del Proyecto
El repositorio está dividido en dos capas principales:
1. `SciencieAPI/`: Contiene el backend (controladores, modelos, DTOs, mapeos y contexto de base de datos).
2. `ClienteWeb/`: Contiene la interfaz de usuario (`index.html`, `app.js`) que consume la API.

## 🚀 Instrucciones de Configuración y Ejecución

### 1. Configuración de la Base de Datos
Ejecuta el siguiente script en SQL Server Management Studio (SSMS) o Azure Data Studio para generar la base de datos y sus tablas con las restricciones de cascada necesarias:

```sql
CREATE DATABASE DescubrimientosCientificosDB;
GO
USE DescubrimientosCientificosDB;
GO

CREATE TABLE Cientificos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    Nacionalidad NVARCHAR(100) NOT NULL,
    FechaNacimiento DATE NOT NULL,
    Email NVARCHAR(150) NOT NULL
);

CREATE TABLE Especialidades (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(MAX)
);

CREATE TABLE CientificoEspecialidad (
    CientificoId INT NOT NULL,
    EspecialidadId INT NOT NULL,
    PRIMARY KEY (CientificoId, EspecialidadId),
    FOREIGN KEY (CientificoId) REFERENCES Cientificos(Id) ON DELETE CASCADE,
    FOREIGN KEY (EspecialidadId) REFERENCES Especialidades(Id) ON DELETE CASCADE
);

CREATE TABLE Descubrimientos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Titulo NVARCHAR(200) NOT NULL,
    Descripcion NVARCHAR(MAX) NOT NULL,
    FechaDescubrimiento DATE NOT NULL,
    Lugar NVARCHAR(150) NOT NULL,
    CientificoId INT NOT NULL,
    FOREIGN KEY (CientificoId) REFERENCES Cientificos(Id) ON DELETE CASCADE
);

CREATE TABLE Investigaciones (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Titulo NVARCHAR(200) NOT NULL,
    AreaEstudio NVARCHAR(150) NOT NULL,
    FechaInicio DATE NOT NULL,
    FechaFin DATE NULL,
    Estado NVARCHAR(50) NOT NULL,
    CientificoId INT NOT NULL,
    FOREIGN KEY (CientificoId) REFERENCES Cientificos(Id) ON DELETE CASCADE
);
```

### 2. Configuración del Backend
1. Abre el archivo `SciencieAPI/appsettings.json`.
2. Verifica que la cadena de conexión (`DefaultConnection`) apunte a tu servidor SQL Server local.
3. Abre una terminal en la carpeta `SciencieAPI` y ejecuta:
   ```bash
   dotnet run
   ```
4. La API se levantará en tu servidor local (usualmente `http://localhost:5112`). Puedes probar los endpoints interactivos añadiendo `/swagger` a la URL.

### 3. Ejecución del Cliente Web
Al ser una aplicación de software libre independiente, no requiere un servidor web para desarrollo. 
Simplemente navega a la carpeta `ClienteWeb` y haz doble clic en el archivo `index.html` para abrirlo en tu navegador.

## 👥 Autores
- Edgar Ruiz
- Miguel Bermudez
