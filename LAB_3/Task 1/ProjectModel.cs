using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using CinemaContext;
using Npgsql;

namespace DB_Manager_Cinema.Model
{
    public class ProjectModel
    {
        public string connectionString = string.Format("Server=localhost;Port=5432;Database=Cinema;User Id=postgres;Password=32147;");
            
        public NpgsqlConnection connectionToPgSql;
        public DataTable dt;

        private CinemaDataContext dc = new CinemaDataContext();

        private static ProjectModel instance;

        public static ProjectModel getInstance()
        {
            if (instance == null) instance = new ProjectModel();

            return instance;
        }

        private ProjectModel() { }
        
        public DataTable LINQResultToDataTable<T>(IEnumerable<T> Linqlist)
        {
            DataTable dataTable = new DataTable();


            PropertyInfo[] columns = null;

            if (Linqlist == null) return dataTable;

            foreach (T Record in Linqlist)
            {

                if (columns == null)
                {
                    columns = ((Type)Record.GetType()).GetProperties();
                    foreach (PropertyInfo GetProperty in columns)
                    {
                        Type colType = GetProperty.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                        == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dataTable.Columns.Add(new DataColumn(GetProperty.Name, colType));
                    }
                }

                DataRow dr = dataTable.NewRow();

                foreach (PropertyInfo pinfo in columns)
                {
                    dr[pinfo.Name] = pinfo.GetValue(Record, null) == null ? DBNull.Value : pinfo.GetValue
                    (Record, null);
                }

                dataTable.Rows.Add(dr);
            }
            return dataTable;
        }

        public DataTable buildAndPerformSelectDeleteQuery(string queryType, string tableName, string queryValue)
        {          
            if (queryType == "select")
            {

                if (tableName == "administrator")
                {
                    IEnumerable<CinemaContext.administrator> selectData =
                        from administrator in dc.administrators
                        select administrator;

                    dt = LINQResultToDataTable(selectData);
                }


                if (tableName == "cashier")
                {
                    IEnumerable<CinemaContext.cashier> selectData =
                        from cashier in dc.cashiers
                        select cashier;

                    dt = LINQResultToDataTable(selectData);
                }


                if (tableName == "customer")
                {
                    IEnumerable<CinemaContext.customer> selectData =
                        from customer in dc.customers
                        select customer;

                    dt = LINQResultToDataTable(selectData);
                }


                if (tableName == "film")
                {
                    IEnumerable<CinemaContext.film> selectData =
                        from film in dc.films
                        select film;

                    dt = LINQResultToDataTable(selectData);
                }
            }

            if (queryType == "delete")
            {
                int idValue;

                var splitRes = queryValue.Split('=');

                idValue = Convert.ToInt32(splitRes[1]);

                if (tableName == "administrator")
                {
                    administrator admin = (from r in dc.administrators
                                           where r.adminid == idValue select r).SingleOrDefault();
                    dc.administrators.DeleteOnSubmit(admin);
                    dc.SubmitChanges();

                    IEnumerable<CinemaContext.administrator> selectData =
                        from administrator in dc.administrators
                        select administrator;

                    dt = LINQResultToDataTable(selectData);
                }


                if (tableName == "cashier")
                {
                    cashier cash = (from r in dc.cashiers
                                    where r.cashierid == idValue
                                           select r).SingleOrDefault();
                    dc.cashiers.DeleteOnSubmit(cash);
                    dc.SubmitChanges();

                    IEnumerable<CinemaContext.cashier> selectData =
                        from cashier in dc.cashiers
                        select cashier;

                    dt = LINQResultToDataTable(selectData);
                }


                if (tableName == "customer")
                {
                    customer cust = (from r in dc.customers
                                     where r.customerid == idValue
                                    select r).SingleOrDefault();
                    dc.customers.DeleteOnSubmit(cust);
                    dc.SubmitChanges();

                    IEnumerable<CinemaContext.customer> selectData =
                        from customer in dc.customers
                        select customer;

                    dt = LINQResultToDataTable(selectData);
                }


                if (tableName == "film")
                {
                    string filmTitle;

                    var splitResult = queryValue.Split('=');

                    filmTitle = splitRes[1];

                    film fil = (from r in dc.films
                                where r.title == filmTitle
                                     select r).SingleOrDefault();
                    dc.films.DeleteOnSubmit(fil);
                    dc.SubmitChanges();

                    IEnumerable<CinemaContext.film> selectData =
                        from film in dc.films
                        select film;

                    dt = LINQResultToDataTable(selectData);
                }
            }
            

            return dt;
        }

