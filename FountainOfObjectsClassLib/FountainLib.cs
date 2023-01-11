namespace FountainOfObjectsClassLib;


public class PlayerChar
{
    public Location Loc {get; set;}
    public bool Alive {get; set;} = true;
    public List<IEquipment> Equipment = new List<IEquipment>();

    private const int StandardStartArrows = 5;

    public PlayerChar()
    {
        for(int i = 0; i < StandardStartArrows; i++)
        {
            Equipment.Add(new Arrow());
        }
    }
}

public class FountainUI
{   public void LossPrint(string deathMessage)
    {
        Console.WriteLine(deathMessage);
        Console.WriteLine("You seem to have gone and died. Game Over.");
    }

    public void VictoryPrint()
    {
        Console.WriteLine("You successfully reactivated the Fountain of Objects and escaped " +
                            "the cavern alive!");
        Console.WriteLine("You Win!");
    }

    public void PrintRoomEffect(string roomMsg)
    {
        Console.WriteLine(roomMsg);
    }

    public ListOfCommands.C GetPlayerCommand()
    {
        string input;
        ListOfCommands.C output = ListOfCommands.C.invalid_command;
        bool match = false;
        while(!match)
        {
            input = Toolbox.ReadString("What do you want to do? ");
            output = ListOfCommands.CommandMatcher(input);
            if(output != ListOfCommands.C.invalid_command) match = true;
        }
        return output;
    }

    public void PrintSensations(List<string> sensations)
    {
        if(sensations[0] != null)
        {
            foreach(string i in sensations)
            {
                if(i != "Empty") Console.WriteLine(i);
            }
        }
    }

    public void PrintLocation(PlayerChar p)
    {
        Console.WriteLine("-----------------------------------------------------------");
        Console.WriteLine($"You are in the room at (Row={p.Loc.X}, Column={p.Loc.Y}).");
    }
}



public class FountainGame
{
    public PlayerChar _player = new PlayerChar();
    public Cavern _cavern = new Cavern();
    public FountainUI _ui = new FountainUI();

    public FountainGame(Cavern.CavernSizes size)
    {
        _cavern = new Cavern(size);
    }

    public void RunGame()
    {
        string deathMessage = "Empty";
        _cavern.GenerateTestMap();

        do{
            deathMessage = PlayOneRound();
            
        }while(!_cavern.Win(_player) && _player.Alive);

        if(!_player.Alive)
        { 
            
            _ui.LossPrint(deathMessage);
        }
        else _ui.VictoryPrint();

    }

    private string PlayOneRound()
    {
        _cavern.FixPlayerLocation(_player); //all rounds start with a legal location

        _ui.PrintLocation(_player);

        List<string> sensations = _cavern.SenseNearbyRooms(_player);

        _ui.PrintSensations(sensations);//prints current room and nearby sensations.
        string result = "Empty";

        ListOfCommands.C playerCommand = _ui.GetPlayerCommand(); //player does action:
        result = _cavern.PlayerAction(_player, playerCommand);


        _cavern.FixPlayerLocation(_player); //check if location still legal;
        Location rememberLoc = new Location(_player.Loc.X,_player.Loc.Y);
        IRoom curRoom = _cavern.GetRoom(_player.Loc);
        string roomMsg = curRoom.SetPlayerEffect(_player);
        do{
            if(curRoom.SpecialEventRoom)
            {
                _cavern.FixPlayerLocation(_player);
                _ui.PrintRoomEffect(roomMsg);
                DoSpecialRoomAction(curRoom,rememberLoc);
                rememberLoc = new Location(_player.Loc.X,_player.Loc.Y);
                curRoom = _cavern.GetRoom(_player.Loc);
                roomMsg = curRoom.SetPlayerEffect(_player);
            }
        }while(curRoom.SpecialEventRoom);

        return roomMsg;
    }

