#include <iostream>
#include <vector>
using namespace std;
//
// MAZE PUZZLE
//
void newroom() // New Room Text
{
	system("CLS");
	cout << "You walk through the exit and follow it into a new room." << endl;
	cout << "There is another wooden sign in the middle of the room and three exits.\n" << endl;
	system("PAUSE");
}

void directions()
{
	cout << "1. Go Left" << endl;
	cout << "2. Go Forwards" << endl;
	cout << "3. Go Right\n" << endl;
}

void roomreturn1()
{
	system("CLS");
	cout << "You exit the room and start walking down a pitch black corridor." << endl;
	cout << "There are too many paths and you soon lose track of where you are.\n" << endl;
	system("PAUSE");
}
vector<int>lostcounter;
void roomreturn2()
{
	system("CLS");
	cout << "Somehow you find yourself in the original room again.\n" << endl;
	lostcounter.push_back(1);
	system("PAUSE");
}
void losttext()
{
	system("CLS");
	cout << "In complete the darkness you have become totally lost." << endl;
	cout << "You don't know where you are or how to get out.\n" << endl;
	system("PAUSE");
	system("CLS");
	cout << "All hope is lost, and so are you...\n" << endl;
	system("PAUSE");
	system("CLS");
	cout << "GAME OVER" << endl;
	cout << "ENDING 3 UNLOCKED!\n" << endl;
	system("PAUSE");
}

void mazepuzzleintro() // Maze Intro Text
{
	system("CLS");
	cout << "You pass through the doorway and find yourself in a small room." << endl;
	cout << "There is a wooden sign in the middle of the room and three exits:" << endl;
	cout << "Left, Forwards, and Right.\n" << endl;
	system("PAUSE");
}

void mazecompletetext()
{
	system("CLS");
	cout << "You enter into a large chamber." << endl;
	cout << "Flaming beacons line the walls." << endl;
	cout << "In the centre of the room lies a grand sarcophagus.\n" << endl;
	system("PAUSE");
}

bool mazecomplete = false;
bool mazepuzzleactive = true;
void mazepuzzletext() // Must follow correct path as instructed
{
	while (mazepuzzleactive = true)
	{
		bool room1 = true;
		bool room2 = false;
		bool room3 = false;
		bool room4 = false;
		bool room5 = false;
		bool room6 = false;
		while (room1 == true) // Room 1
		{
			system("CLS");
			cout << "The wooden sign in the middle of the room reads: Go Forwards\n" << endl; //Forwards
			directions();
			char roomchoice1;
			cin >> roomchoice1;
			if ((roomchoice1 == '1') || (roomchoice1 == '3')) // Back to Room 1
			{
				roomreturn1();
				if ((int)lostcounter.size() == 2)
				{
					losttext();
					mazepuzzleactive = false;
					room1 = false;
					break;
				}
				roomreturn2();
				room1 = true;
			}
			else if (roomchoice1 == '2') // Move to Room 2
			{
				newroom();
				room1 = false;
				room2 = true;
			}
		}
		while (room2 == true)// Room 2
		{
			system("CLS");
			cout << "The wooden sign in the middle of the room reads: Go Left\n" << endl; //Left
			directions();
			char roomchoice2;
			cin >> roomchoice2;
			if ((roomchoice2 == '2') || (roomchoice2 == '3')) // Back to Room 1 
			{
				roomreturn1();
				if ((int)lostcounter.size() == 2)
				{
					losttext();
					mazepuzzleactive = false;
					room2 = false;
					break;
				}
				roomreturn2();
				room2 = false;
				room1 = true;
			}
			else if (roomchoice2 == '1') // Move to Room 3
			{
				newroom();
				room2 = false;
				room3 = true;
			}
		}
		while (room3 == true)
		{
			system("CLS");
			cout << "The wooden sign in the middle of the room reads: Go in the direction you went two rooms ago\n" << endl; //Forwards
			directions();
			char roomchoice3;
			cin >> roomchoice3;
			if ((roomchoice3 == '1') || (roomchoice3 == '3')) // Back to Room 1 
			{
				roomreturn1();
				if ((int)lostcounter.size() == 2)
				{
					losttext();
					mazepuzzleactive = false;
					room3 = false;
					break;
				}
				roomreturn2();
				room3 = false;
				room1 = true;
			}
			else if (roomchoice3 == '2') // Move to Room 4
			{
				newroom();
				room3 = false;
				room4 = true;
			}
		}
		while (room4 == true)
		{
			system("CLS");
			cout << "The wooden sign in the middle of the room reads: Go Right\n" << endl; // Right
			directions();
			char roomchoice4;
			cin >> roomchoice4;
			if ((roomchoice4 == '1') || (roomchoice4 == '2')) // Back to Room 1 
			{
				roomreturn1();
				if ((int)lostcounter.size() == 2)
				{
					losttext();
					mazepuzzleactive = false;
					room4 = false;
					break;
				}
				roomreturn2();
				room4 = false;
				room1 = true;
			}
			else if (roomchoice4 == '3') // Move to Room 5
			{
				newroom();
				room4 = false;
				room5 = true;
			}
		}
		while (room5 == true)
		{
			system("CLS");
			cout << "The wooden sign in the middle of the room reads: Go in the direction you went in the second room\n" << endl; // Left
			directions();
			char roomchoice5;
			cin >> roomchoice5;
			if ((roomchoice5 == '2') || (roomchoice5 == '3')) // Back to Room 1 
			{
				roomreturn1();
				if ((int)lostcounter.size() == 2)
				{
					losttext();
					mazepuzzleactive = false;
					room5 = false;
					break;
				}
				roomreturn2();
				room5 = false;
				room1 = true;
			}
			else if (roomchoice5 == '1') // Move to Room 6
			{
				newroom();
				room5 = false;
				room6 = true;
			}
		}
		while (room6 == true)
		{
			system("CLS");
			cout << "The wooden sign in the middle of the room reads: Go in the direction you went two rooms ago\n" << endl; // Right
			directions();
			char roomchoice6;
			cin >> roomchoice6;
			if ((roomchoice6 == '1') || (roomchoice6 == '2')) // Back to Room 1 
			{
				roomreturn1();
				if ((int)lostcounter.size() == 2)
				{
					losttext();
					mazepuzzleactive = false;
					room6 = false;
					break;
				}
				roomreturn2();
				room6 = false;
				room1 = true;
			}
			else if (roomchoice6 == '3') // Exit Maze
			{
				system("CLS");
				cout << "With some luck you manage to navigate to the other end of the maze.\n" << endl;
				system("PAUSE");
				mazecomplete = true;
				mazepuzzleactive = false;
				room6 = false;
			}
		}
		if ((int)lostcounter.size() == 2) // If lost 3 times ENDING 3
		{
			break;
		}
		if (mazecomplete == true) // Maze Solved
		{
			break;
		}
	}
}
