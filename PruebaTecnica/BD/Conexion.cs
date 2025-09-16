



using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PruebaTecnica.BD

{
    public class Conexion : IDesignTimeDbContextFactory <CoreBD>
    {
        public CoreBD CreateDbContext(string[] args)
        {
            var conectar = new DbContextOptionsBuilder<CoreBD>();
            
            conectar.UseSqlServer("DefaultConnection");

            return new CoreBD(conectar.Options);
        }
    }
}
