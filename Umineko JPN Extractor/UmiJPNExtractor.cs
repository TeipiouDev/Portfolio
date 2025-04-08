using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace UminekoJPNExtractor
{
    class UmiJPNExtractor
    {
        static void Main(string[] args)
        {

            //Haetaan skriptistä jokaisen episodin alku ja rinnastetaan se episodin nimen kanssa.
            //Näyttää hirveältä ja on ehkä purkkaratkaisu, mutta toimii hyvin.
            Dictionary<string, string> titles = new Dictionary<string, string>();
            titles.Add("*umi1_opning", "うみねこのなく頃にEP1　〜Legend of the golden witch〜");
            titles.Add("*umi2_opning", "うみねこのなく頃にEP2　〜Turn of the golden witch〜");
            titles.Add("*umi3_opning", "うみねこのなく頃にEP3　〜Banquet of the golden witch〜");
            titles.Add("*Umi4_opning", "うみねこのなく頃にEP4　〜Alliance of the golden witch〜");
            titles.Add("*umi5_opning", "うみねこのなく頃にEP5　〜End of the golden witch〜");
            titles.Add("*umi6_opning", "うみねこのなく頃にEP6　〜Dawn of the golden witch〜");
            titles.Add("*umi7_opning", "うみねこのなく頃にEP7　〜Requiem of the golden witch〜");
            titles.Add("*umi8_opning", "うみねこのなく頃にEP8　〜Twilight of the golden witch〜");

            titles.Add("*teatime_1", "うみねこのなく頃にEP1　〜Legend of the golden witch〜 Tea Party");
            titles.Add("*teatime_2", "うみねこのなく頃にEP2　〜Turn of the golden witch〜 Tea Party");
            titles.Add("*teatime_3", "うみねこのなく頃にEP3　〜Banquet of the golden witch〜 Tea Party");
            titles.Add("*teatime_4", "うみねこのなく頃にEP4　〜Alliance of the golden witch〜 Tea Party");
            titles.Add("*teatime_5", "うみねこのなく頃にEP5　〜End of the golden witch〜 Tea Party");
            titles.Add("*teatime_6", "うみねこのなく頃にEP6　〜Dawn of the golden witch〜 Tea Party");
            titles.Add("*teatime_7", "うみねこのなく頃にEP7　〜Requiem of the golden witch〜 Tea Party");
            titles.Add("*teatime_8", "うみねこのなく頃にEP8　〜Twilight of the golden witch〜 Tea Party");

            titles.Add("*ura_teatime_1", "うみねこのなく頃にEP1　〜Legend of the golden witch〜 Secret Tea Party");
            titles.Add("*ura_teatime_2", "うみねこのなく頃にEP2　〜Turn of the golden witch〜 Secret Tea Party");
            titles.Add("*ura_teatime_3", "うみねこのなく頃にEP3　〜Banquet of the golden witch〜 Secret Tea Party");
            titles.Add("*Ura_teatime_4", "うみねこのなく頃にEP4　〜Alliance of the golden witch〜 Secret Tea Party");
            titles.Add("*ura_teatime_5", "うみねこのなく頃にEP5　〜End of the golden witch〜 Secret Tea Party");
            titles.Add("*ura_teatime_6", "うみねこのなく頃にEP6　〜Dawn of the golden witch〜 Secret Tea Party");
            titles.Add("*ura_teatime_7", "うみねこのなく頃にEP7　〜Requiem of the golden witch〜 Secret Tea Party");
            titles.Add("*ura_teatime_8", "うみねこのなく頃にEP8　〜Twilight of the golden witch〜 Secret Tea Party");

            //Kaikki roskat jotka poistetaan rivistä.
            List<string> filterWords = new List<string>();
            filterWords.Add("#ff0000");
            filterWords.Add("#ffffff");
            filterWords.Add("#ffcc00");
            filterWords.Add("#cc99ff");
            filterWords.Add("#5decff");
            filterWords.Add("#5DECFF");
            filterWords.Add("");     //Jokin ristimerkki joka ei näy visual studiossa.
            filterWords.Add("@");
            filterWords.Add("\\");
            filterWords.Add("~ib~");

            const int THRESHOLD = 20; //Rivin haluttu pituus.
            
            string path = "0_Answer.utf"; //Tähän Question tai Answer arcin skriptin sijainti.
            string pathEndJPN = "JPN.txt"; //Output-tiedoston päätenimi.
            string pathEndENG = "ENG.txt";
            string pathEndBoth = "Both.txt";


            char[] endCharJPN = { '。', '」', '！', '？', '』'};
            char[] endCharENG = { '.', '!', '?', '"', '>', '』' };

            string line;
            StringBuilder sbJPN = new StringBuilder(); //Tähän kootaan kaikki löydetty dialogi.
            StringBuilder sbENG = new StringBuilder();
            StringBuilder sbBoth = new StringBuilder();

            StreamReader sr = new StreamReader(path);

            string state = null; //Käsiteltävä episodi.

            //Käydään tiedosto läpi rivi kerrallaan.
            while ((line = sr.ReadLine()) != null)
            {

                //Jos rivi oli sopivasti episodin alku, eli rivin sisältö on suoraan Dictin avain.
                if(titles.ContainsKey(line))
                {

                    if (state == null) state = line;

                    //Jos siirryttiin uuteen tarinaan
                    if (state != line)
                    {

                        //Oksennetaan nykyisen StringBuilderin sisältö tiedostoon
                        using (StreamWriter sw = new StreamWriter(titles[state] + pathEndJPN))
                        {
                            sw.Write(sbJPN);
                        }

                        using (StreamWriter sw = new StreamWriter(titles[state] + pathEndENG))
                        {
                            sw.Write(sbENG);
                        }

                        using (StreamWriter sw = new StreamWriter(titles[state] + pathEndBoth))
                        {
                            sw.Write(sbBoth);
                        }

                        sbJPN.Clear();
                        sbENG.Clear();
                        sbBoth.Clear();

                        //Vaihdetaan nykyinen käsiteltävä episodi.
                        state = line;

                    }

                    //Jaksojen otsikot alkuun.
                    //Titles(line, sb, titles);

                }

                //Otetaan huomioon vain ja ainoastaan japanilainen dialogi.
                //Skriptin seassa on mm. kommentteja japaniksi, joita ei haluta mukaan.
                if (line.StartsWith("langjp"))
                {

                    //Regex löytämään kaikki kanjit ja japanilaiset erikoismerkit. Lopun
                    //… merkki on erikseen lisätty, sillä en löytänyt sille heti unicode vastinetta
                    //ja näyttäisi siltä, että se on ainoa japanilainen merkki, joka puuttuu välistä
                    //x2e80 - xff9f.
                    line = Regex.Replace(line, "[^\x2E80-\xFF9F…]", "");
                    
                    Console.WriteLine(line);

                    //Järkevän rivityksen toteutus. Jonkin rajan jälkeen sopiva katkaisu 。」！？ merkin kohdalla.
                    //Idea: Splittaa rivi jokaisesta sellaisesta kohdasta, jossa on lopetusmerkin jälkeen jokin merkki,
                    //joka ei ole lopetusmerkki.
                    if (line.Length > THRESHOLD)
                    {
                    
                        List<string> lines = SplitByCharacter(line, THRESHOLD, endCharJPN);
                        foreach (string str in lines)
                        {
                            sbJPN.Append(str + Environment.NewLine);
                            sbBoth.Append(str + Environment.NewLine);
                        }
                    }
                    else
                    {
                        sbJPN.Append(line + Environment.NewLine);
                        sbBoth.Append(line + Environment.NewLine);
                    }
                }
                //Enkkukäännös erikseen. Pitää vielä keksiä miten väärällä rakenteella olevat
                //dialogit saa mukaan.
                else if (line.StartsWith("langen"))  
                {

                    //Regex etsimään circumflexin välillä oleva dialogi.
                    //Kuudennen episodin lopussa ainakin rakenne hajoaa.
                    MatchCollection mc = Regex.Matches(line, "\\^(.*?)\\^");

                    line = string.Join("", from Match match in mc select match.Value);
                    line = Regex.Replace(line, "\\^", "");

                    //Ylimääräinen roska pois.
                    line = ReplaceAll(line, filterWords, "");
                    
                    Console.WriteLine(line);

                    if (line.Length > THRESHOLD)
                    {

                        List<string> lines = SplitByCharacter(line, THRESHOLD, endCharENG);
                        foreach (string str in lines)
                        {
                            sbENG.Append(str + Environment.NewLine);
                            sbBoth.Append(str + Environment.NewLine);
                        }
                    }
                    else
                    {
                        sbENG.Append(line + Environment.NewLine);
                        sbBoth.Append(line + Environment.NewLine);
                    }

                    sbBoth.Append(Environment.NewLine);
                }

            }

            //Lopuksi stringbuilderin sisällön oksentaminen uuteen tiedostoon
            using (StreamWriter sw = new StreamWriter(titles[state] + pathEndJPN))
            {
                sw.Write(sbJPN);
            }

            using (StreamWriter sw = new StreamWriter(titles[state] + pathEndENG))
            {
                sw.Write(sbENG);
            }

            using (StreamWriter sw = new StreamWriter(titles[state] + pathEndBoth))
            {
                sw.Write(sbBoth);
            }

        }

        //Aliohjelma splittaamaan merkkijono paloiksi haluttujen merkkien mukaan.
        public static List<string> SplitByCharacter(string line, int threshold, char[] limiter)
        {

            int endIndex = 0; //Pitää kirjaa leikatun jonon pääteindeksistä.
            int counter = 0; //Jos rivi on vaikka yli 100 merkkiä pitkä, niin halutaan voida jakaa se useampaan palaan.

            List<string> result = new List<string>();

            
            for (int i = 1; i < line.Length; i++)
            {
                
                counter++;

                if ( (limiter.Contains(line[i - 1]) == true) && (limiter.Contains(line[i]) == false) && (counter > threshold))
                {
                    result.Add(line.Substring(endIndex, i - endIndex));
                    endIndex = i;
                    counter = 0;
                }
                
            }

            result.Add(line.Substring(endIndex));

            return result;
            
        }

        //Korvataan kaikki merkkijonot tai merkit tietyllä merkkijonolla tai merkillä.
        public static string ReplaceAll(string line, List<string> filterList, string replaceString)
        {

            foreach (string str in filterList)
            {
                line = line.Replace(str, replaceString);
            }

            return line;

        }

        //Episodien nimet siististi otsikoksi.
        public static void Titles(string line, StringBuilder sb, Dictionary<string, string> titles)
        {

            sb.Append(Environment.NewLine);
            sb.Append(new String('=', 50));
            sb.Append(Environment.NewLine);
            sb.Append(titles[line]);
            sb.Append(Environment.NewLine);
            sb.Append(new String('=', 50));
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);

        }

    }
}
