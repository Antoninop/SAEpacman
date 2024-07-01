// Vérification de la collecte de l'épée
using System.Windows.Forms;
using LabyBuild;

if (joueur.X == ObtenirEpee.X && joueur.Y == ObtenirEpee.Y)
{
    ObtenirEpee();
    ObtenirEpee.Actif = false;

}

//permet de faire réapparaitre les bonus quand il le bonus effectif n'est plus actif

private void ReappearBonuses()
{
    if (!Pioche.Actif)
    {
        Pioche.Reappear();
    }
    if (!Epee.Actif)
    {
        Epee.Reappear();
    }
}


//permet de changer l'état du joueur et initialise le chrono du bonus 

private void ObtenirEpee()
{
    ObtenirEpee.Actif = true;
    TimerEpee.Start();
}

//code permettant de changer l'état du bonus du joueur ( si bonus actif et récupère un autre le remplace pour ne pas avoir les deux 2 en mm temps)
if (multicolorBonus.Actif && joueur.X == multicolorBonus.X && joueur.Y == multicolorBonus.Y)
{
    multicolorBonus.Actif = false;
    bonusVert.Actif = false;
    DevenirMulticolor();
    bonusReappearTimer.Start();
}


// dans la partie collision avec les ennemies le bonus épée est vérifié ( si il le possède alors supprime l'ennemie sinon la partie est terminé 

for (int i = enemies.Count - 1; i >= 0; i--)
{
    if (joueur.X == enemies[i].X && joueur.Y == enemies[i].Y)
    {
        if (joueur.EstMulticolor)
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


private void InitializeBonusReappearTimer()
{
    bonusReappearTimer = new System.Windows.Forms.Timer
    {
        Interval = 30000
    };
    bonusReappearTimer.Tick += (s, e) => ReappearBonuses();
}

// initialisation de la durée vie des deux bonus

private void InitializeJoueurBonusTimer()
{
    JoueurBonusTimer = new System.Windows.Forms.Timer
    {
        Interval = 15000
    };
    JoueurBonusTimer.Tick += (s, e) => FinirTransformationVerte();
}












// Ajout des bonus dans le labyrinthe
private void AjouterBonus()
{
    Random random = new Random();
    int x, y;

    // Génération aléatoire de la position du bonus pioche
    do
    {
        x = random.Next(1, Lignes);
        y = random.Next(1, Colonnes);
    } while (grille[x, y] != 1 || (x == 1 && y == 1));

    bonusPioche = new Bonus(x, y);

    // Génération aléatoire de la position du bonus multicolor
    do
    {
        x = random.Next(1, Lignes);
        y = random.Next(1, Colonnes);
    } while (grille[x, y] != 1 || (x == 1 && y == 1));

    multicolorBonus = new MulticolorBonus(x, y);
}

// Réapparition des bonus
private void ReappearBonuses()
{
    if (!bonusPioche.Actif)
    {
        ReappearBonusPioche();
    }
    if (!multicolorBonus.Actif)
    {
        multicolorBonus.Reappear(grille, Lignes, Colonnes);
    }
}

// Réapparition du bonus pioche
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

// Initialisation du timer de réapparition des bonus
private void InitializeBonusReappearTimer()
{
    bonusReappearTimer = new System.Windows.Forms.Timer
    {
        Interval = 30000
    };
    bonusReappearTimer.Tick += (s, e) => ReappearBonuses();
}

// Gestion de la transformation verte du joueur
private void DevenirVert()
{
    joueur.EstVert = true;
    joueurVertTimer.Start();
}

// Fin de la transformation verte du joueur
private void FinirTransformationVerte()
{
    joueur.EstVert = false;
    joueur.EstMulticolor = false;
    joueurVertTimer.Stop();
    pictureBox.Invalidate();
}

// Gestion de la collecte des bonus lors du déplacement du joueur
private void DeplacerJoueur()
{
    int dx = 0, dy = 0;
    switch (directionActuelle)
    {
        case Keys.Up: dx = -1; break;
        case Keys.Down: dx = 1; break;
        case Keys.Left: dy = -1; break;
        case Keys.Right: dy = 1; break;
    }

    int newX = joueur.X + dx;
    int newY = joueur.Y + dy;

    if (newX >= 0 && newX < Lignes && newY >= 0 && newY < Colonnes && grille[newX, newY] == 1)
    {
        joueur.Deplacer(dx, dy);

        // Vérification de l'activation du bonus pioche
        if (bonusPioche.Actif && joueur.X == bonusPioche.X && joueur.Y == bonusPioche.Y)
        {
            multicolorBonus.Actif = false;
            bonusPioche.Actif = false;
            DevenirVert();
            bonusReappearTimer.Start();
        }

        // Vérification de l'activation du bonus épée
        if (multicolorBonus.Actif && joueur.X == multicolorBonus.X && joueur.Y == multicolorBonus.Y)
        {
            multicolorBonus.Actif = false;
            bonusPioche.Actif = false;
            DevenirMulticolor();
            bonusReappearTimer.Start();
        }

        pictureBox.Invalidate();
    }
}

// Initialisation du timer de transformation verte du joueur
private void InitializeJoueurVertTimer()
{
    joueurVertTimer = new System.Windows.Forms.Timer
    {
        Interval = 15000
    };
    joueurVertTimer.Tick += (s, e) => FinirTransformationVerte();
}
