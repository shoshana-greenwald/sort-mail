using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll
{
    public static class ChiSquare
    {
        // פונקציה סטטיסטית לחישוב האם המילה בעלת השפעה על נושא/נושאים מסויימים או למילה אין כל השפעה על נושא מסוים //
        public static double Chi(List<Statistic> s)
        {
            long sumn = 0, sumy = 0;
            double[,] probs = new double[s.Count + 1, 3];
            for (int i = 0; i < s.Count; i++)// בונה מטריצה מהרשימה
            {
                probs[i, 0] = s[i].Yes;
                probs[i, 1] = s[i].No;
                probs[i, 2] = s[i].Yes + s[i].No;
                sumy += s[i].Yes;
                sumn += s[i].No;
            }
            probs[s.Count, 0] = sumy;
            probs[s.Count, 1] = sumn;
            probs[s.Count, 2] = sumy + sumn;
            double[,] expected = new double[s.Count, 2];
            for (int i = 0; i < s.Count; i++)
            {
                expected[i, 0] = probs[i, 2] * probs[s.Count, 0] / probs[s.Count, 2];
                expected[i, 1] = probs[i, 2] * probs[s.Count, 1] / probs[s.Count, 2];
            }
            double sum = 0.0;
            for (int i = 0; i < s.Count; i++)
                for (int j = 0; j < 2; j++)
                {
                    if (expected[i, j] != 0)
                        sum += ((probs[i, j] - expected[i, j]) *
                            (probs[i, j] - expected[i, j])) / expected[i, j];
                }
            sum *= 100 / probs[s.Count, 2];
            double res = ChiSquarePval(sum, s.Count - 1);
            //הוקטורים תלויים
            if (res == -1)
            {
                //אם זה תלות בגלל אפס
                if (s[0].No == 0 || s[0].Yes == 0)
                    return 0;
                //הוקטורים הם מכפלה אחד של השני
                return 1;
            }

            for (int i = 0; i < s.Count; i++)
            {
                if (s[i].Yes == 0)
                    return 0;
            }
            return res;
        }



        // chi פונקצית עזר לפונקצית
        public static double ChiSquarePval(double x, int df)
        {
            // x = a computed chi-square value.
            // df = degrees of freedom.
            // output = prob. x value occurred by chance.
            // ACM 299.

            //כשהסכום שווה לאפס אז הוקטורים תלויים לינארית הוא מחזיר תוצאה של מינוס אחד וצריך לבדוק האם התלות גדולה
            if (x == 0)
                return -1;

            if (x <= 0.0 || df < 1)
                throw new Exception("Bad arg in ChiSquarePval()");
            double a = 0.0; // 299 variable names
            double y = 0.0;
            double s = 0.0;
            double z = 0.0;
            double ee = 0.0; // change from e
            double c;
            bool even; // Is df even?
            a = 0.5 * x;
            if (df % 2 == 0) even = true; else even = false;
            if (df > 1) y = Exp(-a); // ACM update remark (4)
            if (even == true) s = y;
            else s = 2.0 * Gauss(-Math.Sqrt(x));



            if (df > 2)
            {
                x = 0.5 * (df - 1.0);
                if (even == true) z = 1.0; else z = 0.5;
                if (a > 40.0) // ACM remark (5)
                {
                    if (even == true) ee = 0.0;
                    else ee = 0.5723649429247000870717135;
                    c = Math.Log(a); // log base e
                    while (z <= x)
                    {
                        ee = Math.Log(z) + ee;
                        s = s + Exp(c * z - a - ee); // ACM update remark (6)
                        z = z + 1.0;
                    }
                    return s;
                } // a > 40.0
                else
                {
                    if (even == true) ee = 1.0;
                    else
                        ee = 0.5641895835477562869480795 / Math.Sqrt(a);
                    c = 0.0;
                    while (z <= x)
                    {
                        ee = ee * (a / z); // ACM update remark (7)
                        c = c + ee;
                        z = z + 1.0;
                    }
                    return c * y + s;
                }
            }
            else
            {
                return s;
            }
        } 


        private static double Exp(double x) // ACM update remark (3)
        {
            if (x < -40.0) // ACM update remark (8)
                return 0.0;
            else
                return Math.Exp(x);
        }


        private static double Gauss(double z)
        {
            // ACM Algorithm #209
            double y; // 209 scratch variable
            double p; // result. called 'z' in 209
            double w; // 209 scratch variable

            if (z == 0.0)
                p = 0.0;
            else
            {
                y = Math.Abs(z) / 2;
                if (y >= 3.0)
                {
                    p = 1.0;
                }
                else if (y < 1.0)
                {
                    w = y * y;
                    p = ((((((((0.000124818987 * w
                      - 0.001075204047) * w
                      + 0.005198775019) * w
                      - 0.019198292004) * w + 0.059054035642) * w
                      - 0.151968751364) * w + 0.319152932694) * w
                      - 0.531923007300) * w + 0.797884560593) * y * 2.0;
                }
                else
                {

                    y = y - 2.0;
                    p = (((((((((((((-0.000045255659 * y
                      + 0.000152529290) * y - 0.000019538132) * y
                      - 0.000676904986) * y + 0.001390604284) * y
                      - 0.000794620820) * y - 0.002034254874) * y
                     + 0.006549791214) * y - 0.010557625006) * y
                    + 0.011630447319) * y - 0.009279453341) * y
                   + 0.005353579108) * y - 0.002141268741) * y
                  + 0.000535310849) * y + 0.999936657524;
                }
            }

            if (z > 0.0)
                return (p + 1.0) / 2;
            else
                return (1.0 - p) / 2;
        } 

    }




    public class Statistic
    {

        public Statistic(int s, int y, int n)
        {
            this.subjectId = s;
            this.Yes = y;
            this.No = n;
        }

        public int subjectId { get; set; }
        public int Yes { get; set; }
        public int No { get; set; }
    }

}

