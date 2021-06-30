using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using UnityEngine;

public class geneticalgorithm : MonoBehaviour
{
    //
    // variables
    //

    // lists
    private List<GameObject> frogPopulation = new List<GameObject>(); // population of frogs
    private List<Genome> geneList = new List<Genome>(); // list of genes
    private List<Genome> parentList = new List<Genome>(); // list of parent genes
    private List<Genome> childList = new List<Genome>(); // list of children genes
    private List<Genome> mutantList = new List<Genome>(); // list of mutated children genes

    // csv output lists
    public string genResFilePath = "C:"; // filepath for generation results
    private List<GenResults> resultsList = new List<GenResults>(); // output list for results
    private List<DeathLocations> deathList = new List<DeathLocations>(); // output list for results
    private List<float> timeList = new List<float>(); // output list for time taken each generation  

    // genetic algorithm parameters
    public int POPULATION_SIZE = 100; // number of frogs in population
    public int STRONG_POPULATION = 10; // number of strong frogs to be selected as parents
    public int GENERATIONS = 5; // number of generations
    private int currentGen = 1; // number of current generation
    public int CROSSOVER_RATE = 8; // 80% chance of crossover 
    public int MUTATION_RATE = 2; // 20% chance of mutation

    // frog parameter bounds
    public int frogSpawnZMax = 8; // max spawnZ 
    public int frogSpawnZMin = -7; // min spawnZ
    public float moveDelayMax = 5; // max moveDelay
    public float moveDelayMin = 0.25f; // min moveDelay
    public float knowledgeDelayMax = 5; // max knowledgeDelay
    public float knowledgeDelayMin = 0; // min knowledgeDelay

    // miscellaneous
    private System.Random rand = new System.Random(); // random number generator
    private Stopwatch timer = new Stopwatch(); // timer
    private bool simulationActive = true; // is the simulation on

    //
    // classes
    // 

    // results class for csv output list
    private class GenResults
    {
        public float avgFitness; // average fitness of generation
        public float bestFitness; // best fitness of generation
        public float worstFitness; // worst fitness of generation
        public float bSpawnZ; // spawnZ gene
        public float bMoveDelay; // moveDelay gene
        public float wSpawnZ; // spawnZ gene
        public float wMoveDelay; // moveDelay gene
        public float bKnowDelay; // knowDelay gene
        public float wKnowDelay; // knowDelay gene
        public float bDeathXPosition; // deathXPosition
        public float wDeathXPosition; // deathXPosition
        public float bDeathZPosition; // deathZPosition
        public float wDeathZPosition; // deathZPosition

        public GenResults(float avgFit, float bestFit, float worstFit, 
                          float bSpawnZGene, float bMoveDelayGene, float bKnowD, float bDeathXPos, float bDeathZPos, 
                          float wSpawnZGene, float wMoveDelayGene, float wKnowD, float wDeathXPos, float wDeathZPos)
        {
            avgFitness = avgFit; // average fitness of generation
            bestFitness = bestFit; // best fitness of generation
            worstFitness = worstFit; // worst fitness of generation
            bSpawnZ = bSpawnZGene; // spawnZ gene
            wSpawnZ = wSpawnZGene; // spawnZ gene
            bMoveDelay = bMoveDelayGene; // moveDelay gene
            wMoveDelay = wMoveDelayGene; // moveDelay gene
            bKnowDelay = bKnowD; // knowDelay gene
            wKnowDelay = wKnowD; // knowDelay gene
            bDeathXPosition = bDeathXPos; // deathXPosition
            wDeathXPosition = wDeathXPos; // deathXPosition
            bDeathZPosition = bDeathZPos; // deathZPosition
            wDeathZPosition = wDeathZPos; // deathZPosition
        }
    }

    // death location class for csv output list
    private class DeathLocations
    {
        public float deathX; // x position at death
        public float deathZ; // z position at death

        public DeathLocations(float deathXPos, float deathZPos)
        {
            deathX = deathXPos; // x position at death
            deathZ = deathZPos; // z position at death
        }
    }
    
    // gene created upon frog death
    private class Genome
    {
        public float fitness; // fitness gene
        public float spawnZ; // spawnZ gene
        public float moveDelay; // moveDelay gene
        public float knowDelay; // knowDelay gene
        public float deathX; // xPosition at death
        public float deathZ; // zPosition at death

        public Genome(float fitnessGene, 
                      float spawnZGene, 
                      float moveDelayGene, 
                      float knowDelayGene, 
                      float deathXPos, 
                      float deathZPos)
        {
            fitness = fitnessGene; // fitness gene
            spawnZ = spawnZGene; // spawnZ gene
            moveDelay = moveDelayGene; // move delay gene
            knowDelay = knowDelayGene; // know delay gene
            deathX = deathXPos; // xPosition at death
            deathZ = deathZPos; // zPosition at death
        }
    }

