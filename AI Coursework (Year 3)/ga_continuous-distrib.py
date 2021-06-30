import random 
from random import randint
import math
from math import exp
from math import cos
from math import sqrt
from math import pi
import numpy as np
import matplotlib.pyplot as plt
import pandas as pd

#
### parameters
#
POPULATION_SIZE = 50 # number of individuals in population
STRONG_POPULATION = 10 # number of individuals to survive
GENERATIONS = 100 # maximum number of generations

CROSSOVER_RATE = 8 # 80% crossover rate
MUTATION_RATE = 2 # 20% mutation rate

#
### individual and population
# 
def createIndividual(min, max, genomeLength): # create genome from genome length and min/max values
    genome = [randint(min, max) for x in range(genomeLength)]
    return genome

def createPopulation(min, max, genomeLength): # create a population of genomes from population size
    return [createIndividual(min, max, genomeLength) for x in range(POPULATION_SIZE)]
    
#
### fitness
# 
def computeFitness(individual, functionType): # compute the fitness for one individual
    
    if (functionType == 0): # sum of squares
        return -sum([(i+1) * v**2 for i, v in enumerate(individual)]) 
    
    elif (functionType == 1): # bent cigar
        return -(individual[0]**2 + 1e6*sum([v**2 for v in individual[1:]]))
    
    elif (functionType == 2): # brown
        return -sum([
            ((individual[i]**2)**(individual[i+1]**2 + 1.0)) +
            ((individual[i+1]**2)**(individual[i]+1.0))
            for i in range(len(individual)-1)])
    
    elif (functionType == 3): # colville
        x1, x2, x3, x4 = individual[0], individual[1], individual[2], individual[3]
        a = 100*(x1**2 - x2)**2
        b = (x1-1)**2
        c = (x3-1)**2
        d = 90*(x3**2 - x4)**2
        e = 10.1*((x2-1)**2 + (x4-1)**2)
        f = 19.8*(x2-1)*(x4-1)
        return -(a + b + c + d + e + f)
    
    elif (functionType == 4): # salomon
        return -(1 - cos(2*pi*sqrt(sum([v**2 for v in individual]))) + 0.1*sqrt(sum([v**2 for v in individual])))
    
    elif (functionType == 5): # zakhorov
        a, b = 0, 0
        for i, val in enumerate(individual):
            a += val**2
            b += 0.5*i*val
        return -(a + b**2 + b**4)
        
def generationalFitness(population, functionType): # compute average fitness of generation
    fitnessList = []
    for individual in population:
        fitnessList.append(computeFitness(individual, functionType)) # compute the fitness of each individual
    return (sum(fitnessList)/POPULATION_SIZE) # return average

def findFittestA(population, functionType): # returns fittest chromosome
    fitnessList = []
    for individual in population:
        fitnessList.append(computeFitness(individual, functionType)) # compute the fitness of each individual
    popDict = dict(zip(fitnessList,population)) # create dictionary of fitness and population genes
    fitnessList = sorted(fitnessList) # sort the list
    
    return ((popDict[fitnessList[-1]]))

def findFittestB(population, functionType): # returns values of highest fitness
    fitnessList = []
    for individual in population:
        fitnessList.append(computeFitness(individual, functionType)) # compute the fitness of each individual
    fitnessList = sorted(fitnessList) # sort the list
    
    return (fitnessList[-1])

#    
### selection of fittest    
#
def rankedRouletteSelection(population, populationSize, strongPop, functionType):
    fitnessList = []
    for individual in population:
        fitnessList.append(computeFitness(individual, functionType)) # compute the fitness of each individual
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

def top10Selection(population, populationSize, strongPop, functionType): # basic selection of top 10 fittest for subtask1.D.
    fitnessList = []
    for individual in population:
        fitnessList.append(computeFitness(individual, functionType)) # compute the fitness of each individual
    popDict = dict(zip(fitnessList,population))
    
    fitnessList = sorted(fitnessList)
    selectedParents = []
    for i in range(1,10):
        selectedParents.append(fitnessList[-i])

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
        rand = randint(1,10)
        rand = randint(0,len(parents)-1)
        father = parents[rand] # randomly assign father
        rand = randint(0,len(parents)-1)
        mother = parents[rand] # randomly assign mother
        
        if (rand < CROSSOVER_RATE): # successful crossover
            crossOverPoint = randint(1,genomeLength -1) # select random crossover point
            rand = randint(1,10)
        
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
def mutate(children, min, max, genomeLength): # mutate genes according to mutatation rate
    for i in range(0,len(children)):
        for j in range(0,genomeLength):
             if (randint(1,10) < MUTATION_RATE):
                children[i][j] = randint(min,max)        