    public void DoSpecialRoomAction(IRoom curRoom, Location roomLoc)
    {
        IRoom emptyRoom = new EmptyRoom();
        IRoom specialRoom1 = new Maelstrom();

        if(curRoom.GetType() == specialRoom1.GetType())
        {
            Location newLoc = new Location(roomLoc.X+1,roomLoc.Y-2);
            if(_cavern.GetRoom(newLoc).GetType() == emptyRoom.GetType())
                _cavern.SetRoom(specialRoom1,newLoc);
            
            _cavern.AlterARoom(roomLoc,ListOfCommands.C.invalid_command);
        }
    }

}



public class Cavern
{
    public enum CavernSizes {small = 4, medium = 6, large = 8};
    public CavernSizes CSize {get;} = CavernSizes.small;
    public int IntCSize {get;}

    private IRoom[,] Rooms {get;}

    public Cavern()
    {
        IntCSize = (int)CSize;
        Rooms = new IRoom[IntCSize,IntCSize];
    }

    public Cavern(CavernSizes newSize)
    {
        CSize = newSize;
        IntCSize = (int)CSize;
        Rooms = new IRoom[IntCSize,IntCSize];
    }

    public void GenerateTestMap()
    {
        for(int a = 0; a < IntCSize; a++)
        {
            for(int b = 0; b < IntCSize; b++)
            {
                Rooms[a,b] = new EmptyRoom();
            }
        }

        Rooms[0,0] = new Enterance();               //IMPORTANT: nterance must always be at 0,0 as it is now
        Rooms[2,3] = new FountainRoomInactive();
        Rooms[2,0] = new PitRoom();
        Rooms[0,3] = new Maelstrom();
    }

    public IRoom GetRoom(Location loc)
    {
        if(LegalLocation(loc)) return Rooms[loc.X,loc.Y];
        else return Rooms[0,0];
    }

    public bool SetRoom(IRoom room, Location loc)
    {
        if(LegalLocation(loc))
        {
            Rooms[loc.X,loc.Y] = room;
            return true;
        }
        return false;
    }

    public void FixPlayerLocation(PlayerChar player)
    {
        int newX = FixOneDirection(player.Loc.X);
        int newY = FixOneDirection(player.Loc.Y);
        player.Loc = new Location(newX,newY);
    }

    public int FixOneDirection(int pos)
    {
        if(pos < 0 ) return 0;
        else if (pos >= IntCSize) return IntCSize-1;
        else return pos;
    }

    public bool LegalLocation(Location loc)
    {
        if(loc.X >= 0 && loc.X < IntCSize)
        {
            if(loc.Y >= 0 && loc.Y < IntCSize) return true;
        }
        return false;
    }

    public List<String> SenseNearbyRooms(PlayerChar player)
    {
        int roomx;
        int roomy;
        Location roomloc;
        List<string> output = new List<string>();

        output.Add(GetRoom(player.Loc).SetPlayerEffect(player));

        for(roomx = player.Loc.X-1; roomx <= player.Loc.X+1; roomx++)
        {
            for(roomy = player.Loc.Y-1; roomy <= player.Loc.Y+1; roomy++)
            {
                roomloc = new Location(roomx,roomy);
                if(LegalLocation(roomloc))
                {
                    if(!roomloc.Equals(player.Loc))
                    output.Add(GetRoom(roomloc).Sense());
                }
            }
        }
        return output;
    }

    public bool Win(PlayerChar p)
    {
        Location enterance = FindRoom(new Enterance());
        if(enterance.Equals(p.Loc))
        {
            if(!enterance.Equals(FindRoom(new FountainRoomActive()))) return true;
        }
        return false;
    }

    public Location FindRoom(IRoom room) //finds the first room of specified type
    {
        Location output = new Location();
        for(int a = 0; a < IntCSize; a++)
        {
            for(int b = 0; b < IntCSize; b++)
            {
                if(Rooms[a,b].GetType() == room.GetType())
                {
                    output = new Location(a,b);
                    return output;
                }
            }
        }
        return output;
    }

