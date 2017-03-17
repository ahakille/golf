using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Npgsql;

namespace Golf4.Models
{
    public class ResultModels
    {
        public DataTable ResultList { get; set; }


        public class Result
        {
            public DataTable GetResultList(int contestid)
            {
                PostgresModels Database = new PostgresModels();
                DataTable dt = new DataTable("data");
                dt = Database.SqlQuery("SELECT golfid, firstname, lastname, result FROM players LEFT JOIN members ON players.memberid = members.id WHERE contestid = @contestid AND publish = true ORDER BY result DESC", PostgresModels.list = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@contestid", contestid),
                });

                return dt;
            }
        }
    }
}