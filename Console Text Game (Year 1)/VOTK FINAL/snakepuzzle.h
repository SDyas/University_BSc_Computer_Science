#include <iostream>
#include <vector>
using namespace std;
//
// SNAKE PUZZLE ~~~
//
bool snakepuzzlecomplete = false;

void snakepuzzletext() // Simple Choice Between Two Options
{
	system("CLS");
	cout << "As you reach the end of the passageway you notice something slithering past your foot...\n" << endl;
	system("PAUSE");
	system("CLS");
	cout << "                          ____     " << endl;
	cout << " ________________________/ O  |~~~ " << endl;
	cout << "<___0__0__0__0__0__0__0_______/    " << endl;
	cout << "\n" << endl;
	system("PAUSE");
	system("CLS");
	cout << "You have wandered into a snake pit!\n" << endl;
	system("PAUSE");
	system("CLS");
	cout << "You must carefully navigate through the snakes to the hallway on the far side of the pit.\n" << endl;
	system("PAUSE");
	system("CLS");
	cout << "Run across the pit or Carefully tip-toe across?\n" << endl;
	cout << "1. RUN!" << endl;
	cout << "2. Carefully tip-toe\n" << endl;
	char snakechoice;
	cin >> snakechoice;
	if (snakechoice == '1') // Leads to Maze
	{
		system("CLS");
		cout << "You make a mad dash to the exit, trampling on many snakes as you go.\n" << endl;
		system("PAUSE");
		system("CLS");
		cout << "With some luck you make it across to the opposite hallway." << endl;
		cout << "You have no choice but to follow it...\n" << endl;
		system("PAUSE");
		snakepuzzlecomplete = true;
	}
	else if (snakechoice == '2') // Ending 2 
	{
		system("CLS");
		cout << "You try to slowly, and carefully tip-toe across the pit.\n" << endl;
		system("PAUSE");
		system("CLS");
		cout << "It would appear your caution was ill-advised as a venomous asp finds it's way up your trouser leg...\n" << endl;
		system("PAUSE");
		system("CLS");
		cout << "You feel unimaginable pain as the asp bites upon your most precious treasure.\n" << endl;
		cout << "OUCH...\n" << endl;
		system("PAUSE");
		system("CLS");
		cout << "GAME OVER" << endl;
		cout << "ENDING 2 UNLOCKED!\n" << endl;
		system("PAUSE");
		snakepuzzlecomplete = false;
	}
}
