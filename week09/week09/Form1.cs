﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using week09.Entities;

namespace week09
{
    public partial class Form1 : Form
    {
        Random rng = new Random(1234);
        List<Person> Population = null;
        List<Birthprobability> BirthProbabilities = null;
        List<DeathProbability> DeathProbabilities = null;
        
        public Form1()
        {
            InitializeComponent();

            BirthProbabilities = GetBirthProbabilities(@"C:\Temp\születés.csv");
            DeathProbabilities = GetDeathProbabilities(@"C:\Temp\halál.csv");
            
        }

        private void StartSimulation(int endYear, string csvPath)
        {
            Population = GetPopulation(csvPath);
            var gy = new List<int>();

            for (int year = 2005; year <= endYear; year++)
            {
                for (int i = 0; i < Population.Count; i++)
                {
                    SimStep(year, Population[i]);
                }

                int nbrOfMales = (from x in Population
                                  where x.Gender == Gender.Male && x.IsAlive
                                  select x).Count();

                int nbrOfFemales = (from x in Population
                                    where x.Gender == Gender.Female && x.IsAlive
                                    select x).Count();

                

                Console.WriteLine(string.Format("Év: {0}\nFiúk: {1}\nLányok: {2}\n",
                    year,
                    nbrOfMales,
                    nbrOfFemales));


                DisplayResults(year, nbrOfMales, nbrOfFemales);

                
            }

            richTextBox1.Text += "-------------------------------------------------------------------\n";

        }

        
        private void DisplayResults(int year, int nbrOfMales, int nbrOfFemales)
        {
            
            richTextBox1.Text+= (string.Format("Év: {0}\nFiúk: {1}\nLányok: {2}\n",
                    year,
                    nbrOfMales,
                    nbrOfFemales));



        }


        private void SimStep(int year, Person person)
        {
            //Ha halott akkor kihagyjuk, ugrunk a ciklus következő lépésére
            if (!person.IsAlive) return;

            // Letároljuk az életkort, hogy ne kelljen mindenhol újraszámolni
            byte age = (byte)(year - person.BirthYear);

            // Halál kezelése
            // Halálozási valószínűség kikeresése
            double pDeath = (from x in DeathProbabilities
                             where x.Gender == person.Gender && x.Age == age
                             select x.P).FirstOrDefault();
            // Meghal a személy?
            if (rng.NextDouble() <= pDeath)
                person.IsAlive = false;

            //Születés kezelése - csak az élő nők szülnek
            if (person.IsAlive && person.Gender == Gender.Female)
            {
                //Szülési valószínűség kikeresése
                double pBirth = (from x in BirthProbabilities
                                 where x.Age == age
                                 select x.P).FirstOrDefault();
                //Születik gyermek?
                if (rng.NextDouble() <= pBirth)
                {
                    Person újszülött = new Person();
                    újszülött.BirthYear = year;
                    újszülött.NbrOfChildren = 0;
                    újszülött.Gender = (Gender)(rng.Next(1, 3));
                    Population.Add(újszülött);
                }
            }
        }


        public List<Person> GetPopulation(string csvPath)
        {
            List<Person> population = new List<Person>();

            using (StreamReader sr = new StreamReader(csvPath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line= sr.ReadLine().Split(';');
                    var p = new Person();
                    p.BirthYear = int.Parse(line[0]);
                    p.Gender = (Gender)Enum.Parse(typeof(Gender), line[1]);
                    p.NbrOfChildren = int.Parse(line[2]);
                    population.Add(p);
                }
            }

            return population;

        }

        public List<Birthprobability> GetBirthProbabilities(string csvPath)
        {
            List<Birthprobability> birthProbability = new List<Birthprobability>();

            using (StreamReader sr = new StreamReader(csvPath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    birthProbability.Add(new Birthprobability()
                    {
                        Age = byte.Parse(line[0]),
                        NbrOfChildren = int.Parse(line[1]),
                        P= double.Parse(line[2])
                    });
                }
            }

            return birthProbability;
        }

        public List<DeathProbability> GetDeathProbabilities(string csvPath)
        {
            List<DeathProbability> deathProbability = new List<DeathProbability>();

            using (StreamReader sr = new StreamReader(csvPath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    deathProbability.Add(new DeathProbability()
                    {
                        Age = byte.Parse(line[0]),
                        Gender = (Gender)Enum.Parse(typeof(Gender), line[1]),
                        P = double.Parse(line[2])
                    });
                }
            }

            return deathProbability;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.FileName = textBoxFile.Text;

            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            textBoxFile.Text = ofd.FileName;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            StartSimulation((int)nuUpD.Value, textBoxFile.Text);

        }



    }
}
