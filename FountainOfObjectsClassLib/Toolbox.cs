namespace FountainOfObjectsClassLib;

//toolbox

public static class Toolbox
{
    public static string ReadString(string prompt) //prevents empty/null string inputs. But not space input.
    {
	    string result;
	    do
	    {
		    Console.Write(prompt);
		    result = Console.ReadLine();
	    } while (result == "" || result == null);
	    return result;
    }
    public static int ReadInt(string prompt, int low, int high) //prompt is the messages sent to the user
    {
	    int result;
	
        do
        {
            string intString = ReadString(prompt); //Use readString method to check against empty string
            if(!Int32.TryParse(intString, out result))
                continue;	//on Parse-failure try over
        } while ((result < low) || (result > high));
        return result;
    }
}