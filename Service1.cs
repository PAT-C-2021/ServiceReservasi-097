using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ServiceReservasi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class Service1 : IService1
    {
        string koneksi = "Data Source=MSI;Initial Catalog=WCFReservasi;Integrated Security=True";
        SqlConnection conn;
        SqlCommand comm;

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public List<DetailLokasi> DetailLokasi()
        {
            List<DetailLokasi> LokasiFull = new List<DetailLokasi>();
            try
            {
                string sql = "select ID_Lokasi, Nama_Lokasi, Deskripsi_Full, Kuota from dbo.Lokasi";
                conn = new SqlConnection(koneksi);
                comm = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    DetailLokasi data = new DetailLokasi();
                    data.IDLokasi = reader.GetString(0);
                    data.NamaLokasi = reader.GetString(1);
                    data.DeskripsiFull = reader.GetString(2);
                    data.Kuota = reader.GetInt32(3);
                    LokasiFull.Add(data);

                }conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return LokasiFull;
        }

        public List<Pemesanan> Pemesanan()
        {
            List<Pemesanan> pemesanans = new List<Pemesanan>();
            try
            {
                string sql = "select ID_Reservasi, Nama_Customer, No_Telpon, Jumlah_Pemesanan, Nama_Lokasi from dbo.Pemesanan p join dbo.Lokasi l on l.ID_Lokasi = p.ID_Lokasi";
                conn = new SqlConnection(koneksi);
                comm = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    Pemesanan data = new Pemesanan();
                    data.IDPemesanan = reader.GetString(0);
                    data.NamaCustomer = reader.GetString(1);
                    data.NoTelepon = reader.GetString(2);
                    data.JumlahPemesanan = reader.GetInt32(3);
                    data.Lokasi = reader.GetString(4);
                    pemesanans.Add(data);

                }
                conn.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            return pemesanans;
        }

        public string pemesanan(string IDPemesanan,  string NamaCustomer, string NoTelpon, int JumlahPemesanan, string IDLokasi)
        {
            string n = "gagal";
            try
            {
                string sql = "INSERT INTO dbo.Pemesanan VALUES('" + IDPemesanan + "','" + NamaCustomer + "','" + NoTelpon + "'," + JumlahPemesanan + ",'" + IDLokasi + "')";
                conn = new SqlConnection(koneksi);
                comm = new SqlCommand(sql, conn);
                conn.Open();
                comm.ExecuteNonQuery();
                conn.Close();

                string sql2 = "UPDATE dbo.Lokasi set Kuota = Kuota - "+JumlahPemesanan+" WHERE ID_lokasi = '"+IDLokasi+"' ";
                conn = new SqlConnection(koneksi);
                comm = new SqlCommand(sql2, conn);
                conn.Open();
                comm.ExecuteNonQuery();
                conn.Close();

                n = "Berhasil";

            }catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            return n;
        }

        public string editPemesanan(string IDPemesanan, string NamaCustomer, string No_telpon)
        {
            string n = "gagal";
            try
            {
                string sql = "UPDATE dbo.Pemesanan SET Nama_customer = '" + NamaCustomer + "', No_telpon = '" + No_telpon + "' WHERE ID_reservasi = '" + IDPemesanan + "' ";
                conn = new SqlConnection(koneksi);
                comm = new SqlCommand(sql, conn);
                conn.Open();
                comm.ExecuteNonQuery();
                conn.Close();

                n = "Berhasil";
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            return n;
        }

        public string deletePemesanan(string IDPemesanan)
        {
            string n = "gagal";
            try
            {
                string sql = "DELETE FROM dbo.Pemesanan WHERE ID_reservasi = '" + IDPemesanan + "' ";
                conn = new SqlConnection(koneksi);
                comm = new SqlCommand(sql, conn);
                conn.Open();
                comm.ExecuteNonQuery();
                conn.Close();

                n = "Berhasil";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return n;
        }

        public List<CekLokasi> ReviewLokasi()
        {
            throw new NotImplementedException();
        }

        public string Login(string username, string password)
        {
            string kategori = "";

            string sql = "select Kategori from Login where Username= '" + username + "' and Password='" + password + "'";
            conn = new SqlConnection(koneksi);
            comm = new SqlCommand(sql, conn);
            conn.Open();
            SqlDataReader reader = comm.ExecuteReader();
            while (reader.Read())
            {
                kategori = reader.GetString(0);
            }
            return kategori;
        }

        public string Register(string username, string password, string kategori)
        {
            string n = "gagal";
            try
            {
                string sql = "INSERT INTO Login VALUES('" + username + "','" + password + "','" + kategori + "')";
                conn = new SqlConnection(koneksi);
                comm = new SqlCommand(sql, conn);
                conn.Open();
                comm.ExecuteNonQuery();
                conn.Close();

                n = "Berhasil";

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return n;
        }

        public string UpdateRegister(string username, string password, string kategori, int id)
        {
            string n = "gagal";
            try
            {
                string sql = "UPDATE Login SET Username = '" + username + "', Password = '" + password + "', kategori = '"+kategori+"' Where ID_login = "+id+"";
                conn = new SqlConnection(koneksi);
                comm = new SqlCommand(sql, conn);
                conn.Open();
                comm.ExecuteNonQuery();
                conn.Close();

                n = "Berhasil";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return n;
        }

        public string DeleteRegister(string username)
        {
            string n = "gagal";
            try
            {
                int id = 0;
                string sql = "SELECT ID_login FROM dbo.Login WHERE Username = '" + username + "' ";
                conn = new SqlConnection(koneksi);
                comm = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    id = reader.GetInt32(0);
                }
                conn.Close();

                string sql2 = "DELETE FROM dbo.Login WHERE ID_login=" + id + "";
                conn = new SqlConnection(koneksi);
                comm = new SqlCommand(sql2, conn);
                conn.Open();
                comm.ExecuteNonQuery();
                conn.Close();

                n = "Berhasil";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return n;
        }

        public List<DataRegister> DataRegist()
        {
            List<DataRegister> list = new List<DataRegister>();
            try
            {
                string sql = "SELECT ID_login, Username, Password, Kategori from Login";
                conn = new SqlConnection(koneksi);
                comm = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    DataRegister data = new DataRegister();
                    data.id = reader.GetInt32(0);
                    data.username = reader.GetString(1);
                    data.password = reader.GetString(2);
                    data.kategori = reader.GetString(3);
                    list.Add(data);

                }
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return list;
        }
    }
}
