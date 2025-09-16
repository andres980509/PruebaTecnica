# 🏥 API de Gestión de Pacientes - .NET Core  

**Nombre del proyecto:** `PruebaTecnica`  

---

## 📖 Descripción  

API RESTful construida con **ASP.NET Core 9**, **Entity Framework Core 9** y **SQL Server**,  
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

### 📂 Pasos  

1. **Clonar el repositorio:**  

   ```bash
   git clone https://github.com/tu-usuario/tu-repositorio.git
   cd tu-repositorio


Crear la base de datos en SQL Server:

CREATE DATABASE PacientesDB;


Configurar la cadena de conexión en appsettings.json:

"ConnectionStrings": {
  "DefaultConnection": "Server=TU_SERVIDOR_SQL;Database=PacientesDB;Trusted_Connection=True;MultipleActiveResultSets=true"
}


🔧 Reemplaza TU_SERVIDOR_SQL por tu instancia de SQL Server (ejemplo: localhost\SQLEXPRESS).

Aplicar migraciones y generar la base de datos:

dotnet ef database update


Si no tienes instalado EF Core CLI, ejecuta:

dotnet tool install --global dotnet-ef


Ejecutar la API:

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

⚡ Puedes probarlos con Swagger o Postman.

⚙️ Decisiones Técnicas

1------Filtrado en el endpoint GET /api/patients:
Se decidió implementar filtros exclusivos y no combinables para simplificar la lógica de consulta y mejorar el rendimiento. Esto significa que:

Si se recibe un parámetro name, solo se filtra por nombre.

Si se recibe documentNumber, solo se filtra por documento.

Si se recibe createdAfter, se invoca un procedimiento almacenado (CreadoDespues) que devuelve pacientes creados después de esa fecha, ideal para generar el reporte CSV.

Paginación:
La paginación es opcional y se controla desde el frontend mediante los parámetros page y pageSize. Si no se envían, se devuelven todos los registros correspondientes al filtro aplicado.

2-----Validación de duplicados al crear pacientes:

En el endpoint POST /api/patients, se implementó una validación previa para evitar la creación de pacientes duplicados.
Antes de guardar un nuevo registro, se verifica si ya existe un paciente con la misma combinación de DocumentType y DocumentNumber.

Si existe, se retorna un código 409 Conflict con un mensaje indicando la duplicidad.
Esta validación asegura la integridad de los datos y evita inconsistencias en el sistema de identificación de pacientes.

3-----Actualización de pacientes: modificación total con validación de duplicados.
En el endpoint PUT /api/patients/{id}, se implementó una actualización total del paciente, es decir, se reemplazan todos los campos con los datos recibidos en la solicitud.

Antes de aplicar la actualización, se valida que no exista otro paciente con la misma combinación de DocumentType y DocumentNumber que se desea asignar. Esto garantiza que no se duplique la identificación única del paciente.

Si se detecta que otro paciente ya tiene ese tipo y número de documento, se retorna un código 409 Conflict con un mensaje que indica la duplicidad.



4-----Configuración del modelo y unicidad

En CoreBD se define un índice único sobre DocumentType y DocumentNumber para evitar duplicados en la base de datos.

Esto garantiza la integridad de los datos y facilita las operaciones CRUD con Entity Framework Core.




🧪 Pruebas Automatizadas

Se implementaron pruebas unitarias para al menos dos endpoints utilizando xUnit.
Las pruebas cubren escenarios básicos de creación y obtención de pacientes para garantizar la correcta funcionalidad del API.

🗂️ Estructura de la base de datos

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
CreatedAt	datetime2	Obligatorio, valor por defecto GETUTCDATE()

Además, se creó el procedimiento almacenado CreadoDespues para obtener pacientes creados después de una fecha específica.

📄 Archivo README.md

Este archivo contiene instrucciones para instalación, configuración, arquitectura y decisiones técnicas del proyecto.

