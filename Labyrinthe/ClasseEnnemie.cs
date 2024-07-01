using System;
using System.Drawing;

namespace LabyBuild
{
    public class ClasseEnnemie
    {
        public int X { get; set; }
        public int Y { get; set; }
        private static Random random = new Random();

        public ClasseEnnemie(int x, int y)
        {
            X = x;
            Y = y;
            SetRandomDirection();
        }

        public void Draw(Graphics g, int tailleCarre)
        {
            Brush brosseBleue = Brushes.Blue;
            Rectangle rect = new Rectangle(Y * tailleCarre, X * tailleCarre, tailleCarre, tailleCarre);
            g.FillRectangle(brosseBleue, rect);
        }

        public void Move(int[,] grille)
        {
            
            int oldX = X;
            int oldY = Y;

            switch (random.Next(4))
            {
                case 0:
                    X++;
                    break;
                case 1:
                    X--;
                    break;
                case 2:
                    Y++;
                    break;
                case 3:
                    Y--;
                    break;
            }

            if (X < 0 || X >= grille.GetLength(0) || Y < 0 || Y >= grille.GetLength(1) || grille[X, Y] != 1)
            {
                X = oldX;
                Y = oldY;
            }
        }

        private void SetRandomDirection()
        {
            int direction = random.Next(4);
            switch (direction)
            {
                case 0:
                    X++;
                    break;
                case 1:
                    X--;
                    break;
                case 2:
                    Y++;
                    break;
                case 3:
                    Y--;
                    break;
            }
        }
    }
}
