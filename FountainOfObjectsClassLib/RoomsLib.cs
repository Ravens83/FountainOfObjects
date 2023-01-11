namespace FountainOfObjectsClassLib;

//ROOM TYPES:
public interface IRoom
{
    public string Sense();
    public string SetPlayerEffect(PlayerChar player);
    public IRoom RoomAltered(Location loc, ListOfCommands.C action);

    public bool SpecialEventRoom {get;}
}

public class EmptyRoom : IRoom
{
    public string Sense() => "Empty";
    public string SetPlayerEffect(PlayerChar p) => "Empty";
    public IRoom RoomAltered(Location loc, ListOfCommands.C action) => this;
    public bool SpecialEventRoom {get;} = false;
}

public class Enterance : IRoom
{
    public string Sense() => "Empty";
    public string SetPlayerEffect(PlayerChar p) => "You see light coming from the cavern entrance.";
    public IRoom RoomAltered(Location loc, ListOfCommands.C action) => this;
    public bool SpecialEventRoom {get;} = false;
}

public class FountainRoomInactive : IRoom
{
    public string Sense() => "Empty";
    public string SetPlayerEffect(PlayerChar p) => "You hear water dripping in this room. "+
                                    "The Fountain of Objects is here!";
    public IRoom RoomAltered(Location loc, ListOfCommands.C action)
    {
        if(action == ListOfCommands.C.enable_fountain) return new FountainRoomActive();
        else return this;
    }
    public bool SpecialEventRoom {get;} = false;
}

public class FountainRoomActive : IRoom
{
    public string Sense() => "Empty";
    public string SetPlayerEffect(PlayerChar p) => "You hear the rushing water from the Fountain of Objects. "+
                                    "It has been reactivated!";
    public IRoom RoomAltered(Location loc, ListOfCommands.C action) => this; 

    public bool SpecialEventRoom {get;} = false;
}

public class PitRoom : IRoom
{
    public string Sense() => "You feel a draft. There is a pit in a nearby room";
    public string SetPlayerEffect(PlayerChar p)
    {
        p.Alive = false;
        return "You fall into a bottomless pit.";
    }
    public IRoom RoomAltered(Location loc, ListOfCommands.C action) => this;

    public bool SpecialEventRoom {get;} = false;
}

public class Maelstrom : IRoom
{
    public string Sense() => "You hear the growling and groaning of a maelstrom nearby.";
    public string SetPlayerEffect(PlayerChar p)
    {
        p.Loc = new Location(p.Loc.X-1,p.Loc.Y+2);
        return "The Maelstrom pushes you around.";
    }
    public IRoom RoomAltered(Location loc, ListOfCommands.C action) => new EmptyRoom();

    public bool SpecialEventRoom {get;} = true;
}

public class Amarok : IRoom
{
    public string Sense() => "You can smell the rotten stench of an Amarok in a nearby room.";
    public string SetPlayerEffect(PlayerChar p)
    {
        p.Alive = false;
        return "As you blindly stumble into the room of an Amarok it neatle tears off you head.";
    }
    public IRoom RoomAltered(Location loc, ListOfCommands.C action)  => new EmptyRoom();

    public bool SpecialEventRoom {get;} = false;
}
