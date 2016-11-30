using ImageUploadDenemesi.Models;
using ImageUploadDenemesi.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageUploadDenemesi.Controllers
{
    public class Deneme01Controller : Controller
    {
        // GET: Deneme1

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
                string resimAdi;
                string resimUzanti;
                resimAdi = model.Adi;
                resimUzanti = Path.GetExtension(model.Resim.FileName);

                //Resim Server a kaydedildi
                //model.Resim.SaveAs(Server.MapPath("~/Upload/" + model.Resim.FileName));
                model.Resim.SaveAs(Server.MapPath("~/Upload/" + resimAdi + resimUzanti));

                //Resim ismi db e kaydediliyor
                //urunModel.Resim = model.Resim.FileName;
                urunModel.Resim = resimAdi + resimUzanti;
                

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
    }
}