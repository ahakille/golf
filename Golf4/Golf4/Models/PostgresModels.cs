using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace Golf4.Models
{
    public class PostgresModels
    {
        private NpgsqlConnection _conn;
        private NpgsqlCommand _cmd;
        private NpgsqlDataReader _dr;
        private DataTable _table;
        private string _error;
        public static List<NpgsqlParameter> list { get; set; }
        public  PostgresModels()
        {
            // db=vår databas
            // Inlogg ändras i appconfig
            _conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["db"].ConnectionString);
            try
            {
                _conn.Open();
            }
            catch (Exception ex)
            {
                _error = ex.Message;
            }
            _table = new DataTable();
        }
        
        /// <summary>
        /// Metod som skickar in en NONQuery samt flera parametrar. Exempel nedan
        ///   x.SqlParameters("insert into schema (fk_barn_id, datum, lämna, hämta, fk_kod_id) values (@par1, @par2, @par3, @par4,'1');", postgres.lista = new List<NpgsqlParameter>()
        ///    {
        ///        new NpgsqlParameter("@par1", a),
        ///        new NpgsqlParameter("@par2", b),
        ///        new NpgsqlParameter("@par3", c),
        ///        new NpgsqlParameter("@par4", d)
        ///    });
        /// </summary>

        public void SqlNonQuery(string sqlquery, List<NpgsqlParameter> parametrar)
        {
            try
            {
                _cmd = new NpgsqlCommand(sqlquery, _conn);
                _cmd.Parameters.AddRange(parametrar.ToArray());
                _cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                _error = ex.Message;
            }

            finally
            {
                _conn.Close();
            }
        }
        /// <summary>
        /// metod som skickar in en SQLquery + parametrar och retunerar en Datatable. Exempel nedan!
        ///  ///   x.SqlParameters("insert into schema (fk_barn_id, datum, lämna, hämta, fk_kod_id) values (@par1, @par2, @par3, @par4,'1');", postgres.lista = new List<NpgsqlParameter>()
        ///    {
        ///        new NpgsqlParameter("@par1", a),
        ///        new NpgsqlParameter("@par2", b),
        ///        new NpgsqlParameter("@par3", c),
        ///        new NpgsqlParameter("@par4", d)
        ///    });
        /// </summary>
        /// <param name="sqlquery"></param>
        /// <param name="parametrar"></param>
        /// <returns>Datatable</returns>
        public DataTable SqlQuery(string sqlquery, List<NpgsqlParameter> parametrar)
        {
            try

            {
                _cmd = new NpgsqlCommand(sqlquery, _conn);
                _cmd.Parameters.AddRange(parametrar.ToArray());
                _dr = _cmd.ExecuteReader();
                _table.Load(_dr);
                return _table;

            }
            catch (Exception ex)
            {
                _error = ex.Message;
                return null;
            }

            finally
            {
                _conn.Close();
            }
        }
    }
}