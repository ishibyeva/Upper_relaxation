#include<iostream>
#include "Dirichlet.h"

using namespace std;

int main()
{
	int Nmax = 0, n , m;
	double E;
	cout << "Enter Nmax = " ;
	cin >> Nmax;
	cout << "Enter E = " ;
	cin >> E;
	cout << "Enter n , m " ;
	cin >> n >> m;

	Diriclet(Nmax, E, n, m);

	return 0;
}