using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESCHRVisualizer
{
    class NESSprite
    {

        private byte[] data = new byte[16];
        private Color[] palette = new Color[4];
        private Bitmap bmp = new Bitmap(8, 8);

        //Muodostaa datasta kuvatiedoston.
        public Bitmap GetBitmap()
        {

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {

                    if (IsBitSet(data[i], j) && IsBitSet(data[i + 8], j))
                    {
                        bmp.SetPixel(7 - j, i, palette[3]);
                    }
                    else if (IsBitSet(data[i], j) && !IsBitSet(data[i + 8], j))
                    {
                        bmp.SetPixel(7 - j, i, palette[1]);
                    }
                    else if (!IsBitSet(data[i], j) && IsBitSet(data[i + 8], j))
                    {
                        bmp.SetPixel(7 - j, i, palette[2]);
                    }
                    else
                    {
                        bmp.SetPixel(7 - j, i, palette[0]);
                    }

                }
            }

            return bmp;

        }

        //Tallentaa spriten tiedostoon, .bmp liite lisätään tiedoston perään.
        public void SaveBitMap(string txtPath)
        {
            bmp.Save(txtPath + ".bmp");
        }

        //Jos taulukossa valmiina tavudata, laitetaan se spriten dataan.
        public void SetData(byte[] d)
        {
            data = d;
        }

        /// <summary>
        /// Lukee tiedostosta tavut talteen.
        /// </summary>
        /// <param name="txtPath">Tiedostopolku</param>
        /// <param name="position">Mistä kohtaan tiedostoa aloitetaan lukeminen</param>
        public void SetData(string txtPath, int position)
        {
            using (FileStream fs = new FileStream(txtPath, FileMode.Open, FileAccess.Read))
            {

                SetData(fs, position);

            }
        }

        /// <summary>
        /// Lukee tiedostovirrasta raakadatan talteen.
        /// </summary>
        /// <param name="fs">Tiedostovirta</param>
        /// <param name="position">Mistä kohtaa tiedostovirtaa aloitetaan lukeminen</param>
        public void SetData(FileStream fs, int position)
        {
            fs.Position = position;

            for (int i = 0; i < 16; i++)
            {
                data[i] = (byte)fs.ReadByte();

            }
        }

        public Color[] Palette
        {
            get { return palette; }
            set { palette = value; }
        }

        //Stackoverflow-foorumeilta röyhkeästi pöllitty simppeli aliohjelma tarkistamaan, onko tavun tietty bitti 1 vai ei. 
        private bool IsBitSet(byte b, int pos)
        {
            return (b & (1 << pos)) != 0;
        }
    }
}


