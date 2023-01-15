namespace FountainOfObjectsClassLib;

//ROOM TYPES:
public interface IRoom
{
    public string Name{get;}
    public string Description{get;}
    public string Sense();
    public string RoomMessage();
    public string SetPlayerEffect(PlayerChar p);
    public IRoom RoomAltered(ListOfCommands.C action);

    public bool SpecialEventRoom {get;} //can alter something in the cavern when player enters it
}

public class EmptyRoom : IRoom
{
    public string Name{get;} = "Empty Room";
    public string Description{get;} = "This is an empty room. Nothing to see here. Or anywhere else for that matter.";
    public string Sense() => "Empty";
    public string RoomMessage() => "Empty";
    public string SetPlayerEffect(PlayerChar p) => "Empty";
    public IRoom RoomAltered(ListOfCommands.C action) => this;
    public bool SpecialEventRoom {get;} = false;
}

public class Enterance : IRoom
{
    public string Name{get;} = "Enterance.";
    public string Description{get;} = "The cave enterance. The only place with any light at all.";
    public string Sense() => "Empty";
    public string RoomMessage() => "You see light coming from the cavern entrance.";
    public string SetPlayerEffect(PlayerChar p) => "You see light coming from the cavern entrance.";
    public IRoom RoomAltered(ListOfCommands.C action) => this;
    public bool SpecialEventRoom {get;} = false;
}

public class FountainRoomInactive : IRoom
{
    public string Name{get;} = "Fountain Room (inactive)";
    public string Description{get;} = "The room that contains the fountain. You will hear water drippign here. "
                +"Activate the fountain with the command \"enable fountain\"";
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
    public string Name{get;} = "Fountain Room (active)";
    public string Description{get;} = "This room appears when you enable the fountain. Go back to the enterance to win the game.";
    public string Sense() => "Empty";
    public string RoomMessage() => "You hear the rushing water from the Fountain of Objects. "+
                                    "It has been reactivated!";
    public string SetPlayerEffect(PlayerChar p) => "Empty";
    public IRoom RoomAltered(ListOfCommands.C action) => this; 

    public bool SpecialEventRoom {get;} = false;
}

public class PitRoom : IRoom
{
    public string Name{get;} = "Pit Room";
    public string Description{get;} = "A room with a deep deadly pit. Walking into this room will surely be your death. "
                                    +"Luckily you will be able to sense a draft from it from adjecent rooms.";
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
    public string Name{get;} = "Maelstrom Room";
    public string Description{get;} = "A room with a malevolent Maelstrom monster in it. Entering this room the Maelstrom"
            +" will attact you and push you into another room. The Maelstrom then moves to a different room. "
            +"While this monster doesnt kill you... the room you end up in just might."
            +" Luckily you can hear this beast from adjecent rooms.";
    public string Sense() => "You hear the growling and groaning of a maelstrom nearby.";
    public string RoomMessage() => "Epmty";
    public string SetPlayerEffect(PlayerChar p)
    {
        if(Toolbox.UseAnItem(p, new WindShieldCharm()))
        {
            return "Your Wind Shield Charm protects you from the Maelstrom. The Maelstrom goes away to another "
                    +"room in frustation just moments before your charm breaks.";
        }
        else
        {
            Random rnd = new Random();
            int rnd1 = rnd.Next(4)-1;
            int rnd2 = rnd.Next(5)-2;
            p.Loc = new Location(p.Loc.X+rnd1,p.Loc.Y+rnd2);
            return "The Maelstrom pushes you around.";
        }
    }
    public IRoom RoomAltered(ListOfCommands.C action) => new EmptyRoom();

    public bool SpecialEventRoom {get;} = true;
}

public class Amarok : IRoom
{
    public string Name{get;} = "Amarok Room";
    public string Description{get;} = "This room is home to a horrible stinking monster called an Amarok."
            +" Do not enter. The beast will kill you. Luckily you can smell it from adjecent rooms.";
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
    public string Name{get;} = "Item Room";
    public string Description{get;} = "You might get lucky and stumble into an room with a useful item for your inventory.";
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

