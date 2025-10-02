using Postgrest.Attributes;
using Postgrest.Models;

namespace DashboardRiego.Web.Models;

[Table("eventos_riego")]
public class EventoRiego : BaseModel
{
    [PrimaryKey("id")]
    public string Id { get; set; } = string.Empty;
    
    [Column("dispositivo_id")]
    public string DispositivoId { get; set; } = string.Empty;
    
    [Column("fecha_inicio")]
    public DateTime FechaInicio { get; set; }
    
    [Column("duracion_minutos")]
    public int DuracionMinutos { get; set; }
    
    [Column("tipo_riego")]
    public string TipoRiego { get; set; } = string.Empty;
    
    [Column("estado")]
    public string Estado { get; set; } = string.Empty;
    
    [Column("timestamp")]
    public DateTime Timestamp { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }
    
    // Propiedades de navegación
    public Dispositivo? Dispositivo { get; set; }
    
    // Propiedades UI
    public string TipoRiegoDisplay => TipoRiego switch
    {
        "programado" => "⏰ Programado",
        "manual" => "� Manual",
        "automatico" => "🤖 Automático",
        _ => TipoRiego
    };

    public string EstadoDisplay => Estado switch
    {
        "programado" => "⏰ Programado",
        "en_curso" => "▶️ En Curso",
        "completado" => "✅ Completado",
        "cancelado" => "� Cancelado",
        "error" => "❌ Error",
        _ => Estado
    };

    public string EstadoBadgeClass => Estado switch
    {
        "programado" => "bg-primary",
        "en_curso" => "bg-success",
        "completado" => "bg-secondary",
        "cancelado" => "bg-danger",
        "error" => "bg-warning",
        _ => "bg-secondary"
    };

    public bool PuedeSerCancelado => 
        Estado == "programado" || Estado == "en_curso";

    public string TiempoTranscurrido
    {
        get
        {
            var diferencia = DateTime.Now - Timestamp;
            
            if (diferencia.TotalMinutes < 60)
                return $"Hace {(int)diferencia.TotalMinutes} min";
            else if (diferencia.TotalHours < 24)
                return $"Hace {(int)diferencia.TotalHours} horas";
            else
                return $"Hace {(int)diferencia.TotalDays} días";
        }
    }
}
