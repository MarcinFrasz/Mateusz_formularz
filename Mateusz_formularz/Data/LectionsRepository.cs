using Mateusz_formularz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mateusz_formularz.Data
{
    public interface LectionsRepository
    {
        Video Select_Video(DateTime date);
        Video Select_Video(int? id);
        Video Video_Select_All();
        Video Insert_Video(Video video);
        Video Update_Video(Video video);

        SlownikDni Select_SlownikDni(string liturgical_day);
        SlownikDni Update_SlownikDni(SlownikDni slownik);
        SlownikDni Insert_SlownikDni(SlownikDni slownik);
        SlownikDni Select_All_SlownikDni();

        Lekcjonarz Select_Lekcjonarz(string liturgical_day);
        Lekcjonarz Select_Lekcjonarz(int? ID);
        Lekcjonarz Select_All_liturgical_days_Lekcjonarz();
        Lekcjonarz Insert_Lekcjonarz(Lekcjonarz lekcjonarz);
        Lekcjonarz Update_Lekcjonarz(Lekcjonarz lekcjonarz);


        Kalendarz Select_Kalendarz();

    }
}