using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Dir
{
    public partial class Form1 : Form
    {

        int n, m;
        double Nmax, E, W;
        Form2 f2 = new Form2();

        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        double function(double x, double y)
        {
            return Math.Sin((3.14159265) * x * y);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Nmax = int.Parse(textBox1.Text);
            E = double.Parse(textBox2.Text);
            n = int.Parse(textBox3.Text);
            m = int.Parse(textBox4.Text);


            if (n < 2)
            {
                textBox1.Text += " Число разбиений по x должно быть >= 2 ";
            }
            if (m < 2)
            {
                textBox1.Text += (" Число разбиений по y должно быть >= 2");
            }

            f2.linkLabel1.Text = Convert.ToString(n);
            f2.linkLabel2.Text = Convert.ToString(m);
            f2.linkLabel3.Text = Convert.ToString(E);

            //Очистка строк и столбцов таблицы
            dataGridView1.Rows.Clear();
            
            dataGridView2.Rows.Clear();
            

            dataGridView1.RowCount = m + 2;
            dataGridView1.ColumnCount = n + 2;

            dataGridView2.RowCount = 2*m + 2;
            dataGridView2.ColumnCount = 2*n + 2;

            int S = 0; // Число проведенных итераций 
            double Emax = 0; // Текущее значение прироста
            double Ecur = 0;  // Для подсчета значения текущего значения прироста

            double a2, k2, h2; // ненулевые элементы матрицы А кудрявая 


            double[,] v = new double[n + 1, m + 1];  // искомый вектор 
            double[,] vh = new double[2 * n + 1, 2 * m + 1];
            double[,] f = new double[n + 1, m + 1];
            double[,] fh = new double[2 * n + 1, 2 * m + 1];
            double[,] r = new double[n + 1, m + 1]; //  Невязка 

            
            //Из методички 
            double a = 1.0, b = 2.0, c = 2.0, d = 3.0;
            int i, j;
            double v_old;
            double v_new;


            double h = (b - a) / n;
            double k = (d - c) / m;
            h2 = (-1.0) * (n / (b - a)) * (n / (b - a));
            k2 = (-1.0) * (m / (d - c)) * (m / (d - c));
            a2 = (-2.0) * (h2 + k2);
            //параметр омега
            if (checkBox1.Checked == true)
            {
                double arg1 = Math.PI * h / (2 * (b - a));
                double arg2 = Math.PI * k / (2 * (d - c));
                double znam = h * h + k * k;
                double lambdaMin = 2 * k * k / (znam) * Math.Asin(arg1) * Math.Asin(arg1)
                    + 2 * h * h / (znam) * Math.Asin(arg2) * Math.Asin(arg2);
                W = 2 / (1 + Math.Sqrt(lambdaMin * (2 - lambdaMin)));
                
               // W = 2 / (1 + Math.Pow(4 * (Math.Pow(Math.Sin((3.14159265 * h) / 2), 2)), 0.5));

            }
            else
                W = double.Parse(textBox5.Text.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);
            f2.linkLabel8.Text = Convert.ToString(W);

            //Правая часть 
            for (int j1 = 1; j1 < m ; j1++)
                {
                    for (int i1 = 1; i1 < n; i1++)
                    {
                        double xi = a + i1 * h;
                        double yj = c + j1 * k;
                        double fi = 0;
                        //тест
                        if (radioButton1.Checked == true)
                            //-f = u"xx + u"yy;
                            fi = (3.14159265) * (3.14159265) * (xi * xi + yj * yj) * (function(xi, yj));
                        //основная
                        if (radioButton1.Checked == false)
                            fi = Math.Exp((-1) * xi * Math.Pow(yj,2));
                        f[i1, j1] = fi;
                    v[i1, j1] = 0;
                    }
                }

                // Граничные условия 
                for (int i1 = 0; i1 < n + 1; i1++)
                {
                    double xi = a + (double)i1 * h;
                    if (radioButton1.Checked == true)
                    {
                        v[i1, 0] = function(xi, c);
                        v[i1, m] = function(xi, d);
                    }
                    if (radioButton2.Checked == true)
                    {
                        v[i1, 0] = (xi - 1) * (xi - 2);
                        v[i1, m] = xi * (xi - 1) * (xi - 2);
                    }
                }

                for (int j1 = 0; j1 < m + 1; j1++)
                {
                    double yj = c + (double)j1 * k;
                    if (radioButton1.Checked == true)
                    {
                        v[0, j1] = function(a, yj);
                        v[n, j1] = function(b, yj);
                    }
                    if (radioButton2.Checked == true)
                    {
                        v[0, j1] = (yj - 2) * (yj - 3);
                        v[n, j1] = yj * (yj - 2) * (yj - 3);
                    }
                }
            


            bool flag = false;
            //Из методички 
            while (!flag)
            {
                Emax = 0;
                for (j = 1; j < m; j++)
                {
                    for (i = 1; i < n; i++)
                    {
                        v_old = v[i, j];
                        v_new = -W * ((h2 * (v[i + 1,j] + v[i - 1,j]) + k2 * (v[i,j + 1] + v[i,j - 1])));
                        v_new = v_new + (1 - W) * a2 * v[i,j] + W * f[i,j];
                        v_new = v_new / a2;
                        Ecur = Math.Abs(v_old - v_new); // модуль 
                        if (Ecur > Emax)
                        {
                            Emax = Ecur;
                        }
                        v[i, j] = v_new;
                    }
                }
                S++;
                if ((Emax < E) || (S >= Nmax))
                {
                    flag = true;
                    f2.linkLabel5.Text = Convert.ToString(Emax);

                }
            }
            f2.linkLabel4.Text = Convert.ToString(S);

            //получим вектор с половинным шагом 

            if (radioButton2.Checked == true)
            {
                double hh = (b - a) / (2 * n);
                double kh = (d - c) / (2 * m);
                double hh2 = (-1.0) * (2*n / (b - a)) * (2*n / (b - a));
                double kh2 = -(1 / kh) * (1 / kh);
                double ah2 = (-2.0) * (hh2 + kh2);


                //Правая часть 
                for (int j1 = 1; j1 < 2 * m; j1++)
                {
                    for (int i1 = 1; i1 < 2 * n; i1++)
                    {
                        double xi = a + (double)i1 * hh;
                        double yj = c + (double)j1 * kh;
                        double fi = 0;
                        fi = Math.Exp((-1) * xi * yj * yj);
                        fh[i1, j1] = fi;
                    }
                }

                // Граничные условия 
                for (int i1 = 0; i1 < 2*n + 1; i1++)
                {
                    double xi = a + (double)i1 * hh;
                   
                    
                        vh[i1, 0] = (xi - 1) * (xi - 2);
                        vh[i1, 2*m] = xi * (xi - 1) * (xi - 2);
                    
                }

                for (int j1 = 0; j1 < 2*m + 1; j1++)
                {
                    double yj = c + (double)j1 * kh;
                   
                        vh[0, j1] = (yj - 2) * (yj - 3);
                        vh[2*n, j1] = yj * (yj - 2) * (yj - 3);
                    
                }

                //параметр омега для половинного шага
                double Wh;
                if (checkBox1.Checked == true)
                {
                    double sq_h = h * h*0.25, sq_k=k*k*0.25;
                    double minl = 2 * (sq_k / (sq_k + sq_h)) * Math.Pow(Math.Asin((3.14159265 * h) * 0.25), 2) +
                    2 * (sq_h / (sq_k + sq_h)) * Math.Pow(Math.Asin((3.14159265 * k) * 0.25), 2);

                    double SQ = Math.Sqrt(minl * (2 - minl));
                    Wh = 2 / (1 + SQ);
                }
                else
                    Wh = double.Parse(textBox5.Text.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);
                f2.label16.Text = Convert.ToString(Wh);
                bool flag1 = false;
            //Из методички 
            S = 0;
            while (!flag1)
            {
                Emax = 0;
                for (j = 1; j < 2*m; j++)
                {
                    for (i = 1; i < 2*n; i++)
                    {
                        v_old = vh[i, j];
                        v_new = (-Wh) * (hh2 * (vh[i + 1, j] + vh[i - 1, j]) + kh2 * (vh[i, j + 1] + vh[i, j - 1]));
                        v_new = v_new + (1.0 - Wh) * ah2 * vh[i, j] + Wh * fh[i, j];
                        v_new = v_new / ah2;
                        Ecur = Math.Abs(v_old - v_new); // модуль 
                        if (Ecur > Emax)
                        {
                            Emax = Ecur;
                        }
                        vh[i, j] = v_new;
                    }
                }
                S++;
                if ((Emax < E) || (S >= Nmax))
                {
                    flag1 = true;
                    f2.label12.Text = Convert.ToString(Emax);
                        f2.label14.Text = Convert.ToString(S);

                    }
                }

        }
    


            //Невязка 
            double r_evk = 0;
            for (int jj = 1; jj < m; jj++)
            {
                for (int ii = 1; ii < n; ii++)
                {
                    r[ii,jj] = (h2 * (v[ii + 1,jj] + v[ii - 1,jj]) + k2 * (v[ii,jj + 1] + v[ii,jj - 1])) + v[ii,jj] * a2 - f[ii,jj];
                    r_evk += r[ii,jj] * r[ii,jj];
                }
            }
            r_evk = Math.Sqrt(r_evk);
            f2.linkLabel7.Text = Convert.ToString(r_evk);


            // Точность 
            double xi1 = 0.0, yj1 = 0.0;
		    double maxE = 0;
		    double curE = 0;
		    for (j = 0; j < m + 1; j++)
		    {
		      	for (i = 0; i < n + 1; i++)
			    {
			    	
                    if (radioButton1.Checked == true)
                    {
                        xi1 = (double)i * h + a;
                        yj1 = (double)j * k + c;
                        curE = Math.Abs(function(xi1, yj1) - v[i, j]);
                    }
                    else
                        curE = Math.Abs(v[i, j]- vh[2*i,2*j]);
			    	if (curE >= maxE)
			    	{
			    		maxE = curE;
				    }
			    }
		    }
            f2.linkLabel6.Text = Convert.ToString(maxE);

            //X
            dataGridView1.Rows[0].Cells[0].Value = "X / Y ";
            dataGridView2.Rows[0].Cells[0].Value = "X / Y ";

            for (int i1 = 1; i1 < n + 2; i1++)
            {
                xi1 = (double)(i1 - 1.0) * h + a;

                dataGridView1.Rows[0].Cells[i1].Style.BackColor = System.Drawing.Color.OrangeRed;
                dataGridView1.Rows[0].Cells[i1].Value = xi1;
            }
            for (int i1 = 1; i1 < 2*n + 2; i1++)
            {
                xi1 = (double)(i1 - 1.0) * h*0.5 + a;

                dataGridView2.Rows[0].Cells[i1].Style.BackColor = System.Drawing.Color.OrangeRed;
                dataGridView2.Rows[0].Cells[i1].Value = xi1;
            }
            //Y

            int rr = 1;
            for (int j1 = m + 1 ; j1 > 0; j1--)
            {
                yj1 = (double)(rr - 1) * k + c;

                dataGridView1.Rows[j1].Cells[0].Style.BackColor = System.Drawing.Color.OrangeRed;
                dataGridView1.Rows[j1].Cells[0].Value = yj1;

                rr++;
            }
            rr = 1;
            for (int j1 = 2*m + 1; j1 > 0; j1--)
            {
                yj1 = (double)(rr - 1) * k*0.5 + c;

                dataGridView2.Rows[j1].Cells[0].Style.BackColor = System.Drawing.Color.OrangeRed;
                dataGridView2.Rows[j1].Cells[0].Value = yj1;

                rr++;
            }

            //table
            for (int j1 = 1; j1 < m + 2; j1++)
            {
                for (int i1 = 1; i1 < n + 2; i1++)
                {
                    //dataGridView1.Rows[j1].Cells[i1].Style.BackColor = System.Drawing.Color.SlateGray;
                    dataGridView1.Rows[j1].Cells[i1].Value = v[i1 - 1,m + 1 - j1];
                }

            }
            for (int j1 = 1; j1 < 2*m + 2; j1++)
            {
                for (int i1 = 1; i1 < 2*n + 2; i1++)
                {
                    //dataGridView1.Rows[j1].Cells[i1].Style.BackColor = System.Drawing.Color.SlateGray;
                    dataGridView2.Rows[j1].Cells[i1].Value = vh[i1 - 1, 2*m + 1 - j1];
                }

            }

           

        }


        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            f2.Show();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
