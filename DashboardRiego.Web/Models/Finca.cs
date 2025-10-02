using Postgrest.Attributes;
using Postgrest.Models;

namespace DashboardRiego.Web.Models;

[Table("fincas")]
public class Finca : BaseModel
{
    [PrimaryKey("id")]
    public string Id { get; set; } = string.Empty;
    
    [Column("nombre")]
    public string Nombre { get; set; } = string.Empty;
    
    [Column("direccion")]
    public string? Direccion { get; set; }
    
    [Column("latitud")]
    public decimal? Latitud { get; set; }
    
    [Column("longitud")]
    public decimal? Longitud { get; set; }
    
    [Column("propietario")]
    public string? Propietario { get; set; }
    
    [Column("telefono")]
    public string? Telefono { get; set; }
    
    [Column("email")]
    public string? Email { get; set; }
    
    [Column("area_total_hectareas")]
    public decimal? AreaTotalHectareas { get; set; }
    
    [Column("ubicacion_municipio")]
    public string? UbicacionMunicipio { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}