        public DataTable buildAndPerformInsertQuery(string tableName, string columnName, string value)
        {
            
            connectionToPgSql = new NpgsqlConnection(connectionString);

            var splitValueResult = value.Split(','); 

            if (tableName == "administrator")
            {
                administrator admin = new administrator();

                admin.adminid = Convert.ToInt32(splitValueResult[0]);
                admin.fullname = splitValueResult[1];
                admin.shift = Convert.ToInt32(splitValueResult[2]);
                admin.salary = Convert.ToDecimal(splitValueResult[3]);

                dc.administrators.InsertOnSubmit(admin);
                dc.SubmitChanges();

                IEnumerable<CinemaContext.administrator> selectData =
                    from administrator in dc.administrators
                    select administrator;

                dt = LINQResultToDataTable(selectData);
            }
            
            if (tableName == "cashier")
            {
                cashier cash = new cashier();

                cash.cashierid = Convert.ToInt32(splitValueResult[0]);
                cash.fullname = splitValueResult[1];
                cash.shift = Convert.ToInt32(splitValueResult[2]);
                cash.salary = Convert.ToDecimal(splitValueResult[3]);

                dc.cashiers.InsertOnSubmit(cash);
                dc.SubmitChanges();

                IEnumerable<CinemaContext.cashier> selectData =
                    from cashier in dc.cashiers
                    select cashier;

                dt = LINQResultToDataTable(selectData);
            }
             
            if (tableName == "customer")
            {
                customer cust = new customer();

                cust.customerid = Convert.ToInt32(splitValueResult[0]);
                cust.cashierid = Convert.ToInt32(splitValueResult[1]);
                cust.category = splitValueResult[2];
                cust.privilege = splitValueResult[3];
                cust.privilegesize = Convert.ToDecimal(splitValueResult[4]);

                dc.customers.InsertOnSubmit(cust);
                dc.SubmitChanges();

                IEnumerable<CinemaContext.customer> selectData =
                       from customer in dc.customers
                       select customer;

                dt = LINQResultToDataTable(selectData);
            }

            if (tableName == "film")
            {
                film fil = new film();

                fil.title = splitValueResult[0];
                fil.genre = splitValueResult[1];
                fil.releasedate = Convert.ToDateTime(splitValueResult[2]);

                dc.films.InsertOnSubmit(fil);
                dc.SubmitChanges();


                IEnumerable<CinemaContext.film> selectData =
                        from film in dc.films
                        select film;

                dt = LINQResultToDataTable(selectData);
            }

            return dt;
        }

        public DataTable buildAndPerformUpdateQuery(string tableName, string newValue, string conditionValue)
        {
            DataSet ds = new DataSet();

            connectionToPgSql = new NpgsqlConnection(connectionString);

            string updateData = "UPDATE " + tableName + " SET " + newValue + " WHERE " + conditionValue + "; " +
                "SELECT * FROM " + tableName +";";

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(updateData, connectionToPgSql);

            NpgsqlCommand command = new NpgsqlCommand(updateData, connectionToPgSql);
            command.CommandType = CommandType.Text;

            da.SelectCommand = command;

            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];

            return dt;
        }


