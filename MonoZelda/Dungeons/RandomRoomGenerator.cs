using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoZelda.Dungeons;

public class RandomRoomGenerator
{
    private const int POOL_SIZE = 11;
    private static readonly string[] RoomPool = {"Room1","Room2","Room3","Room4","Room5","Room8",
                               "Room11","Room12","Room13","Room14","Room16"};

    private int randomRoomNum = 2; 
    private Random rnd = new Random();

    public RandomRoomGenerator()
    {
        rnd = new Random();
    }

    public string GetRoom()
    {
        randomRoomNum = rnd.Next(POOL_SIZE);
        return RoomPool[randomRoomNum];
    }
}
