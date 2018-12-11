using Mateusz_formularz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace Mateusz_formularz.Data
{
    public class LectionsSqlRepository : LectionsRepository
    {
        string connetionString = "Data Source=.\\SQLExpress;Initial Catalog=czytania;User ID=test1234;Password=QazWsx12";


        public Video Select_Video(DateTime date)
        {
            using (var sqlConn = new SqlConnection(connetionString))
            {
                sqlConn.Open();
                var Videos_for_date_list = new List<Video>();
                var video = new Video();
                using (var commandGetVideo = sqlConn.CreateCommand())
                {
                    commandGetVideo.CommandText = "SELECT * FROM Video WHERE Data=@date";
                    DateTime date_for_select = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                    commandGetVideo.Parameters.AddWithValue("@date", date_for_select);
                    var reader = commandGetVideo.ExecuteReader();
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
                using (var commandGetVideo = sqlConn.CreateCommand())
                {
                    commandGetVideo.CommandText = "SELECT TOP 60 * FROM Video ORDER BY Data DESC";
                    var reader = commandGetVideo.ExecuteReader();
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
                using (var commandGetVideo = sqlConn.CreateCommand())
                {
                    commandGetVideo.CommandText = "SELECT * FROM Video WHERE id_Video=@ID";
                    commandGetVideo.Parameters.AddWithValue("@ID", id);
                    var reader = commandGetVideo.ExecuteReader();
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
        public Video Update_Video(Video video)
        {
            using (SqlConnection con = new SqlConnection(connetionString))
            {
                string query = "UPDATE Video SET typ_czytania=@reading_type,youtube_Id=@Youtube_Id WHERE id_Video=@Id_Video";
                //query += " SELECT SCOPE_IDENTITY()";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.Parameters.AddWithValue("@Id_Video", video.Id_Video);
                    cmd.Parameters.AddWithValue("@Date", video.Date);
                    cmd.Parameters.AddWithValue("@reading_type", video.Reading_type);
                    cmd.Parameters.AddWithValue("@Youtube_Id", video.Youtube_Id);
                    if(video.Id_Video!=null && video.Date!=null)
                    {
                      
                        cmd.ExecuteNonQuery();
                    }
                    
                    con.Close();
                }
            }
            return video;
        }
       

        public Video Insert_Video(Video video)
        {
            using (SqlConnection con = new SqlConnection(connetionString))
            {
                string query = "INSERT INTO Video(Data,typ_czytania,youtube_Id) VALUES(@Date,@reading_type,@Youtube_Id)";
                query += " SELECT SCOPE_IDENTITY()";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.Parameters.AddWithValue("@Date", video.Date);
                    cmd.Parameters.AddWithValue("@reading_type", video.Reading_type);
                    cmd.Parameters.AddWithValue("@Youtube_Id", video.Youtube_Id);
                    video.Id_Video = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                }
            }
            return video;
        }


        public SlownikDni Select_SlownikDni(string liturgical_day)
        {
            using (var sqlConn = new SqlConnection(connetionString))
            {
                sqlConn.Open();
                var SlownikDni_for_liturgicalname_list = new List<SlownikDni>();
                var Slownik = new SlownikDni();
                if(liturgical_day!=null)
                {
                    using (var commandGetSlownik = sqlConn.CreateCommand())
                    {
                        commandGetSlownik.CommandText = "SELECT * FROM SlownikDni WHERE dzien_liturgiczny=@liturgical_day";
                        commandGetSlownik.Parameters.AddWithValue("@liturgical_day", liturgical_day);
                        var reader = commandGetSlownik.ExecuteReader();
                        while (reader.Read())
                        {
                            SlownikDni_for_liturgicalname_list.Add(new SlownikDni()
                            {
                                Liturgical_day = reader["dzien_liturgiczny"] as string,
                                Day_name = reader["nazwa_dnia"] as string,
                                Holyday = reader["swieto"] as bool? ?? false,

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
                    using (var commandGetSlownik = sqlConn.CreateCommand())
                    {
                        commandGetSlownik.CommandText = "SELECT * FROM SlownikDni";
                        var reader = commandGetSlownik.ExecuteReader();
                    while (reader.Read())
                    {
                        SlownikDni_for_liturgicalname_list.Add(new SlownikDni()
                        {
                            Liturgical_day = reader["dzien_liturgiczny"] as string,
                            Day_name = reader["nazwa_dnia"] as string,
                            Holyday = reader["swieto"] as bool? ?? false,

                        });

                        Slownik.SlownikDni_list = SlownikDni_for_liturgicalname_list;
                    }
                        reader.Close();
                    
                }

                return Slownik;
            }
        }


        public SlownikDni Update_SlownikDni(SlownikDni slownik)
        {
            using (SqlConnection con = new SqlConnection(connetionString))
            {
                string query = "UPDATE SlownikDni SET nazwa_dnia=@day_name,swieto=@holyday WHERE dzien_liturgiczny=@liturgical_day";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.Parameters.AddWithValue("@liturgical_day", slownik.Liturgical_day);
                    cmd.Parameters.AddWithValue("@day_name", slownik.Day_name);
                    cmd.Parameters.AddWithValue("@holyday", slownik.Holyday);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return slownik;
        }


        public SlownikDni Insert_SlownikDni(SlownikDni slownik)
        {
            bool check = true;
            var slownik_test=Select_SlownikDni(slownik.Liturgical_day);

                if(slownik_test.SlownikDni_list.Count!=0)
                {
                   check = false;
                }
            if(check==true)
            {
                using (SqlConnection con = new SqlConnection(connetionString))
                {
                    string query = "INSERT INTO SlownikDni(dzien_liturgiczny,nazwa_dnia,swieto) VALUES(@liturgical_day,@day_name,@holyday)";
                    query += " SELECT SCOPE_IDENTITY()";
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        con.Open();
                        cmd.Parameters.AddWithValue("@liturgical_day", slownik.Liturgical_day);
                        cmd.Parameters.AddWithValue("@day_name", slownik.Day_name);
                        cmd.Parameters.AddWithValue("@holyday", slownik.Holyday);

                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
            return slownik;
        }



        public Lekcjonarz Select_Lekcjonarz(string liturgical_day)
        {
            using (var sqlConn = new SqlConnection(connetionString))
            {
                sqlConn.Open();
                var Lekcjonarz_for_liturgicalname_list = new List<Lekcjonarz>();
                var Lekcjonarz_for_dropdown_list = new List<Lekcjonarz>();
                var lekcjonarz = new Lekcjonarz();
                if (liturgical_day != null)
                {
                    using (var commandLekcjonarz = sqlConn.CreateCommand())
                    {
                        commandLekcjonarz.CommandText = "SELECT * FROM Lekcjonarz WHERE dzien_liturgiczny=@liturgical_day";
                        commandLekcjonarz.Parameters.AddWithValue("@liturgical_day", liturgical_day);
                        var reader = commandLekcjonarz.ExecuteReader();
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
                        commandLekcjonarz_dropdown.CommandText= "SELECT DISTINCT dzien_liturgiczny FROM Lekcjonarz GROUP BY dzien_liturgiczny ";
                        var reader = commandLekcjonarz_dropdown.ExecuteReader();
                        while(reader.Read())
                        {
                            Lekcjonarz_for_dropdown_list.Add(new Lekcjonarz()
                            {
                                Liturgical_day=reader["dzien_liturgiczny"] as string,
                            });
                            lekcjonarz.Lekcjonarz_list_dropdown = Lekcjonarz_for_dropdown_list;
                        }
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
                    using (var commandLekcjonarz = sqlConn.CreateCommand())
                    {
                        commandLekcjonarz.CommandText = "SELECT * FROM Lekcjonarz WHERE id_llekcjonarz=@ID";
                        commandLekcjonarz.Parameters.AddWithValue("@ID", ID);
                        var reader = commandLekcjonarz.ExecuteReader();
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
              
                    using (var commandLekcjonarz = sqlConn.CreateCommand())
                    {
                        commandLekcjonarz.CommandText = "SELECT DISTINCT dzien_liturgiczny FROM Lekcjonarz GROUP BY dzien_liturgiczny ";
                        var reader = commandLekcjonarz.ExecuteReader();
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

        public Lekcjonarz Insert_Lekcjonarz(Lekcjonarz lekcjonarz)
        {
            using (SqlConnection con = new SqlConnection(connetionString))
            {
                string query = "INSERT INTO Lekcjonarz(dzien_liturgiczny,typ_czytania,siglum,tekst) VALUES(@liturgical_day,@reading_type,@siglum,@text)";
                query += " SELECT SCOPE_IDENTITY()";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.Parameters.AddWithValue("@liturgical_day", lekcjonarz.Liturgical_day);
                    cmd.Parameters.AddWithValue("@reading_type", lekcjonarz.Reading_type);
                    cmd.Parameters.AddWithValue("@siglum", lekcjonarz.Siglum);
                    cmd.Parameters.AddWithValue("@text", lekcjonarz.Text);
                    lekcjonarz.ID_lekcjonarz = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                }
            }
            return lekcjonarz;
        }

        public Lekcjonarz Update_Lekcjonarz(Lekcjonarz lekcjonarz)
        {
            using (SqlConnection con = new SqlConnection(connetionString))
            {
                string query = "UPDATE Lekcjonarz SET dzien_liturgiczny=@liturgical_day,typ_czytania=@reading_type, siglum=@siglum,tekst=@text WHERE id_llekcjonarz=@ID";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.Parameters.AddWithValue("@ID", lekcjonarz.ID_lekcjonarz);
                    cmd.Parameters.AddWithValue("@liturgical_day", lekcjonarz.Liturgical_day);
                    cmd.Parameters.AddWithValue("@reading_type", lekcjonarz.Reading_type);
                    cmd.Parameters.AddWithValue("@siglum", lekcjonarz.Siglum);
                    cmd.Parameters.AddWithValue("@text", lekcjonarz.Text);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return lekcjonarz;
        }

        public Kalendarz Select_Kalendarz()
        {
            {
                using (var sqlConn = new SqlConnection(connetionString))
                {
                    sqlConn.Open();
                    var kalendarz_list = new List<Kalendarz>();
                    var kalendarz_dropdown_liturgicalday_list = new List<Kalendarz>();
                    var kalendarz = new Kalendarz();
                   
                
                        using (var commandLekcjonarz_dropdown = sqlConn.CreateCommand())
                        {
                            commandLekcjonarz_dropdown.CommandText = "SELECT * FROM SlownikDni";
                            var reader = commandLekcjonarz_dropdown.ExecuteReader();
                            while (reader.Read())
                            {
                            kalendarz_dropdown_liturgicalday_list.Add(new Kalendarz()
                                {
                                    Liturgical_day = reader["dzien_liturgiczny"] as string,
                                    Day_name=reader["nazwa_dnia"] as string,
                                });
                                kalendarz.Kalendarz_list_dropdown_liturgical_day= kalendarz_dropdown_liturgicalday_list;
                            }
                        reader.Close();
                        }
                    
                    return kalendarz;
                }
            }
        }



    }
}