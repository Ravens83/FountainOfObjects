namespace FountainOfObjectsClassLib;

//the game codex. part of the text ui. explains all aspects of the game.

public class GameCodex
{
    private List<IEquipment> allEquipment = new List<IEquipment>();
    private List<IRoom> allRooms = new List<IRoom>();

    public void InitialiseLists() //need to manually put all rooms and equipment that you want in the codex into the lists at the moment
    {
        allEquipment = new List<IEquipment>();
        allRooms = new List<IRoom>();

        allEquipment.Add(new Arrow());
        allEquipment.Add(new GlassSword());
        allEquipment.Add(new WindShieldCharm());
        allEquipment.Add(new GrapplingHook());
        
        allRooms.Add(new EmptyRoom());
        allRooms.Add(new Enterance());
        allRooms.Add(new FountainRoomInactive());
        allRooms.Add(new FountainRoomActive());
        allRooms.Add(new PitRoom());
        allRooms.Add(new Maelstrom());
        allRooms.Add(new Amarok());
        allRooms.Add(new ItemRoom());
    }

    public void CodexMenu()
    {
        string command = "";
        while(command != "exit")
        {
            Console.WriteLine("Welcome To The Game Codex");
            Console.WriteLine("The following subjects are available:");
            Console.WriteLine("     Story");
            Console.WriteLine("     Gameplay");
            Console.WriteLine("     Rooms");
            Console.WriteLine("     Equipment");
            command = Toolbox.ReadString("Type the subject you are interested in or 'exit' the leave the codex: ");

            switch (command.ToLower().Trim())
            {
                case "story":
                    CodexStory("Empty");
                    break;
                case "gameplay":
                    CodexGameplay();
                    break;
                case "rooms":
                    CodexRooms();
                    break;
                case "equipment":
                    CodexEquipment();
                    break;
                default:
                    break;
            }
        }
    }

    public void CodexStory(string inPrompt)
    {
        string prompt;
        if(inPrompt == "Empty" || inPrompt == "") prompt = "Type 'exit' to return to the codex menu.";
        else prompt = inPrompt;
        string command = "";
        while(command != "exit" && command != "begin")
        {
            Console.WriteLine(@"
            The Story:
            The Fountain of Object. Yes. That is what you are here for isn't it. Well why else would
            you come to this horrible forsaken place where so many have died gruesome deaths. You can
            still smell it can't you? The smell of old death. Noone dares to come here anymore.
            But the legend of the Fountain has brought you here. The need must be great indeed.
            Legend has it that the Fountain has the power to bring life and creativity back to the lands.
            
            And indeed this was not always such a dark forboding place. The cave used to be beautiful and
            glowing with crystals. Walking here used to feel like you were walking through the skyes on
            a starry night.
            No longer. Not since the Uncoded One cursed the cave and shut off the Fountain.
            Since then it has been a deathtrap of deep pits and horrible monsters. And no light can
            penetrate the darkness of this cave anymore. You tried to bring a torch. But it's light
            was extinguished as soon as you stepped into the cave.
            
            You will have to move forward in the darkness surviving without the use of sight.
            ");
            command = Toolbox.ReadString(prompt);
        }
    }
    public void CodexGameplay()
    {
        string command = "";
        while(command != "exit")
        {
            Console.WriteLine(@"
            The Gameplay:
            The premise is quite simple. You are in a dark cave with no light-sources.
            So you have to rely on your senses of smell and hearing and feeling.
            Your goal is to find the 'Fountain of Objects' and re-activate it. Then escape
            from the cave alive by moving back to the enterance.

            At the start of every turn you will be informed of any dangerous sensations from
            the eigth rooms adjecent to the one that you are in. Be wise and listen to them
            any wrong step could be your last.

            You have the command options to move into rooms in four directions. Or to shoot
            your bow into rooms in four directions. That is, if you have any arrows.
            You might get lucky and find useful items. But most likely you will simply die.
            Like the others who ventured into the cave.
            ");
            command = Toolbox.ReadString("Type 'exit' to return to the codex menu.");
        }
    }
    public void CodexRooms()
    {
        string command = "";
        while(command != "exit")
        {
            Console.WriteLine("This is the codex Rooms menu. The cave is divided into rooms and in any of these rooms "+
                    "could be dangers, treasure or your goal; the fountain! \nThe following rooms are in the game:");
            foreach(IRoom roomy in allRooms)
            {
                Console.WriteLine(roomy.Name);
            }
            command = Toolbox.ReadString("Type the name of the room you want to know more about or type 'exit' to leave:");

            Console.WriteLine();
            foreach(IRoom roomy in allRooms)
            {
                if(roomy.Name.ToLower() == command.ToLower()) Console.WriteLine(roomy.Description);
            }
            command = Toolbox.ReadString("exit or continue reading?");
        }
    }
    public void CodexEquipment()
    {
        string command = "";
        while(command != "exit")
        {
            Console.WriteLine("This is the codex Equipment menu. The following items are in the game:");
            foreach(IEquipment eqqy in allEquipment)
            {
                Console.WriteLine(eqqy.Name);
            }
            command = Toolbox.ReadString("Type the name of the item you want to know more about or type 'exit' to leave:");

            Console.WriteLine();
            foreach(IEquipment eqqy in allEquipment)
            {
                if(eqqy.Name.ToLower() == command.ToLower()) Console.WriteLine(eqqy.Description);
            }
            command = Toolbox.ReadString("exit or continue reading?");
        }
    }
}