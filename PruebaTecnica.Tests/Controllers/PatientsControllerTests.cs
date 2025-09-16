using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using PruebaTecnica.BD;
using PruebaTecnica.Controllers;
using PruebaTecnica.Modelo;
using System;
using System.Threading.Tasks;

namespace PruebaTecnica.Tests.Controllers
{
    public class PatientsControllerTests
    {
        private CoreBD GetDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<CoreBD>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new CoreBD(options);
        }

        [Fact]
        public async Task CreatePatient_NewPatient_ReturnsCreated()
        {
            var context = GetDbContext("CreatePatientDB");
            var controller = new PatientsController(context);

            var newPatient = new Patient
            {
                DocumentType = "DNI",
                DocumentNumber = "999",
                FirstName = "Pedro",
                LastName = "López",
                BirthDate = DateTime.Now
            };

            var result = await controller.CreatePatient(newPatient);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdPatient = Assert.IsType<Patient>(createdResult.Value);
            Assert.Equal("Pedro", createdPatient.FirstName);
        }

        [Fact]
        public async Task GetPatientById_ExistingId_ReturnsPatient()
        {
            var context = GetDbContext("GetByIdDB");
            // Agregamos un paciente para buscar
            context.Patients.Add(new Patient
            {
                PatientId = 1,
                DocumentType = "CC",
                DocumentNumber = "123",
                FirstName = "Juan",
                LastName = "Pérez",
                BirthDate = DateTime.Now
            });
            context.SaveChanges();

            var controller = new PatientsController(context);

            var result = await controller.GetPatientById(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var patient = Assert.IsType<Patient>(okResult.Value);
            Assert.Equal(1, patient.PatientId);
        }
    }
}
