namespace TechnicoRMP.Models;

public class PropertyOwnership
{
    public long Id { get; set; }    
    public long PropertyOwnerId { get; set; }   
    public long PropertyItemId { get; set; }
    public User? PropertyOwner { get; set; }
    public PropertyItem? PropertyItem { get; set; }

}
