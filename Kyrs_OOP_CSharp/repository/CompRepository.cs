using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Kyrs_OOP_CSharp.repository
{
    public class CompRepository : Repository, ICompRepository
    {
        public CompRepository(string filePath) : base(filePath)
        {
        }
        public void GetAllComps(DataGridView dataGridView)
        {
            connection.Open();
            using (var command = new SqliteCommand(@"
                    SELECT id, comp_name, place, 
                    organizer_surname, age FROM comps",
                    connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    var dataTable = new DataTable();
                    dataTable.Load(reader);

                    dataTable.Columns[1].ColumnName = "Название турнира";
                    dataTable.Columns[2].ColumnName = "Место";
                    dataTable.Columns[3].ColumnName = "Фамилия организатора";
                    dataTable.Columns[4].ColumnName = "Возраст";

                    dataGridView.Columns.Clear();
                    dataGridView.DataSource = dataTable;
                    dataGridView.Columns[0].Visible = false;
                    dataGridView.ClearSelection();
                }
            }
            connection.Close();
        }


        public void SaveComp(Comp comp)
        {
            connection.Open();
            using (var command = new SqliteCommand($@"
                    INSERT INTO comps (comp_name, place, organizer_surname, age) 
                    VALUES ('{comp.NameComp}', '{comp.Place}', '{comp.OrgSurname}', '{comp.Age}')",
                    connection))
            {

                command.ExecuteNonQuery();
            }
            connection.Close();
        }

        public void UpdateComp(Comp comp)
        {
            connection.Open();
            using (var command = new SqliteCommand($@"
                    UPDATE comps SET comp_name = '{comp.NameComp}', place = '{comp.Place}', 
                    organizer_surname = '{comp.OrgSurname}', age = '{comp.Age}' WHERE id = {comp.Id}",
                    connection))
            {
                command.ExecuteNonQuery();
            }
            connection.Close();
        }

        public void DeleteComp(int id)
        {
            connection.Open();
            using (var command = new SqliteCommand($@"
                    DELETE FROM comps WHERE id = {id}",
                    connection))
            {
                command.ExecuteNonQuery();
            }
            connection.Close();
        }

        public void FiltrationComps(DataGridView dataGridView, string parameter, string value)
        {
            connection.Open();
            using (var command = new SqliteCommand($@"
                    SELECT id, comp_name, place, organizer_surname, age
                    FROM comps WHERE {parameter} LIKE '%{value}%'",
                    connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    var dataTable = new DataTable();
                    dataTable.Load(reader);

                    dataTable.Columns[1].ColumnName = "Название турнира";
                    dataTable.Columns[2].ColumnName = "Место";
                    dataTable.Columns[3].ColumnName = "Фамилия организатора";
                    dataTable.Columns[4].ColumnName = "Возраст";

                    dataGridView.Columns.Clear();
                    dataGridView.DataSource = dataTable;
                    dataGridView.Columns[0].Visible = false;
                    dataGridView.ClearSelection();
                }
            }
            connection.Close();
        }

        public void FindComps(DataGridView dataGridView, Comp comp)
        {
            dataGridView.ClearSelection();
            connection.Open();
            using (var command = new SqliteCommand($@"
                    SELECT id FROM comps WHERE comp_name LIKE '%{comp.NameComp}%' AND place LIKE '%{comp.Place}%'
                    AND organizer_surname LIKE '%{comp.OrgSurname}%' AND age LIKE '%{comp.Age}%'",
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

        public int GetComp(string name, string place, string orgSurname)
        {
            int id = -1;
            connection.Open();
            using (var command = new SqliteCommand($@"
                    SELECT id FROM comps WHERE comp_name = '{name}' AND place = '{place}'
                    AND organizer_surname = '{orgSurname}'",
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

        public List<string> GetAllPlaces()
        {
            List<string> places = new List<string>();
            connection.Open();
            using (var command = new SqliteCommand($@"SELECT DISTINCT place, organizer_surname FROM comps", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        places.Add($"{reader.GetString("place")} {reader.GetString("organizer_surname")}");

                    }
                }
            }
            connection.Close();
            return places;
        }

        public List<string> GetAllCompsOrganizer(string author)
        {
            string[] authorNameSurname = author.Split(" ");
            List<string> compsName = new List<string>();
            connection.Open();
            using (var command = new SqliteCommand($@"SELECT DISTINCT comp_name FROM comps WHERE 
                    place = '{authorNameSurname[0]}' AND organizer_surname = '{authorNameSurname[1]}'", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        compsName.Add(reader.GetString("comp_name"));

                    }
                }
            }
            connection.Close();
            return compsName;
        }

        public void DeleteAllComps()
        {
            connection.Open();
            using (var command = new SqliteCommand($@"DELETE FROM comps",
                    connection))
            {
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }
}

