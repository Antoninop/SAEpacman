﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InterfaceLab
{
    public partial class Defaite : Form
    {
        public Defaite()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LabyBuild.Form1 labyrintheForm = new LabyBuild.Form1();
            labyrintheForm.Show();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Defaite_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