    public string PlayerAction(PlayerChar p, ListOfCommands.C pCommand)
    {
        string output = "Empty";
        switch (pCommand)
        {
            case ListOfCommands.C.move_east:
                output = TryToMove(p,p.Loc.X,p.Loc.Y+1);
                break;
            case ListOfCommands.C.move_west:
                output = TryToMove(p,p.Loc.X,p.Loc.Y-1);
                break;
            case ListOfCommands.C.move_north:
                output = TryToMove(p,p.Loc.X-1,p.Loc.Y);
                break;
            case ListOfCommands.C.move_south:
                output = TryToMove(p,p.Loc.X+1,p.Loc.Y);
                break;
            case ListOfCommands.C.shoot_east:
                break;
            case ListOfCommands.C.shoot_west:
            case ListOfCommands.C.shoot_north:
            case ListOfCommands.C.shoot_south:
                break;
            case ListOfCommands.C.enable_fountain:
                output = AlterARoom(p.Loc,pCommand);
                break;

        }
        return output;
    }

    public string ShootBow(Location loc, ListOfCommands.C pCommand)
    {
        string output = "";
        if(LegalLocation(loc))
        {
            output = AlterARoom(loc,pCommand);
        }

        return output;
    }

    public string TryToMove(PlayerChar p, int X, int Y)
    {
        Location newLocation = new Location(X,Y);
        if(LegalLocation(newLocation))
        {
            p.Loc = newLocation;
            return "Move sucess";
        }
        else return "Empty";
    }

    public string AlterARoom(Location loc, ListOfCommands.C pCommand)
    {
        var oldRoomType = Rooms[loc.X,loc.Y].GetType();
        IRoom updatedRoom = Rooms[loc.X,loc.Y].RoomAltered(loc,pCommand);
        Rooms[loc.X,loc.Y] = updatedRoom;

        if(oldRoomType == updatedRoom.GetType()) return "Empty";
        else return "Room Changed";

    }

    /*public bool UpdateARoom(Location loc)
    {
        if(LegalLocation(loc))
        {
            IRoom tmp = 
        }
    }*/

    public override string ToString()
    {
        return "CSize: " + CSize + " IntCSize: " + IntCSize;
    }
}

public struct Location
{
    public readonly int X,Y;

    public Location(int newX, int newY)
    {
        X = newX; Y = newY;
    }
}


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

//COMMANDS:
public static class ListOfCommands
{
    public enum C {
        move_east,
        move_west,
        move_north,
        move_south,
        shoot_east,
        shoot_west,
        shoot_north,
        shoot_south,
        enable_fountain,
        invalid_command}

    public static C CommandMatcher(string input)
    {
        if(input == "move east") return C.move_east;
        if(input == "move west") return C.move_west;
        if(input == "move north") return C.move_north;
        if(input == "move south") return C.move_south;
        if(input == "shoot east") return C.shoot_east;
        if(input == "shoot west") return C.shoot_west;
        if(input == "shoot north") return C.shoot_north;
        if(input == "shoot south") return C.shoot_south;
        if(input == "enable fountain") return C.enable_fountain;
        else return C.invalid_command;
    }
}

//toolbox

public static class Toolbox
{
    public static string ReadString(string prompt) //prevents empty/null string inputs. But not space input.
    {
	    string result;
	    do
	    {
		    Console.Write(prompt);
		    result = Console.ReadLine();
	    } while (result == "" || result == null);
	    return result;
    }
    public static int ReadInt(string prompt, int low, int high) //prompt is the messages sent to the user
    {
	    int result;
	
        do
        {
            string intString = ReadString(prompt); //Use readString method to check against empty string
            if(!Int32.TryParse(intString, out result))
                continue;	//on Parse-failure try over
        } while ((result < low) || (result > high));
        return result;
    }
}