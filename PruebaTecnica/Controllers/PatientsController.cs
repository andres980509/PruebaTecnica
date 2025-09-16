using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaTecnica.BD;
using PruebaTecnica.Modelo;

namespace PruebaTecnica.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly CoreBD Datos;

        public PatientsController(CoreBD context)
        {
            Datos = context;
        }

// POST /api/patients
[HttpPost]
public async Task<IActionResult> CreatePatient([FromBody] Patient patient)
{
    // Validar duplicados por DocumentType y DocumentNumber
    bool exists = await Datos.Patients
        .AnyAsync(p => p.DocumentType == patient.DocumentType 
                    && p.DocumentNumber == patient.DocumentNumber);
    if (exists)
    {
        return Conflict(new { message = "Ya existe un paciente con ese tipo y número de documento." });
    }

    Datos.Patients.Add(patient);
    await Datos.SaveChangesAsync();

    return CreatedAtAction(nameof(GetPatientById), new { id = patient.PatientId }, patient);
}
        // GET /api/patients
        [HttpGet]
        public async Task<IActionResult> GetPatients(
     [FromQuery] int? page = null,
     [FromQuery] int? pageSize = null,
     [FromQuery] string? name = null,
     [FromQuery] string? documentNumber = null,
     [FromQuery] DateTime? createdAfter = null)
        {
            // Caso 1: Cuando se especifica una fecha, se ejecuta el procedimiento almacenado.
            // Esta ruta está pensada para reportes y exportaciones (como CSV).
            if (createdAfter.HasValue)
            {
                var lista = await Datos.Patients
                    .FromSqlRaw("EXEC CreadoDespues @fecha = {0}", createdAfter.Value)
                    .AsNoTracking()
                    .ToListAsync();

            

                var total = lista.Count;

                // Si se solicita paginación, esta se aplica en memoria.
                if (page.HasValue && pageSize.HasValue)
                {
                    var paged = lista
                        .Skip((page.Value - 1) * pageSize.Value)
                        .Take(pageSize.Value)
                        .ToList();

                    return Ok(new { total = total, data = paged });
                }

                // Si no se solicita paginación, se retorna la lista completa.
                return Ok(new { total = total, data = lista });
            }

         
            // Se usa cuando no se incluye el parámetro createdAfter.
            var query = Datos.Patients.AsNoTracking().AsQueryable();

            // Filtrado por nombre (FirstName o LastName)
            if (!string.IsNullOrEmpty(name))
                query = query.Where(p => p.FirstName.Contains(name) || p.LastName.Contains(name));

            // Filtrado por número de documento
            if (!string.IsNullOrEmpty(documentNumber))
                query = query.Where(p => p.DocumentNumber.Contains(documentNumber));

            var totalCount = await query.CountAsync();

            // Si se solicitan parámetros de paginación, se aplica skip/take en SQL.
            if (page.HasValue && pageSize.HasValue)
            {
                var patients = await query
                    .Skip((page.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value)
                    .ToListAsync();

                return Ok(new { total = totalCount, data = patients });
            }

            // Si no se solicita paginación, se retorna toda la lista filtrada.
            var all = await query.ToListAsync();
            return Ok(new { total = totalCount, data = all });
        }




        // GET /api/patients/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatientById(int id)
        {
            var patient = await Datos.Patients.FindAsync(id);

            if (patient == null)
                return NotFound();

            return Ok(patient);
        }

        // PUT /api/patients/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(int id, [FromBody] Patient updatedPatient)
        {
            if (id != updatedPatient.PatientId)
                return BadRequest("El ID del paciente no coincide.");

            var existingPatient = await Datos.Patients.FindAsync(id);
            if (existingPatient == null)
                return NotFound();

            bool duplicate = await Datos.Patients
                .AnyAsync(p => p.PatientId != id &&
                               p.DocumentType == updatedPatient.DocumentType &&
                               p.DocumentNumber == updatedPatient.DocumentNumber);

            if (duplicate)
                return Conflict(new { message = "Ya existe un paciente con ese tipo y número de documento." });

            // Actualizar campos
            existingPatient.DocumentType = updatedPatient.DocumentType;
            existingPatient.DocumentNumber = updatedPatient.DocumentNumber;
            existingPatient.FirstName = updatedPatient.FirstName;
            existingPatient.LastName = updatedPatient.LastName;
            existingPatient.BirthDate = updatedPatient.BirthDate;
            existingPatient.PhoneNumber = updatedPatient.PhoneNumber;
            existingPatient.Email = updatedPatient.Email;

            await Datos.SaveChangesAsync();

            // Devuelve el paciente actualizado
            return Ok(existingPatient);
        }


        // DELETE /api/patients/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var patient = await Datos.Patients.FindAsync(id);
            if (patient == null)
                return NotFound();

            Datos.Patients.Remove(patient);
            await Datos.SaveChangesAsync();

            return NoContent();
        }




    }
}
