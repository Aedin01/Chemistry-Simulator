using System.ComponentModel.DataAnnotations;

public class ChemicalReactionFormat
{
    [Key]
    public int ID { get; set; }
    public string Reactant1 { get; set; }
    public string Reactant2 { get; set; }
    public string Product { get; set; }
    public float TemperatureRequired { get; set; }
    public string OtherDetails { get; set; }
}