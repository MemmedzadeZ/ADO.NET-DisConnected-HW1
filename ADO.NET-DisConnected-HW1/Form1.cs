using Microsoft.Data.SqlClient;
using System.Data;
using System.Xml;

namespace ADO.NET_DisConnected_HW1;

public partial class Exec_Button : Form
{
    DataTable? dataTable = null;
    SqlDataAdapter? dataAdapter = null;
    SqlDataReader? dataReader = null;
    SqlConnection? sqlConnection = null;
    SqlCommand? sqlCommand = null;
    DataSet? dataSet = null;
   
    public Exec_Button()
    {
        InitializeComponent();
        string strcon = "Data Source=DESKTOP-QOMBEIP;Initial Catalog=Library;Integrated Security=True;Connect Timeout=30;" +
            "Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        sqlConnection= new SqlConnection(strcon);
    }

    public void ReadDataTable_Execute()
    {
        try
        {
            sqlConnection?.Open();
            var sqlectQuery = textBox1.Text;
            sqlCommand = new SqlCommand(sqlectQuery, sqlConnection);
            dataReader = sqlCommand.ExecuteReader();
            dataTable = new DataTable();

            bool isCheck = true;
            while(dataReader.Read())
            {
                if(isCheck)
                {
                    for (int i = 0; i < dataReader.FieldCount; i++)
                    {
                        dataTable.Columns.Add(dataReader.GetName(i), dataReader[i].GetType());
                    }
                    isCheck = false;
                }
                DataRow dataRow = dataTable.NewRow();
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    dataRow[i] = dataReader[i];
                }
                dataTable.Rows.Add(dataRow);
            }
            dataGridView1.DataSource = dataTable;



        }
        catch (Exception ex)
        {

            MessageBox.Show(ex.Message);
        }
        finally
        {
            sqlConnection?.Close();

        }
    }

    public void Table_Select()
    {
        try
        {
            sqlConnection?.Open();
            var query = textBox1.Text;
            sqlCommand = new SqlCommand(query, sqlConnection);
        
            dataAdapter = new SqlDataAdapter(sqlCommand);

            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);


            dataGridView1.DataSource = dataSet.Tables[1];
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally
        {
            sqlConnection?.Close();
        }
     
    }


    public void Fill_DisConnectedMode()
    {
       string selectQuentity = textBox1.Text;
        dataAdapter = new SqlDataAdapter(selectQuentity, sqlConnection);
        dataTable = new DataTable();
        dataAdapter.Fill(dataTable);
        dataGridView1.DataSource = dataTable;
    }

    public void update_()
    {
        SqlCommandBuilder sqlCommandBuilder = new SqlCommandBuilder(dataAdapter);
        if(dataTable is not null)
        {
            dataAdapter?.Update(dataTable);
        }
     
    }

    private void button1_Click(object sender, EventArgs e)
    {
        ReadDataTable_Execute();
    }

    private void button2_Click(object sender, EventArgs e)
    {
        Fill_DisConnectedMode();
    }

    private void button3_Click(object sender, EventArgs e)
    {

        update_();

    }

}