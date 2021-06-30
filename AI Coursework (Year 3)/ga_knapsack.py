import random 
from random import choice
from random import randint
from collections import namedtuple
import math
import numpy as np
import matplotlib.pyplot as plt
import pandas as pd

#
### parameters
#
POPULATION_SIZE = 10 # number of individuals in population
STRONG_POPULATION = 2 # number of individuals to survive
GENERATIONS = 50 # maximum number of generations (gen 0 to gen n-1)

CROSSOVER_RATE = 8 # 80% crossover rate
MUTATION_RATE = 2 # 20% mutation rate

OBJECT = namedtuple('Object', ['name', 'value', 'weight'])

OBJECT_LIST = [
    
    OBJECT('Water Bottle', 30, 192),
    OBJECT('Laptop', 500, 2200),
    OBJECT('Notebook', 40, 333),
    OBJECT('Coca Cola', 60, 350),
    OBJECT('Mobile Phone', 150, 160)
]

WEIGHT_LIMIT = 3000

#
### individual and population
#
def createIndividual(choicesList, genomeLength): # create genome from genome length and min/max values
    genome = [choice(choicesList) for x in range(genomeLength)]
    return genome

def createPopulation(choiceList, genomeLength): # create a population of genomes from population size
    return[createIndividual(choiceList, genomeLength) for x in range(POPULATION_SIZE)]

#
### fitness
# 
def computeFitness(individual, objectList, weightLimit):
    
    weight = 0
    value = 0

    for i, Object in enumerate(objectList):
        if (individual[i] == 1):
            weight += Object.weight
            value += Object.value
    
            if weight > weightLimit:
                return 0
    
    return value    

def generationalFitness(population, objectList, weightLimit): # compute average fitness of generation
    fitnessList = []
    for individual in population:
        fitnessList.append(computeFitness(individual, objectList, weightLimit))
    return (sum(fitnessList)/POPULATION_SIZE) # return average

def findFittestA(population, objectList, weightLimit): # returns fittest chromosome
    fitnessList = []
    for individual in population:
        fitnessList.append(computeFitness(individual, objectList, weightLimit)) # compute the fitness of each individual
    popDict = dict(zip(fitnessList,population)) # create dictionary of fitness and population genes
    fitnessList = sorted(fitnessList) # sort the list
    
    return ((popDict[fitnessList[-1]]))
    
def findFittestB(population, objectList, weightLimit): # returns value of highest fitness
    fitnessList = []
    for individual in population:
        fitnessList.append(computeFitness(individual, objectList, weightLimit)) # compute the fitness of each individual
    fitnessList = sorted(fitnessList) # sort the list
    
    return (fitnessList[-1])

#    
### selection of fittest    
#
def rankedRouletteSelection(population, populationSize, strongPop, objectList, weightLimit):
    fitnessList = []
    for individual in population:
        fitnessList.append(computeFitness(individual, objectList, weightLimit)) # compute the fitness of each individual
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
def surviveGeneration(children, objectList, weightLimit): # ranked roulette selection on children to reduce to POPULATION_SIZE 
    nextGen = []
    nextGen = rankedRouletteSelection(children, len(children), POPULATION_SIZE, objectList, weightLimit)
    return nextGen

#
### knapsack solution
#
def objectSolution(individual, objectList):
    result = []
    for i, Object in enumerate(objectList):
        if individual[i] == 1:
            result += [Object.name]
            
    return result        
    
#
### main function
#
def main():
    
    GENOME_LENGTH = 8
    CHOICES = [0,1]

    #
    ### create empty lists and initial population
    #
    generationList = [] # empty list for generation numbers
    avgFitList = [] # empty list for average fitness of generation
    fittestList = [] # emtpy list for fittest
    fittestComboList = [] # empty list for fittist chromosome
    generation = 0 # start at 1st gen
    population = createPopulation(CHOICES, GENOME_LENGTH) # create initial population 

    #
    ### keep create new generations until max generations hit
    #
    while (generation != GENERATIONS): # until number of generations met

        genFit = generationalFitness(population, OBJECT_LIST, WEIGHT_LIMIT) # calculate average fitness of generation
        fittest = findFittestA(population, OBJECT_LIST, WEIGHT_LIMIT) # find the fittest
        
        generationList.append(generation) # add generation number to list
        avgFitList.append(genFit) # add avg fitness to list
        fittestList.append(findFittestB(population, OBJECT_LIST, WEIGHT_LIMIT)) # add fittest to list
        fittestComboList.append(fittest) # add fittist gene combo to list

        parents = rankedRouletteSelection(population, POPULATION_SIZE, STRONG_POPULATION, OBJECT_LIST, WEIGHT_LIMIT) # select parents
        children = crossover(parents, GENOME_LENGTH) # create children from parents according to crossover rate
        mutate(children, CHOICES, GENOME_LENGTH) # mutate the children according to mutation rate
        population = surviveGeneration(children, OBJECT_LIST, WEIGHT_LIMIT) # survive children by fitness to reduce number of children to population 
        
        generation +=1
        
    #
    ### present data
    #
    df = pd.DataFrame(list(zip(avgFitList, fittestList, fittestComboList)), columns=['Average Fitness of Generation', 'Fittest of Generation', 'Fittest Chromosome'])
    index = df['Fittest of Generation'].idxmax()
    bestScoreList = df['Fittest of Generation'].loc[[index]].to_list()
    bestScore = bestScoreList[0]
    chromosomeList = df['Fittest Chromosome'].loc[[index]].to_list()
    chromosome = chromosomeList[0]
    
    print("\nBest Fitness Score:", bestScore, "\nChromosome:", chromosome)
    print("Knapsack Solution:", objectSolution(chromosome, OBJECT_LIST))

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
        