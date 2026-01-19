using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooterC2
{
    internal class Highscore 
    {
        public List<Spelare> Scores;

        public Highscore()
        {
            Scores = new List<Spelare>();

            StreamReader sr = new StreamReader("highscore.txt");
            string row;
            while ((row = sr.ReadLine()) != null)
            {
                string[] uppgifter = row.Split("\t");

                int score = int.Parse(uppgifter[0]);
                string datum = uppgifter[1];
                Spelare temp = new Spelare(score, datum);
                Scores.Add(temp);
            }

            Scores.Sort();
        }

    }




    class Spelare : IComparable<Spelare>
    {
        int poäng;
        string datum;

        public Spelare(int poäng, string datum)
        {
            this.poäng = poäng;
            this.datum = datum;
        }

        public int Poäng
        {
            get { return poäng; }
            set { poäng = value; }
        }

        public string Datum
        {
            get { return datum; }
            set { datum = value; }
        }


        public int CompareTo(Spelare other)
        {
            return other.poäng.CompareTo(this.poäng); // fallande
        }
    }
}
