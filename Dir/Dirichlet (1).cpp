#pragma once
#include "Dirichlet.h"

using namespace std;


double function(double x, double y)
{
	return sin((3.14159265)* x * y);
}

/*
Nmax - Максимально допустимое число итераций
E - минимально допустимый прирост приближенного решения на соседних итерациях 
s>= Nmax - выход по числу итераций 
|| x(s) - x(s-1)|| <= E -  выход по точности 
*/

	void Diriclet(int Nmax, double E, int n, int m)
	{
		int S = 0; // Число проведенных итераций 
		double Emax = 0;
		double Ecur = 0; 
		double a2, k2, h2; // ненулевые элементы матрицы А кудрявая 

		vector<vector<double>> v(n + 1); // искомый вектор 
		vector<vector<double>> f(n + 1);
		for (int i = 0; i < n + 1; i++)
		{
			v[i].resize(m + 1);
			f[i].resize(m + 1);
		}

		//Из методички 
		double a = 1.0, b = 2.0, c = 2.0, d = 3.0;
		int i, j;
		double v_old;
		double v_new;
		bool flag = false;
		double h = (b - a) / n;
		double k = (d - c) / m;
		h2 = (-1.0)*(n / (b - a)) * (n / (b - a));
		k2 = (-1.0)*(m / (d - c)) * (m / (d - c));
		a2 = (-2.0)*(h2 + k2);

		//Правая часть 
		for (int j = 0; j < m + 1; j++ )
		{
			for (int i = 0; i < n + 1; i++)
			{
				double xi = a + i * h;
				double yj = c + j * k;
				//-f = u"xx + u"yy;
				double fi = (3.14159265) * (3.14159265) * (xi * xi + yj * yj) * (function(xi, yj));
				f[i][j] = fi;
			}
		}

		// Граничные условия 
		for (int i = 0; i < n + 1; i++)
		{
			double xi = a + (double)i * h;

			v[i][0] = function(xi, c);
			v[i][m] = function(xi, d);
		}

		for (int j = 0; j < m + 1; j++)
		{
			double yj = c + j * k;

			v[0][j] = function(a, yj);
			v[n][j] = function(b, yj);
		}

		//Из методички 
		while (!flag)
		{
			Emax = 0;
			for (j = 1; j < m; j++)
			{
				for (i = 1; i < n; i++)
				{
					v_old = v[i][j];
					v_new = (-1.0)*(h2 * (v[i + 1][j] + v[i - 1][j]) + k2 * (v[i][j + 1] + v[i][j - 1]));
					v_new = v_new + f[i][j];
					v_new = v_new / a2;
					Ecur = fabs(v_old - v_new); // модуль 
					if (Ecur > Emax)
					{
						Emax = Ecur;
					}
					v[i][j] = v_new;
				}
			}
			S++;
			if (Emax < E || S >= Nmax)
			{
				flag = true;
				cout << "Emax dostig = " << Emax << endl;
			}
		}

		// Точность 
		double xi = 0, yj = 0;
		double maxE = 0;
		double curE = 0;
		for (j = 0; j < m + 1; j++)
		{
			for (i = 0; i < n + 1; i++)
			{
				xi = (double)i * h + a;
				yj = (double)j * k + c;

				curE = fabs(function(xi, yj) - v[i][j]);
				if (curE >= maxE)
				{
					maxE = curE;
				}
			}
		}


		//Вывод 
		cout << endl;
		cout << "Nmax = " << Nmax << endl;
		cout << "eps = " << E << endl;
		cout << " S = " << S << endl;
		cout << "eps_max = " << maxE << endl;
		cout << endl;
		for (int j = m ; j > -1; j--)
		{
			for (int i = 0; i < n + 1; i++)
			{
				cout << v[i][j] << "               ";
			}

			cout << endl;
		}

	}
