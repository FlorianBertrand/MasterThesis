using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MT
{
    class Output
    {
        public Output() { }

        public void output(string s, string filename)
        {
            StreamWriter stream = new StreamWriter(filename, true, Encoding.ASCII);
            //stream.WriteLine("sep=,");
            stream.WriteLine(s);
            stream.Close();

        }
        public void output(string filename)
        {
            StreamWriter stream = new StreamWriter(filename, false, Encoding.ASCII);
            stream.WriteLine("sep=,");
            string[] temp = new string[Program.col];
            for (int i = 0; i < Program.rows; i++)
            {

                for (int j = 0; j < Program.col; j++)
                {
                    temp[j] = Program.dataset[i, j].ToString();
                }
                stream.WriteLine(String.Join(",", temp));
            }

            stream.Close();
        }

        public void outputTiling(Tiling t, string filename)
        {   

            StreamWriter stream = new StreamWriter(filename, false, Encoding.ASCII);
            stream.WriteLine("sep=,");
            string[] temp = new string[Program.col];
            foreach (Tile tt in t.geno)
            {

                for (int j = 0; j < Program.col; j++)
                {
                    temp[j] = tt.getGene(j).ToString();
                }
                stream.WriteLine(String.Join(",", temp));
            }


            temp = new string[t.geno.Count];

            int i;
            for (int j = 0; j < Program.rows; j++)
            {
                i = 0;
                foreach (Tile tt in t.geno)
                {
                    temp[i] = tt.getPheno(j).ToString();
                    i++;
                }
                stream.WriteLine(String.Join(",", temp));
            }



            stream.Close();
        }
    }
}
