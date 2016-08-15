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
            //stream.WriteLine("sep=,");
            string[] temp = new string[Program.col];
            for (int i = 0; i < Program.rows; i++)
            {

                for (int j = 0; j < Program.col; j++)
                {
                    if (Program.dataset[i, j])
                    {
                        temp[j] = "1";
                    }
                    else
                    {
                        temp[j] = "0";
                    }
                }
                stream.WriteLine(String.Join(" ", temp));
            }

            stream.Close();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="filename"></param>
        public void outputTiling(Coverage t, string filename)
        {   

            StreamWriter stream = new StreamWriter(filename, false, Encoding.ASCII);
            stream.WriteLine("sep=,");
            string[] temp = new string[Program.col+Program.nTiles];
            for(int k=0; k<Program.nTiles;k++)
            {
                for (int k1 = 0; k1 < Program.nTiles; k1++)
                {
                    temp[k1] = ' '.ToString();
                }
                    for (int j = 0; j < Program.col; j++)
                {
                    temp[Program.nTiles+j] = t.getGene(k,j).ToString();
                }
                stream.WriteLine(String.Join(",", temp));
            }


            temp = new string[Program.nTiles];

            int i;
            for (int j = 0; j < Program.rows; j++)
            {
                i = 0;
                for (int k =0; k<Program.nTiles; k++)
                {
                    temp[i] = t.getPheno(k,j).ToString();
                    i++;
                }
                stream.WriteLine(String.Join(",", temp));
            }



            stream.Close();
        }
    }
}
