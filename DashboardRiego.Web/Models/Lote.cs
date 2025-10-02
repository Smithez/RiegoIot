using Postgrest.Attributes;
using Postgrest.Models;

namespace DashboardRiego.Web.Models;

[Table("lotes")]
public class Lote : BaseModel
{
    [PrimaryKey("id")]
    public string Id { get; set; } = string.Empty;
    
    [Column("finca_id")]
    public string FincaId { get; set; } = string.Empty;
    
    [Column("nombre")]
    public string Nombre { get; set; } = string.Empty;
    
    [Column("codigo")]
    public string? Codigo { get; set; }
    
    [Column("area_hectareas")]
    public decimal? AreaHectareas { get; set; }
    
    [Column("tipo_cultivo")]
    public string? TipoCultivo { get; set; }
    
    [Column("variedad_cultivo")]
    public string? VariedadCultivo { get; set; }
    
    [Column("fecha_siembra")]
    public DateTime? FechaSiembra { get; set; }
    
    [Column("fecha_cosecha_estimada")]
    public DateTime? FechaCosechaEstimada { get; set; }
    
    [Column("estado_cultivo")]
    public string? EstadoCultivo { get; set; }
    
    [Column("descripcion")]
    public string? Descripcion { get; set; }
    
    [Column("coordenadas_poligono")]
    public string? CoordenadasPoligono { get; set; }
    
    [Column("activo")]
    public bool Activo { get; set; } = true;
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }
    
    // Propiedades de navegación
    public Finca? Finca { get; set; }
    public List<Dispositivo> Dispositivos { get; set; } = new();
    
    // Propiedades calculadas
    public string EstadoCultivoDisplay => EstadoCultivo switch
    {
        "preparacion" => "Preparación",
        "sembrado" => "Sembrado",
        "crecimiento" => "Crecimiento",
        "floracion" => "Floración",
        "cosecha" => "Cosecha",
        _ => EstadoCultivo ?? "N/A"
    };
    
    public int? DiasParaCosecha
    {
        get
        {
            if (FechaCosechaEstimada.HasValue)
            {
                return (FechaCosechaEstimada.Value - DateTime.Now).Days;
            }
            return null;
        }
    }
}
