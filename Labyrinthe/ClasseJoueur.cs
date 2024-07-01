public class ClasseJoueur
{
    public int X { get; set; }
    public int Y { get; set; }
    public bool EstPioche { get; set; }
    public bool EstEpee { get; set; }

    public ClasseJoueur(int x, int y)
    {
        X = x;
        Y = y;
        EstPioche = false;
        EstEpee = false;
    }

    public void Deplacer(int dx, int dy)
    {
        X += dx;
        Y += dy;
    }
}
