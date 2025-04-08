using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace UminekoJPNExtractor
{
    class Program
    {
        static void Main(string[] args)
        {

            string path = "0_Question.utf";

            StringBuilder sb = new StringBuilder();

            string line;
            
            StreamReader sr = new StreamReader(path);

            while ((line = sr.ReadLine()) != null)
            {

                if(line.StartsWith("langjp"))
                {

                    line = Regex.Replace(line, "[^\x2E80-\xFF9F…]", String.Empty);

                    /*
                    line = line.Replace("langjp", String.Empty);
                    line = line.Replace("@", String.Empty);
                    line = line.Replace("^^!", String.Empty);
                    line = line.Replace("!", String.Empty);
                    line = line.Replace("d200", String.Empty);
                    line = line.Replace("d300", String.Empty);
                    line = line.Replace("d400", String.Empty);
                    line = line.Replace("d500", String.Empty);
                    line = line.Replace("d600", String.Empty);
                    line = line.Replace("d700", String.Empty);
                    line = line.Replace("d800", String.Empty);
                    line = line.Replace("d900", String.Empty);
                    line = line.Replace("d1000", String.Empty);


                    line = line.Replace("w700", String.Empty);
                    line = line.Replace("w800", String.Empty);

                    line = line.Replace("w1000", String.Empty);
                    line = line.Replace("s0", String.Empty);

                    line = line.Replace("s0", String.Empty);
                    line = line.Replace("sd", String.Empty);
                    line = line.Replace("s1", String.Empty);
                    line = line.Replace("s300", String.Empty);

                    line = line.Replace("#ff0000", String.Empty);
                    line = line.Replace("#ffffff", String.Empty);
                    line = line.Replace("#5decff", String.Empty);

                    line = line.Trim(new Char[] { '\\', '/' });

                 */   

                    Console.WriteLine(line);
                    sb.Append(line + Environment.NewLine);
                }

            }

            using (StreamWriter sw = new StreamWriter(path + "JPN.txt"))
            {
                sw.Write(sb);
            }

        }
    }
}
