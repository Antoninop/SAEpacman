using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using InterfaceLab;



namespace LabyBuild
{
    public partial class Form1 : Form
    {
        private const int TailleCarre = 30;
        private const int Lignes = 30;
        private const int Colonnes = 55;
        private int[,] grille;
        private PictureBox pictureBox;
        private ClasseJoueur joueur;
        private LabyrintheGenerator labyrintheGenerator;
        private List<ClasseEnnemie> enemies;
        private System.Windows.Forms.Timer enemyMoveTimer;
        private System.Windows.Forms.Timer joueurPiocheTimer;
        private System.Windows.Forms.Timer joueurMoveTimer;
        private System.Windows.Forms.Timer bonusReappearTimer;
        private ClassePioche bonusPioche;
        private ClasseEpee EpeeBonus;
        private List<ClassePiece> pieces;
        private Keys directionActuelle;
        private Image joueurImage;
        private Image zombieImage;
        private Image pieceImage;
        private Image jpioche;
        private Image pioche;
        private Image epee;
        private Image jepee;
        private bool gameOver = false;
        private bool victory = false;


        public Form1()
        {
            InitializeComponent();
            InitialiserPictureBox();
            joueur = new ClasseJoueur(1, 1);
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
            this.KeyUp += new KeyEventHandler(Form1_KeyUp);
            this.KeyPreview = true;
            labyrintheGenerator = new LabyrintheGenerator();
            grille = labyrintheGenerator.Grille;
            enemies = new List<ClasseEnnemie>();
            pieces = new List<ClassePiece>();
            AjouterEnnemis();
            AjouterBonus();
            AjouterPieces();
            this.Controls.Add(pictureBox);
            InitializeEnemyMoveTimer();
            InitializeJoueurPiocheTimer();
            InitializeJoueurMoveTimer();
            InitializeBonusReappearTimer(); // Initialiser le timer pour la réapparition des bonus

            joueurImage = Image.FromFile("./personnage.png");
            zombieImage = Image.FromFile("./zombie.png");
            pieceImage = Image.FromFile("./piece.png");

            jpioche = Image.FromFile("./jpioche.png");
            pioche = Image.FromFile("./pioche.png");
            epee = Image.FromFile("./epee.png");
            jepee = Image.FromFile("./jepee.png");
        }

        private void InitialiserPictureBox()
        {
            pictureBox = new PictureBox
            {
                Size = new Size(TailleCarre * Colonnes, TailleCarre * Lignes),
                Dock = DockStyle.Fill
            };
            pictureBox.Paint += PictureBox_Paint;
        }

        private void InitializeEnemyMoveTimer()
        {
            enemyMoveTimer = new System.Windows.Forms.Timer
            {
                Interval = 200
            };
            enemyMoveTimer.Tick += (s, e) => MoveEnemies();
            enemyMoveTimer.Start();
        }

        private void InitializeJoueurPiocheTimer()
        {
            joueurPiocheTimer = new System.Windows.Forms.Timer
            {
                Interval = 15000
            };
            joueurPiocheTimer.Tick += (s, e) => FinirTransformationPioche();
        }

        private void InitializeJoueurMoveTimer()
        {
            joueurMoveTimer = new System.Windows.Forms.Timer
            {
                Interval = 50
            };
            joueurMoveTimer.Tick += (s, e) => DeplacerJoueur();
        }

        private void InitializeBonusReappearTimer()
        {
            bonusReappearTimer = new System.Windows.Forms.Timer
            {
                Interval = 20000 // 20 seconds
            };
            bonusReappearTimer.Tick += (s, e) => ReappearBonuses();
        }

        private void ReappearBonuses()
        {
            if (!bonusPioche.Actif)
            {
                ReappearBonusPioche();
            }
            if (!EpeeBonus.Actif)
            {
                EpeeBonus.Reappear(grille, Lignes, Colonnes);
            }
        }

        private void ReappearBonusPioche()
        {
            Random random = new Random();
            int x, y;
            do
            {
                x = random.Next(1, Lignes);
                y = random.Next(1, Colonnes);
            } while (grille[x, y] != 1 || (x == 1 && y == 1));

            bonusPioche.X = x;
            bonusPioche.Y = y;
            bonusPioche.Actif = true;
        }


        private void AjouterEnnemis()
        {
            Random random = new Random();
            for (int i = 0; i < 10; i++)
            {
                int x, y;
                do
                {
                    x = random.Next(1, Lignes);
                    y = random.Next(1, Colonnes);
                } while (grille[x, y] != 1 || (x == 1 && y == 1));

                enemies.Add(new ClasseEnnemie(x, y));
            }
        }

        private void AjouterBonus()
        {
            Random random = new Random();
            int x, y;
            do
            {
                x = random.Next(1, Lignes);
                y = random.Next(1, Colonnes);
            } while (grille[x, y] != 1 || (x == 1 && y == 1));

            bonusPioche = new ClassePioche(x, y);

            do
            {
                x = random.Next(1, Lignes);
                y = random.Next(1, Colonnes);
            } while (grille[x, y] != 1 || (x == 1 && y == 1));

            EpeeBonus = new ClasseEpee(x, y);
        }

        private void AjouterPieces()
        {
            Random random = new Random();
            for (int i = 0; i < 15; i++)
            {
                int x, y;
                do
                {
                    x = random.Next(1, Lignes);
                    y = random.Next(1, Colonnes);
                } while (grille[x, y] != 1 || (x == 1 && y == 1));

                pieces.Add(new ClassePiece(x, y));
            }
        }

        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            DessinerLabyrinthe(e.Graphics);
        }

