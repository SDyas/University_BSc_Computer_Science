import random 
from random import choice
from random import randint
import math
import numpy as np
import matplotlib.pyplot as plt
import pandas as pd

#
### parameters
#
POPULATION_SIZE = 50 # number of individuals in population
STRONG_POPULATION = 10 # number of individuals to survive
GENERATIONS = 100 # maximum number of generations (gen 0 to gen n-1)

CROSSOVER_RATE = 8 # 80% crossover rate
MUTATION_RATE = 2 # 20% mutation rate

#
### individual and population
#
def createIndividual(choicesList, genomeLength, functionType): # create genome from genome length and min/max values
    if (functionType == 3): # tension/compression spring design
        
        x1Choices =  []
        x1Choices.append(choicesList[0])
        x1Choices.append(choicesList[1])
        
        x2Choices = []
        x2Choices.append(choicesList[2])
        x2Choices.append(choicesList[3])
        
        x3Choices = []
        x3Choices.append(choicesList[4])
        x3Choices.append(choicesList[5])
        
        x1 = choice(x1Choices)
        x2 = choice(x2Choices)
        x3 = choice(x3Choices)
        
        genome = [x1,x2,x3]
        return genome
    
    elif (functionType == 4): # welded beam design problem
        
        x1Choices =  []
        x1Choices.append(choicesList[0])
        x1Choices.append(choicesList[1])
        
        x2Choices = []
        x2Choices.append(choicesList[2])
        x2Choices.append(choicesList[3])
        
        x3Choices = []
        x3Choices.append(choicesList[4])
        x3Choices.append(choicesList[5])
        
        x4Choices = []
        x4Choices.append(choicesList[6])
        x4Choices.append(choicesList[7])
        
        x1 = choice(x1Choices)
        x2 = choice(x2Choices)
        x3 = choice(x3Choices)
        x4 = choice(x4Choices)
        
        genome = [x1,x2,x3,x4]
        return genome
        
    else: 
        genome = [choice(choicesList) for x in range(genomeLength)]
        return genome

def createPopulation(choiceList, genomeLength, functionType): # create a population of genomes from population size
    return[createIndividual(choiceList, genomeLength, functionType) for x in range(POPULATION_SIZE)]

#
### fitness
#
def computeFitness(individual, functionType, password): # compute the fitness for one individual
      
        if (functionType == 0): # minimise to 0 (0,.....0)
            fitnessScore = 0
            for gene in individual:
                if (gene == 0): 
                    fitnessScore += 1
            return fitnessScore        
        
        elif (functionType == 1): # maximise to n (1,....,1)
            fitnessScore = 0
            for gene in individual:
                if (gene == 1):
                    fitnessScore += 1
            return fitnessScore    
        
        elif (functionType == 2): # passcode combination 0-9   
            fitnessScore = 0
            combination = individual
            passcode = password
            for x in range(0, len(combination)):
                if(combination[x] == passcode[x]):
                    fitnessScore += 1
            return fitnessScore
        
        elif (functionType == 3): # tension/compression spring design
            fitnessScore = (individual[2] + 2) * individual[1] * individual[0] **2
            return -fitnessScore
        
        elif (functionType == 4): # welded beam design problem
            fitnessScore = 1.10471 * individual[0] **2 * individual[1] + 0.04811 * individual[2] * individual[3] *(14.0 + individual[1])
            return -fitnessScore
        
def generationalFitness(population, functionType, password): # compute average fitness of generation
    fitnessList = []
    for individual in population:
        fitnessList.append(computeFitness(individual, functionType, password))
    return (sum(fitnessList)/POPULATION_SIZE) # return average

def findFittestA(population, functionType, password): # returns fittest chromosome
    fitnessList = []
    for individual in population:
        fitnessList.append(computeFitness(individual, functionType, password)) # compute the fitness of each individual
    popDict = dict(zip(fitnessList,population)) # create dictionary of fitness and population genes
    fitnessList = sorted(fitnessList) # sort the list
    
    return ((popDict[fitnessList[-1]]))
    
def findFittestB(population, functionType, password): # returns value of highest fitness
    fitnessList = []
    for individual in population:
        fitnessList.append(computeFitness(individual, functionType, password)) # compute the fitness of each individual
    fitnessList = sorted(fitnessList) # sort the list
    
    return (fitnessList[-1])

#    
### selection of fittest    
#
def rankedRouletteSelection(population, populationSize, strongPop, functionType, password):
    fitnessList = []
    for individual in population:
        fitnessList.append(computeFitness(individual, functionType, password)) # compute the fitness of each individual
    popDict = dict(zip(fitnessList,population))
    
    fitnessList = sorted(fitnessList)
    probSelect = (1/populationSize)*100 
    probList = []
    for i in range(0,len(fitnessList)):
        probList.append(probSelect*(i+1)) # assign a ranked probability to each individual
    
    selectedParents = []     
    for i in range(0,strongPop):
        selectedParents.append(random.choices(fitnessList, weights=probList)) # choose parents based upon weighted probabilities
    selectedParents = [val for sublist in selectedParents for val in sublist] 

    parents = []
    for i in range(0,len(selectedParents)):
        parents.append(popDict[selectedParents[i]]) # get parent chromosomes from dictionary
    
    return parents

