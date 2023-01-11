namespace FountainOfObjectsClassLib;


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
        _ui.PrintAction(result);

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

    public void DoSpecialRoomAction(IRoom curRoom, Location roomLoc) //must be expanded with any new special room
    {
        IRoom emptyRoom = new EmptyRoom();
        IRoom specialRoomMaelstrom = new Maelstrom();
        IRoom SpecialRoomItemRoom = new ItemRoom();

        if(curRoom.GetType() == specialRoomMaelstrom.GetType())
        {
            Location newLoc = new Location(roomLoc.X+1,roomLoc.Y-2);
            if(_cavern.GetRoom(newLoc).GetType() == emptyRoom.GetType())
                _cavern.SetRoom(specialRoomMaelstrom,newLoc);
            
            _cavern.AlterARoom(roomLoc,ListOfCommands.C.invalid_command);
        }

        if(curRoom.GetType() == SpecialRoomItemRoom.GetType())
        {
            _cavern.AlterARoom(roomLoc, ListOfCommands.C.invalid_command);
        }
    }

}



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


public struct Location
{
    public readonly int X,Y;

    public Location(int newX, int newY)
    {
        X = newX; Y = newY;
    }
}