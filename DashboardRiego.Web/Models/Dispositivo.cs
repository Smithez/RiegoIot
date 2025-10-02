using Postgrest.Attributes;
using Postgrest.Models;

namespace DashboardRiego.Web.Models;

[Table("dispositivos")]
public class Dispositivo : BaseModel
{
    [PrimaryKey("id")]
    public string Id { get; set; } = string.Empty;
    
    [Column("lote_id")]
    public string LoteId { get; set; } = string.Empty;
    
    [Column("nombre")]
    public string Nombre { get; set; } = string.Empty;
    
    [Column("codigo")]
    public string? Codigo { get; set; }
    
    [Column("tipo")]
    public string Tipo { get; set; } = string.Empty;
    
    [Column("marca")]
    public string? Marca { get; set; }
    
    [Column("modelo")]
    public string? Modelo { get; set; }
    
    [Column("mac_address")]
    public string? MacAddress { get; set; }
    
    [Column("ip_address")]
    public string? IpAddress { get; set; }
    
    [Column("puerto")]
    public int? Puerto { get; set; }
    
    [Column("estado")]
    public string Estado { get; set; } = "activo";
    
    [Column("ubicacion_descripcion")]
    public string? UbicacionDescripcion { get; set; }
    
    [Column("coordenada_x")]
    public decimal? CoordenadaX { get; set; }
    
    [Column("coordenada_y")]
    public decimal? CoordenadaY { get; set; }
    
    [Column("altitud_msnm")]
    public int? AltitudMsnm { get; set; }
    
    [Column("configuracion")]
    public object? Configuracion { get; set; }

    [Column("fecha_instalacion")]
    public DateTime? FechaInstalacion { get; set; }
    
    [Column("fecha_ultimo_mantenimiento")]
    public DateTime? FechaUltimoMantenimiento { get; set; }
    
    [Column("version_firmware")]
    public string? VersionFirmware { get; set; }
    
    [Column("voltaje_bateria")]
    public decimal? VoltajeBateria { get; set; }
    
    [Column("nivel_senal")]
    public int? NivelSenal { get; set; }
    
    [Column("activo")]
    public bool Activo { get; set; } = true;
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }
    
    // Propiedades de navegación
    public Lote? Lote { get; set; }
    public List<EventoRiego> EventosRiego { get; set; } = new();
    public List<LecturaSensor> Lecturas { get; set; } = new();
    
    // Propiedades calculadas
    public string TipoDisplay => Tipo switch
    {
        "riego" => "💧 Riego",
        "sensor_humedad" => "💦 Sensor Humedad",
        "sensor_temperatura" => "🌡️ Sensor Temperatura",
        "sensor_ph" => "⚗️ Sensor pH",
        "sensor_conductividad" => "⚡ Sensor Conductividad",
        "camara" => "📷 Cámara",
        "estacion_meteorologica" => "🌦️ Estación Meteorológica",
        _ => Tipo
    };
    
    public string EstadoDisplay => Estado switch
    {
        "activo" => "✅ Activo",
        "regando" => "💧 Regando",
        "inactivo" => "⚫ Inactivo",
        "mantenimiento" => "🔧 Mantenimiento",
        "error" => "❌ Error",
        "desconectado" => "📵 Desconectado",
        _ => Estado
    };
    
    public string EstadoBadgeClass => Estado.ToLower() switch
    {
        "activo" => "bg-success",
        "regando" => "bg-info",
        "inactivo" => "bg-secondary",
        "mantenimiento" => "bg-warning",
        "error" => "bg-danger",
        "desconectado" => "bg-dark",
        _ => "bg-secondary"
    };
    
    public bool RequiereMantenimiento
    {
        get
        {
            if (FechaUltimoMantenimiento.HasValue)
            {
                var diasDesdeMantenimiento = (DateTime.Now - FechaUltimoMantenimiento.Value).Days;
                return diasDesdeMantenimiento > 90; // Más de 90 días
            }
            return true;
        }
    }
    
    public bool BateriaBaja => VoltajeBateria.HasValue && VoltajeBateria.Value < 3.3m;
    
    public bool SenalDebil => NivelSenal.HasValue && NivelSenal.Value < 30;
}