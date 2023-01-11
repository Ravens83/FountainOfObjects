namespace FountainOfObjectsClassLib;

public class FountainUI
{   
    public void PrintAction(string actionResult)
    {
        switch(actionResult)
        {
            case "Bowshot Room Changed":
                Console.WriteLine("Your bowshot has killed a beast in the room!");
                break;
            case "Bowshot Empty":
                Console.WriteLine("You hit nothing and the arrow was wasted.");
                break;
            case "Out of Arrows.":
                Console.WriteLine("You have run out of arrows and cannot shoot.");
                break;
            case "Empty":
            case null:
                break;
        }
    }

    public void LossPrint(string deathMessage)
    {
        if(deathMessage != "Empty") Console.WriteLine(deathMessage);
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
        if(roomMsg != "Empty") Console.WriteLine(roomMsg);
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

    public void ShowInventory(PlayerChar p)
    {
        Console.WriteLine("Your Inventory contains the following items:");
        foreach(IEquipment eqqy in p.Equipment)
        {
            Console.WriteLine(eqqy.Name);
        }
        string command = "";
        while(command != "exit")
        {
            command = Toolbox.ReadString("Would you like to \"learn\" about an item or \"exit\"?: ");
            if(command == "learn")
            PrintItemInfo(p);
        }
    }

    public void PrintItemInfo(PlayerChar p)
    {
        int itemnr = 0;
        int countnumber = p.Equipment.Count();
        itemnr = Toolbox.ReadInt($"Choose inventory item by number between 1 and {countnumber}: ", 1, countnumber);
        Console.WriteLine(p.Equipment[itemnr-1].Description);
    }

}

