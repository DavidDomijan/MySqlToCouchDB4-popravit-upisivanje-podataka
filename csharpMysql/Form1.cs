using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MyCouch;
using System.Threading;

namespace csharpMysql
{    
    public partial class Form1 : Form
    {
        string nazivBaze="test";
        string revision;
        List<string> k = new List<string>();
        
        string strId,
            strIdBuilding,
            strTM,
            strLogID,
            strLogCaption,
            strLogMessage,
            strUserType,
            strUserName,
            strRFID,
            strKeyOptions,
            strRoomName,
            strCtrlAddr,
            strCmdSrc,
            strSignedInType,
            strSignedInName,
            strDuration;

       

        public Form1()
        {
            InitializeComponent();

            
        }

        

        MySqlConnection connection;
         private void Database_Load()
        {
            
            //ucitavanje mysql podataka i prebacivanje u listu stringova
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                


                string connectionstring = "datasource=localhost;username=gost;password=gost;pooling=true;"; 
                    //"datasource=localhost;port=8087;username=gost;password=gost";
                connection = new MySqlConnection(connectionstring);

                connection.Open();

                //sql query
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM iroomsweb.log limit 1,100000", connection);


                DataSet ds = new DataSet();
                adapter.Fill(ds, "users"); 
                
                
               
                StringBuilder output = new StringBuilder();
                foreach (DataRow rows in ds.Tables[0].Rows)
                {
                    foreach (DataColumn col in ds.Tables[0].Columns)
                    {
                        output.AppendFormat("{0} ", rows[col]);
                    }

                    output.AppendLine();
                }
                
                string [] lines = output.ToString().Split(Environment.NewLine.ToCharArray());
                foreach (string l in lines) {if(l!=""&&l!=null) k.Add(l); }

                
                richTextBox1.Text = output.ToString();
                
                
                connection.Close();
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        

        private void Button3_Click_1(object sender, EventArgs e)
        {
            Database_Load();
            
            
            try
            {

                using (var client = new MyCouchClient("http://127.0.0.1:5984", nazivBaze))
                {
                //dobivanje revisiona potrebnog za update couchdb-a
                var returnRev = client.Entities.GetAsync<CouchDBData>(strId);
                var rev = (returnRev.Result.Content.Rev);
                client.Entities.DeleteAsync(rev);
                revision = rev;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Button4_Click(object sender, EventArgs e)
        {
            

            Cursor.Current = Cursors.WaitCursor;
            foreach (string l in k ) {
                //podjela liste na zasebne podatke
                 strId = l.Substring(0,l.IndexOf(" "));
                 strIdBuilding = l.Substring(l.IndexOf(" ")+1);
                 strTM = strIdBuilding.Substring(strIdBuilding.IndexOf(" ")+1);
                 strLogID = strTM.Substring(strTM.IndexOf(" ")+1);
                 strLogCaption = strLogID.Substring(strLogID.IndexOf(" ")+1);
                 strLogMessage = strLogCaption.Substring(strLogCaption.IndexOf(" ")+1);
                 strUserType = strLogMessage.Substring(strLogMessage.IndexOf(" ")+1);
                 strUserName = strUserType.Substring(strUserType.IndexOf(" ")+1);
                 strRFID = strUserName.Substring(strUserName.IndexOf(" ")+1);
                 strKeyOptions = strRFID.Substring(strUserName.IndexOf(" ")+1);
                 strRoomName = strKeyOptions.Substring(strRFID.IndexOf(" ")+1);
                 strCtrlAddr = strRoomName.Substring(strRoomName.IndexOf(" ")+1);
                 strCmdSrc = strCtrlAddr.Substring(strCtrlAddr.IndexOf(" ")+1);
                 strSignedInType = strCmdSrc.Substring(strCmdSrc.IndexOf(" ")+1);
                 strSignedInName = strSignedInType.Substring(strSignedInType.IndexOf(" ")+1);
                 strDuration = strSignedInName.Substring(strSignedInName.IndexOf(" ")+1);


                if (strIdBuilding.IndexOf(" ") >= 0) strIdBuilding = strIdBuilding.Substring(0, strIdBuilding.IndexOf(" "));
                if (strTM.IndexOf(" ") >= 0) strTM = strTM.Substring(0,strTM.IndexOf(" "));
                if (strLogID.IndexOf(" ") >= 0) strLogID = strLogID.Substring(0, strLogID.IndexOf(" "));
                if (strLogCaption.IndexOf(" ") >= 0) strLogCaption = strLogCaption.Substring(0, strLogCaption.IndexOf(" "));
                if (strLogMessage.IndexOf(" ") >= 0) strLogMessage = strLogMessage.Substring(0, strLogMessage.IndexOf(" "));
                if (strUserType.IndexOf(" ") >= 0) strUserType = strUserType.Substring(0,strUserType.IndexOf(" "));
                if (strUserName.IndexOf(" ") >= 0) strUserName = strUserName.Substring(0,strUserName.IndexOf(" "));
                if (strRFID.IndexOf(" ") >= 0) strRFID = strRFID.Substring(0,strRFID.IndexOf(" "));
                if (strKeyOptions.IndexOf(" ") >= 0) strKeyOptions = strKeyOptions.Substring(0,strKeyOptions.IndexOf(" "));
                if (strRoomName.IndexOf(" ") >= 0) strRoomName = strRoomName.Substring(0,strRoomName.IndexOf(" "));
                if (strCtrlAddr.IndexOf(" ") >= 0) strCtrlAddr = strCtrlAddr.Substring(0,strCtrlAddr.IndexOf(" "));
                if (strCmdSrc.IndexOf(" ") >= 0) strCmdSrc = strCmdSrc.Substring(0,strCmdSrc.IndexOf(" "));
                if (strSignedInType.IndexOf(" ") >= 0) strSignedInType = strSignedInType.Substring(0,strSignedInType.IndexOf(" "));
                if (strSignedInName.IndexOf(" ") >= 0) strSignedInName = strSignedInName.Substring(0,strSignedInName.IndexOf(" "));
                if(strDuration.IndexOf(" ")>=0)strDuration = strDuration.Substring(0,strDuration.IndexOf(" "));
               
                
               using (var client = new MyCouchClient("http://127.0.0.1:5984", nazivBaze))
               {
                //dobivanje revisiona potrebnog za update couchdb-a 
                    try
                    {
                        var returnRev = client.Entities.GetAsync<CouchDBData>(strId);
                        var rev = (returnRev.Result.Content.Rev);
                        client.Entities.DeleteAsync(rev);
                        revision = rev;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                   
                    //upisivanje u couchdb
                    client.Entities.PutAsync(new CouchDBData()
                    {
                    Id = strId,
                    IdBuilding =strIdBuilding ,
                    TM=strTM,
                    LogID=strLogID,
                    LogCaption=strLogCaption,

                    Rev =revision
                    }).Wait();
               }
            }
            Cursor.Current = Cursors.Default;
        }

         private void ButtonStop_Click(object sender, EventArgs e)
        {
            
        }
    }
}
