using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Modelo;

namespace PruebaTecnica.BD
{
    public class CoreBD : DbContext
    {
        public CoreBD(DbContextOptions<CoreBD> options) : base(options) { }
        public DbSet<Patient> Patients { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigurarPatient(modelBuilder);
        }
        private void ConfigurarPatient(ModelBuilder modelBuilder)
        {
            // Indice_Compuesto: DocumentType + DocumentNumber
            modelBuilder.Entity<Patient>()
                .HasIndex(p => new { p.DocumentType, p.DocumentNumber })
                .IsUnique();



        }
    }
}