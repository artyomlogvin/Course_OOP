﻿namespace Kyrs_OOP_CSharp
{
    public interface ICustomerRepository
    {
        void GetAllCustomers(DataGridView dataGridView);
        int GetCustomer(string name, string surname, string number,int compId);
        void SaveCustomer(Customer customer);
        void UpdateCustomer(int id, string name, string surname, string number, string endDate);
        void DeleteCustomer(int id);
        void FiltrationCustomers(DataGridView dataGridView, string parameter, string value);
        void FindCustomers(DataGridView dataGridView, string name, string surname, string number);

        void DeleteAllCustomers();
    }
}
