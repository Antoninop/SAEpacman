using System;
using System.Drawing;

namespace LabyBuild
{
    public class ClasseEpee
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public bool Actif { get; set; }


        public ClasseEpee(int x, int y)
        {
            X = x;
            Y = y;
            Actif = true;
        }

        public void Draw(Graphics g, int tailleCarre)
        {
            if (Actif)
            {
                Rectangle rect = new Rectangle(Y * tailleCarre, X * tailleCarre, tailleCarre, tailleCarre);
                g.FillRectangle(Brushes.Purple, rect);
            }
        }

        public void Reappear(int[,] grille, int lignes, int colonnes)
        {
            Random random = new Random();
            int x, y;
            do
            {
                x = random.Next(1, lignes);
                y = random.Next(1, colonnes);
            } while (grille[x, y] != 1 || (x == 1 && y == 1));

            X = x;
            Y = y;
            Actif = true;
        }
    }
}
