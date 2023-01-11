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

public struct Location
{
    public readonly int X,Y;

    public Location(int newX, int newY)
    {
        X = newX; Y = newY;
    }
}