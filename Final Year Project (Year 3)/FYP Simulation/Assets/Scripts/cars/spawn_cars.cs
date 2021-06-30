using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn_cars : MonoBehaviour
{
    //
    // variables
    //

    private List<Road> roadList = new List<Road>(); // list for road objects
    private System.Random rand = new System.Random(); // random number generator

    //
    // classes
    //

    private class Road
    {
        public float groundPos; // x position of ground
        public int roadDirection; // direction of travel
        public float nextTimeToSpawn; // next time to spawn car
        public float spawnDelayUpper; // time between car spawns 
        public float spawnDelayLower; // time between car spawns

        public Road(float groundP, int roadDir, float nextTime, float spawnDelUp, float spawnDelLow)
        {
            groundPos = groundP; // x position of ground
            roadDirection = roadDir; // direction of travel
            nextTimeToSpawn = nextTime; // next time to spawn car
            spawnDelayUpper = spawnDelUp; // time between car spawns
            spawnDelayLower = spawnDelLow; // time between car spawns
        }
    }

    //
    // methods
    //

    void Start()
    {
        for (int groundNum = 1; groundNum < 101; groundNum ++)
        {
            int direction = rand.Next(0, 2); // 0 for left, 1 for right
            float nextTimeToSpawn = Random.Range(2, 10); // set next time to spawn
            float spawnDelayUpperBound = ((float)(rand.NextDouble() * (10 - 8) + 8)); // set upper bound for spawn delay
            float spawnDelayLowerBound = ((float)(rand.NextDouble() * (5 - 3) + 3)); // set lower bound for spawn delay
            Road road = new Road(((float)-groundNum), direction, nextTimeToSpawn, spawnDelayUpperBound, spawnDelayLowerBound); // create road object
            roadList.Add(road); // add to list

            if (direction == 0) // left cars travelling right
            {
                int numCars = rand.Next(1, 3); // set number of initial cars
                int spawnPos = rand.Next(-5, 5); // set spawn point
                for (int i = 0; i < numCars; i++)
                {
                    obCreate("left_car", -groundNum, 1, spawnPos); // spawn initial car
                    spawnPos = spawnPos - rand.Next(3, 5); // change spawn point for next car
                }
            }
            else
            {
                int numCars = rand.Next(1, 3); // set number of initial cars
                int spawnPos = rand.Next(-5, 5); // set spawn point
                for (int i = 0; i < numCars; i++)
                {
                    obCreate("right_car", -groundNum, 1, spawnPos); // spawn initial car
                    spawnPos = spawnPos + rand.Next(3, 5); // change spawn point for next car
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Road road in roadList)
        {
            if (road.nextTimeToSpawn <= Time.time) // if time to spawn car
            {
                if (road.roadDirection == 0) // left cars travelling right
                {
                    obCreate("left_car", road.groundPos, 1, -10);
                    float spawnDelay = Random.Range(road.spawnDelayLower, road.spawnDelayUpper);
                    road.nextTimeToSpawn = Time.time + spawnDelay; // set next move time
                }
                else // right cars travelling left
                {
                    obCreate("right_car", road.groundPos, 1, 10);
                    float spawnDelay = Random.Range(road.spawnDelayLower, road.spawnDelayUpper);
                    road.nextTimeToSpawn = Time.time + spawnDelay; // set next move time
                }
            }
        }
    }
    
    // create game object
    private void obCreate(string resName, float vecX, float vecY, float vecZ)
    {
        GameObject obName =
                            Instantiate(Resources.Load(resName),
                            new Vector3(vecX, vecY, vecZ),
                            Quaternion.identity) as GameObject;
    }

}
