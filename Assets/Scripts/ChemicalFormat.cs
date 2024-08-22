using System.ComponentModel.DataAnnotations;

public class ChemicalFormat
{
    [Key]
    public int ID { get; set; }
    public string Chemical { get; set; }
    public string Formula { get; set; }
    public float MeltingPoint { get; set; }
    public float BoilingPoint { get; set; }
}