#include <iostream>
#include <vector>
using namespace std;
bool switchpuzzleactive = true;
vector<int>switch2;
vector<int>switch3;
vector<int>switch4;
vector<int>switch5;
int switchchoice2 = 0;
int switchchoice3 = 0;
int switchchoice4 = 0;
int switchchoice5 = 0;

void prefinalpuzzletext()
{
	system("CLS");
	cout << "You are not scared by curses or legend." << endl;
	cout << "Opening the sarcophagus could only bring you great fortune.\n" << endl;
	system("PAUSE");
	system("CLS");
	cout << "You look around the room and see what appears to be one hundred switches along the walls, each numbered." << endl;
	cout << "Along the sarcophagus, several letters and numbers have been inscribed." << endl;
	cout << "The seventh switch has already been pulled down.\n" << endl;
	system("PAUSE");
	system("CLS");
}

void endtext()
{
	system("CLS");
	cout << "Upon pulling the final switch you hear the lid of the sarcophagus move.\n" << endl;
	cout << "Treasure awaits you...\n" << endl;
	system("PAUSE");
	system("CLS");
	cout << "With the now lid removed you can finally feast your eyes upon the contents of the sarcophagus." << endl;
	cout << "What was once a moving, breathing, living human is no more than a decaying mummy." << endl;
	cout << "You look around the body but can find no treasure.\n" << endl;
	system("PAUSE");
	system("CLS");
	cout << "For thousands of years this tomb remained unspoiled." << endl;
	cout << "The body, peacefully at rest." << endl;
	cout << "Though because of you, that is no longer true...\n" << endl;
	system("PAUSE");
	system("CLS");
	cout << "Was it really worth it?\n\n";
	system("PAUSE");
	system("CLS");
	cout << "GAME OVER" << endl;
	cout << "ENDING 4 UNLOCKED!\n" << endl;
	system("PAUSE");
}

void finalpuzzletext()
{
	while (switchpuzzleactive == true)
	{
		cout << "P = 7" << endl;
		cout << "P + Q = 13" << endl;
		cout << "2P + Q + R = 30" << endl;
		cout << "3P + 2Q + R + S = 93" << endl;
		cout << "P + Q + 3R + 2S + T = 164\n" << endl;
		cout << "Combination of Switches:" << endl;
		cout << "First Switch: 7" << endl;
		cout << "Second Switch: " << (int)switch2.size() << endl;
		cout << "Third Switch: " << (int)switch3.size() << endl;
		cout << "Fourth Switch: " << (int)switch4.size() << endl;
		cout << "Fifth Switch: " << (int)switch5.size() << "\n" << endl;
		cout << "Which Switch will you pull?\n" << endl;
		cout << "2. Second Switch" << endl;
		cout << "3. Third Switch" << endl;
		cout << "4. Fourth Switch" << endl;
		cout << "5. Fifth Switch" << endl;
		cout << "6. Reset Switches\n" << endl;
		char switchchoice;
		cin >> switchchoice;
		if (switchchoice == '2')
		{
			cout << "\nWhich Numbered Switch will you choose from the wall?\n" << endl; // Chooses Switch 2
			cin >> switchchoice2;
			for (int i = 0; i < switchchoice2; i++) switch2.push_back(1);
		}
		if (switchchoice == '3')
		{
			cout << "\nWhich Numbered Switch will you choose from the wall?\n" << endl; // Chooses Switch 3
			cin >> switchchoice3;
			for (int i = 0; i < switchchoice3; i++) switch3.push_back(1);
		}
		if (switchchoice == '4')
		{
			cout << "\nWhich Numbered Switch will you choose from the wall?\n" << endl; // Chooses Switch 4
			cin >> switchchoice4;
			for (int i = 0; i < switchchoice4; i++) switch4.push_back(1);
		}
		if (switchchoice == '5')
		{
			cout << "\nWhich Numbered Switch will you choose from the wall?\n" << endl; // Chooses Switch 5
			cin >> switchchoice5;
			for (int i = 0; i < switchchoice5; i++) switch5.push_back(1);
		}
		if (switchchoice == '6')
		{
			switch2.clear();
			switch3.clear();
			switch4.clear();
			switch5.clear();
		}
		system("CLS");
		if ((int)switch2.size() == 6 && (int)switch3.size() == 10 && (int)switch4.size() == 50 && (int)switch5.size() == 21)
		{
			switchpuzzleactive = false;
			break;
		}
	}
}
