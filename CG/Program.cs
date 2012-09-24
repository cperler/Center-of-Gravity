using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CG
{
    class Program
    {
        static Hashtable _current = new Hashtable();

        static void Main(string[] args)
        {
            Vehicle mVehicle = new Vehicle("Seesaw", 0, 0, 0, 0, 0, 0, 300);

            Location[] mLocations = new Location[2];
            mLocations[0] = new Location("P1", 300, .5);
            mLocations[0].AddPosition(2, 0);
            mLocations[1] = new Location("P2", 300, 1);
            mLocations[1].AddPosition(2, 0);
            mVehicle.AddLocations(mLocations);

            Block[] mBlocks = new Block[2];
            mBlocks[0] = new Block("N1", 150, BlockType.Person);
            mBlocks[1] = new Block("N2", 150, BlockType.Person);
            /*
            Vehicle mVehicle = new Vehicle("Dad's Plane", 2470, 208589, 6, 45, 30, .01, 3600);

            Location[] mLocations = new Location[5];
            mLocations[0] = new Location("Forward", 100, .025);
            mLocations[0].AddPosition(0, 2);
            mLocations[1] = new Location("Front", 500, .0117);
            mLocations[1].AddPosition(2, 2);
            mLocations[2] = new Location("Middle", 550, .0083);
            mLocations[2].AddPosition(2, 2);
            mLocations[2].AddPosition(0, 4);
            mLocations[3] = new Location("Rear", 410, .0065);
            mLocations[3].AddPosition(2, 2);
            mLocations[3].AddPosition(0, 4);
            mLocations[4] = new Location("Aft", 100, .0055);
            mLocations[4].AddPosition(0, 3);

            mVehicle.AddLocations(mLocations);

            Block[] mBlocks = new Block[14];
            mBlocks[0] = new Block("Dad", 157, BlockType.Person);
            mBlocks[0].LockInPosition(mLocations[1]);
            mBlocks[1] = new Block("Mom", 115, BlockType.Person);
            mBlocks[1].ExcludeLocation(mLocations[1]);
            mBlocks[2] = new Block("Craig", 133, BlockType.Person);
            mBlocks[3] = new Block("Laurel", 115, BlockType.Person);
            mBlocks[3].ExcludeLocation(mLocations[1]);
            mBlocks[4] = new Block("Justin (and his laptop)", 124, BlockType.Person);
            mBlocks[5] = new Block("Dad's Flight Bag", 11, BlockType.Luggage);
            mBlocks[5].ExcludeLocation(mLocations[0]);
            mBlocks[5].ExcludeLocation(mLocations[3]);
            mBlocks[5].ExcludeLocation(mLocations[4]);
            mBlocks[6] = new Block("Dad's Heavy Bag", 20, BlockType.Luggage);
            mBlocks[7] = new Block("Dad's Light Bag", 13, BlockType.Luggage);
            mBlocks[8] = new Block("Mom's Valpack", 50, BlockType.Luggage);
            mBlocks[8].ExcludeLocation(mLocations[0]);
            mBlocks[8].ExcludeLocation(mLocations[4]);
            mBlocks[9] = new Block("Mom's Makeup Bag", 17, BlockType.Luggage);
            mBlocks[10] = new Block("Craig's Duffel", 18, BlockType.Luggage);
            mBlocks[11] = new Block("Craig's Backpack", 7, BlockType.Luggage);
            mBlocks[11].ExcludeLocation(mLocations[0]);
            mBlocks[12] = new Block("Laurel's Duffel", 35, BlockType.Luggage);
            mBlocks[13] = new Block("Justin's Duffel", 15, BlockType.Luggage);*/

            DateTime mStart = DateTime.Now;
            DateTime mEnd = mVehicle.Solve(mBlocks);
            Console.WriteLine("(solving took this long: " + (mEnd - mStart) + ")");

            Console.WriteLine("\n\nPress Enter to continue...");
            Console.ReadLine();
        }
    }
}
