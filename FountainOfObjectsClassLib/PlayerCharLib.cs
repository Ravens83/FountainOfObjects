namespace FountainOfObjectsClassLib;

public class PlayerChar
{
    public Location Loc {get; set;}
    public bool Alive {get; set;} = true;
    public List<IEquipment> Equipment = new List<IEquipment>();

    private const int StandardStartArrows = 2;

    public PlayerChar()
    {
        for(int i = 0; i < StandardStartArrows; i++)
        {
            Equipment.Add(new Arrow());
        }
    }

    public PlayerChar(int startArrows)
    {
        if(startArrows > 0 && startArrows < 25)
        {
            for(int i = 0; i < startArrows; i++)
            {
                Equipment.Add(new Arrow());
            }
        }
        else if(startArrows == 0)
        {
        }
        else
        {
            for(int i = 0; i < StandardStartArrows; i++)
            {
                Equipment.Add(new Arrow());
            }
        }

    }
}