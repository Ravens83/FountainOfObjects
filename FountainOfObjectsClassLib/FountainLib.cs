namespace FountainOfObjectsClassLib;


public class FountainGame
{
    private PlayerChar _player = new PlayerChar();
    private Cavern _cavern = new Cavern();
    private FountainUI _ui = new FountainUI();
    private Random rnd = new Random();
    
    public FountainGame()
    {
        _cavern = new Cavern(Cavern.CavernSizes.small);
    }

    public void RunGame()
    {
        while(true)
        {
            _ui.InitCodex();
            _ui.Codex.CodexStory("Write 'begin' to start the game: ");
            _cavern = new Cavern(_ui.GetPlayerSizeChoice());
            string deathMessage = "Empty";
            //_cavern.GenerateTestMap();
            _cavern.GenerateMap();

            do{
                deathMessage = PlayOneRound();
                
            }while(!_cavern.Win(_player) && _player.Alive);

            if(!_player.Alive)
            { 
                
                _ui.LossPrint(deathMessage);
            }
            else _ui.VictoryPrint();

            if(!_ui.playAgain()) return;
        }

    }

    private string PlayOneRound()
    {
        _cavern.FixPlayerLocation(_player); //all rounds start with a legal location

        _ui.PrintLocation(_player);

        List<string> sensations = _cavern.SenseNearbyRooms(_player);

        _ui.PrintSensations(sensations);//prints current room and nearby sensations.
        string result = "Empty";
        ListOfCommands.C playerCommand;

        do{
            playerCommand = _ui.GetPlayerCommand();
            result = PlayerAction(playerCommand); //non-action commands will not move on to further the game
        }while(playerCommand == ListOfCommands.C.codex
                || playerCommand == ListOfCommands.C.show_equipment
                || playerCommand == ListOfCommands.C.help_menu); //checks for non-action commands
        
        _ui.PrintAction(result);

        _cavern.FixPlayerLocation(_player); //check if location still legal;
        Location rememberLoc = new Location(_player.Loc.X,_player.Loc.Y);
        IRoom curRoom = _cavern.GetRoom(_player.Loc);
        string roomMsg = curRoom.SetPlayerEffect(_player);
        do{
            if(curRoom.SpecialEventRoom)    //FIX NEEDED - inf loop if the room keeps being a SpecialEvent Room
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

    private void DoSpecialRoomAction(IRoom curRoom, Location roomLoc) //must be expanded with any new special room
                                                                     //room must become a non-special room at some point
                                                                    //to avoid inf loop - FIX NEEDED
    {
        IRoom emptyRoom = new EmptyRoom();
        IRoom specialRoomMaelstrom = new Maelstrom();
        IRoom SpecialRoomItemRoom = new ItemRoom();
        IRoom SpecialRoomAmarok = new Amarok();

        if(curRoom.GetType() == specialRoomMaelstrom.GetType())
        {
            Location newLoc = new Location(rnd.Next(0,_cavern.IntCSize),rnd.Next(0,_cavern.IntCSize));
            if(_cavern.GetRoom(newLoc).GetType() == emptyRoom.GetType())
                _cavern.SetRoom(specialRoomMaelstrom,newLoc);
            
            _cavern.AlterARoom(roomLoc,ListOfCommands.C.invalid_command);
        }

        if(curRoom.GetType() == SpecialRoomItemRoom.GetType())
        {
            _cavern.AlterARoom(roomLoc, ListOfCommands.C.invalid_command);
        }

        if(curRoom.GetType() == SpecialRoomAmarok.GetType())
        {
            _cavern.AlterARoom(roomLoc,ListOfCommands.C.invalid_command);
        }
    }

    public string PlayerAction(ListOfCommands.C pCommand)
    {
        string output = "Empty";
        Location tempLoc = new Location(_player.Loc.X,_player.Loc.Y);
        switch (pCommand)
        {
            case ListOfCommands.C.move_east:
                tempLoc = new Location(_player.Loc.X,_player.Loc.Y+1);
                output = _cavern.TryToMove(_player,tempLoc);
                break;
            case ListOfCommands.C.move_west:
                tempLoc = new Location(_player.Loc.X,_player.Loc.Y-1);
                output = _cavern.TryToMove(_player,tempLoc);
                break;
            case ListOfCommands.C.move_north:
                tempLoc = new Location(_player.Loc.X-1,_player.Loc.Y);
                output = _cavern.TryToMove(_player,tempLoc);
                break;
            case ListOfCommands.C.move_south:
                tempLoc = new Location(_player.Loc.X+1,_player.Loc.Y);
                output = _cavern.TryToMove(_player,tempLoc);
                break;
            case ListOfCommands.C.shoot_east:
                tempLoc = new Location(_player.Loc.X,_player.Loc.Y+1);
                output = _cavern.ShootBow(tempLoc, _player, pCommand);
                break;
            case ListOfCommands.C.shoot_west:
                tempLoc = new Location(_player.Loc.X,_player.Loc.Y-1);
                output = _cavern.ShootBow(tempLoc, _player, pCommand);
                break;
            case ListOfCommands.C.shoot_north:
                tempLoc = new Location(_player.Loc.X-1,_player.Loc.Y);
                output = _cavern.ShootBow(tempLoc, _player, pCommand);
                break;
            case ListOfCommands.C.shoot_south:
                tempLoc = new Location(_player.Loc.X+1,_player.Loc.Y);
                output = _cavern.ShootBow(tempLoc, _player, pCommand);
                break;
            case ListOfCommands.C.enable_fountain:
                output = _cavern.AlterARoom(_player.Loc,pCommand);
                break;
            case ListOfCommands.C.show_equipment:
                _ui.ShowInventory(_player);
                break;
            case ListOfCommands.C.help_menu:
                _ui.PrintControls();
                break;
            case ListOfCommands.C.codex:
                _ui.Codex.CodexMenu();
                break;
        }
        return output;
    }

}