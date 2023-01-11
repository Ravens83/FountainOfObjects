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