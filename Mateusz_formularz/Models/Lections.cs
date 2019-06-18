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
            S_reading_type = new List<String>();
        }
        public int? Id_Video { get; set; }
        public DateTime? Date { get; set; }
        public string Reading_type { get; set; }
        public string Youtube_Id { get; set; }
        public List<Video> Videos { get; set; }
        public List<String> S_reading_type { get; set; }
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
        public string Liturgical_day_search { get; set; }
        public DateTime? TimeStamp { get; set; }
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
        public string Liturgical_day_search { get; set; }
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
        public string patron1 { get; set; }
        public string patron2 { get; set; }
        public string patron3 { get; set; }

        public string book_name { get; set; }
        public bool patroni_show { get; set; }

        public List<Kalendarz> Kalendarz_list { get; set; }
        public List<Kalendarz> Kalendarz_list_dropdown_liturgical_day { get; set; }
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
        public string Photo { get; set; }
        public string Description { get; set; }
        public List<Ksiazki> Ksiazki_list { get; set; }
    }

    public class Komentarze
    {
        public Komentarze()
        {
            Komentarze_list = new List<Komentarze>();

        }
        public int? ID_comment { get; set; }
        public string Liturgical_day { get; set; }
        public string Comment_source { get; set; }
        [AllowHtml]
        public string Text { get; set; }

        public List<Komentarze> Komentarze_list { get; set; }
    }

    public  class Patroni
    {
      public  Patroni()
        {
        Patroni_list = new List<Patroni>();
        }
        public int? ID_patron { get; set; }
        public string Date { get; set; }
        public string Patron { get; set; }
        public bool Main { get; set; }
        public string Description { get; set; }
        public string Text { get; set; }
        public bool Show { get; set; }
        public List<Patroni> Patroni_list { get; set; }
}


    public class S_reading_type
        {
        public S_reading_type()
        {
            S_reading_type_list = new List<String>();
        }
        public List<String> S_reading_type_list { get; set; }
        }

    public class Patroni_id_list
    {
       public Patroni_id_list()
        {
            Patroni_id_list_dropdown = new List<string>();
        }
        public List<string> Patroni_id_list_dropdown { get; set; }
    }

    public class Kalendarz_liturgical_day_dropdown
    {
        public Kalendarz_liturgical_day_dropdown()
        {
            liturgical_day_list_kalendarz = new List<String>();
            comment_source_list_kalendarz = new List<String>();
            book_id_list_kalendarz = new List<String>();
        }
        public List<String> liturgical_day_list_kalendarz { get; set; }
        public List<String> comment_source_list_kalendarz { get; set; }
        public List<String> book_id_list_kalendarz { get; set; }
    }

    public class Data_on_dropdown_select
    {
        public Data_on_dropdown_select()
        {
            day_name = "";
            text = "";
            komentarz_D = "";
            komentarz_M = "";
        }
        public string day_name { get; set; }
        public string text { get; set; }
        public string komentarz_D { get; set; }
        public string komentarz_M { get; set; }      
    }
}