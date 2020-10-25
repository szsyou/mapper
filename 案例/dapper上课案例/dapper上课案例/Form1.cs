using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dapper上课案例
{
    public partial class Form1 : Form
    {

        //MySqlDAL dal = new MySqlDAL();//数据操作类实例化-->mysql
        SqlDAL dal = new SqlDAL();//实例化sql

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataBind();
        }

        //数据绑定
        private void DataBind()
        {
            this.dataGridView1.DataSource = dal.GetList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Account account = new Account();
            account.Name = "何进";
            account.Rest = 12800;
            dal.Add(account);
            DataBind();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<Account> lst = new List<Account>()
            {
                new Account{Name="张三",Rest=800},
                new Account{Name="王武",Rest=600}
            };
            dal.Add(lst);
            DataBind();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Account account = new Account();
            account.Id = 4;
            account.Name = "王二小";
            account.Rest = 10;
            dal.Update(account);
            DataBind();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            dal.Del(int.Parse(this.txtDelID.Text));
            DataBind();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.lblEmpCount.Text = dal.GetEmpCount().ToString();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Account ac = dal.GetAccountInfoByID(int.Parse(this.txtID.Text));
            this.lblName.Text = ac.Name;
            this.lblRest.Text = ac.Rest.ToString();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = dal.GetAccountList();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            List<Account> accounts;
            List<Emp> emps;
            dal.GetMultiList(out accounts, out emps);
            this.dataGridView1.DataSource = accounts;
            this.dataGridView2.DataSource = emps;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Account ac = new Account();
            ac.Name = "Hellen";
            ac.Rest = 9000;
            dal.AddByProc(ac);
            DataBind();//重绑数据
        }

        private void button11_Click(object sender, EventArgs e)
        {
            string content = this.txtContent.Text.Trim();
            int recordCount;
            IList<Account> lst = dal.GetAccountListByProc(content,out recordCount);
            this.dataGridView1.DataSource = lst;
            this.lblAccountCount.Text = recordCount.ToString();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            dal.ABCount();
            DataBind();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            //插入记录
            dal.InsertRecordAsync();
            //绑数据源
            this.dataGridView1.DataSource = dal.GetAccountList();
        }
    }
}
