using System;
using System.Drawing;
using System.IO;

namespace HangFire.Web.BackgroundJobs
{
    public class DelayedJobs
    {
        public static string AddWaterMarkJob(string filename, string watermarkText)
        {
            //bu sefer Enqueue yerine schedule kullandık. // gecikmeyide verdik.
            return Hangfire.BackgroundJob.Schedule(() => ApplyWatermark(filename, watermarkText), TimeSpan.FromSeconds(20)); //dönüş değer olmayan fonksiyonu işaret ettiğimiz için () =>
            //Schedule çalıştıktan sonra id dönüyor. sebebi bir job çalıştıktan sonra başka bir job'un çalışması gerekebilir diye. Continues
        }

        public static void ApplyWatermark(string filename, string watermarkText)
        {
            //directory üzerinden önce güncel directory'i al. sonra wwwroot/pictures'i al. sonra filename dosyayı bul.
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/pictures", filename);

            // bitmap'ler ile çalışmak için system.drawing.common'ı nugget'tan eklemek lazım.
            using (var bitmap = Bitmap.FromFile(path)) //dosyayı bitmap haline dönüştürüyor. resmi aldık. sonrasında sıfırdan boş bir resim oluşturacağız.
            {
                using (Bitmap tempBitmap = new Bitmap(bitmap.Width, bitmap.Height)) // elimizde width ve height'ı olan boş bir çerçeve var.
                {
                    using (Graphics grp = Graphics.FromImage(tempBitmap)) // boş bir grafik oluşturduk.
                    {
                        grp.DrawImage(bitmap, 0, 0); //bitmap'i çizmek üzere al ve sıfıra sıfır noktasından çizmeye başla.

                        var font = new Font(FontFamily.GenericSansSerif, 25, FontStyle.Bold);

                        var color = Color.FromArgb(255, 0, 0);

                        var brush = new SolidBrush(color);

                        var point = new Point(20, bitmap.Height - 50);

                        grp.DrawString(watermarkText, font, brush, point);

                        tempBitmap.Save(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/pictures/watermarks", filename));
                    }
                }
            }
        }
    }
}