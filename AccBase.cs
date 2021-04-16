using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
//using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpForPy
{
    class AccBase : Papa
    {
        static string connectString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source = C:\Users\user\Desktop\ACCESS\db2sharp.accdb";
        static OleDbConnection myConnection;

        static void AccessProcess(string query, string ofName, string header)
        {
            myConnection = new OleDbConnection(connectString);
            myConnection.Open();

            string outText = header;
            //textBox1.Text = "wait...";

            OleDbConnection connection = new OleDbConnection(connectString);
            try
            {
                OleDbCommand command = new OleDbCommand(query, myConnection);
                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string myLine = "";
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        try
                        {
                            if (i < reader.FieldCount - 1) myLine += reader[i].ToString() + ";";
                            else myLine += reader[i].ToString();

                        }
                        catch { }
                    }
                    outText += myLine + "\n";
                }
                //File.WriteAllText(ofName, outText);
                TextToFile(ofName, outText);
                connection.Close();
                //textBox1.Text = "~ " + ofName;

            }
            catch
            {
                if (connection.State == ConnectionState.Open) { connection.Close(); }
                Sos("connect error", query);
            }
            finally { if (connection.State == ConnectionState.Open) { connection.Close(); } }
        }

        public static void AccessAddOtbor(List<string[]> arr)
        {
            myConnection = new OleDbConnection(connectString);
            myConnection.Open();
            string inf = "";

            try
            {
                foreach (var vec in arr)
                {
                    inf = vec[0];
                    string query = String.Format("INSERT INTO Otbor VALUES ({0}, {1})", vec[0], vec[1]);
                    OleDbCommand command = new OleDbCommand(query, myConnection);
                    command.ExecuteNonQuery();
                    pGreen(vec[0]);
                }
            }
            catch
            {
                pRed("err " + inf);
            }
            finally { if (myConnection.State == ConnectionState.Open) { myConnection.Close(); } }
        }

        public static void ClearOtbor()
        {
            myConnection = new OleDbConnection(connectString);
            myConnection.Open();
            //pGreen("Open");

            string query = "DELETE FROM Otbor";
            //string query = String.Format("INSERT INTO Otbor (trem) VALUES (1010200)");
            //textBox1.Text = "wait...";

            try
            {
                OleDbCommand command = new OleDbCommand(query, myConnection);

                command.ExecuteNonQuery();
            }
            catch
            {
                //Sos("connect error", query);
                if (myConnection.State == ConnectionState.Open) { myConnection.Close(); }
            }
            finally { if (myConnection.State == ConnectionState.Open) { myConnection.Close(); } }
        }


        internal static List<List<string>> AccGetData(string query, int minSize = 1)
        {
            myConnection = new OleDbConnection(connectString);
            myConnection.Open();

            List<List<string>> arr = new List<List<string>>();
            //textBox1.Text = "wait...";

            OleDbConnection connection = new OleDbConnection(connectString);
            try
            {
                OleDbCommand command = new OleDbCommand(query, myConnection);
                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    List<string> vec = new List<string>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        try
                        { vec.Add(reader[i].ToString()); }
                        catch { }
                    }
                    if (vec.Count < minSize)
                    {
                        pCyan(String.Format(" BedSize {0}", vec[0]));
                        continue;
                    }

                    arr.Add(vec);
                }
                //File.WriteAllText(ofName, outText);
                //TextToFile(ofName, outText);
                connection.Close();
                //textBox1.Text = "~ " + ofName;

            }
            catch
            {
                if (connection.State == ConnectionState.Open) { connection.Close(); }
                Sos("connect error", query);
            }
            finally { if (connection.State == ConnectionState.Open) { connection.Close(); } }
            return arr;
        }

        public static List<List<string>> AccGetTermData()
        {
            string query = "SELECT * FROM GetTermData;";
            return AccGetData(query);
        }

        public static List<List<string>> AccGetSiteData()
        {
            string query = "SELECT * FROM ACCESS ORDER BY department; ";
            return AccGetData(query, 5);
        }

        public static List<List<string>> AccGetSummuryData()
        {
            string query = "SELECT * FROM Summury ORDER BY department; ";
            return AccGetData(query);
        }

        public static void AccGetTerms()
        {
            string header = "department;termial;model;serial_number;date_manufacture;soft;producer;rne_rro;sealing;fiscal_number;oro_serial;oro_number;ticket_serial;ticket_1sheet;ticket_number;sending;books_arhiv;tickets_arhiv;to_rro;owner_rro\n";
            string query = "SELECT * FROM Терминалы ORDER BY termial; ";
            string ofName = "terminals.csv";
            ofName = System.IO.Path.Combine(dataInPath, ofName);
            AccessProcess(query, ofName, header);
        }

        public static void AccGetDeps()
        {
            string header = "department;region;district_region;district_city;city_type;city;street;street_type;hous;post_index;partner;status;register;edrpou;address;partner_name;id_terminal;koatu;tax_id\n";
            //string query = "SELECT * FROM Отделения WHERE Отделения.partner <> "+ "intime" + "; ";
            string query = "SELECT * FROM Отделения ORDER BY Отделения.department";
            string ofName = "departments.csv";
            ofName = System.IO.Path.Combine(dataInPath, ofName);
            AccessProcess(query, ofName, header);
        }

        public static void AccGetAccess()
        {
            string header = "";
            string query = "SELECT * FROM ACCESS ORDER BY department; ";
            string ofName = "access.csv";
            ofName = System.IO.Path.Combine(dataInPath, ofName);
            AccessProcess(query, ofName, header);
        }

        public static void AccGetVsyo()
        {
            string header = "Терминалы.department;termial;model;serial_number;date_manufacture;soft;producer;rne_rro;sealing;fiscal_number;oro_serial;oro_number;ticket_serial;ticket_1sheet;ticket_number;sending;books_arhiv;tickets_arhiv;to_rro;owner_rro;Отделения.department;region;district_region;city_type;city;district_city;street_type;street;hous;post_index;partner;status;register;edrpou;address;partner name;id_terminal;koatu;tax_id\n";
            string query = "SELECT Терминалы.department, Терминалы.termial, Терминалы.model, Терминалы.serial_number, Терминалы.date_manufacture, Терминалы.soft, Терминалы.producer, Терминалы.rne_rro, Терминалы.sealing, Терминалы.fiscal_number, Терминалы.oro_serial, Терминалы.oro_number, Терминалы.ticket_serial, Терминалы.ticket_1sheet, Терминалы.ticket_number, Терминалы.sending, Терминалы.books_arhiv, Терминалы.tickets_arhiv, Терминалы.to_rro, Терминалы.owner_rro, Отделения.department, Отделения.region, Отделения.district_region, Отделения.city_type, Отделения.city, Отделения.district_city, Отделения.street_type, Отделения.street, Отделения.hous, Отделения.post_index, Отделения.partner, Отделения.status, Отделения.register, Отделения.edrpou, Отделения.address, Отделения.[partner name], Отделения.id_terminal, Отделения.koatu, Отделения.tax_id FROM Терминалы INNER JOIN Отделения ON Терминалы.department = Отделения.department ORDER BY Терминалы.termial;";
            string ofName = Path.Combine(dataInPath, "vsyo_zapros.csv");
            AccessProcess(query, ofName, header);
        }

        public static void AccGetAll()
        {
            AccGetTerms();
            AccGetDeps();
            AccGetAccess();
            AccGetVsyo();
        }
    }
}
