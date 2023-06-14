using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kyrs_OOP_CSharp.repository;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using TextBox = System.Windows.Forms.TextBox;

namespace Kyrs_OOP_CSharp
{
    public partial class CustomersDBForm : Form
    {
        private CustomerRepository CustomerRepository;
        private CompRepository CompRepository;
        private string FilePathDB;
        private int idChange = 1;
        private int indRow = 0;
        public CustomersDBForm(string filePath)
        {
            InitializeComponent();
            CustomerRepository = new CustomerRepository(filePath);
            CompRepository = new CompRepository(filePath);
            FilePathDB = filePath;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void CustomersDBForm_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            dateTimePicker2.Value = dateTimePicker2.Value.AddDays(1);
            CustomerRepository.GetAllCustomers(dataGridView1);
            dataGridView1.ClearSelection();
            textBox6.Text = $"{dataGridView1.Rows.Count - 1}";


            var arrayAuthors = CompRepository.GetAllPlaces().ToArray();

            if (arrayAuthors.Length != 0)
            {
                button1.Enabled = true;
                comboBox3.Items.AddRange(arrayAuthors);
                comboBox3.SelectedIndex = 0;
            }
            else
            {
                button1.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                MessageBox.Show("Введите имя участника!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (textBox3.Text == "")
            {
                MessageBox.Show("Введите фамилию участника!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (textBox4.Text == "")
            {
                MessageBox.Show("Введите номер участника!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string customerName, customerSurname, customerNumber, outDate, inDate, author, compName;
            string[] authorNameSurname;
            int compId;
            Customer customer;
            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    button4.Text = "Показать завершивших";
                    customerName = textBox2.Text;
                    customerSurname = textBox3.Text;
                    customerNumber = textBox4.Text;
                    inDate = dateTimePicker1.Value.ToString("dd-MM-yyyy");
                    outDate = dateTimePicker2.Value.ToString("dd-MM-yyyy");
                    author = comboBox3.Text;
                    compName = comboBox4.Text;
                    authorNameSurname = author.Split(" ");
                    compId = CompRepository.GetComp(compName, authorNameSurname[0], authorNameSurname[1]);

                    if (dateTimePicker2.Value < dateTimePicker1.Value)
                    {
                        MessageBox.Show("Дата завершения должна быть позже даты участия!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (CustomerRepository.GetCustomer(customerName, customerSurname, customerNumber, compId) != -1)
                    {
                        MessageBox.Show("Данный участник уже записан на такой турнир", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    customer = new Customer(customerName, customerSurname, customerNumber, compId, inDate, outDate);
                    CustomerRepository.SaveCustomer(customer);
                    CustomerRepository.GetAllCustomers(dataGridView1);
                    dataGridView1.ClearSelection();
                    textBox6.Text = $"{dataGridView1.Rows.Count - 1}";
                    dataGridView1.Rows[dataGridView1.Rows.Count - 2].Selected = true;
                    MessageBox.Show("Новый участник добавлен", "Учет участников", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case 1:
                    button4.Text = "Показать завершивших";
                    customerName = textBox2.Text;
                    customerSurname = textBox3.Text;
                    customerNumber = textBox4.Text;
                    outDate = dateTimePicker2.Value.ToString("dd-MM-yyyy");

                    if (dateTimePicker2.Value < dateTimePicker1.Value)
                    {
                        MessageBox.Show("Дата завершения должна быть позже даты участия!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    CustomerRepository.UpdateCustomer(idChange, customerName, customerSurname, customerNumber, outDate);
                    CustomerRepository.GetAllCustomers(dataGridView1);
                    dataGridView1.ClearSelection();
                    textBox6.Text = $"{dataGridView1.Rows.Count - 1}";
                    MessageBox.Show("Участник изменен", "Учет участников", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case 2:
                    customerName = textBox2.Text;
                    customerSurname = textBox3.Text;
                    customerNumber = textBox4.Text;
                    CustomerRepository.FindCustomers(dataGridView1, customerName, customerSurname, customerNumber);
                    if (dataGridView1.SelectedRows.Count <= 0)
                    {
                        MessageBox.Show("Запись не найдена", "Учет соревнований", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Запись найдена", "Учет соревнований", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox4.Items.Clear();
            string? author = comboBox3.SelectedItem.ToString();
            comboBox4.Items.AddRange(CompRepository.GetAllCompsOrganizer(author).ToArray());
            comboBox4.SelectedIndex = 0;
        }

        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            Regex regexString = new(@"^\p{IsCyrillic}+$", RegexOptions.IgnorePatternWhitespace);
            Regex regexString2 = new(@"^(?=\s*$)", RegexOptions.IgnorePatternWhitespace);

            if (!regexString.IsMatch(textBox.Text) & !regexString2.IsMatch(textBox.Text))
            {
                e.Cancel = true;
                textBox.Focus();
                textBox.BackColor = Color.Red;
                MessageBox.Show("Ввести необходимо буквы русского алфавита", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                textBox.BackColor = Color.White;
            }
        }

        private void textBox3_Validating(object sender, CancelEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            Regex regexString = new(@"^\p{IsCyrillic}+$", RegexOptions.IgnorePatternWhitespace);
            Regex regexString2 = new(@"^(?=\s*$)", RegexOptions.IgnorePatternWhitespace);

            if (!regexString.IsMatch(textBox.Text) & !regexString2.IsMatch(textBox.Text))
            {
                e.Cancel = true;
                textBox.Focus();
                textBox.BackColor = Color.Red;
                MessageBox.Show("Ввести необходимо буквы русского алфавита", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                textBox.BackColor = Color.White;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    button7.Visible = false;
                    button1.Text = "Добавить";
                    dateTimePicker2.Enabled = true;
                    comboBox3.Enabled = true;
                    comboBox4.Enabled = true;
                    break;
                case 1:
                    button7.Visible = false;
                    button1.Text = "Изменить";
                    dateTimePicker2.Enabled = true;
                    comboBox3.Enabled = false;
                    comboBox4.Enabled = false;
                    break;
                case 2:
                    button7.Visible = true;
                    button1.Text = "Найти";
                    dateTimePicker2.Enabled = false;
                    comboBox3.Enabled = false;
                    comboBox4.Enabled = false;
                    break;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count - 1)
            {
                DataGridView dataGridView = (DataGridView)sender;
                DataGridViewRow dataGridViewRow = dataGridView.Rows[e.RowIndex];
                textBox2.Text = dataGridViewRow.Cells[1].Value.ToString();
                textBox3.Text = dataGridViewRow.Cells[2].Value.ToString();
                textBox4.Text = dataGridViewRow.Cells[3].Value.ToString();
                dateTimePicker1.Value = DateTime.Parse(dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString());
                dateTimePicker2.Value = DateTime.Parse(dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString());
                comboBox3.SelectedItem = dataGridViewRow.Cells[4].Value;
                comboBox4.SelectedItem = dataGridViewRow.Cells[3].Value;
            }
        }
        private void dateTimePicker2_Validating(object sender, CancelEventArgs e)
        {
            DateTimePicker dateTimePicker = (DateTimePicker)sender;
            if (dateTimePicker.Value.ToString("dd-MM-yyyy") == dateTimePicker.MinDate.ToString("dd-MM-yyyy"))
            {
                e.Cancel = true;
                dateTimePicker.Focus();
                MessageBox.Show("Нельзя выбрать эту дату завершения!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button4.Text = "Показать завершивших";
            CustomerRepository.DeleteCustomer(idChange);
            CustomerRepository.GetAllCustomers(dataGridView1);
            dataGridView1.ClearSelection();
            textBox6.Text = $"{dataGridView1.Rows.Count - 1}";
            MessageBox.Show("Участник удален", "Учет участников", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                indRow = selectedRow.Index;
                idChange = Convert.ToInt32(dataGridView1.Rows[indRow].Cells[0].Value);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                label1.Text = "Фильтрация включена";
                label1.ForeColor = Color.Green;
                string parameter = "";
                switch (comboBox1.SelectedIndex)
                {
                    case 0:
                        parameter = "name";
                        break;
                    case 1:
                        parameter = "surname";
                        break;
                    case 2:
                        parameter = "number_phone";
                        break;
                    case 3:
                        parameter = "comps.comp_name";
                        break;
                    case 4:
                        parameter = "comps.place || ' ' || comps.organizer_surname";
                        break;
                    case 5:
                        parameter = "taking_data";
                        break;
                    case 6:
                        parameter = "return_data";
                        break;
                    case 7:
                        string? cellValue;
                        for (int i = dataGridView1.Rows.Count - 2; i >= 0; i--)
                        {
                            cellValue = dataGridView1.Rows[i].Cells[8].Value.ToString();
                            if (!cellValue.Contains(textBox1.Text))
                            {
                                dataGridView1.Rows.RemoveAt(i);
                            }
                        }
                        return;
                }
                CustomerRepository.FiltrationCustomers(dataGridView1, parameter, textBox1.Text);
                textBox6.Text = $"{dataGridView1.Rows.Count - 1}";
            }
            else
            {
                label1.Text = "Фильтрация отключена";
                label1.ForeColor = Color.Red;
                CustomerRepository.GetAllCustomers(dataGridView1);
                textBox6.Text = $"{dataGridView1.Rows.Count - 1}";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (button4.Text == "Показать завершивших")
            {
                string? cellValue;
                for (int i = dataGridView1.Rows.Count - 2; i >= 0; i--)
                {
                    cellValue = dataGridView1.Rows[i].Cells[8].Value.ToString();
                    if (cellValue == "0")
                    {
                        dataGridView1.Rows.RemoveAt(i);
                    }
                }
                textBox6.Text = $"{dataGridView1.Rows.Count - 1}";
                button4.Text = "Показать участников";
            }
            else
            {
                CustomerRepository.GetAllCustomers(dataGridView1);
                textBox6.Text = $"{dataGridView1.Rows.Count - 1}";
                button4.Text = "Показать завершивших";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ChooseDBForm chooseDBForm = new ChooseDBForm(FilePathDB);
            chooseDBForm.Show();
            this.Close();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_Validating(object sender, CancelEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            Regex regexString = new(@"^(\+7|8)?\s?\(?\d{3}\)?[\s-]?\d{3}[\s-]?\d{2}[\s-]?\d{2}$", RegexOptions.IgnorePatternWhitespace);
            Regex regexString2 = new(@"^(?=\s*$)", RegexOptions.IgnorePatternWhitespace);

            if (!regexString.IsMatch(textBox.Text) & !regexString2.IsMatch(textBox.Text))
            {
                e.Cancel = true;
                textBox.Focus();
                textBox.BackColor = Color.Red;
                MessageBox.Show("Ввести необходимо номер такого формата: +7 (922) 555-12-34", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                textBox.BackColor = Color.White;
            }
        }

        private void dataGridView1_DataSourceChanged(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 1)
            {
                button5.Enabled = false;
                button6.Enabled = false;
            }
            else
            {
                button5.Enabled = true;
                button6.Enabled = true;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            CustomerRepository.DeleteAllCustomers();
            CustomerRepository.GetAllCustomers(dataGridView1);
            textBox6.Text = $"{dataGridView1.Rows.Count - 1}";
            MessageBox.Show("Все участники удалены", "Учет участников", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
        }
    }
}
