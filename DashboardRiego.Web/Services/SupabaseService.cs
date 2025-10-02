using Supabase;
using DashboardRiego.Web.Models;
using System.Threading.Tasks;
using static Postgrest.Constants;

namespace DashboardRiego.Web.Services
{
    public class SupabaseService
    {
        private readonly Supabase.Client _supabase;

        public SupabaseService(string url, string key)
        {
            var options = new SupabaseOptions
            {
                AutoConnectRealtime = true
            };

            _supabase = new Supabase.Client(url, key, options);
        }

    private bool _isInitialized = false;
    private readonly SemaphoreSlim _initSemaphore = new SemaphoreSlim(1, 1);
    
    public virtual async Task InitializeAsync()
    {
        if (_isInitialized) return;
        
        await _initSemaphore.WaitAsync();
        try
        {
            if (_isInitialized) return;
            
            await _supabase.InitializeAsync();
            _isInitialized = true;
            Console.WriteLine("Supabase initialized successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing Supabase: {ex.Message}");
            throw;
        }
        finally
        {
            _initSemaphore.Release();
        }
    }        // Métodos para Fincas
        public virtual async Task<List<Finca>> GetFincasAsync()
        {
            var response = await _supabase
                .From<Finca>()
                .Get();
            return response?.Models?.ToList() ?? new List<Finca>();
        }

        public virtual async Task<Finca?> GetFincaByIdAsync(string id)
        {
            var response = await _supabase
                .From<Finca>()
                .Filter("id", Operator.Equals, id)
                .Get();
            return response?.Models?.FirstOrDefault();
        }

        public virtual async Task<Finca?> CreateFincaAsync(Finca finca)
        {
            var response = await _supabase
                .From<Finca>()
                .Insert(finca);
            return response?.Models?.FirstOrDefault();
        }

        public virtual async Task<Finca?> UpdateFincaAsync(Finca finca)
        {
            var response = await _supabase
                .From<Finca>()
                .Update(finca);
            return response?.Models?.FirstOrDefault();
        }

        public virtual async Task DeleteFincaAsync(string id)
        {
            await _supabase
                .From<Finca>()
                .Filter("id", Operator.Equals, id)
                .Delete();
        }

        // Métodos para Dispositivos
        public virtual async Task<List<Dispositivo>> GetDispositivosAsync()
        {
            try
            {
                var response = await _supabase
                    .From<Dispositivo>()
                    .Get();
                
                if (response == null)
                {
                    Console.WriteLine("La respuesta de Supabase es nula");
                    return new List<Dispositivo>();
                }

                if (response.Models == null)
                {
                    Console.WriteLine("Los modelos en la respuesta son nulos");
                    return new List<Dispositivo>();
                }

                var dispositivos = response.Models.ToList();
                Console.WriteLine($"Se encontraron {dispositivos.Count} dispositivos");
                return dispositivos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener dispositivos: {ex}");
                throw;
            }
        }

        public virtual async Task<Dispositivo?> GetDispositivoByIdAsync(string id)
        {
            var response = await _supabase
                .From<Dispositivo>()
                .Filter("id", Operator.Equals, id)
                .Get();
            return response?.Models?.FirstOrDefault();
        }

        public virtual async Task<Dispositivo?> CreateDispositivoAsync(Dispositivo dispositivo)
        {
            var response = await _supabase
                .From<Dispositivo>()
                .Insert(dispositivo);
            return response?.Models?.FirstOrDefault();
        }

        public virtual async Task<Dispositivo?> UpdateDispositivoAsync(Dispositivo dispositivo)
        {
            var response = await _supabase
                .From<Dispositivo>()
                .Update(dispositivo);
            return response?.Models?.FirstOrDefault();
        }

        public virtual async Task DeleteDispositivoAsync(string id)
        {
            await _supabase
                .From<Dispositivo>()
                .Filter("id", Operator.Equals, id)
                .Delete();
        }

        // Métodos para Eventos de Riego
        public virtual async Task<List<EventoRiego>> GetEventosRiegoAsync()
        {
            try
            {
                var response = await _supabase
                    .From<EventoRiego>()
                    .Get();

                if (response?.Models == null)
                {
                    Console.WriteLine("No se encontraron eventos de riego");
                    return new List<EventoRiego>();
                }

                var eventos = response.Models.ToList();
                Console.WriteLine($"Se encontraron {eventos.Count} eventos de riego");

                // Cargar los dispositivos relacionados
                var dispositivos = await GetDispositivosAsync();
                foreach (var evento in eventos)
                {
                    evento.Dispositivo = dispositivos.FirstOrDefault(d => d.Id == evento.DispositivoId);
                }

                return eventos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener eventos de riego: {ex}");
                throw;
            }
        }

        public virtual async Task<EventoRiego?> GetEventoRiegoByIdAsync(string id)
        {
            try
            {
                var response = await _supabase
                    .From<EventoRiego>()
                    .Filter("id", Operator.Equals, id)
                    .Get();

                var evento = response?.Models?.FirstOrDefault();
                if (evento != null)
                {
                    evento.Dispositivo = await GetDispositivoByIdAsync(evento.DispositivoId);
                }

                return evento;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener evento de riego {id}: {ex}");
                throw;
            }
        }

        public virtual async Task<EventoRiego?> CreateEventoRiegoAsync(EventoRiego evento)
        {
            try
            {
                var response = await _supabase
                    .From<EventoRiego>()
                    .Insert(evento);

                var nuevoEvento = response?.Models?.FirstOrDefault();
                if (nuevoEvento != null)
                {
                    nuevoEvento.Dispositivo = await GetDispositivoByIdAsync(nuevoEvento.DispositivoId);
                }

                return nuevoEvento;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear evento de riego: {ex}");
                throw;
            }
        }

        public virtual async Task<EventoRiego?> UpdateEventoRiegoAsync(EventoRiego evento)
        {
            try
            {
                var response = await _supabase
                    .From<EventoRiego>()
                    .Update(evento);

                var eventoActualizado = response?.Models?.FirstOrDefault();
                if (eventoActualizado != null)
                {
                    eventoActualizado.Dispositivo = await GetDispositivoByIdAsync(eventoActualizado.DispositivoId);
                }

                return eventoActualizado;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar evento de riego {evento.Id}: {ex}");
                throw;
            }
        }

        // Métodos para Lecturas de Sensores
        public virtual async Task<List<LecturaSensor>> GetLecturasAsync()
        {
            try
            {
                var response = await _supabase
                    .From<LecturaSensor>()
                    .Get();

                if (response?.Models == null)
                {
                    Console.WriteLine("No se encontraron lecturas de sensores");
                    return new List<LecturaSensor>();
                }

                var lecturas = response.Models.ToList();
                Console.WriteLine($"Se encontraron {lecturas.Count} lecturas de sensores");

                // Cargar los dispositivos relacionados
                var dispositivos = await GetDispositivosAsync();
                foreach (var lectura in lecturas)
                {
                    lectura.Dispositivo = dispositivos.FirstOrDefault(d => d.Id == lectura.DispositivoId);
                }

                return lecturas;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener lecturas de sensores: {ex}");
                throw;
            }
        }
    }
}