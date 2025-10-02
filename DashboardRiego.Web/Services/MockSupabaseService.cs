using DashboardRiego.Web.Models;

namespace DashboardRiego.Web.Services
{
    public class MockSupabaseService : SupabaseService
    {
        public MockSupabaseService() : base("mock", "mock") { }

        public override Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public override Task<List<Finca>> GetFincasAsync()
        {
            // Datos de ejemplo para desarrollo
            var fincas = new List<Finca>
            {
                new Finca { Id = "1", Nombre = "Finca de Prueba 1", UbicacionMunicipio = "Ubicación 1" },
                new Finca { Id = "2", Nombre = "Finca de Prueba 2", UbicacionMunicipio = "Ubicación 2" }
            };
            return Task.FromResult(fincas);
        }

        public override Task<List<Dispositivo>> GetDispositivosAsync()
        {
            // Datos de ejemplo para desarrollo
            var dispositivos = new List<Dispositivo>
            {
                new Dispositivo { Id = "1", Nombre = "Dispositivo de Prueba 1", Tipo = "Sensor", LoteId = "1" },
                new Dispositivo { Id = "2", Nombre = "Dispositivo de Prueba 2", Tipo = "Actuador", LoteId = "1" }
            };
            return Task.FromResult(dispositivos);
        }
    }
}