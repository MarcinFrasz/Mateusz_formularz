using Mateusz_formularz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mateusz_formularz.Data
{
    public interface LectionsRepository
    {
        #region methods_for_Video
        Video Select_Video(DateTime date);
        Video Select_Video(int? id);
        Video Select_Video(DateTime? date, string reading_type);
        Video Select_Video(int? id, DateTime? date, string reading_type);
        Video Video_Select_All();
        Video Insert_Video(Video video);
        Video Update_Video(Video video);
        void Delete_Video(int? id);
        #endregion

        #region methods_for_SlownikDni
        SlownikDni Select_SlownikDni(string liturgical_day);
        SlownikDni Update_SlownikDni(SlownikDni slownik);
        SlownikDni Insert_SlownikDni(SlownikDni slownik);
        void Delete_SlownikDni(string liturgical_day);
        SlownikDni Select_All_SlownikDni();
        #endregion

        #region methods_for_Lekcjonarz
        Lekcjonarz Select_Lekcjonarz();
        Lekcjonarz Select_Lekcjonarz(string liturgical_day);
        Lekcjonarz Select_Lekcjonarz(int? ID);
        Lekcjonarz Select_All_liturgical_days_Lekcjonarz();
        Lekcjonarz Insert_Lekcjonarz(Lekcjonarz lekcjonarz);
        Lekcjonarz Update_Lekcjonarz(Lekcjonarz lekcjonarz);
        void Delete_Lekcjonarz(int? id);
        #endregion

        #region methods_for_Kalendarz
        Kalendarz Select_Kalendarz();
        Kalendarz Select_Kalendarz(DateTime? date);
        Kalendarz Insert_Kalendarz(Kalendarz kalendarz);
        Kalendarz Update_Kalendarz(Kalendarz kalendarz);
        void Delete_Kalendarz(DateTime? date);
        #endregion

        #region methods_for_Komentarze
        Komentarze Select_Komentarze();
        Komentarze Select_Komentarze(string liturgical_day);
        Komentarze Select_Komentarze(int? ID_comment);
        Komentarze Insert_Komentarze(Komentarze komentarze);
        Komentarze Update_Komentarze(Komentarze komentarze);
        void Delete_Komentarze(int? id);
        #endregion

        #region methods_for_Ksiazki
        Ksiazki Select_Ksiazki();
        Ksiazki Select_Ksiazki(int? id);
        Ksiazki Select_Ksiazki(string nazwa);
        Ksiazki Insert_Ksiazki(Ksiazki ksiazki);
        Ksiazki Update_Ksiazki(Ksiazki ksiazki);
        void Delete_Ksiazki(int? id);
        #endregion

        #region methods_for_Patroni
        Patroni Select_Patroni();
        Patroni Select_Patroni(int? id);
        Patroni Select_Patroni(string date);
        Patroni Insert_Patroni(Patroni patroni);
        Patroni Update_Patroni(Patroni patroni);
        Kalendarz Update_Patroni(Kalendarz kalendarz);
        void Delete_Patroni(int? id);
        #endregion

        #region methods_for_dropdowns
        S_reading_type Select_S_reading_type();
        Kalendarz_liturgical_day_dropdown Select_Kalendarz_dropdown_liturgical_day();
        Data_on_dropdown_select Select_data_on_dropdown_select_kalendarz(string liturgical_day,DateTime? date);
        Patroni_id_list Select_dropdown_patroni_id();
        #endregion

    }
}