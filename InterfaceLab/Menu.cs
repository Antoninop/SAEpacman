using LabyBuild;

namespace InterfaceLab
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LabyBuild.Form1 labyrintheForm = new LabyBuild.Form1();
            labyrintheForm.Show();
            this.Hide();

        }
    }
}
