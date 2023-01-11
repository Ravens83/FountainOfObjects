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