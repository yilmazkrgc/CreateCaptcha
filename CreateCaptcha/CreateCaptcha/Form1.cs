using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CreateCaptcha
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //captcha için random string üretir
        //generates random text for captcha
        string GetRandomTextForCaptcha()
        {
            StringBuilder sb = new StringBuilder();
            string alphabet = "012345679ABCDEFGHIJKLMNOPRSTUVWXZabcdefghijkhlmnopqrstuvwxyz";
            Random rnd = new Random();
            for (int i = 0; i < 5; i++)
            {
                sb.Append(alphabet[rnd.Next(alphabet.Length)]);
            }

            return sb.ToString();
        }

        //captcha için Bitmap oluşturup Binary serialize işlemini gerçekleştirir.
        //genereates Bitmap and perform Binary Serialize for Captcha
        public byte[] ConvertImageSerializeForCaptcha()
        {

            //Fontların belirlenmesi
            string[] fontNames = new string[]
   {
 "Comic Sans MS",
 "Arial",
 "Times New Roman",
 "Georgia",
 "Verdana",
 "Geneva"
   };

            //Captcha için metin oluşturma
            string randomText = GetRandomTextForCaptcha();


            Graphics draw;
            Bitmap img;
            Random random = new Random();

            img = new Bitmap(180, 80);
            draw = Graphics.FromImage(img);

            //Rastgele font belirleme(farklı boyutlarda)
            Font font = new Font(fontNames[random.Next(6)], random.Next(20, 40));

            //arkaplan ve yazı rengi rastgele belirlenir.
            Color backColor = Color.FromArgb(random.Next(128), random.Next(128), random.Next(128));
            Color textColor = Color.FromArgb(random.Next(128, 256), random.Next(128, 256), random.Next(128, 256));

            //Arka planın çizgili olması için gerekli fırça tipi seçilir.
            Brush backBrush = new HatchBrush(HatchStyle.DashedVertical, backColor);

            //Yazı tipinin normal olması için gerekli fırça tipi seçilir.
            Brush textBrush = new SolidBrush(textColor);

            //arkaplan oluşturulur.
            draw.FillRectangle(backBrush, 0, 0, img.Size.Width, img.Size.Height);

            //captcha yazısı oluşturulur.
            draw.DrawString(randomText, font, textBrush, 0, 0);

            draw.Save();

            font.Dispose();
            backBrush.Dispose();
            textBrush.Dispose();
            draw.Dispose();

            var stream = new MemoryStream();
            var formatter = new BinaryFormatter();

            //image nesnesi serialize edilir.
            formatter.Serialize(stream, img);

            byte[] temp = stream.ToArray();

            stream.Dispose();

            return temp;

        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            //Binary deserialize işlemi yapılır ve dönen object tipi Image tipine cast edilir.
            byte[] captcha = ConvertImageSerializeForCaptcha();
            var stream = new MemoryStream(captcha);
            var formatter = new BinaryFormatter();
            Image img = (Image)formatter.Deserialize(stream);
            pctr.Image = img;
        }
    }
}
