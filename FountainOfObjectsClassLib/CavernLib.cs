namespace FountainOfObjectsClassLib;

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
