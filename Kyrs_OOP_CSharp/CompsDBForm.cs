using System.ComponentModel;
using System.Text.RegularExpressions;
using Kyrs_OOP_CSharp.repository;

namespace Kyrs_OOP_CSharp
{
    public partial class CompsDBForm : Form
    {
        private CompRepository CompRepository;
        public string FilePathDB;
        private int idChange = 1;
        private int indRow = 0;

        public CompsDBForm(string filePath)
        {
            InitializeComponent();
            FilePathDB = filePath;
            this.CompRepository = new CompRepository(filePath);
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
        }
        private void CompsDBForm_Load(object sender, EventArgs e)
        {
            CompRepository.GetAllComps(dataGridView1);
            dataGridView1.ClearSelection();
            textBox6.Enabled = false;
            textBox6.Text = $"{dataGridView1.Rows.Count - 1}";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                MessageBox.Show("Введите название турнира!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (textBox3.Text == "")
            {
                MessageBox.Show("Введите место проведения!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (textBox4.Text == "")
            {
                MessageBox.Show("Введите фамилию организатора!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (textBox5.Text == "")
            {
                MessageBox.Show("Введите возраст!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string compName, place, orgSurname, age;
            Comp comp;
            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    compName = textBox2.Text;
                    place = textBox3.Text;
                    orgSurname = textBox4.Text;
                    age = textBox5.Text;

                    if (CompRepository.GetComp(compName, place, orgSurname) != -1)
                    {
                        MessageBox.Show("Такое соревнование уже есть в базе данных", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    comp = new Comp(compName, place, orgSurname, age);
                    CompRepository.SaveComp(comp);
                    CompRepository.GetAllComps(dataGridView1);
                    dataGridView1.ClearSelection();
                    textBox6.Text = $"{dataGridView1.Rows.Count - 1}";
                    dataGridView1.Rows[dataGridView1.Rows.Count - 2].Selected = true;
                    MessageBox.Show("Новое соревнование добавлено", "Учет соревнований", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case 1:
                    compName = textBox2.Text;
                    place = textBox3.Text;
                    orgSurname = textBox4.Text;
                    age = textBox5.Text;
                    comp = new Comp(idChange, compName, place, orgSurname, age);
                    CompRepository.UpdateComp(comp);
                    CompRepository.GetAllComps(dataGridView1);
                    dataGridView1.ClearSelection();
                    textBox6.Text = $"{dataGridView1.Rows.Count - 1}";
                    dataGridView1.Rows[indRow].Selected = true;
                    MessageBox.Show("Соревнование изменено", "Учет соревнований", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case 2:
                    compName = textBox2.Text;
                    place = textBox3.Text;
                    orgSurname = textBox4.Text;
                    age = textBox5.Text;
                    comp = new Comp(idChange, compName, place, orgSurname, age);
                    CompRepository.FindComps(dataGridView1, comp);
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

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    button6.Visible = false;
                    button1.Text = "Добавить";
                    break;
                case 1:
                    button6.Visible = false;
                    button1.Text = "Изменить";
                    break;
                case 2:
                    button6.Visible = true;
                    button1.Text = "Найти";
                    break;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CompRepository.DeleteComp(idChange);
            CompRepository.GetAllComps(dataGridView1);
            textBox6.Text = $"{dataGridView1.Rows.Count - 1}";
            MessageBox.Show("Соревнование удалено", "Учет соревнований", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                label9.Text = "Фильтрация включена";
                label9.ForeColor = Color.Green;
                string parameter = "";
                switch (comboBox1.SelectedIndex)
                {
                    case 0:
                        parameter = "comp_name";
                        break;
                    case 1:
                        parameter = "place";
                        break;
                    case 2:
                        parameter = "organizer_surname";
                        break;
                    case 3:
                        parameter = "age";
                        break;
                }
                CompRepository.FiltrationComps(dataGridView1, parameter, textBox1.Text);
                textBox6.Text = $"{dataGridView1.Rows.Count - 1}";
            }
            else
            {
                label9.Text = "Фильтрация отключена";
                label9.ForeColor = Color.Red;
                CompRepository.GetAllComps(dataGridView1);
                textBox6.Text = $"{dataGridView1.Rows.Count - 1}";
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridView dataGridView = (DataGridView)sender;
                DataGridViewRow dataGridViewRow = dataGridView.Rows[e.RowIndex];
                textBox2.Text = dataGridViewRow.Cells[1].Value.ToString();
                textBox3.Text = dataGridViewRow.Cells[2].Value.ToString();
                textBox4.Text = dataGridViewRow.Cells[3].Value.ToString();
                textBox5.Text = dataGridViewRow.Cells[4].Value.ToString();
            }
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

        private void button4_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ChooseDBForm chooseDBForm = new ChooseDBForm(FilePathDB);
            chooseDBForm.Show();
            this.Close();
        }


        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            Regex regexString = new(@"^[\p{IsCyrillic}0-9]+[\-\s]{0,1}[\p{IsCyrillic}0-9]*[\-\s]{0,1}[\p{IsCyrillic}0-9]*$", RegexOptions.IgnorePatternWhitespace);
            Regex regexString2 = new(@"^(?=\s*$)", RegexOptions.IgnorePatternWhitespace);

            if (!regexString.IsMatch(textBox.Text) & !regexString2.IsMatch(textBox.Text))
            {
                e.Cancel = true;
                textBox.Focus();
                textBox.BackColor = Color.Red;
                MessageBox.Show("Ввести необходимо буквы русского алфавита(допускаются цифры)", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                textBox.BackColor = Color.White;
            }
        }

        private void textBox5_Validating(object sender, CancelEventArgs e)
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

        private void textBox4_Validating(object sender, CancelEventArgs e)
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

        private void dataGridView1_DataSourceChanged(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count <= 1)
            {
                button5.Enabled = false;
                button3.Enabled = false;
            }
            else
            {
                button5.Enabled = true;
                button3.Enabled = true;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            CompRepository.DeleteAllComps();
            CompRepository.GetAllComps(dataGridView1);
            textBox6.Text = $"{dataGridView1.Rows.Count - 1}";
            MessageBox.Show("Все соревнования удалены", "Учет соревнований", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
        }
    }
}
