using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frog_movement : MonoBehaviour
{
    // timers
    public float initialWait = 0; // initial wait so level can set up
    private float nextTimeToMove = 0f; // next time for frog to move
    public float moveDelay = 0f; // time between frog moves
    private float nextTimeKnowledge = 0f; // next time for frog to update knowledge
    public float knowDelay = 0f; // time between knowledge updates

    // knowledge
    private bool forwardSafe = true; // obstacles in front
    private bool leftSafe = true; // obstacles left
    private bool rightSafe = true; // obstacles right

    // frog bounds
    public float spawnZ; // frog spawn point along z axis
    public float leftBound; // left frog bound
    public float rightBound; // right frog bound

    // frog status
    public bool isAlive = true; // is frog active alive
    public float fitness = 0; // how far frog has moved forwards

    // Update is called once per frame
    void Update()
    {
        knowledgeTimer(); // update frog's knowledge of cars on timer
        moveTimer(); // move frog on timer
        updateFitness(); // update the fitness of the frog
    }

    // collision with cars
    void OnCollisionEnter(Collision collisionInfo)
    {
        if ((collisionInfo.collider.tag == "left_car") || (collisionInfo.collider.tag == "right_car")) // on collision with car
        {
            isAlive = false; // frog no longer alive 
        }
    }

    // update fitness
    private void updateFitness()
    {
        fitness = -transform.position.x; // how far the frog has travelled forwards
        if (fitness == 100) isAlive = false; // frog reached end of course
    }

    // move frog on timer according to knowledge 
    private void moveTimer()
    {
        if (initialWait <= Time.time)
        {
            if (nextTimeToMove <= Time.time)
            {
                moveForward(); // move forward according to knowledge
                moveLeft(); // move left according to knowledge
                moveRight(); // move right according to knowledge

                nextTimeToMove = Time.time + moveDelay; // set next move time
            }
        }
    }

    // update frog's knowledge of whether a car is too close
    private void knowledgeTimer()
    {
        if (nextTimeKnowledge <= Time.time)
        {
            GameObject[] leftCarList = GameObject.FindGameObjectsWithTag("left_car"); // add all left cars to list
            GameObject[] rightCarList = GameObject.FindGameObjectsWithTag("right_car"); // add all right cars to list

            // cars travelling right from left side
            foreach (GameObject car in leftCarList)
            {
                if (car.transform.position.x == transform.position.x) // if car on same x plane as frog
                {
                    if (car.transform.position.z < transform.position.z) // if car on left of frog
                    {
                        if (transform.position.z - car.transform.position.z <= 2) // if car too close to frog
                        {
                            if (leftSafe) // update only once
                            {
                                leftSafe = false; // danger on the left
                            }
                        }
                    }
                    else if (leftSafe == false) // update only once
                    {
                        leftSafe = true; // car no longer danger
                    }
                }
                else if (car.transform.position.x == transform.position.x - 1) // else if car on x plane in front of frog
                {
                    if (car.transform.position.z < transform.position.z) // if car on left of frog
                    {
                        if (transform.position.z - car.transform.position.z <= 2) // if car too close to frog
                        {
                            if (forwardSafe) // update only once
                            {
                                forwardSafe = false; // danger in front
                            }
                        }
                    }
                    else if (car.transform.position.z > transform.position.z) // if car on right of frog
                    {
                        if (car.transform.position.z - transform.position.z <= 2)
                        {
                            if (forwardSafe) // update only once
                            {
                                forwardSafe = false; // danger in front
                            }
                        }
                        else if (forwardSafe == false) // update only once
                        {
                            forwardSafe = true; // car no longer danger
                        }
                    }
                }
            }

            // cars travelling left from right side
            foreach (GameObject car in rightCarList)
            {
                if (car.transform.position.x == transform.position.x) // if car on same x plane as frog
                {
                    if (car.transform.position.z > transform.position.z) // if car on right of frog
                    {
                        if (car.transform.position.z - transform.position.z <= 2) // if car too close to frog
                        {
                            if (rightSafe) // update only once
                            {
                                rightSafe = false; // danger on the right
                            }
                        }
                    }
                    else if (rightSafe == false) // update only once
                    {
                        rightSafe = true; // car no longer danger
                    }
                }
                else if (car.transform.position.x == transform.position.x - 1) // else if car on x plane in front of frog
                {
                    if (car.transform.position.z > transform.position.z) // if car on right of frog
                    {
                        if (car.transform.position.z - transform.position.z <= 2) // if car too close to frog
                        {
                            if (forwardSafe) // update only once
                            {
                                forwardSafe = false; // danger in front
                            }
                        }
                    }
                    else if (car.transform.position.z < transform.position.z) // if car on left of frog
                    {
                        if (transform.position.z - car.transform.position.z <= 2) // if car too close to frog
                        {
                            if (forwardSafe)
                            {
                                forwardSafe = false; // danger in front
                            }
                        }
                        else if (forwardSafe == false) // update only once
                        {
                            forwardSafe = true; // car no longer danger
                        }
                    }
                }
            }
            nextTimeKnowledge = Time.time + knowDelay; // set next time to update knowledge
        }
    }

    // movement
    private void moveForward() // forward 1
    {
        if(forwardSafe == true)  // if it is safe to move forwards
        {
            move(-1, 0);
        }
    }

    private void moveLeft() // left 1
    {
        if ((rightSafe == false) && (forwardSafe == false)) // if it is not safe to move right or forwards and not at boundary
        {
            if (transform.position.z >= leftBound)
            {
                move(0, -1); // move left
            }
            else if (transform.position.x != 0)
            {
                move(1, 0); // move backwards if stuck
            }
        }
    }

    private void moveRight() // right 1
    {
        if ((leftSafe == false) && (forwardSafe == false)) // if it is not safe to move left or forwards and not at boundary
        {
            if (transform.position.z <= rightBound)
            {
                move(0, 1); // move right
            }
            else if (transform.position.x != 0)
            {
                move(1, 0); // move backwards if stuck
            }
        }
    }
    
    private void move(float xChange, float zChange)  // change transform position
    {
        transform.position = new Vector3(transform.position.x + xChange,
                                         transform.position.y,
                                         transform.position.z + zChange);
    }
}
