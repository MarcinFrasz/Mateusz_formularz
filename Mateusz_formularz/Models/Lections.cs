using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mateusz_formularz.Models
{

    
  


    public class Lections
    {
    }

  public class Video
    {
            public Video()
            {
                Videos = new List<Video>();
            }
        public int? Id_Video { get; set; }
        public DateTime? Date { get; set; }
        public string Reading_type { get; set; }
        public string Youtube_Id { get; set; }
        public List<Video> Videos { get; set; }
    }


    public class SlownikDni
    {
        public SlownikDni()
        {
            SlownikDni_list = new List<SlownikDni>();
        }
        public string Liturgical_day { get; set; }
        public string Day_name { get; set; }
        public bool Holyday { get; set; } 
        public List<SlownikDni> SlownikDni_list { get; set; }
    }

   public class Lekcjonarz
    {
        public Lekcjonarz()
        {
            Lekcjonarz_list = new List<Lekcjonarz>();
            Lekcjonarz_list_dropdown = new List<Lekcjonarz>();
        }
        public int? ID_lekcjonarz { get; set; }
        public string Liturgical_day { get; set; }
        public string Reading_type { get; set; }
        public string Siglum { get; set; }
        [AllowHtml]
        public string Text { get; set; }
        public List<Lekcjonarz> Lekcjonarz_list { get; set; }
        public List<Lekcjonarz> Lekcjonarz_list_dropdown { get; set; }
    }

    public class Kalendarz
    {
        public Kalendarz()
        {
            Kalendarz_list = new List<Kalendarz>();
        }
        public DateTime? Date { get; set; }
        public string Liturgical_day { get; set; }
        public string Day_name { get; set; }
        public string Comment_source1 { get; set; }
        public string Comment_source2 { get; set; }
        public int? id_book1 { get; set; }
        public int? id_book2 { get; set; }
        public int? id_book3 { get; set; }
        public int? week_number { get; set; }

        public List<Kalendarz> Kalendarz_list {get; set;}
        public List<Kalendarz> Kalendarz_list_dropdown_liturgical_day {get; set;}
    }

    public class Ksiazki
    {
        public Ksiazki()
        {
            Ksiazki_list = new List<Ksiazki>();
        }
        public int? ID_book { get; set; }
        public string ID_kmt { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }

        public List<Ksiazki> Ksiazki_list { get; set; }
    }

    public class Komentarze
    {
        public Komentarze()
        {
            Komentarze_list = new List<Komentarze>();
        
        }
        public string Liturgical_day { get; set; }
        public string Comment_source { get; set; }
        public string Text { get; set; }

        public List<Komentarze> Komentarze_list { get; set; }     
    }
}