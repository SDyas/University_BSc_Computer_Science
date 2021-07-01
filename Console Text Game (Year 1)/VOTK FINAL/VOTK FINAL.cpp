#include <iostream> // Approx Lines of Code: 894
#include <string>
#include <vector>
#include "menutext.h"
#include "doorpuzzle.h"
#include "snakepuzzle.h"
#include "waterpuzzle.h"
#include "mazepuzzle.h"
#include "sarcophagus.h"
using namespace std;

int main()
{
	string achievement[5]; //Declares Achievements Array (all initially locked)
	achievement[0] = "Ending 1: Locked";
	achievement[1] = "Ending 2: Locked";
	achievement[2] = "Ending 3: Locked";
	achievement[3] = "Ending 4: Locked";

	bool gameon = true;
	while (gameon = true)
	{
		bool predoorpuzzle = false;//Introduction
		bool doorpuzzle = false; //DoorPuzzle
		bool doorpuzzlepathA = false;//Chooses Asp
		bool doorpuzzlepathB = false;//Chooses Tomb
		bool waterpuzzletext = false;//Water Puzzle
		bool mazepuzzle = false;//MazePuzzle
		bool mazepuzzlechoice = false;
		bool finalpuzzle = false;//FinalPuzzle
		bool inmenu = true;//MainMenu
		while (inmenu == true)
		{
			menutext(); //Displays Main Menu text
			char menuinput;
			cin >> menuinput;
			switch (menuinput)
			{
			case ('1'): //Declares inmenu false and starts game
				inmenu = false;
				break;
			case ('2'): //Displays Achievements then returns to Main Menu
				system("CLS");
				cout << "Achievements:" << endl;
				cout << achievement[0] << endl;
				cout << achievement[1] << endl;
				cout << achievement[2] << endl;
				cout << achievement[3] << "\n" << endl;
				system("PAUSE");
				break;
			case ('3'): //Displays Credits then returns to Main Menu
				menuchoice3();
				break;
			}
		}
		developerchoice(); // Level Select/Developer Options
		char developerchoice;
		cin >> developerchoice;
		if (developerchoice == '1')
		{
			developermode();
			char levelselect;
			cin >> levelselect;
			if (levelselect == '1')
			{
				system("CLS");
				doorpuzzle = true;
			}
			else if (levelselect == '2')
			{
				system("CLS");
				doorpuzzlepathA = true;
			}
			else if (levelselect == '3')
			{
				system("CLS");
				doorpuzzlepathB = true;
			}
			else if (levelselect == '4')
			{
				system("CLS");
				mazepuzzle = true;
			}
			else if (levelselect == '5')
			{
				system("CLS");
				finalpuzzle = true;
			}
			else if (levelselect == '6')
			{
				system("CLS");
				inmenu = true;
			}
		}
		//
		// DOOR PUZZLE
		//
		else
		{
			predoorpuzzle = true;
			system("CLS");
			cout << "For thousands of years the Egyption Valley of Kings has been the final resting place for Pharaohs.\n" << endl;
			cout << "Many adventurers, like you, have tried to uncover the secrets of the tombs." << endl;
			cout << "And perhaps even treasure...\n" << endl;
			system("PAUSE");
		}
		while (predoorpuzzle == true)
		{

			predoorpuzzlechoicetext();
			char prepuzzle1choice;
			cin >> prepuzzle1choice;
			if (prepuzzle1choice == '1') //PrePuzzle1Choice1
			{
				predoorpuzzlechoice1();
			}
			else if (prepuzzle1choice == '2') //PrePuzzle1Choice2
			{
				predoorpuzzlechoice2();
				predoorpuzzle = false;
				doorpuzzle = true;
			}
		}
		while (doorpuzzle == true) //Door Puzzle
		{
			doorpuzzlepicture();
			char doorpuzzlechoice;
			cin >> doorpuzzlechoice;

			if (doorpuzzlechoice == '1')//Chooses Asp (Path A)
			{
				doorpuzzlechoice1();
				doorpuzzle = false;
				doorpuzzlepathA = true;
			}
			else if (doorpuzzlechoice == '4')//Chooses Tomb (Path B)
			{
				doorpuzzlechoice4();
				doorpuzzle = false;
				doorpuzzlepathB = true;
			}
			else //Chooses any other option (Ending 1)
			{
				doorpuzzlechoiceelse();
				achievement[0] = "Ending 1: Between a Rock and A Hard Place";

				doorpuzzle = false;
				inmenu = true; //Returns To Main Menu
			}
			break;
		}
		//
		// PATH A1 SNAKES
		//
		while (doorpuzzlepathA == true) //PathA1
		{
			snakepuzzletext();
			if (snakepuzzlecomplete == true)
			{
				doorpuzzlepathA = false;
				mazepuzzle = true;
				break;
			}
			else if (snakepuzzlecomplete == false)
			{
				achievement[1] = "Ending 2: Had To Be Snakes";
				doorpuzzlepathA = false;
				inmenu = true;
				break;
			}
		}
		//
		// PATH B1 WATER
		//
		while (doorpuzzlepathB == true) //PathB1
		{
			prewaterpuzzletext();
			waterpuzzle();
			system("CLS");
			cout << "As the final pressure plate activates the door opens.\n" << endl;
			system("PAUSE");
			doorpuzzlepathB = false;
			mazepuzzle = true;

		}
		//
		// PATH B2 MAZE
		//
		while (mazepuzzle == true)
		{
			watercontainer1.clear();
			watercontainer2.clear();
			watercontainer3.clear();
			mazepuzzleintro();
			mazepuzzletext();
			if ((int)lostcounter.size() == 2)
			{
				lostcounter.clear();
				achievement[2] = "Ending 3: Alone in The Dark";
				mazepuzzle = false;
				break;
				inmenu = true;
			}
			if (mazecomplete == true)
			{
				mazecompletetext();
				lostcounter.clear();
				mazepuzzle = false;
				mazecomplete = false;
				finalpuzzle = true;
				break;
			}
		}
		//
		// FINAL PUZZLE
		//
		while (finalpuzzle == true)
		{
			prefinalpuzzletext();
			finalpuzzletext();
			finalpuzzle = false;
			endtext();
			achievement[3] = "Ending 4: Tomb Raider";
		}
	}
	return 0;
}