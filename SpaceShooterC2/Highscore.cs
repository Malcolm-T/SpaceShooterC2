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
            string path = "highscore.txt";
            if (!File.Exists(path))
            {
                return;
            }
            try
            {
                using(StreamReader sr = new StreamReader(path))
                {
                    string row;
                    while ((row = sr.ReadLine()) != null)
                    {
                        string[] uppgifter = row.Split("\t");

                        if(uppgifter.Length >= 2)
                        {
                            if (int.TryParse(uppgifter[0], out int score))
                            {
                                string datum = uppgifter[1];
                                Spelare temp = new Spelare(score, datum);
                                Scores.Add(temp);

                            }
                        }
                    }
                }
                Scores.Sort();
            }
            catch(Exception e) {Console.WriteLine("Fel uppstod vid inläsning av highscore" + e.ToString());}
        }


        public void NyttScore (int poäng)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter("highscore.txt", true))
                {
                    writer.WriteLine(poäng + "\t" + DateTime.Now.ToString("yyyy-MM-dd"));
                }
            }
            catch (Exception e) {Console.WriteLine("Fel uppstod vid uppladdning av highscore" + e.ToString());}
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
