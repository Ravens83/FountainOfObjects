namespace FountainOfObjectsClassLib;

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
        show_equipment,
        help_menu,
        codex,
        invalid_command}

    public static C CommandMatcher(string input)
    {
        input = input.ToLower().Trim();
        string tmpstring;
        C curC;

        foreach(var commandnr in Enum.GetValues<C>())
        {
            curC = (C)commandnr;
            tmpstring = CommandMatcher(curC);
            if(input == tmpstring) return curC;
        }
        return C.invalid_command;
    }

    /*public static C CommandMatcher(string input)
    {
        if(input.ToLower().Trim() == "move east") return C.move_east;
        if(input.ToLower().Trim() == "move west") return C.move_west;
        if(input.ToLower().Trim() == "move north") return C.move_north;
        if(input.ToLower().Trim() == "move south") return C.move_south;
        if(input.ToLower().Trim() == "shoot east") return C.shoot_east;
        if(input.ToLower().Trim() == "shoot west") return C.shoot_west;
        if(input.ToLower().Trim() == "shoot north") return C.shoot_north;
        if(input.ToLower().Trim() == "shoot south") return C.shoot_south;
        if(input.ToLower().Trim() == "enable fountain") return C.enable_fountain;
        if(input.ToLower().Trim() == "show equipment") return C.show_equipment;
        if(input.ToLower().Trim() == "help") return C.help_menu;
        if(input.ToLower().Trim() == "codex") return C.codex;
        else return C.invalid_command;
    }*/

    

    public static string CommandMatcher(C input)
    {
        string output;

        output = input switch
        {
            C.move_east => "move east",
            C.move_west => "move west",
            C.move_north => "move north",
            C.move_south => "move south",
            C.shoot_east => "shoot east",
            C.shoot_west => "shoot west",
            C.shoot_north => "shoot north",
            C.shoot_south => "shoot south",
            C.enable_fountain => "enable fountain",
            C.show_equipment => "show equipment",
            C.help_menu => "help",
            C.codex => "codex",
            _ => ""       
        };

        return output;
    }

    public static string CommandExplanations(C command)
    {
        string output;
        output = command switch
        {
            C.move_east => "Will attempt to walk one room east. Increasing the column indicator 1.",
            C.move_west => "Will attempt to walk one room west. Decreasing the column indicator 1.",
            C.move_north => "Will attempt to walk one room north. Decreasing the row indicator 1.",
            C.move_south => "Will attempt to walk one room south. Increasing the column indicator 1.",
            C.shoot_east => "Will shoot an arrow into the room east of you, killing any potential beast in there.",
            C.shoot_west => "Will shoot an arrow into the room west of you, killing any potential beast in there.",
            C.shoot_north => "Will shoot an arrow into the room north of you, killing any potential beast in there.",
            C.shoot_south => "Will shoot an arrow into the room south of you, killing any potential beast in there.",
            C.enable_fountain => "Activates the fountain. Will only work if you are in the fountain room.",
            C.show_equipment => "Opens the inventory menu and shows you all the items you are carrying.",
            C.help_menu => "Displayes this list of explanations.",
            C.codex => "Opens up the game Codex with information about gameplay, monsters and items.",
            _ => ""
        };

        return output;
    }
}