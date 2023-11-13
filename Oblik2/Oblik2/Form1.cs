using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oblik2;
using Oblik2.AccountDataSetTableAdapters;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Oblik2
{
    public partial class Облік : Form
    {
        public Облік()
        {
            InitializeComponent();
        }

        private void Облік_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'accountDataSet.Worker' table. You can move, or remove it, as needed.
            this.workerTableAdapter.Fill(this.accountDataSet.Worker);
            // TODO: This line of code loads data into the 'accountDataSet.Account' table. You can move, or remove it, as needed.
            this.accountTableAdapter.Fill(this.accountDataSet.Account);

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAddWorker f = new FormAddWorker(); // створити форму

            if (f.ShowDialog() == DialogResult.OK) // відобразити форму
            {
                // якщо OK, то додати працівника
                string WName, WPosition, WSex;

                WName = f.textBox1.Text;
                WPosition = f.textBox2.Text;
                WSex = f.comboBox1.Items[f.comboBox1.SelectedIndex].ToString();

                // працює
                this.workerTableAdapter.Insert(WName, WPosition, WSex); // вставка
                this.workerTableAdapter.Fill(this.accountDataSet.Worker); // відображення
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Worker => Delete
            FormDelWorker f = new FormDelWorker(); // створити форму
            int id_worker;
            string WName, WPosition, WSex;
            int index;

            // взяти номер поточного (виділеного) рядка в dataGridView1
            index = dataGridView1.CurrentRow.Index;

            // заповнити внутрішні змінні з поточного рядка dataGridView1
            id_worker = Convert.ToInt32(dataGridView1[0, index].Value);
            WName = Convert.ToString(dataGridView1[1, index].Value);
            WPosition = Convert.ToString(dataGridView1[2, index].Value);
            WSex = Convert.ToString(dataGridView1[3, index].Value);

            // сформувати інформаційний рядок
            f.label2.Text = WName + " " + WPosition;

            if (f.ShowDialog() == DialogResult.OK)
            {
                workerTableAdapter.Delete(id_worker, WName, WPosition, WSex); // метод Delete
                this.workerTableAdapter.Fill(this.accountDataSet.Worker);
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Worker => Edit...
            FormEditWorker f = new FormEditWorker(); // створити форму
            int index;
            string WName, WPosition, WSex;
            int id_worker;

            if (dataGridView1.RowCount <= 1) return;

            // отримати позицію виділеного рядка в dataGridView1
            index = dataGridView1.CurrentRow.Index;

            if (index == dataGridView1.RowCount - 1) return; //

            // отримати дані рядка
            id_worker = (int)dataGridView1.Rows[index].Cells[0].Value;
            WName = (string)dataGridView1.Rows[index].Cells[1].Value;
            WPosition = (string)dataGridView1.Rows[index].Cells[2].Value;
            WSex = (string)dataGridView1.Rows[index].Cells[3].Value;

            // заповнити поля форми f
            f.textBox1.Text = WName;
            f.textBox2.Text = WPosition;

            if (WSex == "M") f.comboBox1.SelectedIndex = 0;
            else f.comboBox1.SelectedIndex = 1;

            if (f.ShowDialog() == DialogResult.OK) // викликати форму FormEditWorker
            {
                string nWName, nWPosition, nWSex;

                // отримати нові (змінені) значення з форми
                nWName = f.textBox1.Text;
                nWPosition = f.textBox2.Text;
                nWSex = f.comboBox1.Items[f.comboBox1.SelectedIndex].ToString();

                // змінити в адаптері
                this.workerTableAdapter.Update(nWName, nWPosition, nWSex, id_worker, WName, WPosition, WSex);
                this.workerTableAdapter.Fill(this.accountDataSet.Worker);
            }
        }

        private void addToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // Inventory => Add...
            FormAddAccount f = new FormAddAccount(); // вікно

            if (f.ShowDialog() == DialogResult.OK)
            {
                int Id_Worker;
                int index;
                string ANum, ADate, AInvNum, AObjName;
                short ACount;
                double APrice;

                // взяти значення ID_Worker з таблиці Worker
                index = dataGridView1.CurrentRow.Index; // позиція в dataGridView1
                Id_Worker = (int)dataGridView1.Rows[index].Cells[0].Value;

                // взяти значення інших полів з форми FormAddAccount
                ANum = f.textBox1.Text;
                ADate = f.textBox2.Text;
                AInvNum = f.textBox3.Text;
                AObjName = f.textBox4.Text;
                ACount = Convert.ToInt16(f.textBox5.Text);
                APrice = Convert.ToDouble(f.textBox6.Text);

                this.accountTableAdapter.Insert(Id_Worker, ANum, ADate, AInvNum, AObjName, ACount, APrice);
                this.accountTableAdapter.Fill(this.accountDataSet.Account);
            }
        }

        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // Inventory => Delete
            FormDelAccount f = new FormDelAccount();
            int id_account, id_worker;
            string ANum, ADate, AInvNum, AObjName;
            short ACount;
            double APrice;
            int index;

            // взяти індекс виділеного (поточного) рядка в dataGridView2
            index = dataGridView2.CurrentRow.Index;

            // взяти значення полів рядка з номером index
            id_account = Convert.ToInt32(dataGridView2[0, index].Value);
            id_worker = Convert.ToInt32(dataGridView2[1, index].Value);
            ANum = Convert.ToString(dataGridView2[2, index].Value);
            ADate = Convert.ToString(dataGridView2[3, index].Value);
            AInvNum = Convert.ToString(dataGridView2[4, index].Value);
            AObjName = Convert.ToString(dataGridView2[5, index].Value);
            ACount = Convert.ToInt16(dataGridView2[6, index].Value);
            APrice = Convert.ToDouble(dataGridView2[7, index].Value);

            // сформувати інформаційний рядок у вікні FormDelAccount
            f.label2.Text = ANum + " / " + ADate + " / " + AInvNum + " / " +
            AObjName + " / " + ACount + " / " + APrice;

            if (f.ShowDialog() == DialogResult.OK) // вивести вікно
            {
                this.accountTableAdapter.Delete(id_account, id_worker,
                ANum, ADate, AInvNum, AObjName, ACount, APrice); // видалити рядок
                this.accountTableAdapter.Fill(this.accountDataSet.Account); // зафіксувати зміни
            }
        }

        private void editToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // Inventory => Edit...
            FormEditAccount f = new FormEditAccount();
            int index;
            int id_account;
            int id_worker;
            string ANum, ADate, AInvNum, AObjName;
            short ACount;
            double APrice;

            if (dataGridView2.RowCount <= 1) return;

            // взяти номер поточного рядка в dataGridView2
            index = dataGridView2.CurrentRow.Index;

            if (index == dataGridView2.RowCount - 1) return; //

            // отримати дані рядка
            id_account = (int)dataGridView2.Rows[index].Cells[0].Value;
            id_worker = (int)dataGridView2.Rows[index].Cells[1].Value;
            ANum = (string)dataGridView2.Rows[index].Cells[2].Value;
            ADate = (string)dataGridView2.Rows[index].Cells[3].Value;
            AInvNum = (string)dataGridView2.Rows[index].Cells[4].Value;
            AObjName = (string)dataGridView2.Rows[index].Cells[5].Value;
            ACount = (short)dataGridView2.Rows[index].Cells[6].Value;
            APrice = (double)dataGridView2.Rows[index].Cells[7].Value;

            // заповнити даними рядка поля у FormEditAccount
            f.textBox1.Text = ANum;
            f.textBox2.Text = ADate;
            f.textBox3.Text = AInvNum;
            f.textBox4.Text = AObjName;
            f.textBox5.Text = Convert.ToString(ACount);
            f.textBox6.Text = Convert.ToString(APrice);

            // заповнити поля форми FormEditAccount
            if (f.ShowDialog() == DialogResult.OK) // викликати форму
            {
                // нові значення рядка
                string nANum, nADate, nAInvNum, nAObjName;
                short nACount;
                double nAPrice;

                // взяти нові значення
                nANum = f.textBox1.Text;
                nADate = f.textBox2.Text;
                nAInvNum = f.textBox3.Text;
                nAObjName = f.textBox4.Text;
                nACount = Convert.ToInt16(f.textBox5.Text);
                nAPrice = Convert.ToDouble(f.textBox6.Text);

                // оновити дані в рядку
                this.accountTableAdapter.Update(id_worker, nANum, nADate, nAInvNum, nAObjName, nACount, nAPrice,
                      id_account, id_worker, ANum, ADate, AInvNum, AObjName, ACount, APrice);
                this.accountTableAdapter.Fill(this.accountDataSet.Account);
            }
        }

        // фільтр у textBox1
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            workerBindingSource.Filter = "WName LIKE '" + textBox1.Text + "%'";
        }

        // Вихід
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FormAddWorker_Load(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            comboBox1.SelectedIndex = 1;
            textBox1.Focus(); // при відкритті вікна, textBox1 отримує фокус
        }

        private void FormEditWorker_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        private void FormAddAccount_Load(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox1.Focus();
        }

        private void FormEditAccount_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            // створити фільтр, який виведе в dataGridView2 те що потрібно
            int id;
            int index;

            index = dataGridView1.CurrentRow.Index;

            // взяти значення id = Worker.ID_Worker
            id = (int)dataGridView1.Rows[index].Cells[0].Value;
            accountBindingSource.Filter = "ID_Worker = " + id.ToString();
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            // створити фільтр, який виведе в dataGridView2 те що потрібно
            int id;
            int index;

            index = dataGridView1.CurrentRow.Index;

            // взяти значення id = Worker.ID_Worker
            id = (int)dataGridView1.Rows[index].Cells[0].Value;
            accountBindingSource.Filter = "ID_Worker = " + id.ToString();
        }

        internal class FormAddAccount
        {
            internal DialogResult ShowDialog()
            {
                throw new NotImplementedException();
            }
        }

    }

    
}


