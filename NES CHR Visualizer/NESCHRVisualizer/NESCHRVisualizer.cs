using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NESCHRVisualizer
{
    class NESCHRVisualizer
    {
        static void Main(string[] args)
        {

            if (args.Length == 0)
            {
                Console.WriteLine("Tiedostoja ei annettu, anna vähintään yksi NES ROM-tiedosto!");
                Console.WriteLine("Esimerkki: NESCHRVisualizer \"Oma NES Peli.nes\" ");
                Environment.Exit(1);
            }

            Color[] palette = { Color.Black, Color.Red, Color.Orange, Color.White };

            string filePath;

            if (args.Length != 0)
            {

                for (int a = 0; a < args.Count(); a++)
                {

                    filePath = args[a];
                    List<NESSprite> sprites = new List<NESSprite>();

                    //Luetaan kahdeksan tavua ja väritetään 1-bitit yhdellä värillä.
                    //Luetaan seuraavat kahdeksan tavua ja väritetään 1-bitit toisella värillä
                    //Jos molemmat bitit ovat 1, väritetään kolmannella värillä.

                    long fileSize;

                    try
                    {
                        using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                        {

                            Console.WriteLine("Käydään ROM-tiedostoa " + filePath + " läpi...");
                            if (!ValidateHeader(fs))
                            {
                                Console.WriteLine("Tiedoston " + filePath + " otsikko ei vastaa iNES-formaattia.");
                                continue;
                            }

                            fs.Position = 0x00000;

                            fileSize = fs.Length;

                            while (fs.Position < fs.Length)
                            {
                                NESSprite sprite = new NESSprite();

                                sprite.Palette = palette;
                                sprite.SetData(fs, (int)fs.Position);
                                sprites.Add(sprite);
                            }
                        }

                        Bitmap img = new Bitmap(1024, (int)fileSize / 256, PixelFormat.Format32bppArgb);

                        Graphics g = Graphics.FromImage(img);

                        g.CompositingMode = CompositingMode.SourceOver;

                        int i = 0;
                        for (int heightInBlocks = 0; heightInBlocks < (int)img.Height / 8; heightInBlocks++)
                        {

                            for (int widthInBlocks = 0; widthInBlocks < (int)img.Width / 8; widthInBlocks++)
                            {

                                g.DrawImage(sprites[i].GetBitmap(), widthInBlocks * 8, heightInBlocks * 8);
                                i++;
                            }
                        }

                        img.Save(Path.GetFileNameWithoutExtension(filePath) + ".png");

                    }

                    catch (FileNotFoundException)
                    {

                        Console.WriteLine("Tiedostoa " + filePath + " ei löytynyt.");
                    }


                }
            }

            Console.WriteLine("Valmis.");

        }

        //Stackoverflow-foorumeilta röyhkeästi pöllitty simppeli aliohjelma tarkistamaan, onko tavun tietty bitti 1 vai ei. 
        static bool IsBitSet(byte b, int pos)
        {
            return (b & (1 << pos)) != 0;
        }

        //Tarkistetaan NES-tiedoston otsikko
        //TODO: Tee tarkistus kunnolla
        static bool ValidateHeader(FileStream fs)
        {
            
            fs.Position = 0;

            //NES-otsikko tiedosto on 16 tavun kokoinen, josta neljä ensimmäistä tavua sisältää kirjaimet N, E ja S ja viimeisenä MS-DOS rivinvaihdon
            //Tarvittaessa laajennetaan tarkemmaksi tarkistukseksi.
            if ((byte)fs.ReadByte() != 0x4E
                || (byte)fs.ReadByte() != 0x45 
                || (byte)fs.ReadByte() != 0x53 
                || (byte)fs.ReadByte() != 0x1A)
                return false;
            

            //PRG ROM koko 16384 tavun yksikköinä
            int prg = fs.ReadByte();

            //CHR ROM koko 8192 tavun yksikköinä, 0 = käytetään CHR RAM
            int chr = fs.ReadByte();

            byte mapper = (byte)fs.ReadByte();
        
    
            return true;

        }
    }
}
