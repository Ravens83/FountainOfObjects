using FountainOfObjectsClassLib;

FountainGame game = new FountainGame();

game.RunGame();

/*
string shoote = "shoot east";

shoote = shoote.ToLower().Trim();
string tmpstring;
ListOfCommands.C curC;

foreach(var commandnr in Enum.GetValues<ListOfCommands.C>())
{
    curC = (ListOfCommands.C)commandnr;
    tmpstring = ListOfCommands.CommandMatcher(curC);
    if(shoote == tmpstring) Console.WriteLine(ListOfCommands.CommandExplanations(curC));
}*/