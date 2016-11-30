using ImageUploadDenemesi.Models;
using ImageUploadDenemesi.ViewModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageUploadDenemesi.Controllers
{
    public class Deneme02Controller : Controller
    {
        // GET: Deneme02
        public ActionResult Index()
        {
            UrunEklemeVM urunModel = ModelGetir();
            return View(urunModel);
        }
        public ActionResult Create()
        {
            UrunEklemeVM urunModel = ModelGetir();
            return View(urunModel);
        }
        [HttpPost]
        public ActionResult Create(UrunEklemeVM model)
        {
            denemeEntities db = new denemeEntities();
            Urun urunModel = new Urun();
            urunModel.KategoriId = model.KategoriId;
            urunModel.Adi = model.Adi;

            

            if (model.Resim != null && model.Resim.ContentLength > 0)
            {
                string ResimAdi = "";
                string ResimUzanti = "";

                ResimAdi = model.Adi;
                ResimUzanti = Path.GetExtension(model.Resim.FileName);

                model.Resim.SaveAs(Server.MapPath("~/Upload/SahteResim" + ResimUzanti));

                //800 px lik Resmimiz
                int Donusturme = 800;
                Bitmap BitMapNesnemiz = new Bitmap(Server.MapPath("/Upload/SahteResim" + ResimUzanti));
                using (Bitmap OrjinalResim = BitMapNesnemiz)
                {
                    double ResimYukseklik = OrjinalResim.Height;
                    double ResimGenislik = OrjinalResim.Width;
                    double Oran = 0;

                    if (ResimGenislik >= Donusturme) 
                    {
                        Oran = ResimGenislik / ResimYukseklik;
                        ResimGenislik = Donusturme;
                        ResimYukseklik = Donusturme / Oran;

                        Size YeniDegerler = new Size(Convert.ToInt32(ResimGenislik), Convert.ToInt32(ResimYukseklik));

                        Bitmap Resim = new Bitmap(OrjinalResim, YeniDegerler);

                        ImageCodecInfo myImageCodecInfo = GetEncoderInfo("image/jpeg");
                        Encoder myEncoder = Encoder.Quality;
                        EncoderParameters myEncoderParameters = new EncoderParameters(1);
                        EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 75L);
                        myEncoderParameters.Param[0] = myEncoderParameter;

                        Resim.Save(Server.MapPath("~/Upload/" + ResimAdi+ResimUzanti), myImageCodecInfo, myEncoderParameters);
                        Resim.Dispose();
                        OrjinalResim.Dispose();
                        BitMapNesnemiz.Dispose();

                    }
                    else
                    {
                        model.Resim.SaveAs(Server.MapPath("~/Upload/" + ResimAdi+ResimUzanti));
                    }
                }


                urunModel.Resim = ResimAdi + ResimUzanti;

                //Dönüştürme işlemleri bittiği zaman Sahte Resmimizi Artık silebiliriz boşuna yer tutmasın serverda.
                FileInfo fiSahteResim = new FileInfo(Server.MapPath("/Upload/SahteResim" + ResimUzanti));
                fiSahteResim.Delete();

            }



            db.Uruns.Add(urunModel);
            db.SaveChanges();
            ModelState.Clear(); //Text Boxları Temziler
            model = ModelGetir();
            model.kullaniciyaMesaj = "Kayıt işlemi başarılı bir şekilde gerçekleştirildi.";


        

            return View(model);
        }


        private UrunEklemeVM ModelGetir()
        {
            UrunEklemeVM model = new UrunEklemeVM();
            denemeEntities db = new denemeEntities();

            model.KategoriListesi = (from cat in db.Kategoris
                                     select new SelectListItem
                                     {
                                         Selected = false,
                                         Text = cat.Adi,
                                         Value = cat.ID.ToString()
                                     }).ToList();
            model.KategoriListesi.Insert(0, new SelectListItem
            {
                Selected = true,
                Value = "",
                Text = "==Kategori Seçiniz=="
            });

            return model;
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }



    }
}