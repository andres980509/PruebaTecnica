# 🏥 API de Gestión de Pacientes - .NET Core  

**Proyecto:** `PruebaTecnica`  

---

## 📖 Descripción  

API RESTful desarrollada con **ASP.NET Core 9**, **Entity Framework Core 9** y **SQL Server**,  
que permite gestionar pacientes mediante operaciones **CRUD**, con **filtros**, **validaciones**, **paginación** y **pruebas automatizadas**.  

Este proyecto corresponde a una **prueba técnica de evaluación**.  

---

## 🚀 Tecnologías utilizadas  

- **ASP.NET Core** `9.0.9`  
- **Entity Framework Core** `9.0.x`  
- **SQL Server** `2019/2022`  
- **.NET SDK** `9.0.305`  
- **Lenguaje:** `C#`  

---

## 🛠️ Instalación, Configuración y Ejecución  

### ✅ Requisitos Previos  

- .NET SDK `9.0`  
- SQL Server `2019` o superior  
- Visual Studio `2022/2025` o Visual Studio Code  
- Postman *(opcional, para pruebas manuales)*  

---

### 📂 Pasos de instalación  

1. **Clonar el repositorio:**  

   ```bash
   git clone https://github.com/andres980509/PruebaTecnica.git
   cd PruebaTecnica
Crear la base de datos en SQL Server:

sql
Copiar código
CREATE DATABASE PacientesDB;
Configurar la cadena de conexión en appsettings.json:

json
Copiar código
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=TU_SERVIDOR_SQL;Database=PacientesDB;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
🔧 Reemplaza TU_SERVIDOR_SQL por tu instancia de SQL Server (ejemplo: localhost\\SQLEXPRESS).

Aplicar migraciones y generar la base de datos:

bash
Copiar código
dotnet ef database update
Si no tienes instalado EF Core CLI, primero ejecuta:

bash
Copiar código
dotnet tool install --global dotnet-ef
Ejecutar la API:

bash
Copiar código
dotnet run
Acceder a la aplicación:

API: http://localhost:5000

Swagger: http://localhost:5000/swagger/index.html

🔗 Endpoints principales
Método	Endpoint	Descripción
POST	/api/patients	Crear paciente
GET	/api/patients	Listar pacientes
GET	/api/patients/{id}	Obtener paciente por ID
PUT	/api/patients/{id}	Actualizar paciente
DELETE	/api/patients/{id}	Eliminar paciente

⚡ Puedes probarlos directamente desde Swagger o Postman.

⚙️ Decisiones Técnicas
Filtrado en GET /api/patients:

Los filtros son exclusivos y no combinables, para simplificar la consulta y mejorar rendimiento.

Casos de uso:

name → filtra por nombre.

documentNumber → filtra por documento.

createdAfter → invoca el procedimiento almacenado CreadoDespues para pacientes creados después de esa fecha (ideal para reportes CSV).

La paginación es opcional mediante page y pageSize. Si no se envían, se devuelven todos los registros filtrados.

Validación de duplicados al crear pacientes:

En POST /api/patients se verifica previamente si ya existe un paciente con la misma combinación de DocumentType y DocumentNumber.

Si existe, se retorna 409 Conflict.

Esto garantiza integridad de datos y evita inconsistencias.

Actualización total con validación de duplicados:

En PUT /api/patients/{id} se reemplazan todos los campos.

Antes de actualizar, se valida que otro paciente no tenga el mismo documento.

Si se detecta duplicidad → 409 Conflict.

Restricciones a nivel de base de datos:

En el modelo se define un índice único sobre (DocumentType, DocumentNumber).

Esto asegura que no haya duplicados incluso si la validación de API fallara.

🧪 Pruebas Automatizadas
Implementadas con xUnit.

Se cubren al menos dos endpoints:

Creación de paciente.

Consulta de paciente.

Validan escenarios básicos para garantizar funcionamiento del API.

🗂️ Estructura de la Base de Datos
Tabla: Patients

Campo	Tipo de dato	Restricciones / Descripción
PatientId	int	PK, Identity (auto-incremental)
DocumentType	nvarchar(10)	Obligatorio
DocumentNumber	nvarchar(20)	Obligatorio, Único
FirstName	nvarchar(80)	Obligatorio
LastName	nvarchar(80)	Obligatorio
BirthDate	datetime2	Obligatorio
PhoneNumber	nvarchar(20)	Opcional
Email	nvarchar(120)	Opcional
CreatedAt	datetime2	Obligatorio, GETUTCDATE() por defecto

Procedimiento almacenado utilizado:

sql
Copiar código
CREATE PROCEDURE CreadoDespues
  @Fecha DATETIME2
AS
BEGIN
  SELECT *
  FROM Patients
  WHERE CreatedAt > @Fecha;
END
📄 Notas Finales
Este archivo README.md contiene:

Instrucciones de instalación y configuración.

Script SQL de la base de datos.

Cadena de conexión JSON.

Explicación de decisiones técnicas.

Documentación de endpoints.

Estructura de datos y procedimientos.
