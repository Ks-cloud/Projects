using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public class Point
    {
        public double distance = 0;
        public string cluster, nameStrg;
        public int nameInt;
        public int CoordX, CoordY;

        public Point(int nameInt, int CoordX, int CoordY)
        {
            this.nameInt = nameInt;
            this.CoordX = CoordX;
            this.CoordY = CoordY;
        }

        public Point(string nameStrg, int CoordX, int CoordY)
        {
            this.nameStrg = nameStrg;
            this.CoordX = CoordX;
            this.CoordY = CoordY;
        }
    }

    public class Cluster : Point
    {
        public Cluster(string name, int CoordX, int CoordY) : base(name, CoordX, CoordY)
        {
            Print();
        }
        public void Print()
        {
            Console.WriteLine("Cluster name is " + nameStrg);
            Console.WriteLine("Coordinate X of centroid " + nameStrg + " is " + CoordX);
            Console.WriteLine("Coordinate Y of centroid " + nameStrg + " is " + CoordY);
        }

    }

    class MainClass
    {
        public static void Main(string[] args)
        {
            // method for random values generation
            int RandomValue()
            {
                Random randNum = new Random();
                int newNum = randNum.Next(1, 15);
                return newNum;
            }

            Console.WriteLine("************************************");
            Console.WriteLine("Centroids");
            // centroids initialization
            int k = 4;
            int[,] centroids = new int[k, 2];

            // centroids random assignment
            List<Cluster> clusters = new List<Cluster>();

            for (int i = 0; i < k; i++)
            {
                int coordX = 0;
                int coordY = 0;
                for (int j = 0; j < 2; j++)
                {

                    centroids[i, j] = RandomValue() + (3 * i + 2 * j);
                    if (j == 0)
                    {
                        coordX = centroids[i, j];
                    }
                    else if (j == 1) { coordY = centroids[i, j]; }
                }

                string centroidName = "centroid" + Convert.ToString(i + 1);
                Console.WriteLine(centroidName);
                clusters.Add(new Cluster(centroidName, coordX, coordY));
            }


            // initial array initialization
            Console.WriteLine("************************************");
            Console.WriteLine("Initial 2D array");
            List<Point> points = new List<Point>();

            int n = 15;
            int[,] initArray2D = new int[n, 2];
            for (int i = 0; i < n; i++)
            {
                int CoordX = 0;
                int CoordY = 0;
                for (int j = 0; j < 2; j++)
                {
                    initArray2D[i, j] = i + 2 * j;

                    if (j == 0)
                    {
                        CoordX = initArray2D[i, j];
                    }
                    else if (j == 1)
                    {
                        CoordY = initArray2D[i, j];
                    }
                }
                points.Add(new Point((i + 1), CoordX, CoordY));
            }

            /*
            // points display / output
            foreach (Point point in points)
            {
                Console.WriteLine("Point number " + point.nameInt);
                Console.WriteLine("X: " + point.CoordX);
                Console.WriteLine("Y: " + point.CoordY);
            }
            */

            // method for distances calculation (between two points)
            double calcDist(int x1, int y1, int x2, int y2)
            {
                return (Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2)));
            }

            Console.WriteLine("The length of clusters list is " + clusters.Count);
            Console.WriteLine("The length of points list is " + points.Count);

            // create matrix for distances
            double[,] results = new double[k, n];

            // create variable res for calcDist (pair of points)
            double res = 0;

            void Calc()
            {
                for (int i = 0; i < clusters.Count; i++)
                {
                    for (int j = 0; j < points.Count; j++)
                    {
                        res = Math.Round(calcDist(clusters[i].CoordX, clusters[i].CoordY, points[j].CoordX, points[j].CoordY), 2);
                        if (i == 0)
                        {
                            points[j].distance = res;
                            points[j].cluster = clusters[i].nameStrg;
                        }
                        else if (res < points[j].distance)
                        {
                            points[j].distance = res;
                            points[j].cluster = clusters[i].nameStrg;
                        }

                        // create the matrix of distances
                        results[i, j] = res;
                        Console.Write("{0}\t", results[i, j]);
                    }
                }

                // display the points with cluster names
                for (int i = 0; i < points.Count; i++)
                {
                    Console.WriteLine("\nPoint number: " + points[i].nameInt);
                    Console.WriteLine("Point distance to cluster center: " + points[i].distance);
                    Console.WriteLine("Point cluster: " + points[i].cluster);
                }
            }

            // objective function calculation
            double ObjFuncCalc()
            {
                double ObjFunction = 0;
                for (int i = 0; i < points.Count; i++)
                {
                    ObjFunction += points[i].distance;
                }
                return ObjFunction;
            }

            // new centroids initialization
            void newCenters()
            {
                for (int i = 0; i < clusters.Count; i++)
                {
                    int sumX = 0, sumY = 0, count = 0;
                    double sumDistance = 0;
                    for (int j = 0; j < points.Count; j++)
                    {
                        if (clusters[i].nameStrg == points[j].cluster)
                        {
                            sumDistance += points[i].distance;
                            sumX += points[j].CoordX;
                            sumY += points[j].CoordY;
                            count += 1;
                        }
                    }

                    /*
                    Console.WriteLine("SumX for cluster " + clusters[i].nameStrg + " is " + sumX);
                    Console.WriteLine("SumY for cluster " + clusters[i].nameStrg + " is " + sumY);
                    Console.WriteLine("Count for cluster " + clusters[i].nameStrg + " is " + count);
                    */
                    // reassign cluster center coordinates 

                    if (count != 0)
                    {
                        clusters[i].CoordX = sumX / count;
                        clusters[i].CoordY = sumY / count;
                    }
                }

                // display new cluster centers

                for (int i = 0; i < clusters.Count; i++)
                {
                    Console.WriteLine("\nCluster CoordX: " + clusters[i].CoordX);
                    Console.WriteLine("Cluster CoordY: " + clusters[i].CoordY);
                    Console.WriteLine("Cluster center: " + clusters[i].nameStrg);
                }
            }

            double minValue = 0;
            for (int i = 1; i <= 10; i++)
            {
                Console.WriteLine("\nIteration " + i + "\n");
                Calc();
                ObjFuncCalc();
                Console.WriteLine("\nThe value of objective function is equal to " + ObjFuncCalc() + "\n");
                newCenters();

                if (i == 1)
                {
                    minValue = ObjFuncCalc();
                    newCenters();
                }
                else if (ObjFuncCalc() < minValue)
                {
                    minValue = ObjFuncCalc();
                }
                else if (ObjFuncCalc() == minValue)
                {
                    break;
                }
            }
            Console.ReadKey();
        }
    }
}





