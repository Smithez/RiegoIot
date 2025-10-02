using Postgrest.Attributes;
using Postgrest.Models;

namespace DashboardRiego.Web.Models;

[Table("lecturas_sensores")]
public class LecturaSensor : BaseModel
{
    [PrimaryKey("id")]
    public string Id { get; set; } = string.Empty;
    
    [Column("dispositivo_id")]
    public string DispositivoId { get; set; } = string.Empty;
    
    [Column("tipo_lectura")]
    public string TipoLectura { get; set; } = string.Empty;
    
    [Column("valor")]
    public decimal Valor { get; set; }
    
    [Column("unidad")]
    public string Unidad { get; set; } = string.Empty;
    
    [Column("calidad_lectura")]
    public int? CalidadLectura { get; set; }
    
    [Column("timestamp")]
    public DateTime Timestamp { get; set; }
    
    [Column("procesado")]
    public bool Procesado { get; set; } = false;
    
    // Propiedades de navegación
    public Dispositivo? Dispositivo { get; set; }
    
    // Propiedades calculadas
    public string TipoLecturaDisplay => TipoLectura switch
    {
        "humedad_suelo" => "💦 Humedad Suelo",
        "temperatura" => "🌡️ Temperatura",
        "humedad_ambiente" => "💨 Humedad Ambiente",
        "ph" => "⚗️ pH",
        "conductividad" => "⚡ Conductividad",
        "presion_atmosferica" => "🌡️ Presión",
        "velocidad_viento" => "💨 Viento",
        "radiacion_solar" => "☀️ Radiación",
        _ => TipoLectura
    };
    
    public string ValorFormateado => $"{Valor:N2} {Unidad}";
    
    public string CalidadBadgeClass => CalidadLectura switch
    {
        >= 90 => "bg-success",
        >= 70 => "bg-info",
        >= 50 => "bg-warning",
        _ => "bg-danger"
    };
    
    public bool EsLecturaReciente => (DateTime.Now - Timestamp).TotalMinutes <= 30;
    
    public bool RequiereAtencion
    {
        get
        {
            return TipoLectura switch
            {
                "humedad_suelo" => Valor < 30 || Valor > 90,
                "temperatura" => Valor < 5 || Valor > 40,
                "ph" => Valor < 5.5m || Valor > 7.5m,
                _ => false
            };
        }
    }
}

