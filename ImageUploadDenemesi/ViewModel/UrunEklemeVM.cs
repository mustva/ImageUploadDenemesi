using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageUploadDenemesi.ViewModel
{
    public class UrunEklemeVM
    {
       
        public UrunEklemeVM()
        {
            this.Title = "Yeni Ürün Ekle";
        }

        [Required(ErrorMessage ="Ürün İsmi Girmediniz")]
        [StringLength(50,ErrorMessage ="Ürün ismmi 50 karakterden fazla olamaz")]
        [Display(Name ="Ürün Adı")]
        public string Adi { get; set; }

        [Required(ErrorMessage ="Kategori Seçiniz")]
        [Display(Name ="Kategori Adı")]
        public int KategoriId { get; set; }

        [Display(Name ="Foto")]
        public HttpPostedFileBase Resim { get; set; }

        public List<SelectListItem> KategoriListesi { get; set; }

        public string kullaniciyaMesaj { get; set; }

        public string Title { get; set; }

    }
}