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
        public DataTable ViewResultList { get; set; }


        public class Result
        {
            public DataTable GetResultList(int contestid)
            {
                PostgresModels Database = new PostgresModels();
                DataTable dt = new DataTable("data");
                dt = Database.SqlQuery("SELECT golfid AS \"GolfID\", firstname AS \"Förnamn\", lastname AS \"Efternamn\", result AS \"Resultat\" FROM players LEFT JOIN members ON players.memberid = members.id LEFT JOIN contests ON players.contestid = contests.id WHERE contestid = @contestid AND publish = true ORDER BY result DESC", PostgresModels.list = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@contestid", contestid),
                });

                return dt;
            }
        }
    }
}