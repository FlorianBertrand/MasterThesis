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
        /// <summary>
        /// Output a string s in file filename
        /// </summary>
        /// <param name="s">String to output</param>
        /// <param name="filename">Name of the file</param>
        public void output(string s, string filename)
        {
            StreamWriter stream = new StreamWriter(filename, true, Encoding.ASCII);
            //stream.WriteLine("sep=,");
            stream.WriteLine(s);
            stream.Close();

        }
        /// <summary>
        /// Output the dataset in a file
        /// </summary>
        /// <param name="filename">Name of the file in which outputing the dataset</param>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="filename"></param>
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
