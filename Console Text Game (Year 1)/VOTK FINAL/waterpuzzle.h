#include <iostream>
#include <vector>
using namespace std;
//
// WATER PUZZLE
//
void prewaterpuzzletext()
{
	system("CLS");
	cout << "As you reach the bottom of the stairs you find yourself in a large well-lit room." << endl;
	cout << "In the centre is a pool of water." << endl;
	cout << "Beside the pool are three stone containers on what appear to be pressure plates." << endl;
	cout << "At the far end of the room is another large, wooden door." << endl;
	cout << "As with the previous door, it cannot be pushed open, the stone containers must somehow open it.\n" << endl;
	system("PAUSE");
	system("CLS");
	cout << "Upon further inspection of the containers you notice each has an inscription scratched upon them:" << endl;
	cout << "\nThe volume of the first container is the volume of the second divided by the volume of the third." << endl;
	cout << "The volume of the second container is three times the volume of the third." << endl;
	cout << "The volume of the third container is the fourth prime number.\n" << endl;
	cout << "There is what appears to be a bucket behind the first container." << endl;
	cout << "It looks like it could hold roughtly one litre of water.\n" << endl;
	system("PAUSE");
	system("CLS");
}
vector<int>watercontainer1;
vector<int>watercontainer2;
vector<int>watercontainer3;
bool waterpuzzleactive = true;
int volume1 = 0;
int	volume2 = 0;
int volume3 = 0;
void waterpuzzle()
{
	while (waterpuzzleactive == true) // Must fill containers with correct volume of water
	{
		cout << "The volume of the first container is the volume of the second divided by the volume of the third." << endl;
		cout << "The volume of the second container is three times the volume of the third." << endl;
		cout << "The volume of the third container is the fourth prime number.\n" << endl;
		cout << "Volume of water in container:\n" << endl;
		cout << "Container One:" << (int)watercontainer1.size() << endl;
		cout << "Container Two:" << (int)watercontainer2.size() << endl;
		cout << "Container Three:" << (int)watercontainer3.size() << endl;
		cout << "\nWhich container will you fill?\n" << endl;
		cout << "1. Container One" << endl;
		cout << "2. Container Two" << endl;
		cout << "3. Container Three" << endl;
		cout << "4. Empty Containers\n" << endl;
		char containerchoice;
		cin >> containerchoice;
		if (containerchoice == '1')
		{
			cout << "\nHow many litres of water will you fill it with?\n" << endl; // Fills first container 
			cin >> volume1;
			for (int i = 0; i < volume1; i++) watercontainer1.push_back(1);
		}
		if (containerchoice == '2')
		{
			cout << "\nHow many litres of water will you fill it with?\n" << endl; // Fills second container
			cin >> volume2;
			for (int i = 0; i < volume2; i++) watercontainer2.push_back(1);
		}
		if (containerchoice == '3')
		{
			cout << "\nHow many litres of water will you fill it with?\n" << endl; // Fills third container
			cin >> volume3;
			for (int i = 0; i < volume3; i++) watercontainer3.push_back(1);
		}
		if (containerchoice == '4') // Empties containers of water
		{
			watercontainer1.clear();
			watercontainer2.clear();
			watercontainer3.clear();
		}
		system("CLS");
		if ((int)watercontainer1.size() == 3) // Correct Container Volume 1
		{
			cout << "The first pressure plate activates." << endl;
		}
		if ((int)watercontainer2.size() == 21) // Correct Container Volume 2
		{
			cout << "The second pressure plate activates." << endl;
		}
		if ((int)watercontainer3.size() == 7) // Correct Container Volume 3
		{
			cout << "The third pressure plate activates." << endl;
		}
		if ((int)watercontainer1.size() == 3 && (int)watercontainer2.size() == 21 && (int)watercontainer3.size() == 7)
		{
			waterpuzzleactive = false;
		}
	}
}
