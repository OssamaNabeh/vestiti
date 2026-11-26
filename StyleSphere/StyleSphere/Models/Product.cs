using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StyleSphere.Models;

public class Product
{
    [Key]
    public int ProductID { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Stock must be 0 or greater")]
    public int Stock { get; set; }

    [StringLength(255)]
    public string? Image { get; set; }

    [Required]
    public int CategoryID { get; set; }

    // Navigation property
    [ForeignKey("CategoryID")]
    public virtual Category? Category { get; set; }
}