    //
    // main methods
    //

    void Start()
    {
        frogPopulation = createInitialPopulation(); // create initial population of frogs
        timer.Start(); // start timer
        UnityEngine.Debug.Log("Current Generation: " + currentGen); // debug line
    }

    // Update is called once per frame
    void Update()
    {
        if (simulationActive) //if simulation active
        {
            if (currentGen < GENERATIONS) // if not at generation limit
            {
                if (frogPopulation.Count == 0) // if there are no frogs
                {
                    timer.Stop(); // stop timer
                    int seconds, milliseconds;
                    seconds = timer.Elapsed.Seconds;
                    milliseconds = timer.Elapsed.Milliseconds;
                    float floatTimeSpan = (float)seconds + ((float)milliseconds / 1000); // convert time taken to float
                    timeList.Add(floatTimeSpan); // add elapsed time to timeList
                    timer.Reset(); // set timer to 0

                    parentList = selectParents(geneList); // select parents of next generation
                    childList = crossoverParents(parentList); // crossover parents to create children
                    mutantList = mutateChildren(childList); // mutate children
                    frogPopulation = createSubsequentPopulation(mutantList); // create subsequent population of frogs
                    timer.Start(); // restart timer
                    geneList.Clear(); // clears the geneList
                    currentGen += 1; // increment generation number
                    UnityEngine.Debug.Log("Current Generation: " + currentGen); // debug line
                }
                checkAlive(frogPopulation); // destroy frogs if they have been run over
            }
            else // on last generation
            {
                if (frogPopulation.Count == 0) // if there are no frogs
                {
                    timer.Stop(); // stop timer
                    int seconds, milliseconds;
                    seconds = timer.Elapsed.Seconds;
                    milliseconds = timer.Elapsed.Milliseconds;
                    float floatTimeSpan = (float)seconds + ((float)milliseconds / 1000); // convert time taken to float
                    timeList.Add(floatTimeSpan); // add elapsed time to timeList
                    parentList = selectParents(geneList); // create unused parents and read final stats
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(@genResFilePath, true)) // headers
                    {
                        file.WriteLine("Average Fitness," +
                                       "Best Fitness," +
                                       "Worst Fitness," +
                                       "Best SpawnZ," +
                                       "Worst SpawnZ," +
                                       "Best MoveDelay," +
                                       "Worst MoveDelay," +
                                       "Best KnowDelay," +
                                       "Worst KnowDelay," +
                                       "Best DeathX," +
                                       "Best DeathZ," +
                                       "Worst DeathX," +
                                       "Worst DeathZ");
                    }
                    foreach (GenResults results in resultsList) // for each generation result
                    {
                        using(System.IO.StreamWriter file = new System.IO.StreamWriter(@genResFilePath, true))
                        {
                            file.WriteLine(results.avgFitness + "," + 
                                results.bestFitness + "," + 
                                results.worstFitness + "," + 
                                results.bSpawnZ + "," + 
                                results.wSpawnZ + "," + 
                                results.bMoveDelay + "," + 
                                results.wMoveDelay + "," +
                                results.bKnowDelay + "," +
                                results.wKnowDelay + "," +
                                results.bDeathXPosition + "," + 
                                results.bDeathZPosition + "," +
                                results.wDeathXPosition + "," +
                                results.wDeathZPosition); // write to csv file
                        }
                    }
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(@genResFilePath, true)) // headers
                    {
                        file.WriteLine("DeathX," + 
                                       "DeathZ");
                    }
                    foreach (DeathLocations deathLocations in deathList) // for each death location result
                    {
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(@genResFilePath, true))
                        {
                            file.WriteLine(deathLocations.deathX + "," +  deathLocations.deathZ); // write to csv file
                        }
                    }
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(@genResFilePath, true)) // header
                    {
                        file.WriteLine("Time Taken for Generation");
                    }
                    foreach (float timeSpan in timeList) // for each time taken for generation
                    {
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(@genResFilePath, true))
                        {
                            file.WriteLine(timeSpan); // write to csv file
                        }
                    }


                    simulationActive = false; // end simulation
                }
                checkAlive(frogPopulation); // destroy frogs if they have been run over
            }
        }
    }

    //
    // individual and population
    //

    // check whether frog is alive or not
    private void checkAlive(List<GameObject> frogPopulation)
    {
        for (int i = 0; i < frogPopulation.Count; i++) // for each frog in the population
        {
            if (frogPopulation[i].GetComponent<frog_movement>().isAlive == false) // if it has been run over
            {
                GameObject deadFrog = frogPopulation[i]; // temp assignment
                frogPopulation.Remove(frogPopulation[i]); // remove from population

                float fitness = deadFrog.GetComponent<frog_movement>().fitness; // get fitness
                float spawnZ = deadFrog.GetComponent<frog_movement>().spawnZ; // get spawnZ
                float moveDelay = deadFrog.GetComponent<frog_movement>().moveDelay; // get moveDelay
                float knowDelay = deadFrog.GetComponent<frog_movement>().knowDelay; // get knowDelay
                float deathX = -deadFrog.transform.position.x; // get deathX
                float deathZ = -deadFrog.transform.position.z; // get deathZ

                Genome genome = new Genome(fitness, spawnZ, moveDelay, knowDelay, deathX, deathZ); // create new genome
                geneList.Add(genome); // add to gene list

                Destroy(deadFrog); // destroy frog
            }
        }
    }

    // create individual frog with genes (initial generation)
    private GameObject createInitialIndividual()
    {
        int frogSpawnZ = rand.Next(frogSpawnZMin, frogSpawnZMax); // set spawnZ
        GameObject frog = // create new frog
                            Instantiate(Resources.Load("frog_cube"),
                            new Vector3(0, 1, frogSpawnZ),
                            Quaternion.identity) as GameObject;

        frog.GetComponent<Renderer>().material.color = new Color((float)(rand.NextDouble() * 1), (float)(rand.NextDouble() * 1), (float)(rand.NextDouble() * 1), 1); // set colour of frog
        frog.GetComponent<frog_movement>().spawnZ = frogSpawnZ; // set spawnZ of frog
        frog.GetComponent<frog_movement>().moveDelay = (float)(rand.NextDouble() * (moveDelayMax - moveDelayMin) + moveDelayMin); // set moveDelay of frog
        frog.GetComponent<frog_movement>().knowDelay = (float)(rand.NextDouble() * (knowledgeDelayMax - knowledgeDelayMin) + knowledgeDelayMin); // set knowDelay of frog

        return frog; // return the frog
    }

    // create individual frog with genes (subsequent generations)
    private GameObject createSubsequentIndividual(Genome predFrog)
    {
        GameObject frog = // create new frog
                            Instantiate(Resources.Load("frog_cube"),
                            new Vector3(0, 1, predFrog.spawnZ),
                            Quaternion.identity) as GameObject;

        frog.GetComponent<Renderer>().material.color = new Color((float)(rand.NextDouble() * 1), (float)(rand.NextDouble() * 1), (float)(rand.NextDouble() * 1), 1); // set colour of frog
        frog.GetComponent<frog_movement>().spawnZ = predFrog.spawnZ; // set spawnZ of frog from predecessor
        frog.GetComponent<frog_movement>().moveDelay = predFrog.moveDelay; // set moveDelay of frog from predecessor
        frog.GetComponent<frog_movement>().knowDelay = predFrog.knowDelay; // set knowDelay of frog from predecessor

        return frog; // return the frog
    }

    // create population of frogs (initial generation)
    private List<GameObject> createInitialPopulation()
    {
        List<GameObject> frogPopulation = new List<GameObject>(); // list for frogs
        for (int i = 0; i < POPULATION_SIZE; i++) // for the size of the population
        {
            GameObject frog = createInitialIndividual(); // create a frog
            frogPopulation.Add(frog); // add frog to population list
        }
        return frogPopulation; // return list of frogs
    }

    // create population of frogs (subsequent generations)
    private List<GameObject> createSubsequentPopulation(List<Genome> mutantList)
    {
        List<GameObject> frogPopulation = new List<GameObject>(); // list for frogs
        foreach (Genome mutant in mutantList)
        {
            GameObject frog = createSubsequentIndividual(mutant); // create a frog from predecessor frogs
            frogPopulation.Add(frog); // add frog to population list
        }
        return frogPopulation; // return list of frogs
    }

    //
    // selection of parents from strongest of population
    //

    private List<Genome> selectParents(List<Genome> geneList)
    {
        float sumOfGen = 0; // sum of fitness of generation
        float avgOfGen = 0; // average fitness of generation
        foreach (Genome genome in geneList) // for each genome
        {
            sumOfGen += genome.fitness; // add to running total fitness
        }
        avgOfGen = sumOfGen / POPULATION_SIZE; // calculate average fitness of generation
        geneList.Sort((g1, g2) => g1.fitness.CompareTo(g2.fitness)); // sort geneList according to fitness (smallest to largest)
        float worstOfGen = geneList[0].fitness; // worst fitness of generation
        geneList.Reverse(); // reverse order of list (largest to smallest)
        float bestOfGen = geneList[0].fitness; // best fitness of generation

        GenResults results = new GenResults(avgOfGen, 
                                            bestOfGen, 
                                            worstOfGen, 
                                            geneList[0].spawnZ, 
                                            geneList[0].moveDelay, 
                                            geneList[0].knowDelay,
                                            geneList[0].deathX,
                                            geneList[0].deathZ,
                                            geneList[geneList.Count-1].spawnZ, 
                                            geneList[geneList.Count-1].moveDelay,
                                            geneList[geneList.Count-1].knowDelay,
                                            geneList[geneList.Count-1].deathX,
                                            geneList[geneList.Count-1].deathZ); // create object to store results
        resultsList.Add(results); // add to resultsList

        foreach(Genome genome in geneList) // for every genome
        {
            DeathLocations deathLocations = new DeathLocations(genome.deathX, genome.deathZ); // get death locations
            deathList.Add(deathLocations); // add to deathList
        }
        
        List<Genome> parentList = new List<Genome>(); // list of parent genes
        for (int i = 0; i < STRONG_POPULATION; i++) // for number of strong genes to pick
        {
            parentList.Add(geneList[i]); // add strong genes to parentList
        }
        return parentList; // return list of parents
    }
    
    //
    // crossover of parents genes to create children according to crossover rate
    //

    private List<Genome> crossoverParents(List<Genome> parentList)
    {
        List<Genome> childList = new List<Genome>(); // list for children genes 

        while(childList.Count < POPULATION_SIZE) // until double the population size
        {
            int fatherIndex = rand.Next(0, parentList.Count - 1); // assign fatherindex
            Genome father = parentList[fatherIndex]; // assign father genome
            int motherIndex = rand.Next(0, parentList.Count - 1); // assign mother index
            Genome mother = parentList[motherIndex]; // assign mother genome

            Genome child = new Genome(0,0,0,0,0,0); // create child genome

            int crossChance = rand.Next(0, 10); // random int 0 - 9
            if (crossChance < CROSSOVER_RATE) // successful crossover
            {
                int sexChoice = rand.Next(0, 10); // choose parent for spawnZ gene
                if (sexChoice < 5) child.spawnZ = father.spawnZ; // inhereit spawnZ from father
                else child.spawnZ = mother.spawnZ; // inherit spawnZ from mother

                sexChoice = rand.Next(0, 10); // choose parent for moveDelay gene
                if (sexChoice < 5) child.moveDelay = father.moveDelay; // inhereit moveDelay from father
                else child.moveDelay = mother.moveDelay; // inherit moveDelay from mother

                sexChoice = rand.Next(0, 10); // choose parent for knowDelay gene
                if (sexChoice < 5) child.knowDelay = father.knowDelay; // inhereit knowDelay from father
                else child.knowDelay = mother.knowDelay; // inherit knowDelay from mother

                childList.Add(child); // add to child gene list
            }
            else
            {
                int sexChoice = rand.Next(0, 10); // choose to be clone of father or of mother

                if (sexChoice < 5)
                {
                    child = father; // child is clone of father
                }
                else
                {
                    child = mother; // child is clone of mother
                }
                childList.Add(child); // add to child gene list
            }
        }
        return childList; // return list of children
    }

    //
    // mutation of child genes according to mutation rate
    //

    private List<Genome> mutateChildren(List<Genome> childList)
    {
        int mutationNum; // number to decide mutation
        foreach (Genome child in childList) // for each child
        {
            float mutationChance = MUTATION_RATE; // set initial mutationChance
            // mutation of spawnZ
            mutationNum = rand.Next(0, 10); // random mutation 
            if (mutationNum < mutationChance) // successful mutation
            {
                child.spawnZ = rand.Next(frogSpawnZMin, frogSpawnZMax); // recalculate spawnZ
                mutationChance = mutationChance / 2; // halve odds of successful mutation
            }
            // mutation of moveDelay
            mutationNum = rand.Next(0, 10); // random mutation 
            if (mutationNum < mutationChance) // successful mutation
            {
                child.moveDelay = (float)(rand.NextDouble() * 
                                  (moveDelayMax - moveDelayMin) + moveDelayMin); // recalculate moveDelay 
                mutationChance = mutationChance / 2; // halve odds of successful mutation
            }
            // mutation of knowDelay
            mutationNum = rand.Next(0, 10); // random mutation 
            if (mutationNum < mutationChance) // successful mutation
            {
                child.knowDelay = (float)(rand.NextDouble() * 
                                  (knowledgeDelayMax - knowledgeDelayMin) + knowledgeDelayMin); // recalculate knowDelay 
            }
        }
        return childList; // return list of mutated children
    }
}
