#include <iostream>
using namespace std;
//
// MAIN MENU
//
void menutext() //Main Menu Screen
{
	system("CLS");
	cout << "Valley of The Kings" << endl;
	cout << "~~~~~~~~~~~~~~~~~~~" << endl;
	cout << "1. Start Game" << endl;
	cout << "2. Achievements" << endl;
	cout << "3. Credits" << endl;
	cout << "~~~~~~~~~~~~~~~~~~~" << endl;
}
//Choice 1 Starts Game
//Choice 2 Opens Achievements

void menuchoice3() //Choice 3 Opens Credits
{
	system("CLS");
	cout << "Valley of The Kings" << endl;
	cout << "Sam Dyas 2019" << endl;
	cout << "All Rights Reserved\n" << endl;
	system("PAUSE");
}
void developerchoice()
{
	system("CLS");
	cout << "Would you like to Select a Level or start a New Game?" << endl;
	cout << "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" << endl;
	cout << "1. Select A Level" << endl;
	cout << "2. New Game\n" << endl;
}

void developermode()
{
	system("CLS");
	cout << "Select Level:" << endl;
	cout << "~~~~~~~~~~~~~~~" << endl;
	cout << "1. Door Puzzle" << endl;
	cout << "2. Snake Puzzle" << endl;
	cout << "3. Water Puzzle" << endl;
	cout << "4. Maze Puzzle" << endl;
	cout << "5. Final Puzzle\n" << endl;
	cout << "6. Return to Main Menu\n" << endl;
}
