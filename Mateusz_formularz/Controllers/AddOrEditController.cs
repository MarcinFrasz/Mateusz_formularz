using Mateusz_formularz.Data;
using Mateusz_formularz.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Mateusz_formularz.Controllers
{
    public class AddOrEditController : Controller
    {

        private readonly LectionsRepository lectionsRepository;
        public AddOrEditController(LectionsRepository lectionsRepository)
        {
            this.lectionsRepository = lectionsRepository;
        }


        // GET: AddOrEdit

        public ActionResult MainVideoView(Video video, string Date)
        {
            var now = DateTime.Now;
            DateTime? date = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            if (video.Date == null)
            {
                video.Date = date;
                if (lectionsRepository.Select_Video(video.Date.Value) != null)
                {
                    video = lectionsRepository.Select_Video(video.Date.Value);
                }
            }
            return View(video);
        }

        [HttpPost]
        public ActionResult MainVideoView(Video video, DateTime? Date, string submit)
        {
            switch (submit)
            {
                case "Wybierz":
                    if (lectionsRepository.Select_Video(Date.Value) != null)
                    {
                        video = lectionsRepository.Select_Video(Date.Value);
                    }     
                    video.Date = Date;
                    return View(video);

                case "Dodaj":
                    lectionsRepository.Insert_Video(video);
                    if (lectionsRepository.Select_Video(video.Date.Value) != null)
                    {
                        video = lectionsRepository.Select_Video(Date.Value);
                    }
                    video.Date = Date;
                    return View(video);


                case "Edytuj":
                    video = lectionsRepository.Select_Video(video.Id_Video);
                    video.Date = Date;
                    return View(video);

                case "Zapisz zmiany":
                    lectionsRepository.Update_Video(video);
                    if (lectionsRepository.Select_Video(Date.Value) != null)
                    {
                        video = lectionsRepository.Select_Video(Date.Value);
                    }
                    video.Date = Date;
                    return View(video);

                default:
                    if (lectionsRepository.Select_Video(Date.Value) != null)
                    {
                        video = lectionsRepository.Select_Video(Date.Value);
                    }
                    video.Date = Date;
                    return View(video);
            }


        }
        public ActionResult ShowAllVideoView(Video video)
        {
            video = lectionsRepository.Video_Select_All();
            return View(video);
        }

        public ActionResult MainSlownikDniView(SlownikDni slownik)
        {
            slownik = lectionsRepository.Select_All_SlownikDni();
            return View(slownik);
        }

        [HttpPost]
        public ActionResult MainSlownikDniView(SlownikDni slownik, string submit)
        {
            switch (submit)
            {
                case "Wybierz":
                    if (lectionsRepository.Select_SlownikDni(slownik.Liturgical_day) != null)
                    {
                        slownik = lectionsRepository.Select_SlownikDni(slownik.Liturgical_day);
                    }
                    return View(slownik);
                case "Edytuj":
                    if (slownik.SlownikDni_list != null)
                    {
                        string dzien_liturgiczny = slownik.Liturgical_day;
                        string nazwa_dnia = slownik.Day_name;
                        bool swieto = slownik.Holyday;
                        slownik = lectionsRepository.Select_SlownikDni(slownik.Liturgical_day);
                        slownik.Liturgical_day = dzien_liturgiczny;
                        slownik.Day_name = nazwa_dnia;
                        slownik.Holyday = swieto;
                    }
                    return View(slownik);

                case "Dodaj":
                    lectionsRepository.Insert_SlownikDni(slownik);
                    if (lectionsRepository.Select_SlownikDni(slownik.Liturgical_day) != null)
                    {
                        slownik = lectionsRepository.Select_SlownikDni(slownik.Liturgical_day);
                    }
                    return View(slownik);

                case "Zapisz zmiany":
                    lectionsRepository.Update_SlownikDni(slownik);
                    slownik = lectionsRepository.Select_SlownikDni(slownik.Liturgical_day);
                    return View(slownik);
                case "Home":
                    return RedirectToAction("MainSlownikDniView", "AddOrEdit");
                default:
                    slownik = lectionsRepository.Select_All_SlownikDni();
                    return View(slownik);
            }
        }

        public ActionResult MainLekcjonarzView(Lekcjonarz lekcjonarz)
        {
            lekcjonarz=lectionsRepository.Select_All_liturgical_days_Lekcjonarz();
            return View(lekcjonarz);
        }

        [HttpPost]
        public ActionResult MainLekcjonarzView(Lekcjonarz lekcjonarz, string submit)
        {
            switch (submit)
            {
                case "Wybierz":
                    if (lectionsRepository.Select_SlownikDni(lekcjonarz.Liturgical_day) != null)
                    {
                        lekcjonarz = lectionsRepository.Select_Lekcjonarz(lekcjonarz.Liturgical_day);
                        return View(lekcjonarz);
                    }
                    break;
                case "Dodaj":
                    lectionsRepository.Insert_Lekcjonarz(lekcjonarz);
                  
                        lekcjonarz = lectionsRepository.Select_Lekcjonarz(lekcjonarz.Liturgical_day);
                    
                    return View(lekcjonarz);
    

                case "Zapisz zmiany":
                    lectionsRepository.Update_Lekcjonarz(lekcjonarz);
                    lekcjonarz = lectionsRepository.Select_Lekcjonarz(lekcjonarz.Liturgical_day);
                    return View(lekcjonarz);
                    

                case "Home":
                    return RedirectToAction("MainLekcjonarzView", "AddOrEdit");
                case "Edytuj":
                    lekcjonarz = lectionsRepository.Select_Lekcjonarz(lekcjonarz.ID_lekcjonarz);
                    return View(lekcjonarz);

                default:
                    return View(lekcjonarz);
                   
            }
            return View(lekcjonarz);
        }


        public ActionResult MainKalendarzView(Kalendarz kalendarz)
        {
            var now = DateTime.Now;
            DateTime? date = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            if (kalendarz.Date == null)
            {
                kalendarz.Date = date;
            }
            kalendarz = lectionsRepository.Select_Kalendarz();
            return View(kalendarz);
        }

        [HttpPost]
        public ActionResult MainKalendarzView(Kalendarz kalendarz, DateTime? Date, string submit)
        {
            
            kalendarz = lectionsRepository.Select_Kalendarz();
            kalendarz.Date = Date;
            return View(kalendarz);
        }

        public ActionResult MainKsiazkiView(Ksiazki ksiazki)
        {
            return View(ksiazki);
        }

        [HttpPost]
        public ActionResult MainKsiazkiView(Ksiazki ksiazki,string submit)
        {
            return View(ksiazki);
        }

        public ActionResult MainKomentarzeView(Komentarze komentarze)
        {
            return View(komentarze);
        }

        [HttpPost]
        public ActionResult MainKomentarzeView(Komentarze komentarze,string submit)
        {
            return View(komentarze);
        }
    }
}