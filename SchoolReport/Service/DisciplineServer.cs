using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolReport.Service
{
    internal class DisciplineServer
    {
        //private readonly ConnectionPostgresDB connection;
        private readonly ConnectionMariaDB connection;
        public DisciplineServer() 
        {
            //connection = new ConnectionPostgresDB();
            connection = new ConnectionMariaDB();
        }

    }
}
