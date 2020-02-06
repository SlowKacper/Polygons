using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab1
{
    public partial class Form1 : Form
    {
        Bitmap MyBitmap = new Bitmap(815, 555);
        List<Point>[] Polygons = new List<Point>[11];
        bool[] Active = new bool[11];
        List<(Point, Point)>[] RelationE1 = new List<(Point, Point)>[11];
        List<(Point, Point)>[] RelationE2 = new List<(Point, Point)>[11];
        int[] HasRealtion = new int[11];
        Colors Col = new Colors();
        int activaded = -1;
        private int action = 0;
        int Howdraw = 1;

        public Form1()
        {
            InitializeComponent();
            Polygons = new List<Point>[11];
            Active = new bool[11];
            MyBitmap = new Bitmap(815, 555);
            List<Point> p = new List<Point>();
            RelationE1 = new List<(Point, Point)>[11];
            HasRealtion = new int[11];
            RelationE2 = new List<(Point, Point)>[11];
            p.Add(new Point(250, 150));
            p.Add(new Point(325, 100));
            p.Add(new Point(400, 150));
            p.Add(new Point(400, 250));
            p.Add(new Point(325, 300));
            p.Add(new Point(250, 250));
            Polygons[0] = p;
            //List<(Point, Point)> tmp = new List<(Point, Point)>();
            //tmp.Add((new Point(250, 150), new Point(325, 100)));
            //RelationE1[0] = tmp;
            //tmp.RemoveAt(0);
            //tmp.Add((new Point(400, 250), new Point(325, 300)));
            //RelationE2[0] = tmp;
            //HasRealtion[0] = 1;
            Polygons[0] = p;
            Active[0] = true;
            Print();
        }


        private void Print()
        {
            MyBitmap = new Bitmap(815, 555);
            for (int z = 0; z < Active.Length; z++)
            {
                if (Active[z])
                {
                    DrawEdge(z);

                    // BresenhamLine(Polygons[z][Polygons[z].Count() - 1], Polygons[z][0]);
                    if (Howdraw == 1)
                        BresenhamLine(Polygons[z][Polygons[z].Count() - 1], Polygons[z][0]);
                    else if (Howdraw == 2)
                        drawAnti(MyBitmap, Polygons[z][Polygons[z].Count() - 1], Polygons[z][0]);
                    else if(Howdraw==3)
                        BresenhamLineSym(Polygons[z][Polygons[z].Count() - 1], Polygons[z][0]);
                    DrawVertex(z);
                }
                else if (activaded >= 0)
                {
                    DrawEdge(activaded);
                    DrawVertex(activaded);
                }
            }
            pictureBox1.Image = MyBitmap;
        }
        private void DrawEdge(int poly)
        {
            for (int i = 0; i < Polygons[poly].Count(); i++)
            {
                if (i >= 1)
                {
                    if (Howdraw == 1)
                        BresenhamLine(Polygons[poly][i - 1], Polygons[poly][i]);
                    else if (Howdraw == 2)
                        drawAnti(MyBitmap, Polygons[poly][i - 1], Polygons[poly][i]);
                    else if(Howdraw==3)
                        BresenhamLineSym(Polygons[poly][i - 1], Polygons[poly][i]);
                    //grap.DrawLine(p, po[i - 1], po[i]);
                }
            }

        }
        private void DrawVertex(int poly)
        {
            List<Point> po = Polygons[poly];
            Graphics grap = Graphics.FromImage(MyBitmap);
            Pen p = new Pen(Color.Black);
            for (int i = 0; i < po.Count(); i++)
            {
                for (int x = po[i].X - 6; x < po[i].X + 5; x++)
                {
                    for (int y = po[i].Y - 6; y < po[i].Y + 5; y++)
                    {
                        MyBitmap.SetPixel(x, y, Col.Colo[poly]);
                    }
                }
            }

        }
        private int FindVertix(Point a, int activ)
        {

            double dist = Math.Sqrt((Polygons[activ][0].X - a.X) * (Polygons[activ][0].X - a.X) + (Polygons[activ][0].Y - a.Y) * (Polygons[activ][0].Y - a.Y));
            int ind = 0;
            for (int i = 1; i < Polygons[activ].Count; i++)
            {
                if (dist > Math.Sqrt((Polygons[activ][i].X - a.X) * (Polygons[activ][i].X - a.X) + (Polygons[activ][i].Y - a.Y) * (Polygons[activ][i].Y - a.Y)))
                {
                    dist = Math.Sqrt((Polygons[activ][i].X - a.X) * (Polygons[activ][i].X - a.X) + (Polygons[activ][i].Y - a.Y) * (Polygons[activ][i].Y - a.Y));
                    ind = i;
                }
            }
            return ind;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {

            int x, y;
            x = e.Location.X;
            y = e.Location.Y;
            Point a = new Point(x, y);
            if (action == 1)
            {
                //int r, g, b;
                //(r, g, b) = Col.Colo[activaded];
                Color w = Color.FromArgb(0, 0, 0, 0);

                if (a.X < 829 && a.Y < 594 && a.X >= 6 && a.Y >= 6)
                {
                    if (MyBitmap.GetPixel(a.X, a.Y).ToArgb() == w.ToArgb())
                    {
                        Polygons[activaded].Add(a);
                    }
                    else if (MyBitmap.GetPixel(a.X, a.Y).ToArgb() == Col.Colo[activaded].ToArgb())
                    {
                        int ind = FindVertix(a, activaded);
                        if (ind == 0 && Polygons[activaded].Count() > 2)
                        {
                            action = 0;
                            Active[activaded] = true;
                            activaded = -1;
                        }
                    }
                }
                Print();
            }
            else if (action == 2 && MyBitmap.GetPixel(e.Location.X, e.Location.Y).ToArgb() == Color.Black.ToArgb())
            {
                (P1, V1, V2) = FindEdge(e.Location);
                if (HasRealtion[P1] > 0 && RelationE1[P1].Contains((Polygons[P1][V1], Polygons[P1][V2])))
                {
                    int ind = RelationE1[P1].IndexOf((Polygons[P1][V1], Polygons[P1][V2]));
                    RelationE1[P1].RemoveAt(ind);
                    RelationE2[P1].RemoveAt(ind);
                    HasRealtion[P1]--;
                }
                else if (HasRealtion[P1] > 0 && RelationE2[P1].Contains((Polygons[P1][V1], Polygons[P1][V2])))
                {
                    int ind = RelationE2[P1].IndexOf((Polygons[P1][V1], Polygons[P1][V2]));
                    RelationE1[P1].RemoveAt(ind);
                    RelationE2[P1].RemoveAt(ind);
                    HasRealtion[P1]--;
                }
                Polygons[P1].Insert(V1 + 1, new Point((Polygons[P1][V1].X + Polygons[P1][V2].X) / 2, (Polygons[P1][V1].Y + Polygons[P1][V2].Y) / 2));
                Print();
            }
            else if (action == 3)
            {

                int activ = FindPolygon(a);
                if (activ >= 0)
                {
                    if (Polygons[activ].Count() == 3)
                    {
                        Polygons[activ] = null;
                        Active[activ] = false;
                        RelationE1[activ] = null;
                        RelationE2[activ] = null;
                        HasRealtion[activ] = 0;
                    }
                    else
                    {
                        int ind = FindVertix(a, activ);
                        if (HasRealtion[activ] > 0)
                        {
                            int R = IsRelation(activ, ind);
                            if (R == 1)
                            {
                                int V1 = ind - 1;
                                if (V1 == -1)
                                    V1 = Polygons[activ].Count() - 1;
                                if (RelationE1[activ].Contains((Polygons[activ][V1], Polygons[activ][ind])))
                                {
                                    int deleteind = RelationE1[activ].IndexOf((Polygons[activ][V1], Polygons[activ][ind]));
                                    RelationE1[activ].RemoveAt(deleteind);
                                    RelationE2[activ].RemoveAt(deleteind);
                                    HasRealtion[activ]--;
                                }
                                else if (RelationE2[activ].Contains((Polygons[activ][V1], Polygons[activ][ind])))
                                {
                                    int deleteind = RelationE2[activ].IndexOf((Polygons[activ][V1], Polygons[activ][ind]));
                                    RelationE1[activ].RemoveAt(deleteind);
                                    RelationE2[activ].RemoveAt(deleteind);
                                    HasRealtion[activ]--;
                                }

                            }
                            else if (R == 2)
                            {
                                int V1 = ind + 1;
                                if (V1 == Polygons[activ].Count())
                                    V1 = 0;
                                if (RelationE1[activ].Contains((Polygons[activ][ind], Polygons[activ][V1])))
                                {
                                    int deleteind = RelationE1[activ].IndexOf((Polygons[activ][ind], Polygons[activ][V1]));
                                    RelationE1[activ].RemoveAt(deleteind);
                                    RelationE2[activ].RemoveAt(deleteind);
                                    HasRealtion[activ]--;
                                }
                                else if (RelationE2[activ].Contains((Polygons[activ][ind], Polygons[activ][V1])))
                                {
                                    int deleteind = RelationE2[activ].IndexOf((Polygons[activ][ind], Polygons[activ][V1]));
                                    RelationE1[activ].RemoveAt(deleteind);
                                    RelationE2[activ].RemoveAt(deleteind);
                                    HasRealtion[activ]--;
                                }
                            }
                        }
                        Polygons[activ].RemoveAt(ind);
                    }

                }
                Print();
            }
            else if (action == 4)
            {
                int activ = -1;
                if (MyBitmap.GetPixel(e.Location.X, e.Location.Y).ToArgb() == Color.Black.ToArgb())
                {
                    int t, tt;
                    (activ, t, tt) = FindEdge(e.Location);
                }
                else if (MyBitmap.GetPixel(e.Location.X, e.Location.Y).ToArgb() != Color.FromArgb(0, 0, 0, 0).ToArgb())
                {
                    activ = FindPolygon(a);
                }

                if (activ >= 0)
                {
                    Active[activ] = false;
                    Polygons[activ] = null;
                    RelationE1[activ] = null;
                    RelationE2[activ] = null;
                    HasRealtion[activ] = 0;
                }
                Print();
            }
            else if (action == 8)
            {

                int tmpv1, tmpv2;
                int P1;
                if (MyBitmap.GetPixel(a.X, a.Y).ToArgb() == Color.Black.ToArgb())
                {

                    (P1, tmpv1, tmpv2) = FindEdge(a);
                    if (HasRealtion[P1] == 0 && FirstV1.X == -1 && Polygons[P1].Count() > 3)
                    {
                        (FirstP, tmpv1, tmpv2) = FindEdge(a);
                        if (RelationE1[FirstP] == null || RelationE1[FirstP].Count() == 0)
                        {
                            RelationE1[FirstP] = new List<(Point, Point)>();
                            FirstV1 = Polygons[FirstP][tmpv1];
                            FirstV2 = Polygons[FirstP][tmpv2];
                            BresenhamLine2(FirstV1, FirstV2);
                            pictureBox1.Image = MyBitmap;
                        }
                    }
                    else if (HasRealtion[P1] == 0 && Polygons[P1].Count() > 3)
                    {
                        (SecondP, tmpv1, tmpv2) = FindEdge(a);
                        if (FirstP == SecondP && (RelationE2[SecondP] == null || RelationE2[SecondP].Count() == 0))
                        {

                            SecondV1 = Polygons[SecondP][tmpv1];
                            SecondV2 = Polygons[SecondP][tmpv2];
                            if (FirstV1 != SecondV1 && FirstV2 != SecondV1 && FirstV1 != SecondV2 && FirstV2 != SecondV2)
                            {
                                RelationE2[FirstP] = new List<(Point, Point)>();

                                action = 0;
                                double k = 0;
                                double d1 = Math.Sqrt((((FirstV1.X - FirstV2.X) * (FirstV1.X - FirstV2.X)) + ((FirstV1.Y - FirstV2.Y) * (FirstV1.Y - FirstV2.Y))));
                                double d2 = Math.Sqrt((((SecondV1.X - SecondV2.X) * (SecondV1.X - SecondV2.X)) + ((SecondV1.Y - SecondV2.Y) * (SecondV1.Y - SecondV2.Y))));
                                if (d1 > d2)
                                {
                                    k = d2 / d1;
                                    int xc = (int)(k * (FirstV2.X - FirstV1.X) + FirstV1.X);
                                    int yc = (int)(k * (FirstV2.Y - FirstV1.Y) + FirstV1.Y);
                                    Polygons[FirstP][Polygons[FirstP].IndexOf(FirstV2)] = new Point(xc, yc);
                                    RelationE1[FirstP].Add((FirstV1, new Point(xc, yc)));
                                    RelationE2[FirstP].Add((SecondV1, SecondV2));
                                    HasRealtion[FirstP]++;
                                    FirstP = -1;
                                    SecondP = -1;
                                    Print();
                                }
                                else if (d2 > d1)
                                {
                                    k = d1 / d2;
                                    int xc = (int)(k * (SecondV2.X - SecondV1.X) + SecondV1.X);
                                    int yc = (int)(k * (SecondV2.Y - SecondV1.Y) + SecondV1.Y);
                                    Polygons[FirstP][Polygons[FirstP].IndexOf(SecondV2)] = new Point(xc, yc);
                                    RelationE1[FirstP].Add((SecondV1, new Point(xc, yc)));
                                    RelationE2[FirstP].Add((FirstV1, FirstV2));
                                    HasRealtion[FirstP]++;
                                    FirstP = -1;
                                    Print();
                                }
                            }

                        }
                    }
                    else if (Polygons[P1].Count() > 3)
                    {
                        (P1, tmpv1, tmpv2) = FindEdge(a);
                        if (Polygons[P1].Count() > 5 && FirstV1.X == -1 && !RelationE1[P1].Contains((Polygons[P1][tmpv1], Polygons[P1][tmpv2])) && !RelationE2[P1].Contains((Polygons[P1][tmpv1], Polygons[P1][tmpv2])))
                        {
                            (FirstP, tmpv1, tmpv2) = FindEdge(a);
                            posible1 = 0;
                            posible1 = Possible(FirstP, tmpv1, tmpv2);
                            if (posible1 == 0)
                            {
                                FirstV1 = Polygons[FirstP][tmpv1];
                                FirstV2 = Polygons[FirstP][tmpv2];
                                BresenhamLine2(FirstV1, FirstV2);
                                pictureBox1.Image = MyBitmap;
                            }
                        }
                        else if (Polygons[P1].Count() > 5 && !RelationE1[P1].Contains((Polygons[P1][tmpv1], Polygons[P1][tmpv2])) && !RelationE2[P1].Contains((Polygons[P1][tmpv1], Polygons[P1][tmpv2])))
                        {
                            (SecondP, tmpv1, tmpv2) = FindEdge(a);
                            if (FirstP == SecondP && Possible(SecondP, tmpv1, tmpv2) == 0)
                            {
                                SecondV1 = Polygons[FirstP][tmpv1];
                                SecondV2 = Polygons[FirstP][tmpv2];
                                if (FirstV1 == SecondV1 || FirstV2 == SecondV1 || FirstV1 == SecondV2 || FirstV2 == SecondV2)
                                    posible1++;
                                if (posible1 == 0)
                                {
                                    SecondV1 = Polygons[FirstP][tmpv1];
                                    SecondV2 = Polygons[FirstP][tmpv2];
                                    posible1 = 0;
                                    Dodaj_Relacje(SecondP, FirstV1, FirstV2, SecondV1, SecondV2);
                                }
                                else
                                    posible1--;
                            }
                        }
                    }
                }
            }
        }

        bool Dodaj_Relacje(int P, Point FV1, Point FV2, Point SV1, Point SV2)
        {
            double k = 0;
            double d1 = Math.Sqrt((((FV1.X - FV2.X) * (FV1.X - FV2.X)) + ((FV1.Y - FV2.Y) * (FV1.Y - FV2.Y))));
            double d2 = Math.Sqrt((((SV1.X - SV2.X) * (SV1.X - SV2.X)) + ((SV1.Y - SV2.Y) * (SV1.Y - SV2.Y))));
            if (d1 > d2)
            {
                k = d2 / d1;
                int xc = (int)(k * (FV2.X - FV1.X) + FV1.X);
                int yc = (int)(k * (FV2.Y - FV1.Y) + FV1.Y);
                Polygons[P][Polygons[P].IndexOf(FV2)] = new Point(xc, yc);
                RelationE1[P].Add((FV1, new Point(xc, yc)));
                RelationE2[P].Add((SV1, SV2));
                HasRealtion[P]++;
                Print();
            }
            else if (d2 > d1)
            {
                k = d1 / d2;
                int xc = (int)(k * (SV2.X - SV1.X) + SV1.X);
                int yc = (int)(k * (SV2.Y - SV1.Y) + SV1.Y);
                Polygons[P][Polygons[P].IndexOf(SV2)] = new Point(xc, yc);
                RelationE1[P].Add((SV1, new Point(xc, yc)));
                RelationE2[P].Add((FV1, FV2));
                HasRealtion[P]++;
                Print();
            }
            return false;
        }
        int IsRelation(int P, int V)
        {
            int V1 = V - 1;
            int V2 = V + 1;
            if (V1 == -1)
                V1 = Polygons[P].Count() - 1;
            if (V2 == Polygons[P].Count())
                V2 = 0;

            if (RelationE1[P].Contains((Polygons[P][V], Polygons[P][V2])) || RelationE2[P].Contains((Polygons[P][V], Polygons[P][V2])))
            {
                return 2;
            }
            if (RelationE1[P].Contains((Polygons[P][V1], Polygons[P][V])) || RelationE2[P].Contains((Polygons[P][V1], Polygons[P][V])))
            {
                return 1;
            }
            return 0;
        }
        int Possible(int P, int V1, int V2)
        {
            int V11 = V1 - 1;
            int V12 = V1 - 2;
            int V21 = V2 + 1;
            int V22 = V2 + 2;
            if (V11 == -1)
                V11 = Polygons[P].Count() - 1;
            if (V21 == Polygons[P].Count())
                V21 = 0;
            if (RelationE1[P].Contains((Polygons[P][V11], Polygons[P][V1])) || RelationE2[P].Contains((Polygons[P][V11], Polygons[P][V1])))
            {
                return 1;
            }
            else if (RelationE1[P].Contains((Polygons[P][V2], Polygons[P][V21])) || RelationE2[P].Contains((Polygons[P][V2], Polygons[P][V21])))
            {
                return 1;
            }
            return 0;
        }
        int FirstP = -1;
        Point FirstV1 = new Point(-1, -1);
        Point FirstV2 = new Point(-1, -1);
        int posible1;
        int SecondP = -1;
        Point SecondV1 = new Point(-1, -1);
        Point SecondV2 = new Point(-1, -1);
        private int FindPolygon(Point a)
        {
            for (int i = 0; i < Active.Length; i++)
            {
                if (Active[i])
                {
                    //int r, g, b;
                    //(r, g, b) = Col.Colo[i];
                    if (MyBitmap.GetPixel(a.X, a.Y).ToArgb() == Col.Colo[i].ToArgb())
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            activaded = -1;
            Print();
            action = 1;
            for (int i = 0; i < Active.Length; i++)
            {
                if (Active[i] == false)
                {
                    activaded = i;
                    //Active[i] = true;
                    break;
                }
            }
            Polygons[activaded] = new List<Point>();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            activaded = -1;
            Print();
            action = 2;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            activaded = -1;
            Print();
            action = 3;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            activaded = -1;
            Print();
            action = 4;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            activaded = -1;
            Print();
            action = 5;
        }
        private void button6_Click(object sender, EventArgs e)
        {
            activaded = -1;
            Print();
            action = 6;
        }
        private void button7_Click(object sender, EventArgs e)
        {
            activaded = -1;
            action = 7;
            Print();
        }
        private void button8_Click(object sender, EventArgs e)
        {
            activaded = -1;
            FirstP = -1;
            SecondP = -1;
            FirstV1.X = -1;
            action = 8;
            Print();
        }
        private (int, int, int) FindEdge(Point a)
        {
            int v1, v2;
            int pol = -1;
            v1 = -1;
            v2 = -1;
            double dm = Int32.MaxValue;
            for (int i = 0; i < Polygons.Length; i++)
            {

                double d;
                if (Active[i])
                {
                    double A, B, C;
                    int num = Polygons[i].Count();
                    int x1, y1, x2, y2;
                    for (int v = 0; v < num - 1; v++)
                    {
                        A = (Polygons[i][v].Y - Polygons[i][v + 1].Y);
                        B = -(Polygons[i][v].X - Polygons[i][v + 1].X);
                        C = (Polygons[i][v].Y * (-B)) - (A * Polygons[i][v].X);
                        d = Math.Abs(A * a.X + B * a.Y + C) / Math.Sqrt(A * A + B * B);

                        x1 = Polygons[i][v].X;
                        x2 = Polygons[i][v + 1].X;
                        y1 = Polygons[i][v].Y;
                        y2 = Polygons[i][v + 1].Y;
                        if (x1 > x2)
                        {
                            int tmp = x1;
                            x1 = x2;
                            x2 = tmp;
                        }
                        if (y1 > y2)
                        {
                            int tmp = y1;
                            y1 = y2;
                            y2 = tmp;
                        }
                        if (d == 0)
                            return (i, v, v + 1);
                        if (dm > d && a.X >= x1 - 1 && a.X <= x2 + 1 && a.Y >= y1 - 1 && a.Y <= y2 + 1)
                        {
                            pol = i;
                            v1 = v;
                            v2 = v + 1;
                            dm = d;
                        }
                    }
                    A = (Polygons[i][num - 1].Y - Polygons[i][0].Y);
                    B = -(Polygons[i][num - 1].X - Polygons[i][0].X);
                    C = (-B * Polygons[i][num - 1].Y) - (A * Polygons[i][num - 1].X);
                    d = Math.Abs(A * a.X + B * a.Y + C) / Math.Sqrt(A * A + B * B);

                    x1 = Polygons[i][num - 1].X;
                    x2 = Polygons[i][0].X;
                    y1 = Polygons[i][num - 1].Y;
                    y2 = Polygons[i][0].Y;
                    if (x1 > x2)
                    {
                        int tmp = x1;
                        x1 = x2;
                        x2 = tmp;
                    }
                    if (y1 > y2)
                    {
                        int tmp = y1;
                        y1 = y2;
                        y2 = tmp;
                    }
                    if (d == 0)
                        return (i, num - 1, 0);
                    if (dm > d && a.X >= x1 - 1 && a.X <= x2 + 1 && a.Y >= y1 - 1 && a.Y <= y2 + 1)
                    {
                        pol = i;
                        v1 = num - 1;
                        v2 = 0;
                        dm = d;
                    }
                }
            }
            return (pol, v1, v2);
        }


        int px = 0;
        int py = 0;
        int pv = 0;
        int pp = 0;
        bool trackV;
        bool trackE;
        bool trackPE;
        bool trackPV;
        int LocX;
        int LocY;
        int V1;
        int V2;
        int P1;
        Point LocV1;
        Point LocV2;
        List<Point> TmpP;
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (action == 5)
            {
                Point a = new Point(e.Location.X, e.Location.Y);
                pp = FindPolygon(a);
                key = 0;
                if (pp > -1)
                {
                    pv = FindVertix(a, pp);
                    if (HasRealtion[pp] > 0)
                        key = IsRelation(pp, pv);

                    px = Polygons[pp][pv].X - a.X;
                    py = Polygons[pp][pv].Y - a.Y;
                    trackV = true;
                }
            }
            else if (action == 6 && MyBitmap.GetPixel(e.Location.X, e.Location.Y).ToArgb() == Color.Black.ToArgb())
            {
                (P1, V1, V2) = FindEdge(e.Location);
                LocX = e.Location.X;
                LocY = e.Location.Y;
                LocV1 = Polygons[P1][V1];
                LocV2 = Polygons[P1][V2];
                trackE = true;
            }
            else if (action == 7)
            {
                if (MyBitmap.GetPixel(e.Location.X, e.Location.Y).ToArgb() == Color.Black.ToArgb())
                {
                    (P1, V1, V2) = FindEdge(e.Location);
                    LocX = e.Location.X;
                    LocY = e.Location.Y;
                    TmpP = new List<Point>(Polygons[pp]);
                    trackPE = true;
                }
                else if (MyBitmap.GetPixel(e.Location.X, e.Location.Y).ToArgb() != Color.FromArgb(0, 0, 0, 0).ToArgb())
                {
                    pp = FindPolygon(e.Location);
                    LocX = e.Location.X;
                    LocY = e.Location.Y;
                    TmpP = new List<Point>(Polygons[pp]);
                    trackPV = true;
                }
            }
        }
        bool Correct(int r, int P, double d, (Point, Point) Edge)
        {
            action = 0;

            FirstV1 = Edge.Item1;
            FirstV2 = Edge.Item2;
            double d1 = Math.Sqrt((((FirstV1.X - FirstV2.X) * (FirstV1.X - FirstV2.X)) + ((FirstV1.Y - FirstV2.Y) * (FirstV1.Y - FirstV2.Y))));
            double k = d / d1;
            int xc = (int)(k * (FirstV2.X - FirstV1.X) + FirstV1.X);
            int yc = (int)(k * (FirstV2.Y - FirstV1.Y) + FirstV1.Y);
            if (xc <= 809 && yc <= 549 && xc >= 6 && yc >= 6)
            {

                if (r == 2)
                {
                    RelationE1[P][RelationE1[P].IndexOf(Edge)] = (FirstV1, new Point(xc, yc));
                    Polygons[P][Polygons[P].IndexOf(FirstV2)] = new Point(xc, yc);
                    return true;
                }
                else if (r == 1)
                {
                    RelationE2[P][RelationE2[P].IndexOf(Edge)] = ((FirstV1, new Point(xc, yc)));
                    Polygons[P][Polygons[P].IndexOf(FirstV2)] = new Point(xc, yc);
                    return true;
                }
                return true;
            }
            return false;
        }
        int key = 0;
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (trackV)
            {

                if (key == 0 && e.Location.X + px <= 809 && e.Location.Y + py <= 549 && e.Location.X + px >= 6 && e.Location.Y + py >= 6)
                {
                    Polygons[pp][pv] = new Point(e.Location.X + px, e.Location.Y + py);
                    Print();
                }
                else if (key == 1)
                {
                    if (e.Location.X + px <= 809 && e.Location.Y + py <= 549 && e.Location.X + px >= 6 && e.Location.Y + py >= 6)
                    {
                        int V1 = pv - 1;
                        if (V1 == -1)
                            V1 = Polygons[pp].Count() - 1;
                        double d1 = Math.Sqrt((((Polygons[pp][V1].X - e.Location.X + px) * (Polygons[pp][V1].X - e.Location.X + px) + ((Polygons[pp][V1].Y - e.Location.Y + py) * (Polygons[pp][V1].Y - e.Location.Y + py)))));
                        if (RelationE1[pp].Contains((Polygons[pp][V1], Polygons[pp][pv])))
                        {
                            int ind = RelationE1[pp].IndexOf((Polygons[pp][V1], Polygons[pp][pv]));
                            if (Correct(1, pp, d1, RelationE2[pp][ind]))
                            {
                                Polygons[pp][pv] = new Point(e.Location.X + px, e.Location.Y + py);
                                RelationE1[pp][ind] = (Polygons[pp][V1], Polygons[pp][pv]);

                                Print();
                            }

                        }
                        else if (RelationE2[pp].Contains((Polygons[pp][V1], Polygons[pp][pv])))
                        {
                            int ind = RelationE2[pp].IndexOf((Polygons[pp][V1], Polygons[pp][pv]));
                            if (Correct(2, pp, d1, RelationE1[pp][ind]))
                            {
                                Polygons[pp][pv] = new Point(e.Location.X + px, e.Location.Y + py);
                                RelationE2[pp][ind] = (Polygons[pp][V1], Polygons[pp][pv]);

                                Print();
                            }
                        }

                    }

                    //Print();
                }
                else if (key == 2)
                {
                    if (e.Location.X + px <= 809 && e.Location.Y + py <= 549 && e.Location.X + px >= 6 && e.Location.Y + py >= 6)
                    {
                        int V1 = pv + 1;
                        if (V1 == Polygons[pp].Count())
                            V1 = 0;
                        double d1 = Math.Sqrt((((Polygons[pp][V1].X - e.Location.X + px) * (Polygons[pp][V1].X - e.Location.X + px) + ((Polygons[pp][V1].Y - e.Location.Y + py) * (Polygons[pp][V1].Y - e.Location.Y + py)))));
                        if (RelationE1[pp].Contains((Polygons[pp][pv], Polygons[pp][V1])))
                        {
                            int ind = RelationE1[pp].IndexOf((Polygons[pp][pv], Polygons[pp][V1]));
                            if (Correct(1, pp, d1, RelationE2[pp][ind]))
                            {
                                Polygons[pp][pv] = new Point(e.Location.X + px, e.Location.Y + py);
                                RelationE1[pp][ind] = (Polygons[pp][pv], Polygons[pp][V1]);
                                Print();
                            }

                        }
                        else if (RelationE2[pp].Contains((Polygons[pp][pv], Polygons[pp][V1])))
                        {
                            int ind = RelationE2[pp].IndexOf((Polygons[pp][pv], Polygons[pp][V1]));
                            if (Correct(2, pp, d1, RelationE1[pp][ind]))
                            {
                                Polygons[pp][pv] = new Point(e.Location.X + px, e.Location.Y + py);
                                RelationE2[pp][ind] = (Polygons[pp][pv], Polygons[pp][V1]);

                                Print();
                            }
                        }
                    }
                }

            }
            else if (trackE)
            {

                int ChangeX = e.Location.X - LocX;
                int ChangeY = e.Location.Y - LocY;

                if (LocV1.X + ChangeX <= 809 && LocV1.Y + ChangeY <= 549 && LocV1.X + ChangeX >= 6 && LocV1.Y + ChangeY >= 6)
                    if (LocV2.X + ChangeX <= 809 && LocV2.Y + ChangeY <= 549 && LocV2.X + ChangeX >= 6 && LocV2.Y + ChangeY >= 6)
                    {
                        Polygons[P1][V1] = new Point(LocV1.X + ChangeX, LocV1.Y + ChangeY);
                        Polygons[P1][V2] = new Point(LocV2.X + ChangeX, LocV2.Y + ChangeY);
                        Print();
                    }
            }
            else if (trackPV || trackPE)
            {
                int ChangeX = e.Location.X - LocX;
                int ChangeY = e.Location.Y - LocY;
                bool Clear = true; ;
                for (int i = 0; i < TmpP.Count(); i++)
                {
                    if (TmpP[i].X + ChangeX <= 809 && TmpP[i].Y + ChangeY <= 549 && TmpP[i].X + ChangeX >= 6 && TmpP[i].Y + ChangeY >= 6)
                        Clear = true;
                    else
                    {
                        Clear = false;
                        break;
                    }
                }
                if (Clear)
                {
                    for (int i = 0; i < TmpP.Count(); i++)
                    {
                        Polygons[pp][i] = new Point(TmpP[i].X + ChangeX, TmpP[i].Y + ChangeY);
                    }
                }
                Print();
            }

        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            trackE = false;
            trackV = false;
            trackPE = false;
            trackPV = false;
            key = 0;
        }
        private void Form1_Click(object sender, EventArgs e)
        {
            activaded = -1;
            action = 0;
            Print();
        }
        //Check Edge to relation
        private void BresenhamLine2(Point p, Point n)
        {
            int x1 = p.X;
            int y1 = p.Y;
            int x2 = n.X;
            int y2 = n.Y;


            // zmienne pomocnicze
            int d, dx, dy, ai, bi, xi, yi;
            int x = x1, y = y1;

            // ustalenie kierunku rysowania
            if (x1 < x2)
            {
                xi = 1;
                dx = x2 - x1;
            }
            else
            {
                xi = -1;
                dx = x1 - x2;
            }
            // ustalenie kierunku rysowania
            if (y1 < y2)
            {
                yi = 1;
                dy = y2 - y1;
            }
            else
            {
                yi = -1;
                dy = y1 - y2;
            }
            // pierwszy piksel
            //if (MyBitmap.GetPixel(x, y).ToArgb() == Color.FromArgb(0, 0, 0, 0).ToArgb())
            for (int i = x - 1; i <= x + 1; i++)
                for (int j = y - 1; j <= y + 1; j++)
                    MyBitmap.SetPixel(i, j, Color.Orange);
            // oś wiodąca OX
            if (dx > dy)
            {
                ai = (dy - dx) * 2;
                bi = dy * 2;
                d = bi - dx;
                // pętla po kolejnych x
                while (x != x2)
                {
                    // test współczynnika
                    if (d >= 0)
                    {
                        x += xi;
                        y += yi;
                        d += ai;
                    }
                    else
                    {
                        d += bi;
                        x += xi;
                    }
                    // if (MyBitmap.GetPixel(x, y).ToArgb() == Color.FromArgb(0, 0, 0, 0).ToArgb())
                    for (int i = x - 1; i <= x + 1; i++)
                        for (int j = y - 1; j <= y + 1; j++)
                            MyBitmap.SetPixel(i, j, Color.Orange);
                }
            }
            // oś wiodąca OY
            else
            {
                ai = (dx - dy) * 2;
                bi = dx * 2;
                d = bi - dy;
                // pętla po kolejnych y
                while (y != y2)
                {
                    // test współczynnika
                    if (d >= 0)
                    {
                        x += xi;
                        y += yi;
                        d += ai;
                    }
                    else
                    {
                        d += bi;
                        y += yi;
                    }
                    //if (MyBitmap.GetPixel(x, y).ToArgb() == Color.FromArgb(0, 0, 0, 0).ToArgb())
                    for (int i = x - 1; i <= x + 1; i++)
                        for (int j = y - 1; j <= y + 1; j++)
                            MyBitmap.SetPixel(i, j, Color.Orange);
                }
            }
        }

        //AntiAliasing
        private void plot(Bitmap bitmap, double x, double y, double c)
        {
            int alpha = (int)(c * 255);
            if (alpha > 255) alpha = 255;
            if (alpha < 0) alpha = 0;
            Color color = Color.FromArgb(alpha, Color.Black);

            bitmap.SetPixel((int)x, (int)y, color);

        }
        int ipart(double x) { return (int)x; }
        int round(double x) { return ipart(x + 0.5); }
        double fpart(double x)
        {
            if (x < 0) return (1 - (x - Math.Floor(x)));
            return (x - Math.Floor(x));
        }
        double rfpart(double x)
        {
            return 1 - fpart(x);
        }
        public void drawAnti(Bitmap bitmap, Point p, Point n)
        {
            double x0 = p.X;
            double y0 = p.Y;
            double x1 = n.X;
            double y1 = n.Y;
            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            double temp;
            if (steep)
            {
                temp = x0;
                x0 = y0;
                y0 = temp;

                temp = x1;
                x1 = y1;
                y1 = temp;
            }
            if (x0 > x1)
            {
                temp = x0;
                x0 = x1;
                x1 = temp;

                temp = y0;
                y0 = y1;
                y1 = temp;
            }

            double dx = x1 - x0;
            double dy = y1 - y0;
            double gradient = dy / dx;

            double xEnd = round(x0);
            double yEnd = y0 + gradient * (xEnd - x0);
            double xGap = rfpart(x0 + 0.5);
            double xPixel1 = xEnd;
            double yPixel1 = ipart(yEnd);

            if (steep)
            {
                plot(bitmap, yPixel1, xPixel1, rfpart(yEnd) * xGap);
                plot(bitmap, yPixel1 + 1, xPixel1, fpart(yEnd) * xGap);
            }
            else
            {
                plot(bitmap, xPixel1, yPixel1, rfpart(yEnd) * xGap);
                plot(bitmap, xPixel1, yPixel1 + 1, fpart(yEnd) * xGap);
            }
            double intery = yEnd + gradient;

            xEnd = round(x1);
            yEnd = y1 + gradient * (xEnd - x1);
            xGap = fpart(x1 + 0.5);
            double xPixel2 = xEnd;
            double yPixel2 = ipart(yEnd);
            if (steep)
            {
                plot(bitmap, yPixel2, xPixel2, rfpart(yEnd) * xGap);
                plot(bitmap, yPixel2 + 1, xPixel2, fpart(yEnd) * xGap);
            }
            else
            {
                plot(bitmap, xPixel2, yPixel2, rfpart(yEnd) * xGap);
                plot(bitmap, xPixel2, yPixel2 + 1, fpart(yEnd) * xGap);
            }

            if (steep)
            {
                for (int x = (int)(xPixel1 + 1); x <= xPixel2 - 1; x++)
                {
                    plot(bitmap, ipart(intery), x, rfpart(intery));
                    plot(bitmap, ipart(intery) + 1, x, fpart(intery));
                    intery += gradient;
                }
            }
            else
            {
                for (int x = (int)(xPixel1 + 1); x <= xPixel2 - 1; x++)
                {
                    plot(bitmap, x, ipart(intery), rfpart(intery));
                    plot(bitmap, x, ipart(intery) + 1, fpart(intery));
                    intery += gradient;
                }
            }
        }

        private void BresenhamLine(Point p, Point n)
        {
            int x1 = p.X;
            int y1 = p.Y;
            int x2 = n.X;
            int y2 = n.Y;


            // zmienne pomocnicze
            int d, dx, dy, ai, bi, xi, yi;
            int x = x1, y = y1;

            // ustalenie kierunku rysowania
            if (x1 < x2)
            {
                xi = 1;
                dx = x2 - x1;
            }
            else
            {
                xi = -1;
                dx = x1 - x2;
            }
            // ustalenie kierunku rysowania
            if (y1 < y2)
            {
                yi = 1;
                dy = y2 - y1;
            }
            else
            {
                yi = -1;
                dy = y1 - y2;
            }
            // pierwszy piksel

            for (int i = x - 1; i <= x + 1; i++)
                for (int j = y - 1; j <= y + 1; j++)
                    MyBitmap.SetPixel(i, j, Color.Black);
            // oś wiodąca OX
            if (dx > dy)
            {
                ai = (dy - dx) * 2;
                bi = dy * 2;
                d = bi - dx;
                // pętla po kolejnych x
                while (x != x2)
                {
                    // test współczynnika
                    if (d >= 0)
                    {
                        x += xi;
                        y += yi;
                        d += ai;
                    }
                    else
                    {
                        d += bi;
                        x += xi;
                    }
                    for (int i = x - 1; i <= x + 1; i++)
                        for (int j = y - 1; j <= y + 1; j++)
                            MyBitmap.SetPixel(i, j, Color.Black);
                }
            }
            // oś wiodąca OY
            else
            {
                ai = (dx - dy) * 2;
                bi = dx * 2;
                d = bi - dy;
                // pętla po kolejnych y
                while (y != y2)
                {
                    // test współczynnika
                    if (d >= 0)
                    {
                        x += xi;
                        y += yi;
                        d += ai;
                    }
                    else
                    {
                        d += bi;
                        y += yi;
                    }
                    for (int i = x - 1; i <= x + 1; i++)
                        for (int j = y - 1; j <= y + 1; j++)
                            MyBitmap.SetPixel(i, j, Color.Black);
                }
            }
        }
        private void BresenhamLineSym(Point p, Point n)
        {
            int x1 = p.X;
            int y1 = p.Y;
            int x2 = n.X;
            int y2 = n.Y;


            // zmienne pomocnicze
            int d, dx, dy, ai, bi, xi, yi;
            int xf = x1, yf = y1, xb = x2, yb = y2;
            // ustalenie kierunku rysowania
            if (x1 < x2)
            {
                xi = 1;
                dx = x2 - x1;
            }
            else
            {
                xi = -1;
                dx = x1 - x2;
            }
            // ustalenie kierunku rysowania
            if (y1 < y2)
            {
                yi = 1;
                dy = y2 - y1;
            }
            else
            {
                yi = -1;
                dy = y1 - y2;
            }
            // pierwszy piksel

            MyBitmap.SetPixel(xf, yf, Color.Black);
            MyBitmap.SetPixel(xb, yb, Color.Black);

            // oś wiodąca OX
            if (dx > dy)
            {
                ai = (dy - dx) * 2;
                bi = dy * 2;
                d = bi - dx;
                // pętla po kolejnych x
                while ((x1 < x2) == (xf < xb))
                {
                    // test współczynnika

                    if (d >= 0)
                    {
                        xf += xi;
                        xb -= xi;
                        yf += yi;
                        yb -= yi;
                        d += ai;
                    }
                    else
                    {
                        d += bi;
                        xf += xi;
                        xb -= xi;
                    }
                    MyBitmap.SetPixel(xf, yf, Color.Black);
                    MyBitmap.SetPixel(xb, yb, Color.Black);

                }
            }
            else
            {
                ai = (dx - dy) * 2;
                bi = dx * 2;
                d = bi - dy;
                // pętla po kolejnych y
                while ((y1 < y2) == (yf < yb))
                {
                    // test współczynnika
                    if (d >= 0)
                    {
                        xf += xi;
                        xb -= xi;
                        yf += yi;
                        yb -= yi;
                        d += ai;
                    }
                    else
                    {
                        d += bi;
                        yf += yi;
                        yb -= yi;
                    }

                    MyBitmap.SetPixel(xf, yf, Color.Black);
                    MyBitmap.SetPixel(xb, yb, Color.Black);
                }
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Howdraw = 1;
            Print();
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            Howdraw = 2;
            Print();
        }
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            Howdraw = 3;
            Print();
        }
    }














}