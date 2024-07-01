using System;
using System.Drawing;

namespace LabyBuild
{
    public class ClassePioche
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool Actif { get; set; }

        public ClassePioche(int x, int y)
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
                g.FillRectangle(Brushes.Green, rect);
            }
        }
    }
}
