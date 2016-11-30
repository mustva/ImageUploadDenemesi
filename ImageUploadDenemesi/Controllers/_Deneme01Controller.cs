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
    public class _Deneme01Controller : Controller
    {
        // GET: Deneme01

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

            string ResimAdi = "";
            string ResimUzanti = "";

            if (urunModel.Resim != null)
            {
                //Resim Uzantısını alıyoruz yüklenen resimden.
                ResimUzanti = Path.GetExtension(model.Resim.FileName);
                //ResimAdi nı oluşturuyoruz.
                ResimAdi = DateTime.Now.Day + ResimUzanti;

                //fuHaberResim ile ../Images/HaberResimleri dizinine SahteResim ismiyle Resimimizi kaydediyoruz.
                 model.Resim.SaveAs(Server.MapPath("~/Images/FotoGaleri/SahteResim/"+ResimAdi+ResimUzanti));

                //800 px lik Resmimiz
                int Donusturme = 800;

                //Bitmap nesnesi oluşturuyoruz ve sistemimize kaydetmiş olduğumuz SahteResmi açıyoruz.
                Bitmap BitMapNesnemiz = new Bitmap(Server.MapPath("~/Images/FotoGaleri/SahteResim"+ResimAdi+ResimUzanti));


                //Yeni bir Bitmap nesnesi oluşturuyoruz Orjinal Resim adında ve bunu BitMapNesnemiz le eşleştiriyoruz.
               
                using (Bitmap OrjinalResim = BitMapNesnemiz)
                {
                    double ResimYukseklik = OrjinalResim.Height;
                    double ResimGenislik = OrjinalResim.Width;
                    double Oran = 0;
                    // Yüklenen Resim Bizim Belirlediğimiz 800 px den büyük veya eşitse
                    if (ResimGenislik >= Donusturme) 
                    {
                        Oran = ResimGenislik / ResimYukseklik;
                        ResimGenislik = Donusturme;
                        ResimYukseklik = Donusturme / Oran;

                        //Yeni Boyutlardaki Resmimizi Oluşturmak için Size Nesnemizi Oluşturuyoruz.
                        //ResimGenislik ve ResimYukseklik dEğerlerimiz double olduğu için int e çeviriyoruz.
                        Size YeniDegerler = new Size(Convert.ToInt32(ResimGenislik), Convert.ToInt32(ResimYukseklik));

                        //Tekrar Bitmap Nesnesi oluşturuyoruz.
                        //Burada YeniResimAdlı Bitmap nesnemize OrjinalResim bitmap nesnesinden YeniDegerlerimizi veriyoruz.
                        //Orjinal Resmimi YeniDegerlerle YeniResim Bitmap ine atıyorum.
                        Bitmap YeniResim = new Bitmap(OrjinalResim, YeniDegerler);

                        //Yeni Resmimiz Oluştu bizim belirlediğimiz yeni Değerlerle 800 px boyutunda
                        //Artık Sıra Geldi Bu YeniResmimizi Kaydetmeye

                        ImageCodecInfo myImageCodecInfo = GetEncoderInfo("image/jpeg");
                        Encoder myEncoder = Encoder.Quality;
                        EncoderParameters myEncoderParameters = new EncoderParameters(1);
                        EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 75L);
                        myEncoderParameters.Param[0] = myEncoderParameter;

                        YeniResim.Save(Server.MapPath("~/Images/FotoGaleri/800/" + ResimAdi+ResimUzanti), myImageCodecInfo, myEncoderParameters);
                        YeniResim.Dispose();
                        OrjinalResim.Dispose();
                        BitMapNesnemiz.Dispose();

                    }
                    else
                    {
                        model.Resim.SaveAs(Server.MapPath("~/Images/FotoGaleri/800/" + ResimAdi+ResimUzanti));
                    }

                   
                }


                //Dönüştürme işlemleri bittiği zaman Sahte Resmimizi Artık silebiliriz boşuna yer tutmasın serverda.
                FileInfo fiSahteResim = new FileInfo(Server.MapPath("~/Images/FotoGaleri/SahteResim/"+ResimAdi+ResimUzanti));

                fiSahteResim.Delete();

                urunModel.Resim = ResimAdi + ResimUzanti;

            }


            db.Uruns.Add(urunModel);
            db.SaveChanges();
            ModelState.Clear(); //Text Boxları Temziler
            model = ModelGetir();
            model.kullaniciyaMesaj = "Kayıt işlemi başarılı bir şekilde gerçekleştirildi.";
            return View(model);

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


        //[HttpPost]
        //public ActionResult Create(UrunEklemeVM model)
        //{
        //    denemeEntities db = new denemeEntities();
        //    Urun urunModel = new Urun();
        //    urunModel.KategoriId = model.KategoriId;
        //    urunModel.Adi = model.Adi;

        //    if (model.Resim!=null && model.Resim.ContentLength>0)
        //    {
        //        string resimAdi;
        //        string resimUzanti;
        //        resimAdi = model.Adi;
        //        resimUzanti = Path.GetExtension(model.Resim.FileName);

        //        //string number = Guid.NewGuid().ToString();
        //        //string ext = Path.GetExtension(model.AnaResim.FileName);
        //        //model.AnaResim = Request.Files[name];


        //        //Resim Server a kaydedildi
        //        //model.Resim.SaveAs(Server.MapPath("~/Upload/" + model.Resim.FileName));
        //        model.Resim.SaveAs(Server.MapPath("~/Upload/" + resimAdi+resimUzanti));

        //        //Resim ismi db e kaydediliyor
        //        //urunModel.Resim = model.Resim.FileName;
        //        urunModel.Resim = resimAdi+resimUzanti;

        //    }


        //    db.Uruns.Add(urunModel);
        //    db.SaveChanges();
        //    ModelState.Clear(); //Text Boxları Temziler
        //    model = ModelGetir();
        //    model.kullaniciyaMesaj = "Kayıt işlemi başarılı bir şekilde gerçekleştirildi."; 
        //    return View(model);

        //}

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
    }
}