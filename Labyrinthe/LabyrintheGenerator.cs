using System;
using System.Collections.Generic;

namespace LabyBuild
{
    public class LabyrintheGenerator
    {
        private const int Lignes = 30;
        private const int Colonnes = 55;
        private int[,] grille;
        private static Random random = new Random();
        private const int MaxConsecutiveMoves = 3;

        public int[,] Grille { get { return grille; } }

        public LabyrintheGenerator()
        {
            grille = new int[Lignes, Colonnes];
            GenererLabyrinthe();
        }

        private void GenererLabyrinthe()
        {
            RemplirGrille();
            ParcoursEnProfondeur(1, 1);
            AjouterPassagesAleatoires();
        }

        private void ParcoursEnProfondeur(int x, int y, int lastDirection = -1, int moveCount = 0)
        {
            grille[x, y] = 1;

            int[] deplacementX = { 0, 0, 1, -1 };
            int[] deplacementY = { 1, -1, 0, 0 };

            var directions = new List<int> { 0, 1, 2, 3 };
            Melanger(directions);

            foreach (var direction in directions)
            {
                int nx = x + deplacementX[direction] * 2;
                int ny = y + deplacementY[direction] * 2;

                if (nx >= 1 && ny >= 1 && nx < Lignes - 1 && ny < Colonnes - 1 && grille[nx, ny] == 0)
                {
                    if (direction == lastDirection && moveCount < MaxConsecutiveMoves)
                    {
                        grille[nx, ny] = 1;
                        grille[x + deplacementX[direction], y + deplacementY[direction]] = 1;
                        ParcoursEnProfondeur(nx, ny, direction, moveCount + 1);
                    }
                    else if (direction != lastDirection)
                    {
                        grille[nx, ny] = 1;
                        grille[x + deplacementX[direction], y + deplacementY[direction]] = 1;
                        ParcoursEnProfondeur(nx, ny, direction, 1);
                    }
                }
            }
        }

        private void AjouterPassagesAleatoires()
        {
            for (int i = 1; i < Lignes - 1; i += 2)
            {
                for (int j = 1; j < Colonnes - 1; j += 2)
                {
                    if (random.NextDouble() < 0.5)
                    {
                        int direction = random.Next(4);
                        int nx = i + (direction == 2 ? 1 : direction == 3 ? -1 : 0);
                        int ny = j + (direction == 0 ? 1 : direction == 1 ? -1 : 0);
                        if (nx >= 1 && ny >= 1 && nx < Lignes - 1 && ny < Colonnes - 1)
                        {
                            grille[nx, ny] = 1;
                        }
                    }
                }
            }
        }

        private void RemplirGrille()
        {
            for (int i = 0; i < Lignes; i++)
            {
                for (int j = 0; j < Colonnes; j++)
                {
                    grille[i, j] = 0;
                }
            }
        }

        private void Melanger<T>(IList<T> liste)
        {
            int n = liste.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T valeur = liste[k];
                liste[k] = liste[n];
                liste[n] = valeur;
            }
        }
    }
}