#
### crossover of parents
#
def crossover(parents, genomeLength):
    children = []
    
    while(len(children) < POPULATION_SIZE * 2):
        rand = randint(0,9)
        rand = randint(0,len(parents)-1)
        father = parents[rand] # randomly assign father
        rand = randint(0,len(parents)-1)
        mother = parents[rand] # randomly assign mother
        
        if (rand < CROSSOVER_RATE): # successful crossover
            crossOverPoint = randint(1,genomeLength -1) # select random crossover point
            rand = randint(0,9)
        
            if (rand < 5):
                childLeft = father[crossOverPoint:] # father left
                childRight = mother[:crossOverPoint] # mother right
            else:
                childLeft = father[:crossOverPoint] # father right
                childRight = mother[crossOverPoint:] # mother left
            
            child = childLeft + childRight # combine parts to make child
        
        else: # crossover not achieved 
            rand = randint(0,1)
            if(rand == 0): 
                child = father # child is 'clone' of father
            else: 
                child = mother # child is 'clone' of mother
        
        children.append(child) # add to children list
       
    return children

#
### mutation of children
#
def mutate(children, choicesList, genomeLength): # mutate genes according to mutation rate
    for i in range(0,len(children)):
        for j in range(0,genomeLength):
            if (randint(1,10) < MUTATION_RATE):
                children[i][j] = choice(choicesList)
                
#
### survive to next generation
#
def surviveGeneration(children, functionType, password): # ranked roulette selection on children to reduce to POPULATION_SIZE 
    nextGen = []
    nextGen = rankedRouletteSelection(children, len(children), POPULATION_SIZE, functionType, password)
    return nextGen

#
### main function
#
def main(functionType):
    
    PASSWORD = [] # default empty password
    
    if (functionType == 0): # minimise to 0 (0,......,0)
        
        GENOME_LENGTH = 8
        CHOICES = [0,1]
        
    elif (functionType == 1): # maximise to 1 (1,......,1)    
        
        GENOME_LENGTH = 8
        CHOICES = [0,1]
        
    elif (functionType == 2): # passcode combination 0-9    
        GENOME_LENGTH = 4
        CHOICES = [0,1,2,3,4,5,6,7,8,9]
        PASSWORD = createIndividual(CHOICES, GENOME_LENGTH, functionType) # generate a password combination
        print("PASSCODE: ", PASSWORD)
        
    elif (functionType == 3): # tension/compression spring design
        GENOME_LENGTH = 3
        CHOICES = [2,15,    
                   0.25,1.3,
                   0.05,2]
    
    elif (functionType == 4): # welded beam design problem
        GENOME_LENGTH = 4
        CHOICES = [0.1,2,
                   0.1,10,
                   0.1,10,
                   0.1,2]
    
    #
    ### create empty lists and initial population
    #
    generationList = [] # empty list for generation numbers
    avgFitList = [] # empty list for average fitness of generation
    fittestList = [] # emtpy list for fittest
    fittestComboList = [] # empty list for fittist chromosome
    generation = 0 # start at 1st gen
    population = createPopulation(CHOICES, GENOME_LENGTH, functionType) # create initial population  
    
    #
    ### keep create new generations until max generations hit
    #
    while (generation != GENERATIONS): # until number of generations met
    
        genFit = generationalFitness(population, functionType, PASSWORD) # calculate average fitness of generation
        fittest = findFittestA(population, functionType, PASSWORD) # find the fittest
        
        generationList.append(generation) # add generation number to list
        avgFitList.append(genFit) # add avg fitness to list
        fittestList.append(findFittestB(population, functionType, PASSWORD)) # add fittest to list
        fittestComboList.append(fittest) # add fittist gene combo to list
        
        parents = rankedRouletteSelection(population, POPULATION_SIZE, STRONG_POPULATION, functionType, PASSWORD) # select parents
        children = crossover(parents, GENOME_LENGTH) # create children from parents according to crossover rate
        mutate(children, CHOICES, GENOME_LENGTH) # mutate the children according to mutation rate
        population = surviveGeneration(children, functionType, PASSWORD) # survive children by fitness to reduce number of children to population size

        generation +=1
    
    #
    ### present data
    #
    df = pd.DataFrame(list(zip(avgFitList, fittestList, fittestComboList)), columns=['Average Fitness of Generation', 'Fittest of Generation', 'Fittest Chromosome'])
    index = df['Fittest of Generation'].idxmax()
    bestScore = df['Fittest of Generation'].loc[[index]]
    chromosome = df['Fittest Chromosome'].loc[[index]]
    
    print('Generation:', index)
    print("\nBest Fitness Score:", bestScore.values, "\nChromosome:", chromosome.values)
    
    plt.figure(figsize=(12, 12))
    
    plt.subplot(2,1,1)
    plt.plot(generationList, avgFitList, color='red', marker='x') # # average fitness of generation
    plt.title('Average Fitness of Generation', fontsize = 14)
    plt.xlabel('Generation',fontsize=14)
    plt.ylabel('Fitness',fontsize=14)
    plt.grid(True)
    
    plt.subplot(2,1,2)
    plt.plot(generationList, fittestList, color='blue', marker='x') # fittest of generation
    plt.title('Fittest of Generation', fontsize = 14)
    plt.xlabel('Generation',fontsize=14)
    plt.ylabel('Fitness',fontsize=14)
    plt.grid(True)
    
    plt.show