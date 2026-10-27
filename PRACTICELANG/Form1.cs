using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PRACTICELANG
{
	public partial class Form1 : Form
	{
		//variables
		private double basicpay, rateofhr, payday, overtime, overtimehrs, absences, noabsences, tardy, notardy, gross, honor, hdmf, sss_contrib, phil_contrib, deduction, netpay;

		private void button1_Click(object sender, EventArgs e)
		{
			string searchValue = txtSearch.Text.Trim();

			for (int i = 0; i < dataGridView1.Rows.Count; i++)
			{
				if (dataGridView1.Rows[i].IsNewRow)
					continue;

				string employeeId = dataGridView1.Rows[i].Cells["Department"].Value?.ToString() ?? "";

				if (employeeId.Equals(searchValue, StringComparison.OrdinalIgnoreCase))
				{
					dataGridView1.ClearSelection();
					dataGridView1.Rows[i].Selected = true;
					dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells["Department"];

				}
			}

		}

		private void button2_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void saveBtn_Click(object sender, EventArgs e)
		{
			//DATABASE 
			connection.Open();

			sql = "INSERT INTO quizTbl (CompanyName, Department,EmployeeCode, Cutoff, EmployeeName, PayPeriod, BasicPay, Overtime, Absences, Honorarium, Tardy, GrossEarning, TotalDeduction, NetPay) VALUES ('" + companynameTxtbox.Text + "','" + departmentTxtbox.Text + "','" + employeecodeTxtbox.Text + "','" + cutoffTxtbox.Text + "','" + employeenameTxtbox.Text + "','" + payperiodTxtbox.Text + 
			"','" + basicpayTxtbox.Text + "','" + overtimeTxtbox.Text + "','" + absencesTxtbox.Text + "','" + honorariumTxtbox.Text + "','" + tardyTxtbox.Text + "','" + grossearningsTxtbox.Text + "','" + deductionsTxtbox.Text + "','" + netpayTxtbox.Text + "')";
			command = new SqlCommand(sql, connection);

			command.CommandType = CommandType.Text;
			adaptersql = new SqlDataAdapter();
			adaptersql.InsertCommand = command;
			command.ExecuteNonQuery();

			sql = "SELECT * FROM quizTbl";
			command = new SqlCommand(sql, connection);
			command.CommandType = CommandType.Text;
			adaptersql = new SqlDataAdapter();
			adaptersql.SelectCommand = command;
			command.ExecuteNonQuery();
			dset = new DataSet();
			adaptersql.Fill(dset, "quizTbl");
			dataGridView1.DataSource = dset.Tables[0];

			dset = new DataSet();
			adaptersql.Fill(dset, "quizTbl");

			companynameTxtbox.Clear();
			employeecodeTxtbox.Clear();
			employeenameTxtbox.Clear();
			departmentTxtbox.Clear();
			cutoffTxtbox.Clear();
			payperiodTxtbox.Clear();
			honorariumTxtbox.Clear();
			rateperhrTxtbox.Clear();
			hoursperdayTxtbox.Clear();
			overtimehrsTxtbox.Clear();
			noabsencesTxtbox.Clear();
			nooftardyTxtbox.Clear();

			connection.Close();
		}
		//stringdatabase
		String connectionString = null;
		SqlConnection connection;
		SqlCommand command;
		DataSet dset;
		SqlDataAdapter adaptersql;
		string sql = null;

		private void basicpayTxtbox_TextChanged(object sender, EventArgs e)
		{

		}

		private void calculateBtn_Click(object sender, EventArgs e)
		{
			//basic pay
			rateofhr = Convert.ToDouble(rateperhrTxtbox.Text); //inputs
			payday = Convert.ToDouble(hoursperdayTxtbox.Text); //inputs
			basicpay = (rateofhr * payday); //calculation
			basicpayTxtbox.Text = basicpay.ToString("c"); //displaying the result in the textbox(OUTPUTS)

			//overtime
			overtimehrs = Convert.ToDouble(overtimehrsTxtbox.Text);
			overtime = (overtimehrs * rateofhr);
			overtimeTxtbox.Text = overtime.ToString("c");

			//absences
			noabsences = Convert.ToDouble(noabsencesTxtbox.Text);
			absences = (noabsences * rateofhr);
			absencesTxtbox.Text = absences.ToString("c");

			//tardiness
			notardy = Convert.ToDouble(nooftardyTxtbox.Text);
			tardy = (notardy * rateofhr);
			tardyTxtbox.Text = tardy.ToString("c");

			//Grossearning
			honor = Convert.ToDouble(honorariumTxtbox.Text);
			gross = (basicpay + overtime + honor);
			grossearningsTxtbox.Text = gross.ToString("c");
			//SSS/philhealth(IF/ELSE/IF)
			if (gross > 0 && gross <= 20000)
			{
				sss_contrib = 125.75;
			}
			else if (gross > 20001 && gross <= 50000)
			{
				sss_contrib = 2200.50;
			}
			else if (gross > 50001 && gross <= 75008)
			{
				sss_contrib = 4800;
			}
			else
			{
				sss_contrib = 5800;
			}
			if (gross > 0 && gross <= 20000)
			{
				phil_contrib = 125.75;
			}
			else if (gross > 20001 && gross <= 50000)
			{
				phil_contrib = 2200.50;
			}
			else if (gross > 50001 && gross <= 75000)
			{
				phil_contrib = 4800;
			}
			else
			{
				phil_contrib = 5800;
			}
			//DEDUCTION
			deduction = (absences + tardy + sss_contrib + phil_contrib + hdmf);
			deductionsTxtbox.Text = deduction.ToString("c");
			sssTxtbox.Text = sss_contrib.ToString("c");
			philhealthTxtbox.Text = phil_contrib.ToString("c");
			//netpay
			netpay = (gross - deduction);
			netpayTxtbox.Text = netpay.ToString("c");


		}

		public Form1()
		{

			InitializeComponent();
			//DATABASE CONNECTION
			// connectionString = "Data Source=C203-06; Initial Catalog=MidtermDB; user id=SA; password=B1Admin123@; TrustServerCertificate=True";
			//connectionString = "Data Source=C203-06; Initial Catalog=MidtermDB; user id=SA; password=B1Admin123@; TrustServerCertificate=True";
			connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=MidtermDB;Integrated Security=True;";
			connection = new SqlConnection(connectionString);
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			//fixed constant
			hdmf = 100;
			hdmfTxtbox.Text = hdmf.ToString("c");

			connection.Open();
			sql = "SELECT * FROM quizTbl";
			command = new SqlCommand(sql, connection);
			command.CommandType = CommandType.Text;
			adaptersql = new SqlDataAdapter();
			adaptersql.SelectCommand = command;
			command.ExecuteNonQuery();
			dset = new DataSet();
			adaptersql.Fill(dset, "quizTbl");
			dataGridView1.DataSource = dset.Tables[0];

			companynameTxtbox.Clear();
			employeecodeTxtbox.Clear();
			employeenameTxtbox.Clear();
			departmentTxtbox.Clear();
			cutoffTxtbox.Clear();
			payperiodTxtbox.Clear();
			honorariumTxtbox.Clear();
			rateperhrTxtbox.Clear();
			hoursperdayTxtbox.Clear();
			overtimehrsTxtbox.Clear();
			noabsencesTxtbox.Clear();
			nooftardyTxtbox.Clear();

			connection.Close();

		}
	}
}
