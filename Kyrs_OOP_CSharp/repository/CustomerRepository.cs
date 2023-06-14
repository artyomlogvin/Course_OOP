using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Kyrs_OOP_CSharp.repository
{
    public class CustomerRepository : Repository, ICustomerRepository
    {
        public CustomerRepository(string filePath) : base(filePath)
        {
        }

        public void DeleteCustomer(int id)
        {
            connection.Open();
            using (var command = new SqliteCommand($@"
                    DELETE FROM customers WHERE id = {id}",
                    connection))
            {
                command.ExecuteNonQuery();
            }
            connection.Close();
        }

        public void UpdateCustomer(int id, string name, string surname, string number, string endDate)
        {
            connection.Open();
            using (var command = new SqliteCommand($@"
                    UPDATE customers SET name = '{name}', surname = '{surname}', number_phone = '{number}',
                    return_data = '{endDate}' WHERE id = {id}",
                    connection))
            {
                command.ExecuteNonQuery();
            }
            connection.Close();
        }

        public void FiltrationCustomers(DataGridView dataGridView, string parameter, string value)
        {
            connection.Open();
            using (var command = new SqliteCommand($@"
                    SELECT customers.id, customers.name, customers.surname, customers.number_phone, comps.comp_name,
                    comps.place || ' ' || comps.organizer_surname AS org, 
                    customers.taking_data, customers.return_data  
                    FROM customers JOIN comps ON customers.comp_id = comps.id WHERE {parameter} LIKE '%{value}%'",
                    connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    var dataTable = new DataTable();
                    dataTable.Load(reader);

                    dataTable.Columns[1].ColumnName = "Имя";
                    dataTable.Columns[2].ColumnName = "Фамилия";
                    dataTable.Columns[3].ColumnName = "Номер";
                    dataTable.Columns[4].ColumnName = "Название турнира";
                    dataTable.Columns[5].ColumnName = "Место";
                    dataTable.Columns[6].ColumnName = "Дата участия";
                    dataTable.Columns[7].ColumnName = "Дата завершения";

                    dataTable.Columns[5].DataType = typeof(string);
                    dataTable.Columns.Add("Дней после завершения");

                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        DateTime currentDate = DateTime.Now.Date;

                        DateTime returnDate = DateTime.ParseExact(dataTable.Rows[i][7].ToString(),
                                                               "dd-MM-yyyy", null);

                        TimeSpan overdueDays = currentDate - returnDate;

                        if (overdueDays.TotalDays > 0)
                        {
                            dataTable.Rows[i][8] = overdueDays.TotalDays;
                        }
                        else
                        {
                            dataTable.Rows[i][8] = 0;
                        }
                    }


                    dataGridView.Columns.Clear();
                    dataGridView.DataSource = dataTable;
                    dataGridView.Columns[0].Visible = false;
                    dataGridView.ClearSelection();
                }
            }
            connection.Close();
        }

        public void FindCustomers(DataGridView dataGridView, string name, string surname, string number)
        {
            dataGridView.ClearSelection();
            connection.Open();
            using (var command = new SqliteCommand($@"
                    SELECT id FROM customers WHERE name LIKE '%{name}%' AND surname LIKE '%{surname}%'
                    AND number_phone LIKE '%{number}%'",
                    connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    int id;
                    while (reader.Read())
                    {
                        id = reader.GetInt32("id");
                        foreach (DataGridViewRow row in dataGridView.Rows)
                        {
                            if (Convert.ToInt32(row.Cells[0].Value) == id)
                            {
                                row.Selected = true;
                            }
                        }
                    }
                }
            }
            connection.Close();
        }

        public void GetAllCustomers(DataGridView dataGridView)
        {
            connection.Open();
            using (var command = new SqliteCommand($@"
                    SELECT customers.id, customers.name, customers.surname, customers.number_phone, comps.comp_name,
                    comps.place || ' ' || comps.organizer_surname AS author, 
                    customers.taking_data, customers.return_data  
                    FROM customers JOIN comps ON customers.comp_id = comps.id;",
                    connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    var dataTable = new DataTable();
                    dataTable.Load(reader);

                    dataTable.Columns[1].ColumnName = "Имя";
                    dataTable.Columns[2].ColumnName = "Фамилия";
                    dataTable.Columns[3].ColumnName = "Номер";
                    dataTable.Columns[4].ColumnName = "Название турнира";
                    dataTable.Columns[5].ColumnName = "Место";
                    dataTable.Columns[6].ColumnName = "Дата участия";
                    dataTable.Columns[7].ColumnName = "Дата завершения";

                    dataTable.Columns.Add("Дней после завершения");
                    dataTable.Columns[5].DataType = typeof(string);

                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        DateTime currentDate = DateTime.Now.Date;

                        DateTime returnDate = DateTime.ParseExact(dataTable.Rows[i][7].ToString(),
                                                               "dd-MM-yyyy", null);

                        TimeSpan overdueDays = currentDate - returnDate;

                        if (overdueDays.TotalDays > 0)
                        {
                            dataTable.Rows[i][8] = overdueDays.TotalDays;
                        }
                        else
                        {
                            dataTable.Rows[i][8] = 0;
                        }
                    }


                    dataGridView.Columns.Clear();
                    dataGridView.DataSource = dataTable;
                    dataGridView.Columns[0].Visible = false;
                    dataGridView.ClearSelection();
                }
            }
            connection.Close();
        }

        public void SaveCustomer(Customer customer)
        {
            connection.Open();
            using (var command = new SqliteCommand($@"
                    INSERT INTO customers (name, surname, number_phone, comp_id, taking_data, return_data) 
                    VALUES ('{customer.Name}',  '{customer.Surname}', '{customer.Number}', '{customer.IdComp}', '{customer.InDate}',
                    '{customer.OutDate}')",
                    connection))
            {
                command.ExecuteNonQuery();
            }
            connection.Close();
        }

        public int GetCustomer(string name, string surname, string number, int compId)
        {
            int id = -1;

            connection.Open();
            using (var command = new SqliteCommand($@"
                    SELECT id FROM customers WHERE name = '{name}' AND surname = '{surname}'
                    AND number_phone = '{number}' AND comp_id = '{compId}'",
                    connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        id = reader.GetInt32("id");

                    }
                }
            }
            connection.Close();
            return id;
        }

        public void DeleteAllCustomers()
        {
            connection.Open();
            using (var command = new SqliteCommand($@"DELETE FROM customers",
                    connection))
            {
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }
}
