namespace FountainOfObjectsClassLib;

//ROOM TYPES:
public interface IRoom
{
    public string Sense();
    public string RoomMessage();
    public string SetPlayerEffect(PlayerChar p);
    public IRoom RoomAltered(ListOfCommands.C action);

    public bool SpecialEventRoom {get;} //can alter something in the cavern when player enters it
}

public class EmptyRoom : IRoom
{
    public string Sense() => "Empty";
    public string RoomMessage() => "Empty";
    public string SetPlayerEffect(PlayerChar p) => "Empty";
    public IRoom RoomAltered(ListOfCommands.C action) => this;
    public bool SpecialEventRoom {get;} = false;
}

public class Enterance : IRoom
{
    public string Sense() => "Empty";
    public string RoomMessage() => "Empty";
    public string SetPlayerEffect(PlayerChar p) => "You see light coming from the cavern entrance.";
    public IRoom RoomAltered(ListOfCommands.C action) => this;
    public bool SpecialEventRoom {get;} = false;
}

public class FountainRoomInactive : IRoom
{
    public string Sense() => "Empty";
    public string RoomMessage() => "You hear water dripping in this room. "+
                                    "The Fountain of Objects is here!";
    public string SetPlayerEffect(PlayerChar p) => "Empty";
    public IRoom RoomAltered(ListOfCommands.C action)
    {
        if(action == ListOfCommands.C.enable_fountain) return new FountainRoomActive();
        else return this;
    }
    public bool SpecialEventRoom {get;} = false;
}

public class FountainRoomActive : IRoom
{
    public string Sense() => "Empty";
    public string RoomMessage() => "You hear the rushing water from the Fountain of Objects. "+
                                    "It has been reactivated!";
    public string SetPlayerEffect(PlayerChar p) => "Empty";
    public IRoom RoomAltered(ListOfCommands.C action) => this; 

    public bool SpecialEventRoom {get;} = false;
}

public class PitRoom : IRoom
{
    public string Sense() => "You feel a draft. There is a pit in a nearby room";
    public string RoomMessage() => "Somehow you are alive in a Pit room. Better move in a direction before whatever holds you breaks.";
    public string SetPlayerEffect(PlayerChar p)
    {
        if(Toolbox.UseAnItem(p, new GrapplingHook()))
        {
            return "Your Grappling hook helps you cross a deadly pit. But it is too damaged to be used again.";
        }
        else
        {
            p.Alive = false;
            return "You fall into a bottomless pit.";
        }
    }
    public IRoom RoomAltered(ListOfCommands.C action) => this;

    public bool SpecialEventRoom {get;} = false;
}

public class Maelstrom : IRoom
{
    public string Sense() => "You hear the growling and groaning of a maelstrom nearby.";
    public string RoomMessage() => "Epmty";//"By some magic you are able to be in the same room as a Maelstrom. "
           // +"The Maelstrom flees in frustation!";
    public string SetPlayerEffect(PlayerChar p)
    {
        if(Toolbox.UseAnItem(p, new WindShieldCharm()))
        {
            return "Your Wind Shield Charm protects you from the Maelstrom. The Maelstrom goes away to another "
                    +"room in frustation just moments before your charm breaks.";
        }
        else
        {
            p.Loc = new Location(p.Loc.X-1,p.Loc.Y+2);
            return "The Maelstrom pushes you around.";
        }
    }
    public IRoom RoomAltered(ListOfCommands.C action) => new EmptyRoom();

    public bool SpecialEventRoom {get;} = true;
}

public class Amarok : IRoom
{
    public string Sense() => "You can smell the rotten stench of an Amarok in a nearby room.";
    public string RoomMessage() => "Empty";
    public string SetPlayerEffect(PlayerChar p)
    {
        if(Toolbox.UseAnItem(p, new GlassSword()))
        {
            return "You wildly swing your Glass Sword when you feel the presense of a monster. The Amarok dies gurling "
                +"as by a lucky stroke your weapon shatters in it's throat";
        }
        else
        {
            p.Alive = false;
            return "As you blindly stumble into the room of an Amarok it neatle tears off you head.";
        }
    }
    public IRoom RoomAltered(ListOfCommands.C action)  => new EmptyRoom();

    public bool SpecialEventRoom {get;} = true;
}

public class ItemRoom : IRoom
{
    IEquipment item {get;}
    public string Sense() => "Empty";
    public string RoomMessage() => "Empty";
    public string SetPlayerEffect(PlayerChar p)
    {
        p.Equipment.Add(item);
        return $"You have found a {item.Name}!";
    }
    public IRoom RoomAltered(ListOfCommands.C action)  => new EmptyRoom();

    public bool SpecialEventRoom {get;} = true;

    public ItemRoom()
    {
        item = new Arrow();
    }
    public ItemRoom(IEquipment inItem)
    {
        item = inItem;
    }
}