        public DataTable randomDataQuery(string tableName)
        {
            DataSet ds = new DataSet();

            connectionToPgSql = new NpgsqlConnection(connectionString);

            string insertRandomData = null;

            if (tableName == "administrator")
                insertRandomData = "INSERT INTO " + tableName + " SELECT DISTINCT nextval('administrator_adminid_seq'), " +
                    "MD5(random()::text), " +
                    "floor(random()*(6-3+1))+3, (floor(random()*(5500-2500+1))+2500)::int FROM GENERATE_SERIES(1,2); " +
                    "SELECT * FROM " + tableName + ";";

            if (tableName == "cashier")
                insertRandomData = "INSERT INTO " + tableName + " SELECT DISTINCT nextval('cashier_cashierid_seq'), " +
                    "(SELECT adminid FROM administrator ORDER BY random() LIMIT 1)," +
                    " MD5(random()::text), floor(random()*(6-3+1))+3," +
                    "(floor(random()*(5500-2500+1))+2500)::int FROM GENERATE_SERIES(1,2); SELECT * FROM " + tableName + ";";

            if (tableName == "customer")
                insertRandomData = "INSERT INTO " + tableName + " SELECT DISTINCT nextval('customer_customerid_seq'), " +
                    "(SELECT cashierid FROM cashier ORDER BY random() LIMIT 1), " +
                    "(CASE(random() * 4)::INT " +
                    "WHEN 0 THEN 'Student' " +
                    "WHEN 1 THEN 'Child' " +
                    "WHEN 2 THEN 'Adult' " +
                    "WHEN 3 THEN 'Veteran' " +
                    "WHEN 4 THEN 'Disabled' " +
                    "END), " +
                    "(CASE (random() * 4)::INT " +
                    "WHEN 0 THEN 'is available' " +
                    "WHEN 1 THEN 'is available' " +
                    "WHEN 2 THEN 'not available' " +
                    "WHEN 3 THEN 'is available' " +
                    "WHEN 4 THEN 'is available' " +
                    "END), " +
                    "(floor(random()*(85-0+1))+0)::int FROM GENERATE_SERIES(1,2); SELECT * FROM " + tableName + ";";

            if (tableName == "film")
                insertRandomData = "INSERT INTO " + tableName + " SELECT DISTINCT MD5(random()::text), " +
                    "MD5(random()::text), " +
                    "timestamp '2019-01-10 20:00:00' + random() * (timestamp '2019-01-20 20:00:00' - timestamp '2019-01-10 10:00:00') " +
                    "FROM GENERATE_SERIES(1,2); SELECT * FROM " + tableName + ";";


            NpgsqlDataAdapter da = new NpgsqlDataAdapter(insertRandomData, connectionToPgSql);

            NpgsqlCommand command = new NpgsqlCommand(insertRandomData, connectionToPgSql);
            command.CommandType = CommandType.Text;

            da.SelectCommand = command;

            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];

            return dt;
        }

        public DataTable attSearchQuery(int startRange, int endRange, string title)
        {
            DataSet ds = new DataSet();

            connectionToPgSql = new NpgsqlConnection(connectionString);

            string findDataByAttributes = null;

            findDataByAttributes = "SELECT * FROM customer JOIN customer_film ON " +
                "(customer_film.customerid = customer.customerid)" +
                " WHERE customer.customerid BETWEEN " + startRange + " AND " + endRange + " AND customer_film.title='" + title + "';";

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(findDataByAttributes, connectionToPgSql);

            NpgsqlCommand command = new NpgsqlCommand(findDataByAttributes, connectionToPgSql);
            command.CommandType = CommandType.Text;

            da.SelectCommand = command;

            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];

            return dt;
        }

        public DataTable displayTableFullSearch(string tableName)
        {
            DataSet ds = new DataSet();

            connectionToPgSql = new NpgsqlConnection(connectionString);

            string selectTable = "SELECT * FROM " + tableName + ";";

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(selectTable, connectionToPgSql);

            NpgsqlCommand command = new NpgsqlCommand(selectTable, connectionToPgSql);
            command.CommandType = CommandType.Text;

            da.SelectCommand = command;

            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];

            return dt;
        }

        public DataTable fullSearchQuery(string tableName, string phraseToSearch)
        {
            DataSet ds = new DataSet();

            connectionToPgSql = new NpgsqlConnection(connectionString);

            string fullTextSearch = null;

            if (tableName == "administrator")
            {
                fullTextSearch = "SELECT * FROM " + tableName + " WHERE to_tsvector(fullname)" +
            " @@ plainto_tsquery('" + phraseToSearch + "');";
            }

            if (tableName == "cashier")
            {
                fullTextSearch = "SELECT * FROM " + tableName + " WHERE to_tsvector(fullname)" +
            " @@ plainto_tsquery('" + phraseToSearch + "');";
            }

            if (tableName == "customer")
            {
                fullTextSearch = "SELECT * FROM " + tableName + " WHERE to_tsvector(category) || to_tsvector(privilege)" +
            " @@ plainto_tsquery('" + phraseToSearch + "');";
            }

            if (tableName == "film")
            {
                fullTextSearch = "SELECT * FROM " + tableName + " WHERE to_tsvector(title) || to_tsvector(genre)" +
            " @@ plainto_tsquery('" + phraseToSearch + "');";
            }

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(fullTextSearch, connectionToPgSql);

            NpgsqlCommand command = new NpgsqlCommand(fullTextSearch, connectionToPgSql);
            command.CommandType = CommandType.Text;

            da.SelectCommand = command;

            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];

            return dt;
        }
    }
}
