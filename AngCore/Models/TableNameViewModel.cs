using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngCore.Models
{

    public class TableViewModel
    {
        public String Name { get; set; }

        public List<String> Columns { get; set; }
    }

    public class TableNamesList
    {
        public String Database_name { get; set; }
        public List<String> Tables { get; set; }
    }

    public class Databases
    {
        public List<String> List { get; set; }
    }



}