using Mateusz_formularz.Data;
using Mateusz_formularz.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading;
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
        #region MainVideoView
        public ActionResult MainVideoView(Video video, string Date)
        {
            if(System.Web.HttpContext.Current.User != null && System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                if (video.Date == null)
                {
                    var now = DateTime.Now;
                    DateTime? date = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
                    video.Date = date;
                    if (lectionsRepository.Select_Video(video.Date.Value) != null)
                    {
                        video = lectionsRepository.Select_Video(video.Date.Value);
                    }
                }
                var S_reading_types = lectionsRepository.Select_S_reading_type();
                ViewBag.S_reading_types = S_reading_types.S_reading_type_list;
                return View(video);
            }
            else
            {
                Response.Redirect("~/Login.aspx");
                return null;
            }
        }

        [HttpPost]
        public ActionResult MainVideoView(Video video, DateTime? Date, string submit)
        {
            if (System.Web.HttpContext.Current.User != null && System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var S_reading_types = lectionsRepository.Select_S_reading_type();
                ViewBag.S_reading_types = S_reading_types.S_reading_type_list;
                ModelState.Clear();
                int? id = video.Id_Video;
                String reading_type = video.Reading_type;
                string youtube_id = video.Youtube_Id;
                switch (submit)
                {
                    case "Wybierz":
                        if (lectionsRepository.Select_Video(Date.Value) != null)
                        {
                            video = lectionsRepository.Select_Video(Date.Value);
                        }
                        video.Id_Video = id;
                        video.Reading_type = reading_type;
                        video.Youtube_Id = youtube_id;
                        video.Date = Date;
                        return View(video);

                    case "Dodaj":
                        try
                        {
                            Video check = lectionsRepository.Select_Video(video.Date.Value, video.Reading_type);
                            if (check.Videos.Count == 0)
                            {
                                lectionsRepository.Insert_Video(video);
                                video = lectionsRepository.Select_Video(Date.Value);
                                video.Date = Date;
                            }
                            else
                            {
                                ViewBag.Message = "Wpis dla tego dnia i typu czytania już istnieje!";
                                video = lectionsRepository.Select_Video(Date.Value);
                                video.Id_Video = id;
                                video.Reading_type = reading_type;
                                video.Youtube_Id = youtube_id;
                                video.Date = Date;
                            }
                        }
                        catch
                        {
                            if (video.Youtube_Id == null)
                            {
                                ViewBag.Message = "Podaj youtube_id!";
                                if (lectionsRepository.Select_Video(Date.Value) != null)
                                {
                                    video = lectionsRepository.Select_Video(Date.Value);
                                }
                            }
                            video.Id_Video = id;
                            video.Reading_type = reading_type;
                            video.Youtube_Id = youtube_id;
                            video.Date = Date;
                            video.Reading_type = video.Reading_type;
                        }

                        return View(video);


                    case "Edytuj":
                        if (video.Id_Video != null)
                        {
                            video = lectionsRepository.Select_Video(video.Id_Video);
                            video.Id_Video = video.Videos[0].Id_Video;
                            video.Date = video.Videos[0].Date;
                            video.Reading_type = video.Videos[0].Reading_type;
                            video.Youtube_Id = video.Videos[0].Youtube_Id;
                        }
                        return View(video);

                    case "Usuń":
                        lectionsRepository.Delete_Video(video.Id_Video);
                        video = lectionsRepository.Select_Video(Date.Value);
                        video.Date = Date;
                        return View(video);

                    case "Zapisz zmiany":
                        id = video.Id_Video;
                        reading_type = video.Reading_type;
                        youtube_id = video.Youtube_Id;
                        try
                        {
                            bool exists = false;
                            Video check = lectionsRepository.Select_Video(Date.Value);
                            for (int i = 0; i < check.Videos.Count; i++)
                            {
                                if (video.Id_Video == check.Videos[i].Id_Video)
                                {
                                    exists = true;
                                    i = check.Videos.Count;
                                }
                            }
                            if (exists == true)
                            {
                                check = lectionsRepository.Select_Video(video.Id_Video, video.Date.Value, video.Reading_type);
                                if (check.Videos.Count == 0)
                                {
                                    video = lectionsRepository.Update_Video(video);
                                    if (lectionsRepository.Select_Video(Date.Value) != null)
                                    {
                                        video = lectionsRepository.Select_Video(Date.Value);
                                    }
                                    video.Date = Date;
                                }
                                else
                                {
                                    ViewBag.Message = "Wpis dla tego dnia i typu czytania już istnieje!";
                                    video = lectionsRepository.Select_Video(Date.Value);
                                    video.Id_Video = id;
                                    video.Reading_type = reading_type;
                                    video.Youtube_Id = youtube_id;
                                    video.Date = Date;
                                }
                            }
                            else
                            {
                                ViewBag.Message = "Ten wpis nie istnieje w bazie danych! Zapis zmian niemożliwy!";
                                video = lectionsRepository.Select_Video(Date.Value);
                            }

                        }
                        catch
                        {
                            if (video.Id_Video == null)
                            {
                                ViewBag.Message = "Nie wybrano rekordu do edycji!";
                                if (lectionsRepository.Select_Video(Date.Value) != null)
                                {
                                    video = lectionsRepository.Select_Video(Date.Value);
                                }
                            }
                            else
                            {
                                if (video.Youtube_Id == null)
                                {
                                    ViewBag.Message = "Podaj youtube_id!";
                                    if (lectionsRepository.Select_Video(video.Id_Video) != null)
                                    {
                                        video = lectionsRepository.Select_Video(video.Id_Video);
                                    }
                                }
                            }
                            video.Id_Video = id;
                            video.Reading_type = reading_type;
                            video.Youtube_Id = youtube_id;
                            video.Date = Date;
                        }
                        return View(video);


                    case "Home":
                        return RedirectToAction("MainVideoView", "AddOrEdit");

                    case "Wyświetl wszystkie wpisy":
                        video = lectionsRepository.Video_Select_All();
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
            else
            {
                Response.Redirect("~/Login.aspx");
                return null; 
            }
        }
        #endregion

        #region ShowAllVideoView
        public ActionResult ShowAllVideoView(Video video)
        {
            if (System.Web.HttpContext.Current.User != null && System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                video = lectionsRepository.Video_Select_All();
                return View(video);
            }
            else
            {
                Response.Redirect("~/Login.aspx");
                return null;
            }
        }
        #endregion

        #region MainSlownikDniView
        public ActionResult MainSlownikDniView(SlownikDni slownik)
        {

            if (System.Web.HttpContext.Current.User != null && System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var kalendarz_liturgicalday_dropdown = lectionsRepository.Select_Kalendarz_dropdown_liturgical_day();
                ViewBag.kalendarz_liturgicalday_dropdown = kalendarz_liturgicalday_dropdown.liturgical_day_list_kalendarz;

                slownik = lectionsRepository.Select_All_SlownikDni();
                return View(slownik);
            }
            else
            {
                Response.Redirect("~/Login.aspx");
                return null;
            }
        }

        [HttpPost]
        public ActionResult MainSlownikDniView(SlownikDni slownik, string submit)
        {
            if (System.Web.HttpContext.Current.User != null && System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                ModelState.Clear();
                var kalendarz_liturgicalday_dropdown = lectionsRepository.Select_Kalendarz_dropdown_liturgical_day();
                ViewBag.kalendarz_liturgicalday_dropdown = kalendarz_liturgicalday_dropdown.liturgical_day_list_kalendarz;
                string dzien_liturgiczny = slownik.Liturgical_day;
                string nazwa_dnia = slownik.Day_name;
                bool swieto = slownik.Holyday;
                string dzien_liturgiczny_szukaj = slownik.Liturgical_day_search;
                switch (submit)
                {
                    case "Wybierz":
                        if (lectionsRepository.Select_SlownikDni(slownik.Liturgical_day) != null)
                        {
                            slownik = lectionsRepository.Select_SlownikDni(slownik.Liturgical_day);
                        }
                        slownik.Liturgical_day = dzien_liturgiczny;
                        slownik.Day_name = nazwa_dnia;
                        slownik.Holyday = swieto;
                        return View(slownik);
                    case "Edytuj":

                        if (slownik.SlownikDni_list != null)
                        {
                            slownik = lectionsRepository.Select_SlownikDni(slownik.Liturgical_day);
                        }
                        slownik.Liturgical_day = dzien_liturgiczny;
                        slownik.Day_name = nazwa_dnia;
                        slownik.Holyday = swieto;
                        return View(slownik);

                    case "Dodaj":
                        try
                        {
                            SlownikDni check = lectionsRepository.Select_SlownikDni(slownik.Liturgical_day);
                            if (check.SlownikDni_list.Count == 0)
                            {
                                lectionsRepository.Insert_SlownikDni(slownik);
                                slownik = lectionsRepository.Select_SlownikDni(dzien_liturgiczny);
                                kalendarz_liturgicalday_dropdown = lectionsRepository.Select_Kalendarz_dropdown_liturgical_day();
                                ViewBag.kalendarz_liturgicalday_dropdown = kalendarz_liturgicalday_dropdown.liturgical_day_list_kalendarz;
                            }
                            else
                            {
                                ViewBag.Message = "Wpis dla tego dnia liturgicznego już istnieje!";
                                slownik = lectionsRepository.Select_SlownikDni(slownik.Liturgical_day);
                                slownik.Liturgical_day = dzien_liturgiczny;
                                slownik.Day_name = nazwa_dnia;
                                slownik.Holyday = swieto;
                            }
                        }
                        catch
                        {
                            if (slownik.Liturgical_day == null)
                            {
                                ViewBag.Message = "Nie podano dnia liturgicznego!";
                                slownik = lectionsRepository.Select_All_SlownikDni();
                            }
                            else
                            {
                                if (slownik.Day_name == null)
                                {
                                    ViewBag.Message = "Nie podano nazwy dnia!";
                                    slownik = lectionsRepository.Select_All_SlownikDni();
                                }
                                else
                                {
                                    ViewBag.Message = "Wystąpił błąd podczas wykonywania polecenia!";
                                    slownik = lectionsRepository.Select_All_SlownikDni();
                                }
                            }
                            slownik.Liturgical_day = dzien_liturgiczny;
                            slownik.Day_name = nazwa_dnia;
                            slownik.Holyday = swieto;
                        }
                        return View(slownik);

                    case "Zapisz zmiany":
                        try
                        {
                            SlownikDni check = lectionsRepository.Select_SlownikDni(slownik.Liturgical_day);
                            if (check.SlownikDni_list.Count != 0)
                            {
                                lectionsRepository.Update_SlownikDni(slownik);
                                slownik = lectionsRepository.Select_All_SlownikDni();
                            }
                            else
                            {
                                ViewBag.Message = "Wpis dla tego dnia liturgicznego nie istnieje w bazie danych!";
                                slownik = lectionsRepository.Select_SlownikDni(dzien_liturgiczny);
                                slownik.Liturgical_day = dzien_liturgiczny;
                                slownik.Day_name = nazwa_dnia;
                                slownik.Holyday = swieto;
                            }
                        }
                        catch
                        {
                            if (slownik.Liturgical_day == null)
                            {
                                ViewBag.Message = "Nie podano dnia liturgicznego!";
                                slownik = lectionsRepository.Select_All_SlownikDni();
                            }
                            else
                            {
                                if (slownik.Day_name == null)
                                {
                                    ViewBag.Message = "Nie podano nazwy dnia!";
                                    slownik = lectionsRepository.Select_All_SlownikDni();
                                }
                                else
                                {
                                    ViewBag.Message = "Wystąpił błąd podczas wykonywania polecenia!";
                                    slownik = lectionsRepository.Select_All_SlownikDni();
                                }
                            }
                            slownik.Liturgical_day = dzien_liturgiczny;
                            slownik.Day_name = nazwa_dnia;
                            slownik.Holyday = swieto;
                        }
                        return View(slownik);
                    case "Home":
                        return RedirectToAction("MainSlownikDniView", "AddOrEdit");
                    case "Usuń":
                        lectionsRepository.Delete_SlownikDni(slownik.Liturgical_day);
                        slownik = lectionsRepository.Select_All_SlownikDni();
                        kalendarz_liturgicalday_dropdown = lectionsRepository.Select_Kalendarz_dropdown_liturgical_day();
                        ViewBag.kalendarz_liturgicalday_dropdown = kalendarz_liturgicalday_dropdown.liturgical_day_list_kalendarz;
                        return View(slownik);
                    case "Szukaj":
                        slownik = lectionsRepository.Select_SlownikDni(slownik.Liturgical_day_search);
                        slownik.Liturgical_day_search = dzien_liturgiczny_szukaj;
                        return View(slownik);
                    default:
                        slownik = lectionsRepository.Select_All_SlownikDni();
                        return View(slownik);
                }
            }
            else
            {
                Response.Redirect("~/Login.aspx");
                return null;
            }
        }
        #endregion

        #region MainLekcjonarzView
        public ActionResult MainLekcjonarzView(Lekcjonarz lekcjonarz)
        {
            if (System.Web.HttpContext.Current.User != null && System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var kalendarz_liturgicalday_dropdown = lectionsRepository.Select_Kalendarz_dropdown_liturgical_day();
                ViewBag.kalendarz_liturgicalday_dropdown = kalendarz_liturgicalday_dropdown.liturgical_day_list_kalendarz;
                var S_reading_types = lectionsRepository.Select_S_reading_type();
                ViewBag.S_reading_types = S_reading_types.S_reading_type_list;
                if (lekcjonarz.Liturgical_day == null)
                {
                    lekcjonarz = lectionsRepository.Select_Lekcjonarz(kalendarz_liturgicalday_dropdown.liturgical_day_list_kalendarz[0]);
                }
                return View(lekcjonarz);
            }
            else
            {
                Response.Redirect("~/Login.aspx");
                return null;
            }
        }

        [HttpPost]
        public ActionResult MainLekcjonarzView(Lekcjonarz lekcjonarz, string submit)
        {
            if (System.Web.HttpContext.Current.User != null && System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                ModelState.Clear();
                var kalendarz_liturgicalday_dropdown = lectionsRepository.Select_Kalendarz_dropdown_liturgical_day();
                ViewBag.kalendarz_liturgicalday_dropdown = kalendarz_liturgicalday_dropdown.liturgical_day_list_kalendarz;
                var S_reading_types = lectionsRepository.Select_S_reading_type();
                ViewBag.S_reading_types = S_reading_types.S_reading_type_list;
                int? id = lekcjonarz.ID_lekcjonarz;
                string liturgical_day = lekcjonarz.Liturgical_day;
                string reading_type = lekcjonarz.Reading_type;
                string siglum = lekcjonarz.Siglum;
                string text = lekcjonarz.Text;
                string dzien_liturgiczny_szukaj = lekcjonarz.Liturgical_day_search;
                switch (submit)
                {
                    case "Wybierz":
                        if (lectionsRepository.Select_SlownikDni(lekcjonarz.Liturgical_day) != null)
                        {
                            lekcjonarz = lectionsRepository.Select_Lekcjonarz(lekcjonarz.Liturgical_day);
                            lekcjonarz.ID_lekcjonarz = id;
                            lekcjonarz.Liturgical_day = liturgical_day;
                            lekcjonarz.Reading_type = reading_type;
                            lekcjonarz.Siglum = siglum;
                            lekcjonarz.Text = text;
                            return View(lekcjonarz);
                        }
                        break;
                    case "Dodaj":
                        try
                        {
                            Lekcjonarz check = lectionsRepository.Select_Lekcjonarz(lekcjonarz.Liturgical_day);
                            bool exists = false;
                            for (int i = 0; i < check.Lekcjonarz_list.Count; i++)
                            {
                                if (check.Lekcjonarz_list[i].Reading_type == lekcjonarz.Reading_type)
                                {
                                    exists = true;
                                    i = check.Lekcjonarz_list.Count;
                                }
                            }
                            if (exists == false)
                            {
                                lectionsRepository.Insert_Lekcjonarz(lekcjonarz);
                                lekcjonarz = lectionsRepository.Select_Lekcjonarz(lekcjonarz.Liturgical_day);
                            }
                            else
                            {
                                ViewBag.Message = "Wpis dla tego dnia liturgicznego i typu czytania już istnieje!";
                                lekcjonarz = lectionsRepository.Select_Lekcjonarz(lekcjonarz.Liturgical_day);
                                lekcjonarz.ID_lekcjonarz = id;
                                lekcjonarz.Liturgical_day = liturgical_day;
                                lekcjonarz.Reading_type = reading_type;
                                lekcjonarz.Siglum = siglum;
                                lekcjonarz.Text = text;
                            }
                        }
                        catch
                        {
                            if (lekcjonarz.Text == null)
                            {
                                ViewBag.Message = "Nie podano tekstu!";
                                lekcjonarz = lectionsRepository.Select_Lekcjonarz(lekcjonarz.Liturgical_day);
                                lekcjonarz.ID_lekcjonarz = id;
                                lekcjonarz.Liturgical_day = liturgical_day;
                                lekcjonarz.Reading_type = reading_type;
                                lekcjonarz.Siglum = siglum;
                                lekcjonarz.Text = text;
                            }
                            else
                            {
                                ViewBag.Message = "Wystąpił błąd podczas przetwarzania!";
                                lekcjonarz = lectionsRepository.Select_Lekcjonarz(lekcjonarz.Liturgical_day);
                                lekcjonarz.ID_lekcjonarz = id;
                                lekcjonarz.Liturgical_day = liturgical_day;
                                lekcjonarz.Reading_type = reading_type;
                                lekcjonarz.Siglum = siglum;
                                lekcjonarz.Text = text;
                            }

                        }
                        return View(lekcjonarz);


                    case "Zapisz zmiany":
                        try
                        {
                            Lekcjonarz check;
                            Lekcjonarz check_id = lectionsRepository.Select_Lekcjonarz(lekcjonarz.ID_lekcjonarz);
                            if (check_id.Lekcjonarz_list[0].Liturgical_day == lekcjonarz.Liturgical_day)
                            {
                                check = lectionsRepository.Select_Lekcjonarz(lekcjonarz.Liturgical_day);
                            }
                            else
                            {
                                check = lectionsRepository.Select_Lekcjonarz();
                            }
                            bool exists = false;
                            bool exists_reading_type = false;
                            for (int i = 0; i < check.Lekcjonarz_list.Count; i++)
                            {
                                if (check.Lekcjonarz_list[i].ID_lekcjonarz == lekcjonarz.ID_lekcjonarz)
                                {
                                    exists = true;
                                    i = check.Lekcjonarz_list.Count;
                                }
                            }
                            if (exists == true)
                            {
                                for (int i = 0; i < check.Lekcjonarz_list.Count; i++)
                                {
                                    if (check.Lekcjonarz_list[i].Liturgical_day == lekcjonarz.Liturgical_day && check.Lekcjonarz_list[i].Reading_type == lekcjonarz.Reading_type && check.Lekcjonarz_list[i].ID_lekcjonarz != lekcjonarz.ID_lekcjonarz)
                                    {
                                        exists_reading_type = true;
                                        i = check.Lekcjonarz_list.Count;
                                    }
                                }
                                if (exists_reading_type == false)
                                {
                                    lectionsRepository.Update_Lekcjonarz(lekcjonarz);
                                    lekcjonarz = lectionsRepository.Select_Lekcjonarz(lekcjonarz.Liturgical_day);
                                }
                                else
                                {
                                    ViewBag.Message = "Wpis dla tego dnia liturgicznego i typu czytania już istnieje!";
                                    lekcjonarz = lectionsRepository.Select_Lekcjonarz(lekcjonarz.Liturgical_day);
                                    lekcjonarz.ID_lekcjonarz = id;
                                    lekcjonarz.Liturgical_day = liturgical_day;
                                    lekcjonarz.Reading_type = reading_type;
                                    lekcjonarz.Siglum = siglum;
                                    lekcjonarz.Text = text;
                                }
                            }
                            else
                            {
                                ViewBag.Message = "Nie wybrano rekordu do edycji!";
                                lekcjonarz = lectionsRepository.Select_Lekcjonarz(lekcjonarz.Liturgical_day);
                                lekcjonarz.ID_lekcjonarz = id;
                                lekcjonarz.Liturgical_day = liturgical_day;
                                lekcjonarz.Reading_type = reading_type;
                                lekcjonarz.Siglum = siglum;
                                lekcjonarz.Text = text;
                            }

                        }
                        catch
                        {
                            if (lekcjonarz.ID_lekcjonarz == null)
                            {
                                ViewBag.Message = "Nie wybrano rekordu do edycji!";
                                lekcjonarz = lectionsRepository.Select_Lekcjonarz(lekcjonarz.Liturgical_day);
                                lekcjonarz.ID_lekcjonarz = id;
                                lekcjonarz.Liturgical_day = liturgical_day;
                                lekcjonarz.Reading_type = reading_type;
                                lekcjonarz.Siglum = siglum;
                                lekcjonarz.Text = text;
                            }
                            else
                            {
                                if (lekcjonarz.Text == null)
                                {
                                    ViewBag.Message = "Nie podano tekstu!";
                                    lekcjonarz = lectionsRepository.Select_Lekcjonarz(lekcjonarz.Liturgical_day);
                                    lekcjonarz.ID_lekcjonarz = id;
                                    lekcjonarz.Liturgical_day = liturgical_day;
                                    lekcjonarz.Reading_type = reading_type;
                                    lekcjonarz.Siglum = siglum;
                                    lekcjonarz.Text = text;
                                }
                                else
                                {
                                    ViewBag.Message = "Wystąpił błąd podczas wykonywania!";
                                }
                            }
                        }
                        return View(lekcjonarz);

                    case "Home":
                        return RedirectToAction("MainLekcjonarzView", "AddOrEdit");
                    case "Usuń":
                        lectionsRepository.Delete_Lekcjonarz(lekcjonarz.ID_lekcjonarz);
                        lekcjonarz = lectionsRepository.Select_Lekcjonarz(lekcjonarz.Liturgical_day);
                        return View(lekcjonarz);
                    case "Edytuj":
                        lekcjonarz = lectionsRepository.Select_Lekcjonarz(lekcjonarz.ID_lekcjonarz);
                        lekcjonarz.ID_lekcjonarz = id;
                        lekcjonarz.Liturgical_day = liturgical_day;
                        lekcjonarz.Reading_type = reading_type;
                        lekcjonarz.Siglum = siglum;
                        lekcjonarz.Text = text;
                        SlownikDni slownik_for_liturgical_day_merge = lectionsRepository.Select_SlownikDni(liturgical_day);
                        lekcjonarz.Liturgical_day = slownik_for_liturgical_day_merge.SlownikDni_list[0].Liturgical_day + "|" + slownik_for_liturgical_day_merge.SlownikDni_list[0].Day_name;
                        return View(lekcjonarz);
                    case "Szukaj":
                        lekcjonarz = lectionsRepository.Select_Lekcjonarz(lekcjonarz.Liturgical_day_search);
                        lekcjonarz.Liturgical_day_search = dzien_liturgiczny_szukaj;
                        return View(lekcjonarz);
                    default:
                        return View(lekcjonarz);

                }
                return View(lekcjonarz);
            }
            else
            {
                Response.Redirect("~/Login.aspx");
                return null;
            }
        }
        #endregion

        #region MainKalendarzView
        public ActionResult MainKalendarzView(Kalendarz kalendarz, string Date)
        {
            if (System.Web.HttpContext.Current.User != null && System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                    var kalendarz_liturgicalday_dropdown = lectionsRepository.Select_Kalendarz_dropdown_liturgical_day();
                    ViewBag.kalendarz_liturgicalday_dropdown = kalendarz_liturgicalday_dropdown.liturgical_day_list_kalendarz;
                    ViewBag.kalendarz_commentsource_dropdown = kalendarz_liturgicalday_dropdown.comment_source_list_kalendarz;
                    ViewBag.kalendarz_bookid_dropdown = kalendarz_liturgicalday_dropdown.book_id_list_kalendarz;

                    if (kalendarz.Date == null)
                    {
                        var now = DateTime.Now;
                        DateTime? date = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
                        kalendarz.Date = date;
                        if (lectionsRepository.Select_Kalendarz(kalendarz.Date.Value) != null)
                        {
                            kalendarz = lectionsRepository.Select_Kalendarz(kalendarz.Date.Value);
                        }
                    }

                    return View(kalendarz);
                }
            else
            {
                Response.Redirect("~/Login.aspx");
                return null;
            }
        }

        [HttpPost]
        public ActionResult MainKalendarzView(Kalendarz kalendarz, DateTime? Date, string submit)
        {
            if (System.Web.HttpContext.Current.User != null && System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                    ModelState.Clear();
                    var kalendarz_liturgicalday_dropdown = lectionsRepository.Select_Kalendarz_dropdown_liturgical_day();
                    ViewBag.kalendarz_liturgicalday_dropdown = kalendarz_liturgicalday_dropdown.liturgical_day_list_kalendarz;
                    ViewBag.kalendarz_commentsource_dropdown = kalendarz_liturgicalday_dropdown.comment_source_list_kalendarz;
                    ViewBag.kalendarz_bookid_dropdown = kalendarz_liturgicalday_dropdown.book_id_list_kalendarz;
                    DateTime? date=kalendarz.Date;
                    string liturgical_day=kalendarz.Liturgical_day;
                    string day_name=kalendarz.Day_name;
                    string comment_source1=kalendarz.Comment_source1;
                    string comment_source2 = kalendarz.Comment_source2;
                    string book_name = kalendarz.book_name;
                    int? book_id = kalendarz.id_book1;
                    int? week_number=kalendarz.week_number;
                bool show = kalendarz.patroni_show;
                switch (submit)
                {
                    case "Wybierz":
                        if (lectionsRepository.Select_Kalendarz(Date.Value) != null)
                        {
                            kalendarz=lectionsRepository.Select_Kalendarz(Date.Value);
                            kalendarz.Date = date;
                            kalendarz.Liturgical_day = liturgical_day;
                            kalendarz.Day_name = day_name;
                            kalendarz.Comment_source1 = comment_source1;
                            kalendarz.Comment_source2 = comment_source2;
                            kalendarz.book_name = book_name;
                            kalendarz.id_book1 = book_id;
                            kalendarz.week_number = week_number;
                            kalendarz.patroni_show = show;
                        }
                        kalendarz.Date = Date;
                        return View(kalendarz);

                    case "Dodaj":
                        try
                        {
                            Kalendarz check = lectionsRepository.Select_Kalendarz(kalendarz.Date);
                            if (check.Kalendarz_list.Count == 0)
                            {
                                string[] book_split = kalendarz.book_name.Split('|');
                                kalendarz.id_book1 = Int32.Parse(book_split[0]);
                                Ksiazki book = lectionsRepository.Select_Ksiazki(kalendarz.id_book1);
                                kalendarz.id_book1 = book.Ksiazki_list[0].ID_book;
                                lectionsRepository.Insert_Kalendarz(kalendarz);
                                lectionsRepository.Update_Patroni(kalendarz);
                                if (lectionsRepository.Select_Kalendarz(kalendarz.Date.Value) != null)
                                {
                                    kalendarz = lectionsRepository.Select_Kalendarz(Date.Value);
                                }
                                kalendarz.Date = Date;
                                return View(kalendarz);
                            }
                            else
                            {
                                ViewBag.Message = "Wpis dla tej daty już istnieje!";
                                kalendarz = lectionsRepository.Select_Kalendarz(Date.Value);
                                kalendarz.Date = Date;
                                kalendarz.Liturgical_day = liturgical_day;
                                kalendarz.Day_name = day_name;
                                kalendarz.Comment_source1 = comment_source1;
                                kalendarz.Comment_source2 = comment_source2;
                                kalendarz.book_name = book_name;
                                kalendarz.id_book1 = book_id;
                                kalendarz.week_number = week_number;
                                kalendarz.patroni_show = show;
                                return View(kalendarz);
                            }
                        }
                        catch
                        {
                            if (kalendarz.Day_name == null)
                            {
                                ViewBag.Message = "Nie wybrano dnia liturgicznego! Brak nazwy dnia!";
                                kalendarz = lectionsRepository.Select_Kalendarz(Date.Value);
                                kalendarz.Date = Date;
                                kalendarz.Liturgical_day = liturgical_day;
                                kalendarz.Day_name = day_name;
                                kalendarz.Comment_source1 = comment_source1;
                                kalendarz.Comment_source2 = comment_source2;
                                kalendarz.book_name = book_name;
                                kalendarz.id_book1 = book_id;
                                kalendarz.week_number = week_number;
                                kalendarz.patroni_show = show;
                            }
                            else
                            {
                                ViewBag.Message = "Wystąpił błąd podczas wykonywania!";
                                kalendarz = lectionsRepository.Select_Kalendarz(Date.Value);
                                kalendarz.Date = Date;
                                kalendarz.Liturgical_day = liturgical_day;
                                kalendarz.Day_name = day_name;
                                kalendarz.Comment_source1 = comment_source1;
                                kalendarz.Comment_source2 = comment_source2;
                                kalendarz.book_name = book_name;
                                kalendarz.id_book1 = book_id;
                                kalendarz.week_number = week_number;
                                kalendarz.patroni_show = show;
                            }
                            return View(kalendarz);
                        }              

                        case "Zapisz zmiany":
                        try
                        {
                            Kalendarz check = lectionsRepository.Select_Kalendarz(kalendarz.Date);
                            if (check.Kalendarz_list.Count > 0)
                            {
                                string[] book_split = kalendarz.book_name.Split('|');
                                kalendarz.id_book1 = Int32.Parse(book_split[0]);
                                Ksiazki book = lectionsRepository.Select_Ksiazki(kalendarz.id_book1);
                                kalendarz.id_book1 = book.Ksiazki_list[0].ID_book;
                                lectionsRepository.Update_Kalendarz(kalendarz);
                                lectionsRepository.Update_Patroni(kalendarz);
                                if (lectionsRepository.Select_Kalendarz(Date.Value) != null)
                                {
                                    kalendarz = lectionsRepository.Select_Kalendarz(Date.Value);
                                }
                                kalendarz.Date = Date;
                                return View(kalendarz);
                            }
                            else
                            {
                                ViewBag.Message = "Nie wybrano rekordu do edycji!";
                                kalendarz = lectionsRepository.Select_Kalendarz(Date.Value);
                                kalendarz.Date = Date;
                                kalendarz.Liturgical_day = liturgical_day;
                                kalendarz.Day_name = day_name;
                                kalendarz.Comment_source1 = comment_source1;
                                kalendarz.Comment_source2 = comment_source2;
                                kalendarz.book_name = book_name;
                                kalendarz.id_book1 = book_id;
                                kalendarz.week_number = week_number;
                                kalendarz.patroni_show = show;
                                return View(kalendarz);
                            }
                           
                        }
                        catch
                        {
                            if (kalendarz.Day_name == null)
                            {
                                ViewBag.Message = "Nie wybrano dnia liturgicznego! Brak nazwy dnia!";
                                kalendarz = lectionsRepository.Select_Kalendarz(Date.Value);
                                kalendarz.Date = Date;
                                kalendarz.Liturgical_day = liturgical_day;
                                kalendarz.Day_name = day_name;
                                kalendarz.Comment_source1 = comment_source1;
                                kalendarz.Comment_source2 = comment_source2;
                                kalendarz.book_name = book_name;
                                kalendarz.id_book1 = book_id;
                                kalendarz.week_number = week_number;
                                kalendarz.patroni_show = show;
                            }
                            else
                            {
                                ViewBag.Message = "Wystąpił błąd podczas wykonywania!";
                                kalendarz = lectionsRepository.Select_Kalendarz(Date.Value);
                                kalendarz.Date = Date;
                                kalendarz.Liturgical_day = liturgical_day;
                                kalendarz.Day_name = day_name;
                                kalendarz.Comment_source1 = comment_source1;
                                kalendarz.Comment_source2 = comment_source2;
                                kalendarz.book_name = book_name;
                                kalendarz.id_book1 = book_id;
                                kalendarz.week_number = week_number;
                                kalendarz.patroni_show = show;
                            }
                            return View(kalendarz);
                        }

                        case "Home":
                            return RedirectToAction("MainKalendarzView", "AddOrEdit");

                        case "Edytuj":
                            kalendarz = lectionsRepository.Select_Kalendarz(kalendarz.Date);
                        SlownikDni liturgical_day_merge = lectionsRepository.Select_SlownikDni(kalendarz.Kalendarz_list[0].Liturgical_day);
                        Ksiazki id_book_merge = lectionsRepository.Select_Ksiazki(kalendarz.Kalendarz_list[0].id_book1);

                        DateTime dt = Date.Value;
                        string month = dt.Month.ToString();
                        string day = dt.Day.ToString();
                        string date_patroni;
                        if (month.Length < 2)
                        {
                            month = "0" + month;
                            if (day.Length < 2)
                            {
                                day = "0" + day;
                            }
                        }
                        else
                        {
                            if (day.Length < 2)
                            {
                                day = "0" + day;
                            }
                        }
                        date_patroni = month + "-" + day;
                        Patroni patroni_show = lectionsRepository.Select_Patroni(date_patroni);
                            
                            kalendarz.Date = Date;
                            kalendarz.Liturgical_day = liturgical_day_merge.SlownikDni_list[0].Liturgical_day + "|" + liturgical_day_merge.SlownikDni_list[0].Day_name;
                            kalendarz.Day_name = day_name;
                            kalendarz.Comment_source1 = comment_source1;
                            kalendarz.Comment_source2 = comment_source2;
                            kalendarz.book_name = book_name;
                            kalendarz.id_book1 = book_id;
                            kalendarz.book_name= book_id + "|" + id_book_merge.Ksiazki_list[0].Title;
                            kalendarz.week_number = week_number;
                        if (patroni_show.Patroni_list.Count > 0)
                        {
                            kalendarz.patroni_show = patroni_show.Patroni_list[0].Show;
                        }
                        else
                        {
                            ViewBag.Message = "Błąd! Brak patrona dla wybranej daty! Proszę uzupełnić tabelę Patroni.";
                        }
                            

                        return View(kalendarz);
                        case "Usuń":
                        {
                            lectionsRepository.Delete_Kalendarz(kalendarz.Date);
                            kalendarz = lectionsRepository.Select_Kalendarz(kalendarz.Date);
                            kalendarz.Date = date;
                            return View(kalendarz);
                        }

                        default:
                            if (lectionsRepository.Select_Kalendarz(Date.Value) != null)
                            {
                                kalendarz = lectionsRepository.Select_Kalendarz(Date.Value);
                            }
                            kalendarz.Date = Date;
                            return View(kalendarz);
                    }
                    return View(kalendarz);

                }
            else
            {
                Response.Redirect("~/Login.aspx");
                return null;
            }
        }

        #endregion

        #region MainKomentarzeView
        public ActionResult MainKomentarzeView(Komentarze komentarze)
        {
            if (System.Web.HttpContext.Current.User != null && System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var kalendarz_liturgicalday_dropdown = lectionsRepository.Select_Kalendarz_dropdown_liturgical_day();
                ViewBag.kalendarz_liturgicalday_dropdown = kalendarz_liturgicalday_dropdown.liturgical_day_list_kalendarz;
                ViewBag.kalendarz_comment_source = kalendarz_liturgicalday_dropdown.comment_source_list_kalendarz;
                if (komentarze.Liturgical_day == null)
                {
                    komentarze.Liturgical_day = kalendarz_liturgicalday_dropdown.liturgical_day_list_kalendarz[0];
                }
                komentarze = lectionsRepository.Select_Komentarze(komentarze.Liturgical_day);
                return View(komentarze);
            }
            else
            {
                Response.Redirect("~/Login.aspx");
                return null;
            }
        }

        [HttpPost]
        public ActionResult MainKomentarzeView(Komentarze komentarze, string submit)
        {
            if (System.Web.HttpContext.Current.User != null && System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                ModelState.Clear();
                var kalendarz_liturgicalday_dropdown = lectionsRepository.Select_Kalendarz_dropdown_liturgical_day();
                ViewBag.kalendarz_liturgicalday_dropdown = kalendarz_liturgicalday_dropdown.liturgical_day_list_kalendarz;
                ViewBag.kalendarz_comment_source = kalendarz_liturgicalday_dropdown.comment_source_list_kalendarz;
                int? id = komentarze.ID_comment;
                string liturgical_day = komentarze.Liturgical_day;
                string comment_source = komentarze.Comment_source;
                string text = komentarze.Text;

                switch (submit)
                {
                    case "Wybierz":
                        if (lectionsRepository.Select_Komentarze(komentarze.Liturgical_day) != null)
                        {
                            string[] split_liturgical_day = komentarze.Liturgical_day.Split('|');
                            
                            komentarze = lectionsRepository.Select_Komentarze(split_liturgical_day[0]);
                            komentarze.ID_comment = id;
                            komentarze.Liturgical_day = liturgical_day;
                            komentarze.Comment_source = comment_source;
                            komentarze.Text = text;
                        }

                        return View(komentarze);

                    case "Dodaj":
                        try
                        {
                            Komentarze check = lectionsRepository.Select_Komentarze(komentarze.Liturgical_day);
                            bool exists = false;
                            for (int i = 0; i < check.Komentarze_list.Count; i++)
                            {
                                if (check.Komentarze_list[i].Comment_source == komentarze.Comment_source)
                                {
                                    exists = true;
                                    i = check.Komentarze_list.Count;
                                }
                            }
                            if (exists == false)
                            {                    
                                lectionsRepository.Insert_Komentarze(komentarze);
                                komentarze = lectionsRepository.Select_Komentarze(komentarze.Liturgical_day);
                            }
                            else
                            {
                                ViewBag.Message = "Wpis dla tego dnia liturgicznego z podanym źródłem komentarza już istnieje!";
                                komentarze = lectionsRepository.Select_Komentarze(komentarze.Liturgical_day);
                                komentarze.ID_comment = id;
                                komentarze.Liturgical_day = liturgical_day;
                                komentarze.Comment_source = comment_source;
                                komentarze.Text = text;
                            }

                        }
                        catch
                        {
                            if (komentarze.Text == null)
                            {
                                ViewBag.Message = "Nie podano tekstu!";
                                komentarze = lectionsRepository.Select_Komentarze(komentarze.Liturgical_day);
                                komentarze.ID_comment = id;
                                komentarze.Liturgical_day = liturgical_day;
                                komentarze.Comment_source = comment_source;
                                komentarze.Text = text;
                            }
                            else
                            {
                                ViewBag.Message = "Wystąpił błąd podczas wykonywania!";
                                komentarze = lectionsRepository.Select_Komentarze(komentarze.Liturgical_day);
                                komentarze.ID_comment = id;
                                komentarze.Liturgical_day = liturgical_day;
                                komentarze.Comment_source = comment_source;
                                komentarze.Text = text;
                            }

                        }
                        return View(komentarze);

                    case "Zapisz zmiany":
                        try
                        {
                            Komentarze check;
                            Komentarze check_id = lectionsRepository.Select_Komentarze(komentarze.ID_comment);
                            if (check_id.Komentarze_list[0].Liturgical_day == komentarze.Liturgical_day)
                            {
                                check = lectionsRepository.Select_Komentarze(komentarze.Liturgical_day);
                            }
                            else
                            {
                                check = lectionsRepository.Select_Komentarze();
                            }


                            bool exists = false;
                            bool exists_comment_source = false;
                            for (int i = 0; i < check.Komentarze_list.Count; i++)
                            {
                                if (check.Komentarze_list[i].ID_comment == komentarze.ID_comment)
                                {
                                    exists = true;
                                    i = check.Komentarze_list.Count;
                                }
                            }
                            if (exists == true)
                            {
                                for (int i = 0; i < check.Komentarze_list.Count; i++)
                                {
                                    if (check.Komentarze_list[i].Liturgical_day == komentarze.Liturgical_day && check.Komentarze_list[i].Comment_source == komentarze.Comment_source && komentarze.ID_comment != check.Komentarze_list[i].ID_comment)
                                    {
                                        exists_comment_source = true;
                                    }
                                }
                                if (exists_comment_source == false)
                                {
                                    lectionsRepository.Update_Komentarze(komentarze);
                                    komentarze = lectionsRepository.Select_Komentarze(komentarze.Liturgical_day);
                                }
                                else
                                {
                                    ViewBag.Message = "Wpis dla tego dnia liturgicznego z podanym źródłem komentarza już istnieje!";
                                    komentarze = lectionsRepository.Select_Komentarze(komentarze.Liturgical_day);
                                    komentarze.ID_comment = id;
                                    komentarze.Liturgical_day = liturgical_day;
                                    komentarze.Comment_source = comment_source;
                                    komentarze.Text = text;
                                }

                            }
                            else
                            {
                                ViewBag.Message = "Nie wybrano rekordu do edycji!";
                                komentarze = lectionsRepository.Select_Komentarze(komentarze.Liturgical_day);
                                komentarze.ID_comment = id;
                                komentarze.Liturgical_day = liturgical_day;
                                komentarze.Comment_source = comment_source;
                                komentarze.Text = text;
                            }
                        }
                        catch
                        {
                            if (komentarze.Text == null)
                            {
                                ViewBag.Message = "Nie podano tekstu!";
                                komentarze = lectionsRepository.Select_Komentarze(komentarze.Liturgical_day);
                                komentarze.ID_comment = id;
                                komentarze.Liturgical_day = liturgical_day;
                                komentarze.Comment_source = comment_source;
                                komentarze.Text = text;
                            }
                            else
                            {
                                ViewBag.Message = "Wystąpił błąd podczas wykonywania!";
                                komentarze = lectionsRepository.Select_Komentarze(komentarze.Liturgical_day);
                                komentarze.ID_comment = id;
                                komentarze.Liturgical_day = liturgical_day;
                                komentarze.Comment_source = comment_source;
                                komentarze.Text = text;
                            }
                        }
                        return View(komentarze);

                    case "Home":
                        return RedirectToAction("MainKomentarzeView", "AddOrEdit");
                    case "Edytuj":
                        komentarze = lectionsRepository.Select_Komentarze(komentarze.ID_comment);
                        komentarze.ID_comment = id;
                        SlownikDni liturgical_day_merge = lectionsRepository.Select_SlownikDni(komentarze.Komentarze_list[0].Liturgical_day);
                        komentarze.Liturgical_day = liturgical_day_merge.SlownikDni_list[0].Liturgical_day + "|" + liturgical_day_merge.SlownikDni_list[0].Day_name;
                       // komentarze.Liturgical_day = liturgical_day;
                        komentarze.Comment_source = comment_source;
                        komentarze.Text = text;
                        return View(komentarze);
                    case "Usuń":
                        lectionsRepository.Delete_Komentarze(komentarze.ID_comment);
                        komentarze = lectionsRepository.Select_Komentarze(komentarze.Liturgical_day);
                        return View(komentarze);

                    default:
                        return View(komentarze);
                }
            }
            else
            {
                Response.Redirect("~/Login.aspx");
                return null;
            }
        }
        #endregion

        #region MainKsiazkiView
        public ActionResult MainKsiazkiView(Ksiazki ksiazki)
        {
            if (System.Web.HttpContext.Current.User != null && System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var kalendarz_liturgicalday_dropdown = lectionsRepository.Select_Kalendarz_dropdown_liturgical_day();
                ViewBag.kalendarz_bookid_dropdown = kalendarz_liturgicalday_dropdown.book_id_list_kalendarz;
                if (ksiazki.Ksiazki_list.Count == 0)
                {
                    ksiazki = lectionsRepository.Select_Ksiazki();
                }
                return View(ksiazki);
            }
            else
            {
                Response.Redirect("~/Login.aspx");
                return null;
            }
        }

        [HttpPost]
        public ActionResult MainKsiazkiView(Ksiazki ksiazki, string submit)
        {
            if (System.Web.HttpContext.Current.User != null && System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                ModelState.Clear();
                var kalendarz_liturgicalday_dropdown = lectionsRepository.Select_Kalendarz_dropdown_liturgical_day();
                ViewBag.kalendarz_bookid_dropdown = kalendarz_liturgicalday_dropdown.book_id_list_kalendarz;
                int? id = ksiazki.ID_book;
                string id_kmt = ksiazki.ID_kmt;
                string title = ksiazki.Title;
                string author = ksiazki.Author;
                string photo = ksiazki.Photo;
                string description = ksiazki.Description;
                switch (submit)
                {
                    case "Wybierz":                       
                        string[] split = ksiazki.Title.Split('|');
                        ksiazki.ID_book = Int32.Parse(split[0]);
                        ksiazki = lectionsRepository.Select_Ksiazki(ksiazki.ID_book);
                        ksiazki.ID_book = id;
                        ksiazki.ID_kmt = id_kmt;
                        ksiazki.Title = title;
                        ksiazki.Author = author;
                        ksiazki.Photo = photo;
                        ksiazki.Description = description;
                        return View(ksiazki);

                    case "Dodaj":
                        try
                        {
                            lectionsRepository.Insert_Ksiazki(ksiazki);
                            ksiazki = lectionsRepository.Select_Ksiazki();
                            kalendarz_liturgicalday_dropdown = lectionsRepository.Select_Kalendarz_dropdown_liturgical_day();
                            ViewBag.kalendarz_bookid_dropdown = kalendarz_liturgicalday_dropdown.book_id_list_kalendarz;
                        }
                        catch
                        {
                            if (ksiazki.ID_kmt == null)
                            {
                                ViewBag.Message = "Nie podano id_kmt!";
                                ksiazki = lectionsRepository.Select_Ksiazki();
                                ksiazki.ID_book = id;
                                ksiazki.ID_kmt = id_kmt;
                                ksiazki.Title = title;
                                ksiazki.Author = author;
                                ksiazki.Photo = photo;
                                ksiazki.Description = description;
                            }
                            else
                            {
                                if (ksiazki.Title == null)
                                {
                                    ViewBag.Message = "Nie podano tytułu!";
                                    ksiazki = lectionsRepository.Select_Ksiazki();
                                    ksiazki.ID_book = id;
                                    ksiazki.ID_kmt = id_kmt;
                                    ksiazki.Title = title;
                                    ksiazki.Author = author;
                                    ksiazki.Photo = photo;
                                    ksiazki.Description = description;
                                }
                                else
                                {
                                    if (ksiazki.Photo == null)
                                    {
                                        ViewBag.Message = "Nie podano zdjęcia!";
                                        ksiazki = lectionsRepository.Select_Ksiazki();
                                        ksiazki.ID_book = id;
                                        ksiazki.ID_kmt = id_kmt;
                                        ksiazki.Title = title;
                                        ksiazki.Author = author;
                                        ksiazki.Photo = photo;
                                        ksiazki.Description = description;
                                    }
                                    else
                                    {
                                        if (ksiazki.Description == null)
                                        {
                                            ViewBag.Message = "Nie podano opisu!";
                                            ksiazki = lectionsRepository.Select_Ksiazki();
                                            ksiazki.ID_book = id;
                                            ksiazki.ID_kmt = id_kmt;
                                            ksiazki.Title = title;
                                            ksiazki.Author = author;
                                            ksiazki.Photo = photo;
                                            ksiazki.Description = description;
                                        }
                                        else
                                        {
                                            ViewBag.Message = "Wystąpił błąd podczas wykonywania!";
                                            ksiazki = lectionsRepository.Select_Ksiazki();
                                            ksiazki.ID_book = id;
                                            ksiazki.ID_kmt = id_kmt;
                                            ksiazki.Title = title;
                                            ksiazki.Author = author;
                                            ksiazki.Photo = photo;
                                            ksiazki.Description = description;
                                        }
                                    }
                                }
                            }

                        }
                        return View(ksiazki);


                    case "Zapisz zmiany":
                        try
                        {
                            Ksiazki check = lectionsRepository.Select_Ksiazki(ksiazki.ID_book);
                            if (check.Ksiazki_list.Count > 0)
                            {
                                lectionsRepository.Update_Ksiazki(ksiazki);
                                ksiazki = lectionsRepository.Select_Ksiazki();
                                kalendarz_liturgicalday_dropdown = lectionsRepository.Select_Kalendarz_dropdown_liturgical_day();
                                ViewBag.kalendarz_bookid_dropdown = kalendarz_liturgicalday_dropdown.book_id_list_kalendarz;
                            }
                            else
                            {
                                ViewBag.Message = "Nie wybrano rekordu do edycji!";
                                ksiazki = lectionsRepository.Select_Ksiazki();
                                ksiazki.ID_book = id;
                                ksiazki.ID_kmt = id_kmt;
                                ksiazki.Title = title;
                                ksiazki.Author = author;
                                ksiazki.Photo = photo;
                                ksiazki.Description = description;
                            }
                        }
                        catch
                        {
                            if (ksiazki.ID_book == null)
                            {
                                ViewBag.Message = "Nie wybrano rekordu do edycji!";
                                ksiazki = lectionsRepository.Select_Ksiazki();
                                ksiazki.ID_book = id;
                                ksiazki.ID_kmt = id_kmt;
                                ksiazki.Title = title;
                                ksiazki.Author = author;
                                ksiazki.Photo = photo;
                                ksiazki.Description = description;
                            }
                            else
                            {
                                if (ksiazki.ID_kmt == null)
                                {
                                    ViewBag.Message = "Nie podano id_kmt!";
                                    ksiazki = lectionsRepository.Select_Ksiazki();
                                    ksiazki.ID_book = id;
                                    ksiazki.ID_kmt = id_kmt;
                                    ksiazki.Title = title;
                                    ksiazki.Author = author;
                                    ksiazki.Photo = photo;
                                    ksiazki.Description = description;
                                }
                                else
                                {
                                    if (ksiazki.Title == null)
                                    {
                                        ViewBag.Message = "Nie podano tytułu!";
                                        ksiazki = lectionsRepository.Select_Ksiazki();
                                        ksiazki.ID_book = id;
                                        ksiazki.ID_kmt = id_kmt;
                                        ksiazki.Title = title;
                                        ksiazki.Author = author;
                                        ksiazki.Photo = photo;
                                        ksiazki.Description = description;
                                    }
                                    else
                                    {
                                        if (ksiazki.Photo == null)
                                        {
                                            ViewBag.Message = "Nie podano zdjęcia!";
                                            ksiazki = lectionsRepository.Select_Ksiazki();
                                            ksiazki.ID_book = id;
                                            ksiazki.ID_kmt = id_kmt;
                                            ksiazki.Title = title;
                                            ksiazki.Author = author;
                                            ksiazki.Photo = photo;
                                            ksiazki.Description = description;
                                        }
                                        else
                                        {
                                            if (ksiazki.Description == null)
                                            {
                                                ViewBag.Message = "Nie podano opisu!";
                                                ksiazki = lectionsRepository.Select_Ksiazki();
                                                ksiazki.ID_book = id;
                                                ksiazki.ID_kmt = id_kmt;
                                                ksiazki.Title = title;
                                                ksiazki.Author = author;
                                                ksiazki.Photo = photo;
                                                ksiazki.Description = description;
                                            }
                                            else
                                            {
                                                ViewBag.Message = "Wystąpił błąd podczas wykonywania!";
                                                ksiazki = lectionsRepository.Select_Ksiazki();
                                                ksiazki.ID_book = id;
                                                ksiazki.ID_kmt = id_kmt;
                                                ksiazki.Title = title;
                                                ksiazki.Author = author;
                                                ksiazki.Photo = photo;
                                                ksiazki.Description = description;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        return View(ksiazki);

                    case "Home":
                        return RedirectToAction("MainKsiazkiView", "AddOrEdit");

                    case "Edytuj":
                        ksiazki = lectionsRepository.Select_Ksiazki(ksiazki.ID_book);
                        ksiazki.ID_book = id;
                        ksiazki.ID_kmt = id_kmt;
                        ksiazki.Title = title;
                        ksiazki.Author = author;
                        ksiazki.Photo = photo;
                        ksiazki.Description = description;
                        return View(ksiazki);

                    case "Usuń":
                        lectionsRepository.Delete_Ksiazki(ksiazki.ID_book);
                        ksiazki = lectionsRepository.Select_Ksiazki();
                        return View(ksiazki);

                    default:
                        return View(ksiazki);

                }
            }
            else
            {
                Response.Redirect("~/Login.aspx");
                return null;
            }
        }
        #endregion

        #region MainPatroniView
        public ActionResult MainPatroniView(Patroni patroni)
        {
            if (System.Web.HttpContext.Current.User != null && System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var Patroni_id_dropdown = lectionsRepository.Select_dropdown_patroni_id();
                ViewBag.Patroni_id_dropdown = Patroni_id_dropdown.Patroni_id_list_dropdown;
                if (patroni.Patroni_list.Count == 0)
                {
                    patroni = lectionsRepository.Select_Patroni();
                }
                return View(patroni);
            }
            else
            {
                Response.Redirect("~/Login.aspx");
                return null;
            }
        }

        [HttpPost]
        public ActionResult MainPatroniView(Patroni patroni, string submit)
        {
            if (System.Web.HttpContext.Current.User != null && System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                ModelState.Clear();
                var Patroni_id_dropdown = lectionsRepository.Select_dropdown_patroni_id();
                ViewBag.Patroni_id_dropdown = Patroni_id_dropdown.Patroni_id_list_dropdown;
                int? id = patroni.ID_patron;
                string date = patroni.Date;
                string patron = patroni.Patron;
                bool main = patroni.Main;
                string descritpion = patroni.Description;
                string text = patroni.Text;
                bool show = patroni.Show;

                switch (submit)
                {
                    case "Wybierz":
                        patroni = lectionsRepository.Select_Patroni(patroni.Date);
                        patroni.ID_patron = id;
                        patroni.Date = date;
                        patroni.Patron = patron;
                        patroni.Main = main;
                        patroni.Description = descritpion;
                        patroni.Text = text;
                        patroni.Show = show;
                        return View(patroni);

                    case "Dodaj":
                        try
                        {
                            lectionsRepository.Insert_Patroni(patroni);
                            patroni = lectionsRepository.Select_Patroni(patroni.Date);
                            Patroni_id_dropdown = lectionsRepository.Select_dropdown_patroni_id();
                            patroni.Date = date;
                            ViewBag.Patroni_id_dropdown = Patroni_id_dropdown.Patroni_id_list_dropdown;
                        }
                        catch
                        {
                            if (patroni.Date == null)
                            {
                                ViewBag.Message = "Nie podano daty!";
                                patroni = lectionsRepository.Select_Patroni();
                                patroni.ID_patron = id;
                                patroni.Date = date;
                                patroni.Patron = patron;
                                patroni.Main = main;
                                patroni.Description = descritpion;
                                patroni.Text = text;
                                patroni.Show = show;
                            }
                            else
                            {
                                if (patroni.Patron == null)
                                {
                                    ViewBag.Message = "Nie podano patrona!";
                                    patroni = lectionsRepository.Select_Patroni(patroni.Date);
                                    patroni.ID_patron = id;
                                    patroni.Date = date;
                                    patroni.Patron = patron;
                                    patroni.Main = main;
                                    patroni.Description = descritpion;
                                    patroni.Text = text;
                                    patroni.Show = show;
                                }
                                else
                                {
                                    if (patroni.Description == null)
                                    {
                                        ViewBag.Message = "Nie podano opisu!";
                                        patroni = lectionsRepository.Select_Patroni(patroni.Date);
                                        patroni.ID_patron = id;
                                        patroni.Date = date;
                                        patroni.Patron = patron;
                                        patroni.Main = main;
                                        patroni.Description = descritpion;
                                        patroni.Text = text;
                                        patroni.Show = show;
                                    }
                                    else
                                    {
                                        if (patroni.Description == null)
                                        {
                                            ViewBag.Message = "Wystąpił błąd podczas wykonywania!";
                                            patroni = lectionsRepository.Select_Patroni(patroni.Date);
                                            patroni.ID_patron = id;
                                            patroni.Date = date;
                                            patroni.Patron = patron;
                                            patroni.Main = main;
                                            patroni.Description = descritpion;
                                            patroni.Text = text;
                                            patroni.Show = show;
                                        }
                                    }
                                }
                            }
                        }
                        return View(patroni);


                    case "Zapisz zmiany":
                        try
                        {
                            Patroni check = lectionsRepository.Select_Patroni(patroni.ID_patron);
                            if(check.Patroni_list.Count > 0)
                            {
                                lectionsRepository.Update_Patroni(patroni);
                                patroni = lectionsRepository.Select_Patroni(date);
                                Patroni_id_dropdown = lectionsRepository.Select_dropdown_patroni_id();
                                ViewBag.Patroni_id_dropdown = Patroni_id_dropdown.Patroni_id_list_dropdown;
                                patroni.Date = date;

                            }
                            else
                            {
                                ViewBag.Message = "Nie wybrano rekordu do edycji!";
                                patroni = lectionsRepository.Select_Patroni(patroni.Date);
                                patroni.ID_patron = id;
                                patroni.Date = date;
                                patroni.Patron = patron;
                                patroni.Main = main;
                                patroni.Description = descritpion;
                                patroni.Text = text;
                                patroni.Show = show;
                            }
                        }
                        catch
                        {
                            if (patroni.ID_patron == null)
                            {
                                ViewBag.Message = "Nie wybrano rekordu do edycji!";
                                patroni = lectionsRepository.Select_Patroni(patroni.Date);
                                patroni.ID_patron = id;
                                patroni.Date = date;
                                patroni.Patron = patron;
                                patroni.Main = main;
                                patroni.Description = descritpion;
                                patroni.Text = text;
                                patroni.Show = show;
                            }
                            else
                            {
                                if (patroni.Date == null)
                                {
                                    ViewBag.Message = "Nie podano daty!";
                                    patroni = lectionsRepository.Select_Patroni();
                                    patroni.ID_patron = id;
                                    patroni.Date = date;
                                    patroni.Patron = patron;
                                    patroni.Main = main;
                                    patroni.Description = descritpion;
                                    patroni.Text = text;
                                    patroni.Show = show;
                                }
                                else
                                {
                                    if (patroni.Patron == null)
                                    {
                                        ViewBag.Message = "Nie podano patrona!";
                                        patroni = lectionsRepository.Select_Patroni(patroni.Date);
                                        patroni.ID_patron = id;
                                        patroni.Date = date;
                                        patroni.Patron = patron;
                                        patroni.Main = main;
                                        patroni.Description = descritpion;
                                        patroni.Text = text;
                                        patroni.Show = show;
                                    }
                                    else
                                    {
                                        if (patroni.Description == null)
                                        {
                                            ViewBag.Message = "Nie podano opisu!";
                                            patroni = lectionsRepository.Select_Patroni(patroni.Date);
                                            patroni.ID_patron = id;
                                            patroni.Date = date;
                                            patroni.Patron = patron;
                                            patroni.Main = main;
                                            patroni.Description = descritpion;
                                            patroni.Text = text;
                                            patroni.Show = show;
                                        }
                                        else
                                        {
                                            if (patroni.Description == null)
                                            {
                                                ViewBag.Message = "Wystąpił błąd podczas wykonywania!";
                                                patroni = lectionsRepository.Select_Patroni(patroni.Date);
                                                patroni.ID_patron = id;
                                                patroni.Date = date;
                                                patroni.Patron = patron;
                                                patroni.Main = main;
                                                patroni.Description = descritpion;
                                                patroni.Text = text;
                                                patroni.Show = show;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        return View(patroni);

                    case "Home":
                        return RedirectToAction("MainPatroniView", "AddOrEdit");
                    case "Edytuj":
                        patroni = lectionsRepository.Select_Patroni(patroni.ID_patron);
                        patroni.ID_patron = id;
                        patroni.Date = date;
                        patroni.Patron = patron;
                        patroni.Main = main;
                        patroni.Description = descritpion;
                        patroni.Text = text;
                        patroni.Show = show;
                        return View(patroni);
                    case "Usuń":
                        lectionsRepository.Delete_Patroni(patroni.ID_patron);
                        patroni = lectionsRepository.Select_Patroni(patroni.Date);
                        patroni.ID_patron = id;
                        patroni.Date = date;
                        patroni.Patron = patron;
                        patroni.Main = main;
                        patroni.Description = descritpion;
                        patroni.Text = text;
                        patroni.Show = show;
                        return View(patroni);

                    default:
                        return View(patroni);
                }
            }
            else
            {
                Response.Redirect("~/Login.aspx");
                return null;
            }
        }
        #endregion


        [HttpPost]
        public ActionResult GetValues(string val,DateTime? date)
        {
            if (val != null && date!=null)
            {
                var on_select = lectionsRepository.Select_data_on_dropdown_select_kalendarz(val,date);
                var day_name = on_select.day_name;
                return Json(new { Success = "true", Day_name = day_name });
            }
            else
            {
                return Json(new { Success = "false" });
            }
        }

        public ActionResult Logout_action()
        {
            var authenticationManager = System.Web.HttpContext.Current.GetOwinContext().Authentication;
            authenticationManager.SignOut();
            Response.Redirect("~/Login.aspx");
            return null;
        }
    }
}