        private void DessinerLabyrinthe(Graphics g)
        {
            Brush brosseGris = Brushes.Gray;
            Pen styloNoir = Pens.Black;

            for (int i = 0; i < Lignes; i++)
            {
                for (int j = 0; j < Colonnes; j++)
                {
                    if (grille[i, j] == 0)
                    {
                        Rectangle rect = new Rectangle(j * TailleCarre, i * TailleCarre, TailleCarre, TailleCarre);
                        g.FillRectangle(brosseGris, rect);
                        g.DrawRectangle(styloNoir, rect);
                    }
                }
            }

            // Dessiner l'image du joueur en fonction de son état
            if (joueur.EstPioche)
            {
                g.DrawImage(jpioche, joueur.Y * TailleCarre, joueur.X * TailleCarre, TailleCarre, TailleCarre);
            }
            else if (joueur.EstEpee)
            {
                g.DrawImage(jepee, joueur.Y * TailleCarre, joueur.X * TailleCarre, TailleCarre, TailleCarre);
            }
            else
            {
                g.DrawImage(joueurImage, joueur.Y * TailleCarre, joueur.X * TailleCarre, TailleCarre, TailleCarre);
            }

            foreach (var enemy in enemies)
            {
                g.DrawImage(zombieImage, enemy.Y * TailleCarre, enemy.X * TailleCarre, TailleCarre, TailleCarre);
            }

            if (bonusPioche.Actif)
            {
                g.DrawImage(pioche, bonusPioche.Y * TailleCarre, bonusPioche.X * TailleCarre, TailleCarre, TailleCarre);
            }

            if (EpeeBonus.Actif)
            {
                g.DrawImage(epee, EpeeBonus.Y * TailleCarre, EpeeBonus.X * TailleCarre, TailleCarre, TailleCarre);
            }

            // Dessiner les pièces
            foreach (var piece in pieces)
            {
                if (piece.Actif)
                {
                    g.DrawImage(pieceImage, piece.Y * TailleCarre, piece.X * TailleCarre, TailleCarre, TailleCarre);
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            directionActuelle = e.KeyCode;
            joueurMoveTimer.Start();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == directionActuelle)
            {
                joueurMoveTimer.Stop();
            }
        }

        private void DevenirEpee()
        {
            joueur.EstEpee = true;
            joueurPiocheTimer.Start();
        }

        private void DeplacerJoueur()
        {
            int dx = 0, dy = 0;

            switch (directionActuelle)
            {
                case Keys.Up:
                    dx = -1;
                    break;
                case Keys.Down:
                    dx = 1;
                    break;
                case Keys.Left:
                    dy = -1;
                    break;
                case Keys.Right:
                    dy = 1;
                    break;
            }

            int newX = joueur.X + dx;
            int newY = joueur.Y + dy;

            if (newX >= 0 && newX < Lignes && newY >= 0 && newY < Colonnes)
            {
                if (grille[newX, newY] == 1)
                {
                    joueur.Deplacer(dx, dy);
                }
                else if (joueur.EstPioche && grille[newX, newY] == 0)
                {
                    grille[newX, newY] = 1;
                    joueur.Deplacer(dx, dy);
                }

                // Vérification de l'activation du bonus pioche
                if (bonusPioche.Actif && joueur.X == bonusPioche.X && joueur.Y == bonusPioche.Y)
                {
                    EpeeBonus.Actif = false;
                    bonusPioche.Actif = false;
                    DevenirPioche();
                    bonusReappearTimer.Start();
                }

                // Vérification de l'activation du bonus epee
                if (EpeeBonus.Actif && joueur.X == EpeeBonus.X && joueur.Y == EpeeBonus.Y)
                {
                    EpeeBonus.Actif = false;
                    bonusPioche.Actif = false;
                    DevenirEpee();
                    bonusReappearTimer.Start();
                }

                // Permet de retirer une piece
                foreach (var piece in pieces)
                {
                    if (piece.Actif && joueur.X == piece.X && joueur.Y == piece.Y)
                    {
                        piece.Actif = false;

                    }
                }

                for (int i = enemies.Count - 1; i >= 0; i--)
                {
                    if (joueur.X == enemies[i].X && joueur.Y == enemies[i].Y)
                    {
                        if (joueur.EstEpee)
                        {
                            enemies.RemoveAt(i);
                        }
                        else
                        {

                            if (!gameOver)
                            {
                                gameOver = true;
                                this.Close();
                                InterfaceLab.Defaite defaite = new InterfaceLab.Defaite();
                                defaite.Show();
                            }

                        }
                    }
                }


                // Vérification de la collecte de l'épée
                if (joueur.X == EpeeBonus.X && joueur.Y == EpeeBonus.Y)
                {
                    DevenirEpee();
                    EpeeBonus.Actif = false;
                    pictureBox.Invalidate();
                }

                if (ToutesPiecesCollectees())
                {
                    if (!victory)
                    {
                        victory = true;
                        this.Close();
                        InterfaceLab.Victoire victoire = new InterfaceLab.Victoire();
                        victoire.Show();
                    }

                }

                pictureBox.Invalidate();
            }
        }


        private bool ToutesPiecesCollectees()
        {
            foreach (var piece in pieces)
            {
                if (piece.Actif)
                {
                    return false;
                }
            }
            return true;
        }

        private void DevenirPioche()
        {
            joueur.EstPioche = true;
            joueurPiocheTimer.Start();
        }

        private void FinirTransformationPioche()
        {
            joueur.EstPioche = false;
            joueur.EstEpee = false;
            joueurPiocheTimer.Stop();
            pictureBox.Invalidate();
        }

        private void MoveEnemies()
        {
            foreach (var enemy in enemies)
            {
                enemy.Move(grille);
            }
            pictureBox.Invalidate();
        }

    }
}
