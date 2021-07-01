#include <iostream>
using namespace std;
//
// DOOR PUZZLE
//
void doorpuzzlepicture() //Door Puzzle, Wordsearch
{
	system("CLS");
	cout << "R W N A V	Asp" << endl;
	cout << "Z R W B M	Rope" << endl;
	cout << "Q P P M R	Torch" << endl;
	cout << "B S L O T	Tomb" << endl;
	cout << "A E S T R	Gold\n" << endl;
	cout << "Which rope will you pull on?\n" << endl;
	cout << "1. Asp" << endl;
	cout << "2. Rope" << endl;
	cout << "3. Torch" << endl;
	cout << "4. Tomb" << endl;
	cout << "5. Gold\n" << endl;
}

void predoorpuzzlechoicetext() //Text Before Door Puzzle
{
	system("CLS");
	cout << "In front of you is a large wooden door attached to a system of ropes and pulleys.\n" << endl;
	cout << "1. Attempt to push open the door" << endl;
	cout << "2. Inspect the system of ropes and pulleys\n" << endl;
}

void predoorpuzzlechoice1() //Choice 1 returns to predoorpuzzletext
{
	system("CLS");
	cout << "You stand before the great wooden door and attempt to push it open," << endl;
	cout << "however even with all your strength it will not move.\n" << endl;
	system("PAUSE");
}

void predoorpuzzlechoice2() //Choice 2 starts Door Puzzle
{
	system("CLS");
	cout << "You look closer at the system and discover it leads to an alcove in the wall." << endl;
	cout << "Inside the alcove is a grid of English letters beside five ropes." << endl;
	cout << "Each rope has a word scratched into the stone next to it." << endl;
	cout << "Pulling one of the ropes must open the door...\n" << endl;
	system("PAUSE");
}

void doorpuzzlechoice1() //Chooses Asp
{
	system("CLS");
	cout << "The pulleys whir to life as you pull the rope.\n" << endl;
	system("PAUSE");
	system("CLS");
	cout << "The door remains shut as the machinery stops." << endl;
	cout << "Opposite the alcove a section of the wall has opened," << endl;
	cout << "revealing a dimly lit passageway.\n" << endl;
	cout << "You have no other choice but to enter...\n" << endl;
	system("PAUSE");
}

void doorpuzzlechoice4() //Chooses Tomb
{
	system("CLS");
	cout << "The pulleys whir to life as you pull the rope.\n" << endl;
	system("PAUSE");
	system("CLS");
	cout << "Rumbling can be heard as the large door slowly opens," << endl;
	cout << "revealing a staircase.\n" << endl;
	cout << "You have no other choice but to descend...\n" << endl;
	system("PAUSE");
}

void doorpuzzlechoiceelse() //Leads to Ending 1
{
	system("CLS");
	cout << "The pulleys whir to life as you pull the rope.\n" << endl;
	system("PAUSE");
	system("CLS");
	cout << "A large rumbling can be heard...\n" << endl;
	system("PAUSE");
	system("CLS");
	cout << "The door remains shut as the ceiling begins to cave in around you." << endl;
	cout << "You try to run back outside of the tomb but it is too late." << endl;
	cout << "Rocks have sealed the entrance.\n" << endl;
	system("PAUSE");
	system("CLS");
	cout << "You are now trapped inside a tomb of your own making.\n" << endl;
	system("PAUSE");
	system("CLS");
	cout << "There is no escape...\n" << endl;
	system("PAUSE");
	system("CLS");
	cout << "GAME OVER" << endl;
	cout << "ENDING 1 UNLOCKED!\n" << endl;
	system("PAUSE");
}
