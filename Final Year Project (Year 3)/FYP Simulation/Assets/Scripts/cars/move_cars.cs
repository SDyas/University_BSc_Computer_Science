using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move_cars : MonoBehaviour
{
    //
    // variables
    //

    public float leftSpeed; // speed left car travels along road
    public float rightSpeed; // speed right car travels along road
    public float leftEndZ; // end point on other side of road
    public float rightEndZ; // end point on other side of road

    //
    // methods
    //

    // main
    void Update()
    {
        GameObject[] leftCarList = GameObject.FindGameObjectsWithTag("left_car"); // add all left cars to list
        GameObject[] rightCarList = GameObject.FindGameObjectsWithTag("right_car"); // add all right cars to list

        foreach(GameObject car in leftCarList) // for each left car
        {
            move(car, leftSpeed); // move the car along the road
            carDestroy(car, leftEndZ, 0); // destroy after reaching end of road
        }

        foreach(GameObject car in rightCarList) // for each right car
        {
            move(car, rightSpeed); // move the car along the road
            carDestroy(car, rightEndZ, 1); // destroy after reaching end of road
        }
    }

    // car movement
    private void move(GameObject car, float speed)
    {
        car.transform.position = new Vector3(car.transform.position.x,
                                             car.transform.position.y,
                                             car.transform.position.z + speed);
    }

    // car destruction
    private void carDestroy(GameObject car, float endZ, int direction)
    {
        if (direction == 0) // if left car
        {
            if (car.transform.position.z >= endZ) // if reached end of road
            {
                Destroy(car); // destroy car
            }
        }
        else
        {
            if (car.transform.position.z <= endZ) // if reached end of road
            {
                Destroy(car); // destroy car
            }
        }
    }
}
