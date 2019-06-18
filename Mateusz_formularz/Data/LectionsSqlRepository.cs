using Mateusz_formularz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;

namespace Mateusz_formularz.Data
{
    public class LectionsSqlRepository : LectionsRepository
    {
        string connetionString = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
        #region methods_for_SELECT_Video
        public Video Select_Video(DateTime date)
        {
            using (var sqlConn = new SqlConnection(connetionString))
            {
                sqlConn.Open();
                var Videos_for_date_list = new List<Video>();
                var video = new Video();
                using (var cmd = sqlConn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM Video WHERE Data=@date";
                    DateTime date_for_select = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                    cmd.Parameters.AddWithValue("@date", date_for_select);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Videos_for_date_list.Add(new Video()
                        {
                            Id_Video = reader["id_Video"] as int?,
                            Date = reader["Data"] as DateTime?,
                            Reading_type = reader["typ_czytania"] as string,
                            Youtube_Id = reader["youtube_Id"] as string,
                        });

                        video.Videos = Videos_for_date_list;


                    }
                    reader.Close();



                }
                return video;
            }


        }

        public Video Video_Select_All()
        {
            using (var sqlConn = new SqlConnection(connetionString))
            {
                sqlConn.Open();
                var Videos_for_date_list = new List<Video>();
                var video = new Video();
                using (var cmd = sqlConn.CreateCommand())
                {
                    cmd.CommandText = "SELECT TOP 60 * FROM Video ORDER BY Data DESC";
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Videos_for_date_list.Add(new Video()
                        {
                            Id_Video = reader["id_Video"] as int?,
                            Date = reader["Data"] as DateTime?,
                            Reading_type = reader["typ_czytania"] as string,
                            Youtube_Id = reader["youtube_Id"] as string,
                        });

                        video.Videos = Videos_for_date_list;
                    }
                    reader.Close();
                }
                return video;
            }
        }

        public Video Select_Video(int? id)
        {
            using (var sqlConn = new SqlConnection(connetionString))
            {
                sqlConn.Open();
                var Videos_for_date_list = new List<Video>();
                var video = new Video();
                using (var cmd = sqlConn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM Video WHERE id_Video=@ID";
                    cmd.Parameters.AddWithValue("@ID", id);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Videos_for_date_list.Add(new Video()
                        {
                            Id_Video = reader["id_Video"] as int?,
                            Date = reader["Data"] as DateTime?,
                            Reading_type = reader["typ_czytania"] as string,
                            Youtube_Id = reader["youtube_Id"] as string,
                        });
                        video.Videos = Videos_for_date_list;
                    }
                    reader.Close();
                }
                return video;
            }
        }

        public Video Select_Video(DateTime? date, string reading_type)
        {
            using (var sqlConn = new SqlConnection(connetionString))
            {
                sqlConn.Open();
                var Videos_for_date_list = new List<Video>();
                var video = new Video();
                using (var cmd = sqlConn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM Video WHERE Data=@date AND typ_czytania=@reading_type";
                    cmd.Parameters.AddWithValue("@date", date);
                    cmd.Parameters.AddWithValue("@reading_type", reading_type);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Videos_for_date_list.Add(new Video()
                        {
                            Id_Video = reader["id_Video"] as int?,
                            Date = reader["Data"] as DateTime?,
                            Reading_type = reader["typ_czytania"] as string,
                            Youtube_Id = reader["youtube_Id"] as string,
                        });
                        video.Videos = Videos_for_date_list;
                    }
                    reader.Close();
                }
                return video;
            }
        }

        public Video Select_Video(int? id, DateTime? date, string reading_type)
        {
            using (var sqlConn = new SqlConnection(connetionString))
            {
                sqlConn.Open();
                var Videos_for_date_list = new List<Video>();
                var video = new Video();
                using (var cmd = sqlConn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM Video WHERE Data=@date AND typ_czytania=@reading_type AND id_Video!=@id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@date", date);
                    cmd.Parameters.AddWithValue("@reading_type", reading_type);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Videos_for_date_list.Add(new Video()
                        {
                            Id_Video = reader["id_Video"] as int?,
                            Date = reader["Data"] as DateTime?,
                            Reading_type = reader["typ_czytania"] as string,
                            Youtube_Id = reader["youtube_Id"] as string,
                        });
                        video.Videos = Videos_for_date_list;
                    }
                    reader.Close();
                }
                return video;
            }
        }
        #endregion

        #region method_for_UPDATE_Video
        public Video Update_Video(Video video)
        {
            using (SqlConnection sqlConn = new SqlConnection(connetionString))
            {
                string query = "UPDATE Video SET typ_czytania=@reading_type,youtube_Id=@Youtube_Id WHERE id_Video=@Id_Video";
                //query += " SELECT SCOPE_IDENTITY()";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = sqlConn;
                    sqlConn.Open();
                    cmd.Parameters.AddWithValue("@Id_Video", video.Id_Video);
                    cmd.Parameters.AddWithValue("@Date", video.Date);
                    cmd.Parameters.AddWithValue("@reading_type", video.Reading_type);
                    cmd.Parameters.AddWithValue("@Youtube_Id", video.Youtube_Id);
                    if (video.Id_Video != null && video.Date != null)
                    {

                        cmd.ExecuteNonQuery();
                    }

                    sqlConn.Close();
                }
            }
            return video;
        }
        #endregion

        #region method_for_INSERT_Video
        public Video Insert_Video(Video video)
        {
            using (SqlConnection sqlConn = new SqlConnection(connetionString))
            {
                string query = "INSERT INTO Video(Data,typ_czytania,youtube_Id) VALUES(@Date,@reading_type,@Youtube_Id)";
                query += " SELECT SCOPE_IDENTITY()";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = sqlConn;
                    sqlConn.Open();
                    cmd.Parameters.AddWithValue("@Date", video.Date);
                    cmd.Parameters.AddWithValue("@reading_type", video.Reading_type);
                    cmd.Parameters.AddWithValue("@Youtube_Id", video.Youtube_Id);
                    video.Id_Video = Convert.ToInt32(cmd.ExecuteScalar());
                    sqlConn.Close();
                }
            }
            return video;
        }
        #endregion

        #region method_for_DELETE_Video
        public void Delete_Video(int? id)
        {
            using (SqlConnection sqlConn = new SqlConnection(connetionString))
            {
                string query = "DELETE FROM Video WHERE id_Video=@Id_Video";
                //query += " SELECT SCOPE_IDENTITY()";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = sqlConn;
                    sqlConn.Open();
                    cmd.Parameters.AddWithValue("@Id_Video", id);
                    if (id != null)
                    {
                        cmd.ExecuteNonQuery();
                    }
                    sqlConn.Close();
                }
            }
        }
        #endregion



        #region methods_for_SELECT_SlownikDni
        public SlownikDni Select_SlownikDni(string liturgical_day)
        {
            using (var sqlConn = new SqlConnection(connetionString))
            {
                sqlConn.Open();
                var SlownikDni_for_liturgicalname_list = new List<SlownikDni>();
                var Slownik = new SlownikDni();
                var time = DateTime.Now;
                string[] liturgical_day_split = liturgical_day.Split('|');
                liturgical_day = liturgical_day_split[0];
                if (liturgical_day != null)
                {
                    using (var cmd = sqlConn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT * FROM SlownikDni WHERE dzien_liturgiczny=@liturgical_day";
                        cmd.Parameters.AddWithValue("@liturgical_day", liturgical_day);
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            SlownikDni_for_liturgicalname_list.Add(new SlownikDni()
                            {
                                Liturgical_day = reader["dzien_liturgiczny"] as string,
                                Day_name = reader["nazwa_dnia"] as string,
                                Holyday = reader["swieto"] as bool? ?? false,
                                TimeStamp=reader["timestamp"] as DateTime?,

                            });

                            Slownik.SlownikDni_list = SlownikDni_for_liturgicalname_list;
                        }
                        reader.Close();
                    }
                }

                return Slownik;
            }
        }

        public SlownikDni Select_All_SlownikDni()
        {
            using (var sqlConn = new SqlConnection(connetionString))
            {
                sqlConn.Open();
                var SlownikDni_for_liturgicalname_list = new List<SlownikDni>();
                var Slownik = new SlownikDni();
                using (var cmd = sqlConn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM SlownikDni";
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        SlownikDni_for_liturgicalname_list.Add(new SlownikDni()
                        {
                            Liturgical_day = reader["dzien_liturgiczny"] as string,
                            Day_name = reader["nazwa_dnia"] as string,
                            Holyday = reader["swieto"] as bool? ?? false,
                            TimeStamp = reader["timestamp"] as DateTime?,

                        });

                        Slownik.SlownikDni_list = SlownikDni_for_liturgicalname_list;
                    }
                    reader.Close();

                }

                return Slownik;
            }
        }
        #endregion

        #region method_for_UPDATE_SlownikDni
        public SlownikDni Update_SlownikDni(SlownikDni slownik)
        { var time = DateTime.Now;
            using (SqlConnection sqlConn = new SqlConnection(connetionString))
            {
                string query = "UPDATE SlownikDni SET nazwa_dnia=@day_name,swieto=@holyday,timestamp=@timestamp WHERE dzien_liturgiczny=@liturgical_day";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = sqlConn;
                    sqlConn.Open();
                    cmd.Parameters.AddWithValue("@liturgical_day", slownik.Liturgical_day);
                    cmd.Parameters.AddWithValue("@day_name", slownik.Day_name);
                    cmd.Parameters.AddWithValue("@holyday", slownik.Holyday);
                    cmd.Parameters.AddWithValue("@timestamp", time);
                    cmd.ExecuteNonQuery();
                    sqlConn.Close();
                }
            }
            return slownik;
        }
        #endregion

        #region method_for_INSERT_SlownikDni
        public SlownikDni Insert_SlownikDni(SlownikDni slownik)
        {
            DateTime time = DateTime.Now;
            bool check = true;
            var slownik_test = Select_SlownikDni(slownik.Liturgical_day);

            if (slownik_test.SlownikDni_list.Count != 0)
            {
                check = false;
            }
            if (check == true)
            {
                using (SqlConnection sqlConn = new SqlConnection(connetionString))
                {
                    string query = "INSERT INTO SlownikDni(dzien_liturgiczny,nazwa_dnia,swieto,timestamp) VALUES(@liturgical_day,@day_name,@holyday,@timestamp)";
                    query += " SELECT SCOPE_IDENTITY()";
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = sqlConn;
                        sqlConn.Open();
                        cmd.Parameters.AddWithValue("@liturgical_day", slownik.Liturgical_day);
                        cmd.Parameters.AddWithValue("@day_name", slownik.Day_name);
                        cmd.Parameters.AddWithValue("@holyday", slownik.Holyday);
                        cmd.Parameters.AddWithValue("@timestamp", time);

                        cmd.ExecuteNonQuery();
                        sqlConn.Close();
                    }
                }
            }
            return slownik;
        }
        #endregion

        #region method_for_DELETE_SlownikDni
        public void Delete_SlownikDni(string liturgical_day)
        {
            using (SqlConnection sqlConn = new SqlConnection(connetionString))
            {
                string query = "DELETE FROM SlownikDni WHERE dzien_liturgiczny=@liturgical_day";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = sqlConn;
                    sqlConn.Open();
                    cmd.Parameters.AddWithValue("@liturgical_day", liturgical_day);
                    cmd.ExecuteNonQuery();
                    sqlConn.Close();
                }
            }
        }
        #endregion



        #region methods_for_SELECT_Lekcjonarz

        public Lekcjonarz Select_Lekcjonarz()
        {
            using (var sqlConn = new SqlConnection(connetionString))
            {
                sqlConn.Open();
                var Lekcjonarz_for_liturgicalname_list = new List<Lekcjonarz>();
                var Lekcjonarz_for_dropdown_list = new List<Lekcjonarz>();
                var lekcjonarz = new Lekcjonarz();
                    using (var cmd = sqlConn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT * FROM Lekcjonarz";
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            Lekcjonarz_for_liturgicalname_list.Add(new Lekcjonarz()
                            {
                                ID_lekcjonarz = reader["id_llekcjonarz"] as int?,
                                Liturgical_day = reader["dzien_liturgiczny"] as string,
                                Reading_type = reader["typ_czytania"] as string,
                                Siglum = reader["siglum"] as string,
                                Text = reader["tekst"] as string,
                            });
                            lekcjonarz.Lekcjonarz_list = Lekcjonarz_for_liturgicalname_list;
                        }
                        reader.Close();
                    }
                return lekcjonarz;
            }
        }
        


        public Lekcjonarz Select_Lekcjonarz(string liturgical_day)
        {
            using (var sqlConn = new SqlConnection(connetionString))
            {
                sqlConn.Open();
                var Lekcjonarz_for_liturgicalname_list = new List<Lekcjonarz>();
                var Lekcjonarz_for_dropdown_list = new List<Lekcjonarz>();
                var lekcjonarz = new Lekcjonarz();
                string[] liturgical_day_split = liturgical_day.Split('|');
                liturgical_day = liturgical_day_split[0];
                if (liturgical_day != null)
                {
                    using (var cmd = sqlConn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT * FROM Lekcjonarz WHERE dzien_liturgiczny=@liturgical_day";
                        cmd.Parameters.AddWithValue("@liturgical_day", liturgical_day);
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            Lekcjonarz_for_liturgicalname_list.Add(new Lekcjonarz()
                            {
                                ID_lekcjonarz = reader["id_llekcjonarz"] as int?,
                                Liturgical_day = reader["dzien_liturgiczny"] as string,
                                Reading_type = reader["typ_czytania"] as string,
                                Siglum = reader["siglum"] as string,
                                Text = reader["tekst"] as string,


                            });

                            lekcjonarz.Lekcjonarz_list = Lekcjonarz_for_liturgicalname_list;
                        }
                        reader.Close();
                    }
                    using (var commandLekcjonarz_dropdown = sqlConn.CreateCommand())
                    {
                        commandLekcjonarz_dropdown.CommandText = "SELECT DISTINCT dzien_liturgiczny FROM Lekcjonarz GROUP BY dzien_liturgiczny ";
                        var reader = commandLekcjonarz_dropdown.ExecuteReader();
                        while (reader.Read())
                        {
                            Lekcjonarz_for_dropdown_list.Add(new Lekcjonarz()
                            {
                                Liturgical_day = reader["dzien_liturgiczny"] as string,
                            });
                            lekcjonarz.Lekcjonarz_list_dropdown = Lekcjonarz_for_dropdown_list;
                        }
                        reader.Close();
                    }

                }
                return lekcjonarz;
            }
        }

        public Lekcjonarz Select_Lekcjonarz(int? ID)
        {
            using (var sqlConn = new SqlConnection(connetionString))
            {
                sqlConn.Open();
                var Lekcjonarz_for_liturgicalname_list = new List<Lekcjonarz>();
                var Lekcjonarz_for_dropdown_list = new List<Lekcjonarz>();
                var lekcjonarz = new Lekcjonarz();
                if (ID != null)
                {
                    using (var cmd = sqlConn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT * FROM Lekcjonarz WHERE id_llekcjonarz=@ID";
                        cmd.Parameters.AddWithValue("@ID", ID);
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            Lekcjonarz_for_liturgicalname_list.Add(new Lekcjonarz()
                            {
                                ID_lekcjonarz = reader["id_llekcjonarz"] as int?,
                                Liturgical_day = reader["dzien_liturgiczny"] as string,
                                Reading_type = reader["typ_czytania"] as string,
                                Siglum = reader["siglum"] as string,
                                Text = reader["tekst"] as string,


                            });

                            lekcjonarz.Lekcjonarz_list = Lekcjonarz_for_liturgicalname_list;
                        }
                        reader.Close();
                    }
                    using (var commandLekcjonarz_dropdown = sqlConn.CreateCommand())
                    {
                        commandLekcjonarz_dropdown.CommandText = "SELECT DISTINCT dzien_liturgiczny FROM Lekcjonarz GROUP BY dzien_liturgiczny ";
                        var reader = commandLekcjonarz_dropdown.ExecuteReader();
                        while (reader.Read())
                        {
                            Lekcjonarz_for_dropdown_list.Add(new Lekcjonarz()
                            {
                                Liturgical_day = reader["dzien_liturgiczny"] as string,
                            });
                            lekcjonarz.Lekcjonarz_list_dropdown = Lekcjonarz_for_dropdown_list;
                        }
                    }
                }
                return lekcjonarz;
            }
        }

        public Lekcjonarz Select_All_liturgical_days_Lekcjonarz()
        {
            using (var sqlConn = new SqlConnection(connetionString))
            {
                sqlConn.Open();
                var Lekcjonarz_for_liturgicalname_list = new List<Lekcjonarz>();
                var lekcjonarz = new Lekcjonarz();

                using (var cmd = sqlConn.CreateCommand())
                {
                    cmd.CommandText = "SELECT DISTINCT dzien_liturgiczny FROM Lekcjonarz GROUP BY dzien_liturgiczny ";
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Lekcjonarz_for_liturgicalname_list.Add(new Lekcjonarz()
                        {
                            Liturgical_day = reader["dzien_liturgiczny"] as string,

                        });
                        lekcjonarz.Lekcjonarz_list_dropdown = Lekcjonarz_for_liturgicalname_list;
                    }
                    reader.Close();
                }


                return lekcjonarz;
            }
        }
        #endregion

        #region method_for_INSERT_Lekcjonarz
        public Lekcjonarz Insert_Lekcjonarz(Lekcjonarz lekcjonarz)
        {
            using (SqlConnection sqlConn = new SqlConnection(connetionString))
            {
                if (lekcjonarz.Siglum == null)
                {
                    string query = "INSERT INTO Lekcjonarz(dzien_liturgiczny,typ_czytania,tekst) VALUES(@liturgical_day,@reading_type,@text)";
                    query += " SELECT SCOPE_IDENTITY()";
                    string[] liturgical_day_split = lekcjonarz.Liturgical_day.Split('|');
                    lekcjonarz.Liturgical_day = liturgical_day_split[0];
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = sqlConn;
                        sqlConn.Open();
                        cmd.Parameters.AddWithValue("@liturgical_day", lekcjonarz.Liturgical_day);
                        cmd.Parameters.AddWithValue("@reading_type", lekcjonarz.Reading_type);
                        cmd.Parameters.AddWithValue("@text", lekcjonarz.Text);
                        lekcjonarz.ID_lekcjonarz = Convert.ToInt32(cmd.ExecuteScalar());
                        sqlConn.Close();
                    }
                }
                else
                {
                    string query = "INSERT INTO Lekcjonarz(dzien_liturgiczny,typ_czytania,siglum,tekst) VALUES(@liturgical_day,@reading_type,@siglum,@text)";
                    query += " SELECT SCOPE_IDENTITY()";
                    string[] liturgical_day_split = lekcjonarz.Liturgical_day.Split('|');
                    lekcjonarz.Liturgical_day = liturgical_day_split[0];
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = sqlConn;
                        sqlConn.Open();
                        cmd.Parameters.AddWithValue("@liturgical_day", lekcjonarz.Liturgical_day);
                        cmd.Parameters.AddWithValue("@reading_type", lekcjonarz.Reading_type);
                        cmd.Parameters.AddWithValue("@siglum", lekcjonarz.Siglum);
                        cmd.Parameters.AddWithValue("@text", lekcjonarz.Text);
                        lekcjonarz.ID_lekcjonarz = Convert.ToInt32(cmd.ExecuteScalar());
                        sqlConn.Close();
                    }
                }
            }
            return lekcjonarz;
        }
        #endregion

        #region method_for_UPDATE_Lekcjonarz
        public Lekcjonarz Update_Lekcjonarz(Lekcjonarz lekcjonarz)
        {
            using (SqlConnection sqlConn = new SqlConnection(connetionString))
            {
                if (lekcjonarz.Siglum == null)
                {
                    lekcjonarz.Siglum = "";
                }
                string[] liturgical_day_split = lekcjonarz.Liturgical_day.Split('|');
                lekcjonarz.Liturgical_day = liturgical_day_split[0];
                string query = "UPDATE Lekcjonarz SET dzien_liturgiczny=@liturgical_day,typ_czytania=@reading_type, siglum=@siglum,tekst=@text WHERE id_llekcjonarz=@ID";
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = sqlConn;
                        sqlConn.Open();
                        cmd.Parameters.AddWithValue("@ID", lekcjonarz.ID_lekcjonarz);
                        cmd.Parameters.AddWithValue("@liturgical_day", lekcjonarz.Liturgical_day);
                        cmd.Parameters.AddWithValue("@reading_type", lekcjonarz.Reading_type);
                        cmd.Parameters.AddWithValue("@siglum", lekcjonarz.Siglum);
                        cmd.Parameters.AddWithValue("@text", lekcjonarz.Text);
                        cmd.ExecuteNonQuery();
                        sqlConn.Close();
                    }
                }
            return lekcjonarz;
        }
        #endregion

        #region method_for_DELETE_Lekcjonarz
        public void Delete_Lekcjonarz(int? id)
        {
            using (SqlConnection sqlConn = new SqlConnection(connetionString))
            {
                string query = "DELETE FROM Lekcjonarz WHERE id_llekcjonarz=@ID";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = sqlConn;
                    sqlConn.Open();
                    cmd.Parameters.AddWithValue("@ID", id);
                    if (id != null)
                    {
                        cmd.ExecuteNonQuery();
                    }
                    sqlConn.Close();
                }
            }
        }
        #endregion



        #region methods_for_SELECT_Kalendarz
        public Kalendarz Select_Kalendarz()
        {
            {
                using (var sqlConn = new SqlConnection(connetionString))
                {
                    sqlConn.Open();
                    var kalendarz_list = new List<Kalendarz>();
                    var kalendarz = new Kalendarz();


                    using (var cmd = sqlConn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT * FROM Kalendarz";
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            kalendarz_list.Add(new Kalendarz()
                            {
                                Date = reader["Data"] as DateTime?,
                                Liturgical_day = reader["dzien_liturgiczny"] as string,
                                Day_name = reader["nazwa_dnia"] as string,
                                Comment_source1 = reader["kom_zrodloD"] as string,
                                Comment_source2 = reader["kom_zrodloM"] as string,
                                id_book1 = reader["id_ksiazka1"] as int?,
                                id_book2 = reader["id_ksiazka2"] as int?,
                                id_book3 = reader["id_ksiazka3"] as int?,
                                week_number = reader["numer_tygodnia"] as int?,
                                patron1 = reader["patron1"] as string,
                                patron2 = reader["patron2"] as string,
                                patron3 = reader["patron3"] as string,
                            });
                            kalendarz.Kalendarz_list = kalendarz_list;
                        }
                        reader.Close();
                    }

                    return kalendarz;
                }
            }
        }

        public Kalendarz Select_Kalendarz(DateTime? date)
        {
            {
                using (var sqlConn = new SqlConnection(connetionString))
                {
                    sqlConn.Open();
                    var kalendarz_list = new List<Kalendarz>();
                    var kalendarz = new Kalendarz();

                    using (var cmd = sqlConn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT * FROM Kalendarz WHERE Data=@date";
                        cmd.Parameters.AddWithValue("@date", date);
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            kalendarz_list.Add(new Kalendarz()
                            {
                                Date = reader["Data"] as DateTime?,
                                Liturgical_day = reader["dzien_liturgiczny"] as string,
                                Day_name = reader["nazwa_dnia"] as string,
                                Comment_source1 = reader["kom_zrodloD"] as string,
                                Comment_source2 = reader["kom_zrodloM"] as string,
                                id_book1 = reader["id_ksiazka1"] as int?,
                                id_book2 = reader["id_ksiazka2"] as int?,
                                id_book3 = reader["id_ksiazka3"] as int?,
                                week_number = reader["numer_tygodnia"] as int?,
                                patron1 = reader["patron1"] as string,
                                patron2 = reader["patron2"] as string,
                                patron3 = reader["patron3"] as string,
                            });
                            kalendarz.Kalendarz_list = kalendarz_list;
                        }
                        reader.Close();
                    }

                    return kalendarz;
                }
            }
        }
        #endregion

        #region method_for_INSERT_Kalendarz
        public Kalendarz Insert_Kalendarz(Kalendarz kalendarz)
        {
            if (kalendarz != null)
            {
                if (kalendarz.id_book2.HasValue == false && kalendarz.id_book3.HasValue == false && String.IsNullOrEmpty(kalendarz.patron1) == true && String.IsNullOrEmpty(kalendarz.patron2) && String.IsNullOrEmpty(kalendarz.patron3))
                {
                    using (SqlConnection sqlConn = new SqlConnection(connetionString))
                    {
                        string query = "INSERT INTO Kalendarz(Data,dzien_liturgiczny,nazwa_dnia,kom_zrodloD,kom_zrodloM,id_ksiazka1,numer_tygodnia) VALUES(@date,@liturgical_day,@day_name,@comment_source1,@comment_source2,@id_book1,@week_number)";
                        query += " SELECT SCOPE_IDENTITY()";
                        string[] liturgical_day_split = kalendarz.Liturgical_day.Split('|');
                        kalendarz.Liturgical_day = liturgical_day_split[0];

                       

                        using (SqlCommand cmd = new SqlCommand(query))
                        {
                            cmd.Connection = sqlConn;
                            sqlConn.Open();
                            cmd.Parameters.AddWithValue("@date", kalendarz.Date);
                            cmd.Parameters.AddWithValue("@liturgical_day", kalendarz.Liturgical_day);
                            cmd.Parameters.AddWithValue("@day_name", kalendarz.Day_name);
                            cmd.Parameters.AddWithValue("@comment_source1", kalendarz.Comment_source1);
                            cmd.Parameters.AddWithValue("@comment_source2", kalendarz.Comment_source2);
                            cmd.Parameters.AddWithValue("@id_book1", kalendarz.id_book1);
                            cmd.Parameters.AddWithValue("@week_number", kalendarz.week_number);
                            cmd.ExecuteNonQuery();
                            sqlConn.Close();
                        }
                    }
                }
            }
            else
            {
                using (SqlConnection sqlConn = new SqlConnection(connetionString))
                {
                    string query = "INSERT INTO Kalendarz(Data,dzien_liturgiczny,nazwa_dnia,kom_zrodloD,kom_zrodloM,id_ksiazka1,id_ksiazka2,id_ksiazka3,numer_tygodnia,patron1,patron2,patron3) VALUES(@date,@liturgical_day,@day_name,@comment_source1,@comment_source2,@id_book1,@id_book2,@id_book3,@week_number,@patron1,@patron2,@patron3)";
                    query += " SELECT SCOPE_IDENTITY()";
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = sqlConn;
                        sqlConn.Open();
                        cmd.Parameters.AddWithValue("@date", kalendarz.Date);
                        cmd.Parameters.AddWithValue("@liturgical_day", kalendarz.Liturgical_day);
                        cmd.Parameters.AddWithValue("@day_name", kalendarz.Day_name);
                        cmd.Parameters.AddWithValue("@comment_source1", kalendarz.Comment_source1);
                        cmd.Parameters.AddWithValue("@comment_source2", kalendarz.Comment_source2);
                        cmd.Parameters.AddWithValue("@id_book1", kalendarz.id_book1);
                        cmd.Parameters.AddWithValue("@id_book2", kalendarz.id_book2);
                        cmd.Parameters.AddWithValue("@id_book3", kalendarz.id_book3);
                        cmd.Parameters.AddWithValue("@week_number", kalendarz.week_number);
                        cmd.Parameters.AddWithValue("@patron1", kalendarz.patron1);
                        cmd.Parameters.AddWithValue("@patron2", kalendarz.patron2);
                        cmd.Parameters.AddWithValue("@patron3", kalendarz.patron3);
                        cmd.ExecuteNonQuery();
                        sqlConn.Close();
                    }
                }
            }
            return kalendarz;
        }
        #endregion

        #region method_for_UPDATE_Kalendarz
        public Kalendarz Update_Kalendarz(Kalendarz kalendarz)
        {
            using (SqlConnection sqlConn = new SqlConnection(connetionString))
            {
                string query = "UPDATE Kalendarz SET Data=@date,dzien_liturgiczny=@liturgical_day,nazwa_dnia=@day_name,kom_zrodloD=@comment_source1,kom_zrodloM=@comment_source2,id_ksiazka1=@id_book1,numer_tygodnia=@week_number WHERE Data=@date";
                //query += " SELECT SCOPE_IDENTITY()";
                string[] liturgical_day_split = kalendarz.Liturgical_day.Split('|');
                kalendarz.Liturgical_day = liturgical_day_split[0];
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = sqlConn;
                    sqlConn.Open();
                    cmd.Parameters.AddWithValue("@date", kalendarz.Date);
                    cmd.Parameters.AddWithValue("@liturgical_day", kalendarz.Liturgical_day);
                    cmd.Parameters.AddWithValue("@day_name", kalendarz.Day_name);
                    cmd.Parameters.AddWithValue("@comment_source1", kalendarz.Comment_source1);
                    cmd.Parameters.AddWithValue("@comment_source2", kalendarz.Comment_source2);
                    cmd.Parameters.AddWithValue("@id_book1", kalendarz.id_book1);
                    cmd.Parameters.AddWithValue("@week_number", kalendarz.week_number);
                    cmd.ExecuteNonQuery();
                    sqlConn.Close();
                }
            }
            return kalendarz;
        }
        #endregion

        #region method_for_DELETE_Kalendarz
        public void Delete_Kalendarz(DateTime? date)
        {
            using (SqlConnection sqlConn = new SqlConnection(connetionString))
            {
                string query = "DELETE FROM Kalendarz WHERE Data=@date";
                //query += " SELECT SCOPE_IDENTITY()";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = sqlConn;
                    sqlConn.Open();
                    cmd.Parameters.AddWithValue("@date", date);
                    cmd.ExecuteNonQuery();
                    sqlConn.Close();
                }
            }
        }
        #endregion

        #region method_for_UPDATE_Patroni_FROM_Kalendarz
        public Kalendarz Update_Patroni(Kalendarz kalendarz)
        {
            DateTime  Date = kalendarz.Date.Value;
            string month = Date.Month.ToString();
            string day = Date.Day.ToString();
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
            using (SqlConnection sqlConn = new SqlConnection(connetionString))
            {
                string query = "UPDATE patroni SET wyswietl=@show WHERE data=@date";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = sqlConn;
                    sqlConn.Open();
                    
                    cmd.Parameters.AddWithValue("@date", date_patroni);
                    cmd.Parameters.AddWithValue("@show", kalendarz.patroni_show);
                    cmd.ExecuteNonQuery();
                    sqlConn.Close();
                }
            }
            return kalendarz;
        }
        #endregion


        #region methods_for_SELECT_Komentarze
        public Komentarze Select_Komentarze()
        {
            {
                using (var sqlConn = new SqlConnection(connetionString))
                {
                    sqlConn.Open();
                    var komentarze_list = new List<Komentarze>();
                    var komentarze = new Komentarze();


                    using (var cmd = sqlConn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT * FROM Komentarze";
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            komentarze_list.Add(new Komentarze()
                            {
                                ID_comment = reader["idkom"] as int?,
                                Liturgical_day = reader["dzien_liturgiczny"] as string,
                                Comment_source = reader["kom_zrodlo"] as string,
                                Text = reader["tekst"] as string,
                            });
                            komentarze.Komentarze_list = komentarze_list;
                        }
                        reader.Close();
                    }

                    return komentarze;
                }
            }
        }

        public Komentarze Select_Komentarze(string liturgical_day)
        {
            {
                using (var sqlConn = new SqlConnection(connetionString))
                {
                    sqlConn.Open();
                    var komentarze_list = new List<Komentarze>();
                    var komentarze = new Komentarze();


                    using (var cmd = sqlConn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT * FROM Komentarze WHERE dzien_liturgiczny=@liturgical_day";
                        cmd.Parameters.AddWithValue("@liturgical_day", liturgical_day);
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            komentarze_list.Add(new Komentarze()
                            {
                                ID_comment = reader["idkom"] as int?,
                                Liturgical_day = reader["dzien_liturgiczny"] as string,
                                Comment_source = reader["kom_zrodlo"] as string,
                                Text = reader["tekst"] as string,
                            });
                            komentarze.Komentarze_list = komentarze_list;
                        }
                        reader.Close();
                    }

                    return komentarze;
                }
            }
        }


        public Komentarze Select_Komentarze(int? ID_comment)
        {
            {
                using (var sqlConn = new SqlConnection(connetionString))
                {
                    sqlConn.Open();
                    var komentarze_list = new List<Komentarze>();
                    var komentarze = new Komentarze();


                    using (var cmd = sqlConn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT * FROM Komentarze WHERE idkom=@ID_comment";
                        cmd.Parameters.AddWithValue("@ID_comment", ID_comment);
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            komentarze_list.Add(new Komentarze()
                            {
                                ID_comment = reader["idkom"] as int?,
                                Liturgical_day = reader["dzien_liturgiczny"] as string,
                                Comment_source = reader["kom_zrodlo"] as string,
                                Text = reader["tekst"] as string,
                            });
                            komentarze.Komentarze_list = komentarze_list;
                        }
                        reader.Close();
                    }

                    return komentarze;
                }
            }
        }

        #endregion

        #region method_for_INSERT_Komentarze
        public Komentarze Insert_Komentarze(Komentarze komentarze)
        {
            using (SqlConnection sqlConn = new SqlConnection(connetionString))
            {
                string query = "INSERT INTO Komentarze(dzien_liturgiczny,kom_zrodlo,tekst) VALUES(@liturgical_day,@comment_source,@text)";
                query += " SELECT SCOPE_IDENTITY()";
                string[] split_Liturgical_day = komentarze.Liturgical_day.Split('|');
                komentarze.Liturgical_day = split_Liturgical_day[0];
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = sqlConn;
                    sqlConn.Open();
                    cmd.Parameters.AddWithValue("@liturgical_day", komentarze.Liturgical_day);
                    cmd.Parameters.AddWithValue("@comment_source", komentarze.Comment_source);
                    cmd.Parameters.AddWithValue("@text", komentarze.Text);
                    komentarze.ID_comment = Convert.ToInt32(cmd.ExecuteScalar());
                    sqlConn.Close();
                }
            }
            return komentarze;
        }
        #endregion

        #region method_for_UPDATE_Komentarze
        public Komentarze Update_Komentarze(Komentarze komentarze)
        {
            using (SqlConnection sqlConn = new SqlConnection(connetionString))
            {
                string query = "UPDATE Komentarze SET dzien_liturgiczny=@liturgical_day,kom_zrodlo=@comment_source, tekst=@text WHERE idkom=@ID_comment";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = sqlConn;
                    sqlConn.Open();
                    cmd.Parameters.AddWithValue("@ID_comment", komentarze.ID_comment);
                    cmd.Parameters.AddWithValue("@liturgical_day", komentarze.Liturgical_day);
                    cmd.Parameters.AddWithValue("@comment_source", komentarze.Comment_source);
                    cmd.Parameters.AddWithValue("@text", komentarze.Text);
                    cmd.ExecuteNonQuery();
                    sqlConn.Close();
                }
            }
            return komentarze;
        }
        #endregion

        #region method_for_DELETE_Komentarze
        public void Delete_Komentarze(int? id)
        {
            using (SqlConnection sqlConn = new SqlConnection(connetionString))
            {
                string query = "DELETE FROM Komentarze WHERE idkom=@ID_comment";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = sqlConn;
                    sqlConn.Open();
                    cmd.Parameters.AddWithValue("@ID_comment", id);
                    cmd.ExecuteNonQuery();
                    sqlConn.Close();
                }
            }
        }
        #endregion



        #region methods_for_SELECT_Ksiazki
        public Ksiazki Select_Ksiazki()
        {
            {
                using (var sqlConn = new SqlConnection(connetionString))
                {
                    sqlConn.Open();
                    var ksiazki_list = new List<Ksiazki>();
                    var ksiazki = new Ksiazki();


                    using (var cmd = sqlConn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT * FROM Ksiazki";

                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            ksiazki_list.Add(new Ksiazki()
                            {
                                ID_book = reader["id_ksiazki"] as int?,
                                ID_kmt = reader["id_kmt"] as string,
                                Title = reader["tytul"] as string,
                                Author = reader["autor"] as string,
                                Photo = reader["foto"] as string,
                                Description = reader["opis"] as string,
                            });
                            ksiazki.Ksiazki_list = ksiazki_list;
                        }
                        reader.Close();
                    }

                    return ksiazki;
                }
            }
        }


        public Ksiazki Select_Ksiazki(int? id)
        {
            {
                using (var sqlConn = new SqlConnection(connetionString))
                {
                    sqlConn.Open();
                    var ksiazki_list = new List<Ksiazki>();
                    var ksiazki = new Ksiazki();

                    using (var cmd = sqlConn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT * FROM Ksiazki WHERE id_ksiazki=@id";
                        cmd.Parameters.AddWithValue("@id", id);
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            ksiazki_list.Add(new Ksiazki()
                            {
                                ID_book = reader["id_ksiazki"] as int?,
                                ID_kmt = reader["id_kmt"] as string,
                                Title = reader["tytul"] as string,
                                Author = reader["autor"] as string,
                                Photo = reader["foto"] as string,
                                Description = reader["opis"] as string,
                            });
                        }
                        ksiazki.Ksiazki_list = ksiazki_list;
                        reader.Close();
                    }
                    return ksiazki;
                }
            }
        }


        public Ksiazki Select_Ksiazki(string title)
        {

            {
                using (var sqlConn = new SqlConnection(connetionString))
                {
                    sqlConn.Open();
                    var ksiazki_list = new List<Ksiazki>();
                    var ksiazki = new Ksiazki();

                    using (var cmd = sqlConn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT * FROM Ksiazki WHERE tytul=@title";
                        cmd.Parameters.AddWithValue("@title", title);
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {                          
                            ksiazki_list.Add(new Ksiazki()
                            {
                                ID_book = reader["id_ksiazki"] as int?,
                                ID_kmt = reader["id_kmt"] as string,
                                Title = reader["tytul"] as string,
                                Author = reader["autor"] as string,
                                Photo = reader["foto"] as string,
                                Description = reader["opis"] as string,
                            });

                            ksiazki.Ksiazki_list = ksiazki_list;
                        }
                        reader.Close();
                    }

                    return ksiazki;
                }
            }
        }
        #endregion

        #region method_for_INSERT_Ksiazki
        public Ksiazki Insert_Ksiazki(Ksiazki ksiazki)
        {
            using (SqlConnection sqlConn = new SqlConnection(connetionString))
            {
                if(ksiazki.Author==null)
                {
                    string query = "INSERT INTO Ksiazki(id_kmt,tytul,foto,opis) VALUES(@id_kmt,@title,@photo,@description)";
                    query += " SELECT SCOPE_IDENTITY()";
                    
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = sqlConn;
                        sqlConn.Open();
                        cmd.Parameters.AddWithValue("@id_kmt", ksiazki.ID_kmt);
                        cmd.Parameters.AddWithValue("@title", ksiazki.Title);
                        cmd.Parameters.AddWithValue("@Photo", ksiazki.Photo);
                        cmd.Parameters.AddWithValue("@description", ksiazki.Description);

                        ksiazki.ID_book = Convert.ToInt32(cmd.ExecuteScalar());
                        sqlConn.Close();
                    }
                }
                else
                {
                    string query = "INSERT INTO Ksiazki(id_kmt,tytul,autor,foto,opis) VALUES(@id_kmt,@title,@author,@photo,@description)";
                    query += " SELECT SCOPE_IDENTITY()";
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = sqlConn;
                        sqlConn.Open();
                        cmd.Parameters.AddWithValue("@id_kmt", ksiazki.ID_kmt);
                        cmd.Parameters.AddWithValue("@title", ksiazki.Title);
                        cmd.Parameters.AddWithValue("@author", ksiazki.Author);
                        cmd.Parameters.AddWithValue("@Photo", ksiazki.Photo);
                        cmd.Parameters.AddWithValue("@description", ksiazki.Description);

                        ksiazki.ID_book = Convert.ToInt32(cmd.ExecuteScalar());
                        sqlConn.Close();
                    }
                }
               
            }
            return ksiazki;
        }
        #endregion

        #region method_for_UPDATE_Ksiazki
        public Ksiazki Update_Ksiazki(Ksiazki ksiazki)
        {
            using (SqlConnection sqlConn = new SqlConnection(connetionString))
            {
                if(ksiazki.Author==null)
                {
                    ksiazki.Author = "";
                }
                string query = "UPDATE Ksiazki SET id_kmt=@id_kmt,tytul=@title, autor=@author,foto=@photo, opis=@description WHERE id_ksiazki=@id_book";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = sqlConn;
                    sqlConn.Open();
                    cmd.Parameters.AddWithValue("@id_book", ksiazki.ID_book);
                    cmd.Parameters.AddWithValue("@id_kmt", ksiazki.ID_kmt);
                    cmd.Parameters.AddWithValue("@title", ksiazki.Title);
                    cmd.Parameters.AddWithValue("@author", ksiazki.Author);
                    cmd.Parameters.AddWithValue("@Photo", ksiazki.Photo);
                    cmd.Parameters.AddWithValue("@description", ksiazki.Description);
                    cmd.ExecuteNonQuery();
                    sqlConn.Close();
                }
            }
            return ksiazki;
        }
        #endregion

        #region method_for_DELETE_Ksiazki
        public void Delete_Ksiazki(int? id)
        {
            using (SqlConnection sqlConn = new SqlConnection(connetionString))
            {
                string query = "DELETE FROM Ksiazki WHERE id_ksiazki=@id_book";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = sqlConn;
                    sqlConn.Open();
                    cmd.Parameters.AddWithValue("@id_book", id);
                    cmd.ExecuteNonQuery();
                    sqlConn.Close();
                }
            }
        }
        #endregion



        #region methods_for_SELECT_Patroni
        public Patroni Select_Patroni()
        {
            {
                using (var sqlConn = new SqlConnection(connetionString))
                {
                    sqlConn.Open();
                    var patroni_list = new List<Patroni>();
                    var patroni = new Patroni();


                    using (var cmd = sqlConn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT * FROM patroni";

                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            patroni_list.Add(new Patroni()
                            {
                                ID_patron = reader["ID"] as int?,
                                Date = reader["data"] as string,
                                Patron = reader["patron"] as string,
                                Main = reader["glowny"] as bool? ?? false,
                                Description = reader["opis"] as string,
                                Text=reader["tekst"] as string,
                                Show=reader["wyswietl"] as bool? ?? false,

                            });
                            patroni.Patroni_list = patroni_list;
                        }
                        reader.Close();
                    }

                    return patroni;
                }
            }
        }


        public Patroni Select_Patroni(int? id)
        {
            {
                using (var sqlConn = new SqlConnection(connetionString))
                {
                    sqlConn.Open();
                    var patroni_list = new List<Patroni>();
                    var patroni = new Patroni();


                    using (var cmd = sqlConn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT * FROM patroni WHERE ID=@id_patron";
                        cmd.Parameters.AddWithValue("@id_patron", id);
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            patroni_list.Add(new Patroni()
                            {
                                ID_patron = reader["ID"] as int?,
                                Date = reader["data"] as string,
                                Patron = reader["patron"] as string,
                                Main = reader["glowny"] as bool? ?? false,
                                Description = reader["opis"] as string,
                                Text=reader["tekst"] as string,
                                Show = reader["wyswietl"] as bool? ?? false,

                            });
                            patroni.Patroni_list = patroni_list;
                        }
                        reader.Close();
                    }

                    return patroni;
                }
            }
        }

        public Patroni Select_Patroni(string date)
        {
            {
                using (var sqlConn = new SqlConnection(connetionString))
                {
                    sqlConn.Open();
                    var patroni_list = new List<Patroni>();
                    var patroni = new Patroni();


                    using (var cmd = sqlConn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT * FROM patroni WHERE data=@date";
                        cmd.Parameters.AddWithValue("@date", date);
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            patroni_list.Add(new Patroni()
                            {
                                ID_patron = reader["ID"] as int?,
                                Date = reader["data"] as string,
                                Patron = reader["patron"] as string,
                                Main = reader["glowny"] as bool? ?? false,
                                Description = reader["opis"] as string,
                                Text=reader["tekst"] as string,
                                Show = reader["wyswietl"] as bool? ?? false,

                            });
                            patroni.Patroni_list = patroni_list;
                        }
                        reader.Close();
                    }

                    return patroni;
                }
            }
        }
        #endregion

        #region method_for_INSERT_Patroni
        public Patroni Insert_Patroni(Patroni patroni)
        {
            using (SqlConnection sqlConn = new SqlConnection(connetionString))
            {
                if (patroni.Text != null)
                {
                    string query = "INSERT INTO patroni(data,patron,glowny,opis,tekst,wyswietl) VALUES(@date,@patron,@main,@description,@text,@show)";
                    query += " SELECT SCOPE_IDENTITY()";
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = sqlConn;
                        sqlConn.Open();

                        cmd.Parameters.AddWithValue("@date", patroni.Date);
                        cmd.Parameters.AddWithValue("@patron", patroni.Patron);
                        cmd.Parameters.AddWithValue("@main", patroni.Main);
                        cmd.Parameters.AddWithValue("@description", patroni.Description);
                        cmd.Parameters.AddWithValue("@text", patroni.Text);
                        cmd.Parameters.AddWithValue("@show", patroni.Show);
                        patroni.ID_patron = Convert.ToInt32(cmd.ExecuteScalar());
                        sqlConn.Close();
                    }
                }
                else
                {
                    string query = "INSERT INTO patroni(data,patron,glowny,opis,wyswietl) VALUES(@date,@patron,@main,@description,@show)";
                    query += " SELECT SCOPE_IDENTITY()";
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = sqlConn;
                        sqlConn.Open();

                        cmd.Parameters.AddWithValue("@date", patroni.Date);
                        cmd.Parameters.AddWithValue("@patron", patroni.Patron);
                        cmd.Parameters.AddWithValue("@main", patroni.Main);
                        cmd.Parameters.AddWithValue("@description", patroni.Description);
                        cmd.Parameters.AddWithValue("@show", patroni.Show);
                        patroni.ID_patron = Convert.ToInt32(cmd.ExecuteScalar());
                        sqlConn.Close();
                    }
                }
            }
            return patroni;
        }
        #endregion

        #region method_for_UPDATE_Patroni
        public Patroni Update_Patroni(Patroni patroni)
        {
            using (SqlConnection sqlConn = new SqlConnection(connetionString))
            {
                if (patroni.Text != null)
                {
                    string query = "UPDATE patroni SET data=@date,@patron=@patron,glowny=@main,opis=@description,tekst=@text,wyswietl=@show WHERE ID=@id";

                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = sqlConn;
                        sqlConn.Open();
                        cmd.Parameters.AddWithValue("@id", patroni.ID_patron);
                        cmd.Parameters.AddWithValue("@date", patroni.Date);
                        cmd.Parameters.AddWithValue("@patron", patroni.Patron);
                        cmd.Parameters.AddWithValue("@main", patroni.Main);
                        cmd.Parameters.AddWithValue("@description", patroni.Description);
                        cmd.Parameters.AddWithValue("@text", patroni.Text);
                        cmd.Parameters.AddWithValue("@show", patroni.Show);
                        patroni.ID_patron = Convert.ToInt32(cmd.ExecuteScalar());
                        sqlConn.Close();
                    }
                }
                else
                {
                    string query = "UPDATE patroni SET data=@date,@patron=@patron,glowny=@main,opis=@description,tekst=@text,wyswietl=@show WHERE ID=@id";

                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = sqlConn;
                        sqlConn.Open();
                        cmd.Parameters.AddWithValue("@id", patroni.ID_patron);
                        cmd.Parameters.AddWithValue("@date", patroni.Date);
                        cmd.Parameters.AddWithValue("@patron", patroni.Patron);
                        cmd.Parameters.AddWithValue("@main", patroni.Main);
                        cmd.Parameters.AddWithValue("@description", patroni.Description);
                        cmd.Parameters.AddWithValue("@text", "");
                        cmd.Parameters.AddWithValue("@show", patroni.Show);
                        patroni.ID_patron = Convert.ToInt32(cmd.ExecuteScalar());
                        sqlConn.Close();
                    }
                }
            }

            return patroni;
           
        }
        #endregion

        #region method_for_DELETE_Patroni
        public void Delete_Patroni(int? id)
        {
            using (SqlConnection sqlConn = new SqlConnection(connetionString))
            {
                string query = "DELETE FROM patroni WHERE ID=@id_patron";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = sqlConn;
                    sqlConn.Open();
                    cmd.Parameters.AddWithValue("@id_patron", id);
                    cmd.ExecuteNonQuery();
                    sqlConn.Close();
                }
            }
        }
        #endregion



        #region methods_for_dropdowns

        public Kalendarz_liturgical_day_dropdown Select_Kalendarz_dropdown_liturgical_day()
        {
            {
                using (var sqlConn = new SqlConnection(connetionString))
                {
                    sqlConn.Open();

                    var kalendarz_dropdown_liturgicalday_list = new List<String>();
                    var kalendarz_dropdown_liturgicaldayname_list = new List<String>();
                    var kalendarz_dropdown_SlownikDni_dropdown_list = new List<String>();
                    var kalendarz_dropdown_comment_source = new List<String>();
                    var kalendarz_dropdown_book_id = new List<String>();
                    var kalendarz_book_id = new List<int?>();
                    var kalendarz_book_title = new List<String>();
                    var kalendarz_drop_down_liturgicalday = new Kalendarz_liturgical_day_dropdown();


                    using (var commandLekcjonarz_dropdown = sqlConn.CreateCommand())
                    {
                        commandLekcjonarz_dropdown.CommandText = "SELECT dzien_liturgiczny,nazwa_dnia FROM SlownikDni ORDER BY timestamp desc";
                        var reader = commandLekcjonarz_dropdown.ExecuteReader();
                        while (reader.Read())
                        {
                            kalendarz_dropdown_liturgicalday_list.Add(reader["dzien_liturgiczny"] as string);
                            kalendarz_dropdown_liturgicaldayname_list.Add(reader["nazwa_dnia"] as string);          
                        }
                        string join;
                        for(int i=0;i<kalendarz_dropdown_liturgicalday_list.Count;i++)
                        {
                            join = kalendarz_dropdown_liturgicalday_list[i] + "|" + kalendarz_dropdown_liturgicaldayname_list[i];
                            kalendarz_dropdown_SlownikDni_dropdown_list.Add(join);
                        }
                        kalendarz_drop_down_liturgicalday.liturgical_day_list_kalendarz = kalendarz_dropdown_SlownikDni_dropdown_list;
                        reader.Close();
                    }

                    using (var commandLekcjonarz_dropdown = sqlConn.CreateCommand())
                    {
                        commandLekcjonarz_dropdown.CommandText = "SELECT s_zrodla FROM S_komentarze_zrodla";
                        var reader = commandLekcjonarz_dropdown.ExecuteReader();
                        while (reader.Read())
                        {
                            kalendarz_dropdown_comment_source.Add(reader["s_zrodla"] as string);
                        }
                        for(int i=0;i<kalendarz_dropdown_comment_source.Count;i++)
                        {
                            string s = kalendarz_dropdown_comment_source[i];
                            s = s.Trim();
                            kalendarz_drop_down_liturgicalday.comment_source_list_kalendarz.Add(s);
                        }
                        reader.Close();
                    }

                    using (var commandLekcjonarz_dropdown = sqlConn.CreateCommand())
                    {
                        commandLekcjonarz_dropdown.CommandText = "SELECT id_ksiazki,tytul FROM Ksiazki ORDER BY id_ksiazki DESC";
                        var reader = commandLekcjonarz_dropdown.ExecuteReader();
                        while (reader.Read())
                        {
                            kalendarz_book_id.Add(reader["id_ksiazki"] as int?);
                            kalendarz_book_title.Add(reader["tytul"] as string);
                        }

                        string join;
                        for (int i = 0; i < kalendarz_book_id.Count; i++)
                        {
                            join = kalendarz_book_id[i].ToString() + "|" + kalendarz_book_title[i];
                            kalendarz_dropdown_book_id.Add(join);
                        }

                        for (int i = 0; kalendarz_dropdown_book_id.Count > i; i++)
                        {
                            string s = kalendarz_dropdown_book_id[i];
                            s = s.Trim();
                            kalendarz_drop_down_liturgicalday.book_id_list_kalendarz.Add(s);
                        }
                        reader.Close();
                    }

                   

                    return kalendarz_drop_down_liturgicalday;
                }
            }
        }



        public Data_on_dropdown_select Select_data_on_dropdown_select_kalendarz(string liturgical_day,DateTime? date)
        {
            using (var sqlConn = new SqlConnection(connetionString))
            {
                sqlConn.Open();

                var Data_on_dropdown = new Data_on_dropdown_select();
                string[] liturgical_day_split = liturgical_day.Split('|');
                using (var command_data_on_dropdown_select = sqlConn.CreateCommand())
                {
                    command_data_on_dropdown_select.CommandText = "SELECT nazwa_dnia FROM SlownikDni WHERE dzien_liturgiczny=@lit_day";
                    command_data_on_dropdown_select.Parameters.AddWithValue("@lit_day", liturgical_day_split[0]);
                    var reader = command_data_on_dropdown_select.ExecuteReader();
                    while (reader.Read())
                    {
                        Data_on_dropdown.day_name = reader["nazwa_dnia"] as string;
                    }
                    reader.Close();
                }
                DateTime Date = date.Value;
                string month = Date.Month.ToString();
                string day = Date.Day.ToString();
                string date_patroni;
                if (month.Length<2)
                {
                    month = "0" + month;
                  if(day.Length<2)
                    {
                        day = "0" + day;
                    }
                }
                else
                {
                    if(day.Length<2)
                    {
                        day = "0" + day;
                    }
                }
                date_patroni = month + "-" + day;
                
                using (var command_data_on_dropdown_select = sqlConn.CreateCommand())
                {
                    command_data_on_dropdown_select.CommandText = "SELECT tekst FROM patroni WHERE data=@date";
                    command_data_on_dropdown_select.Parameters.AddWithValue("@date", date_patroni);
                    var reader = command_data_on_dropdown_select.ExecuteReader();
                    while (reader.Read())
                    {
                        Data_on_dropdown.text = reader["tekst"] as string;
                    }
                    reader.Close();
                }
                try
                {
                    string[] split_to_get_name = Data_on_dropdown.day_name.Split('-');
                    if (Data_on_dropdown.text == "")
                    {
                        Data_on_dropdown.day_name = split_to_get_name[0];
                    }
                    else
                    {
                        Data_on_dropdown.day_name = split_to_get_name[0] +"-" + Data_on_dropdown.text;
                    }
                }
                catch
                {

                }
                return Data_on_dropdown;
            }
        }

        public S_reading_type Select_S_reading_type()
        {
            using (var sqlConn = new SqlConnection(connetionString))
            {
                sqlConn.Open();
                var S_reading_type_list = new List<String>();
                var S_reading_type = new S_reading_type();


                using (var commandS_reading_type = sqlConn.CreateCommand())
                {
                    commandS_reading_type.CommandText = "SELECT * FROM S_typ_czytania";
                    var reader = commandS_reading_type.ExecuteReader();
                    while (reader.Read())
                    {
                        S_reading_type_list.Add(reader["s_typ_czytania"] as string);
                    }
                    reader.Close();
                }
                for (int i = 0; i < S_reading_type_list.Count; i++)
                {
                    string s = S_reading_type_list[i];
                    s = s.Trim();
                    S_reading_type.S_reading_type_list.Add(s);
                }
                return S_reading_type;
            }
        }


        public Patroni_id_list Select_dropdown_patroni_id()
        {
            using (var sqlConn = new SqlConnection(connetionString))
            {
                sqlConn.Open();
                var patroni_id_list = new List<string>();
                var Patroni_id = new Patroni_id_list();


                using (var commandS_reading_type = sqlConn.CreateCommand())
                {
                    commandS_reading_type.CommandText = "SELECT DISTINCT data FROM Patroni ORDER BY data ASC";
                    var reader = commandS_reading_type.ExecuteReader();
                    while (reader.Read())
                    {
                        patroni_id_list.Add(reader["data"] as string);
                        Patroni_id.Patroni_id_list_dropdown = patroni_id_list;
                    }
                    reader.Close();
                }

                return Patroni_id;
            }
        }
        #endregion
    }

}