#
### survive to next generation
#
def surviveGeneration(children, functionType): # ranked roulette selection on children to reduce to POPULATION_SIZE                
    nextGen = []
    nextGen = rankedRouletteSelection(children, len(children), POPULATION_SIZE, functionType)
    return nextGen

def surviveGenerationTop10(children, functionType):
    nextGen = []
    nextGen = top10Selection(children, len(children), POPULATION_SIZE, functionType)
    return nextGen
    
#
### main function
#
def main(functionType):
    
    if (functionType == 0): # sum of squares     miminum at (0,.....,0)
    
        GENOME_LENGTH = 8
        MIN_GENE = -10
        MAX_GENE = 10
        TARGET = 0
    
    elif (functionType == 1): # bent cigar     minimum at (0,.....,0)
        
        GENOME_LENGTH = 8
        MIN_GENE = -100
        MAX_GENE = 100
        TARGET = 0
    
    elif (functionType == 2): # brown     minimum at (0,.....,0)
        
        GENOME_LENGTH = 8
        MIN_GENE = -1
        MAX_GENE = 4
        TARGET = 0 
        
    elif (functionType == 3): # coville     mimimum at (1,1,1,1)    
    
        GENOME_LENGTH = 4
        MIN_GENE = -10
        MAX_GENE = 10
        TARGET = 0
    
    elif (functionType == 4): # salomon     minimum at (0,.....,0)
        
        GENOME_LENGTH = 8
        MIN_GENE = -100
        MAX_GENE = 100
        TARGET = 0
    
    elif (functionType == 5): # zakhorov     minimum at (0,.....,0)
     
        GENOME_LENGTH = 8
        MIN_GENE = -5
        MAX_GENE = 10
        TARGET = 0
    
    #
    ### create empty lists and initial population
    #
    generationList = [] # empty list for generation numbers
    avgFitList = [] # empty list for average fitness of generation
    fittestList = [] # emtpy list for fittest
    fittestComboList = [] # empty list for fittist chromosome
    generation = 0 # start at 1st gen
    population = createPopulation(MIN_GENE, MAX_GENE, GENOME_LENGTH) # create initial population  
    
    #
    ### keep create new generations until max generations hit
    #
    while (generation != GENERATIONS): # until number of generations met
    
        genFit = generationalFitness(population, functionType) # calculate average fitness of generation
        fittest = findFittestA(population, functionType) # find the fittest
        
        generationList.append(generation) # add generation number to list
        avgFitList.append(genFit) # add avg fitness to list
        fittestList.append(findFittestB(population, functionType)) # add fittest to list
        fittestComboList.append(fittest) # add fittist gene combo to list
        
        parents = rankedRouletteSelection(population, POPULATION_SIZE, STRONG_POPULATION, functionType) # select parents
        children = crossover(parents, GENOME_LENGTH) # create children from parents according to crossover rate
        mutate(children, MIN_GENE, MAX_GENE, GENOME_LENGTH) # mutate the children according to mutation rate 
        population = surviveGeneration(children, functionType) # surive children by fitness to reduce number of children to population size
        
        generation +=1
    
    #
    ### present data
    #
    
    df = pd.DataFrame(list(zip(avgFitList, fittestList, fittestComboList)), columns=['Average Fitness of Generation', 'Fittest of Generation', 'Fittest Chromosome'])
    index = df['Fittest of Generation'].idxmax()
    bestScore = df['Fittest of Generation'].loc[[index]]
    chromosome = df['Fittest Chromosome'].loc[[index]]
    
    print('Generation:', index)
    print("Best Fitness Score:", bestScore.values, "\nChromosome:", chromosome.values)
    
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