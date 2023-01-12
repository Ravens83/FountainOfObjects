namespace FountainOfObjectsClassLib;

public class MapGenerator
{
    List<IRoom> roomlist = new List<IRoom>();

    Random rnd = new Random();

    public void Generate(IRoom[,] cave, Cavern.CavernSizes type) 
    {
        int intCSize = cave.GetLength(0);

        for(int a = 0; a < intCSize; a++)
        {
            for(int b = 0; b < intCSize; b++)
            {
                cave[a,b] = new EmptyRoom();
            }
        }

        cave[0,0] = new Enterance(); //must always be at this loc
        AddFountainRoom(cave,intCSize);
        
        PopulateList(intCSize, type);

        foreach(IRoom room in roomlist)
        {
            AddRoomToMap(room, cave, intCSize);
        }
    }



    public void PopulateList(int intCSize, Cavern.CavernSizes type) //add event rooms to a list based on the size of the map
    {
        //rooms to always add:  ENTERANCE EATS 3 SPOTS, SO MAXIMUM OF 16 - 3 = 13 rooms here
        roomlist.Add(new Amarok());
        roomlist.Add(new Amarok());
        roomlist.Add(new PitRoom());
        roomlist.Add(new Maelstrom());
        roomlist.Add(new ItemRoom(RandomItem()));
        roomlist.Add(new ItemRoom(RandomItem()));

        //extra rooms for larger maps:
        switch(type)
        {
            case Cavern.CavernSizes.medium: //assuming medium is set to 6x6 = 36 rooms. 13 used. Will add 13 more
                for(int i = 0; i < 13; i++) roomlist.Add(RandomRoom());
                roomlist.Add(new ItemRoom(RandomItem()));
                roomlist.Add(new ItemRoom(RandomItem()));
                roomlist.Add(new ItemRoom(RandomItem()));
                break;
            case Cavern.CavernSizes.large://assuming large is set to 8x8 = 64 rooms. 13 used. Will add 30 more
                for(int i = 0; i < 30; i++) roomlist.Add(RandomRoom());
                roomlist.Add(new ItemRoom(RandomItem()));
                roomlist.Add(new ItemRoom(RandomItem()));
                roomlist.Add(new ItemRoom(RandomItem()));
                roomlist.Add(new ItemRoom(RandomItem()));
                roomlist.Add(new ItemRoom(RandomItem()));
                break;
            default:
                break;
        }
    }

    public void AddFountainRoom(IRoom[,] cave, int intCSize)
    {
        int a = rnd.Next(2, intCSize);
        int b = rnd.Next(2, intCSize);
        cave[a,b] = new FountainRoomInactive();
    }

    public void AddRoomToMap(IRoom room, IRoom[,] cave, int intCSize)
    {
        int a;
        int b;
        IRoom testRoom = new EmptyRoom();
        do{                                                 //WARNING inf loop if no empty rooms left
            a = rnd.Next(1, intCSize);
            b = rnd.Next(1, intCSize);
        }while(cave[a,b].GetType() != testRoom.GetType());
        cave[a,b] = room;
    }

    public IRoom RandomRoom()
    {
        IRoom room;
        int high = 6;

        int number = rnd.Next(high);
        switch(number)
        {
            case 0:
                room = new Amarok();
                break;
            case 1:
                room = new PitRoom();
                break;
            case 2:
                room = new Maelstrom();
                break;
            default:
                room = new ItemRoom(RandomItem());
                break;
        }
        return room;
    }
    public IEquipment RandomItem()
    {
        IEquipment item;
        int high = 7;

        int number = rnd.Next(high);
        switch(number)
        {
            case 0:
            case 1:
                item = new GrapplingHook();
                break;
            case 2:
                item = new WindShieldCharm();
                break;
            case 3:
                item = new GlassSword();
                break;
            default:
                item = new Arrow();
                break;

        }
        return item;  
    }
}