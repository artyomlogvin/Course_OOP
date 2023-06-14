using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyrs_OOP_CSharp.repository
{
    public interface ICompRepository
    {
        void GetAllComps(DataGridView dataGridView);
        List<string> GetAllPlaces();
        List<string> GetAllCompsOrganizer(string org);
        int GetComp(string name, string place, string orgSurname);
        void SaveComp(Comp comp);
        void UpdateComp(Comp comp);
        void DeleteComp(int id);
        void FiltrationComps(DataGridView dataGridView, string parameter, string value);
        void FindComps(DataGridView dataGridView, Comp comp);

        void DeleteAllComps();
    }
}
