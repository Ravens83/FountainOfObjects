namespace FountainOfObjectsClassLib;

//EQUIPMENT TYPES:
public interface IEquipment
{
    string Name {get;}
    string Description {get;}
}

public class Arrow : IEquipment
{
    public string Name {get;} = "Arrow";
    public string Description {get;} = "An arrow for your bow.";
}

public class GlassSword : IEquipment
{
    public string Name {get;} = "Glass Sword";
    public string Description {get;} = "The Glass Sword let's you survive an encounter with an Amarok, "
                + "but breaks as you kill the beast.";
}

public class WindShieldCharm : IEquipment
{
    public string Name {get;} = "Wind Shield Charm";
    public string Description {get;} = "The Wind Shield Charm makes you immune to the effects of Mealstroms."
                    + " But breaks on use.";
}
public class GrapplingHook : IEquipment
{
    public string Name {get;} = "Grappling Hook";
    public string Description {get;} = "The Grappling Hook let's you traverse a pit to get to the other side."
                    + " But the rope is old and rotten and will only work once.";
}