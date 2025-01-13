using static Dapper.SqlMapper;

namespace HotelsSystem.Data
{
    public class SqlDataAccess : ISqlDataAccess
    {
        private readonly IConfiguration _config;

        public SqlDataAccess(IConfiguration config)
        {
            _config = config;
        }


        public async Task<IEnumerable<T>> GetDataTable<T, U>(string StoredProcedure, U Parameters)
        {
            Console.WriteLine(StoredProcedure + " " + Parameters);
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("connection")))
            {
                var result = await db.QueryAsync<T>(StoredProcedure, Parameters, commandType: CommandType.StoredProcedure);

                if (result == null)
                    return Activator.CreateInstance<IEnumerable<T>>();

                return result;
            }
        }

        public async Task<T> GetOneInfo<T, U>(string StoredProcedure, U Parameters)
        {
            Console.WriteLine("GetOneInfo " + StoredProcedure + " " + Parameters);
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("connection")))
            {
                var result = await db.QueryFirstOrDefaultAsync<T>(StoredProcedure, Parameters, commandType: CommandType.StoredProcedure);

                if (result == null)
                    if (typeof(T) == typeof(byte[]))
                        return (T)Convert.ChangeType(Array.Empty<byte>(), typeof(T));
                    else
                        return Activator.CreateInstance<T>();

                return result;
            }
        }

        public async Task<T> SaveData<T, U>(string StoredProcedure, U Parameters)
        {
            Console.WriteLine(StoredProcedure + " " + Parameters);
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("connection")))
            {
                var result = (await db.QueryAsync<T>(StoredProcedure, Parameters, commandType: CommandType.StoredProcedure));

                if (result.Any())
                    return result.First();
                else
                    return Activator.CreateInstance<T>();
            }
        }

        public async Task<PagedResult<T>> GetGridResult<T, U>(string StoredProcedure, U Parameters, int PageNumber = 1, int PageSize = 10, int MaxNavigationPages = 5)
        {
            Console.WriteLine(StoredProcedure + " " + Parameters);

            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("connection")))
            {
                using var multi = await db.QueryMultipleAsync(StoredProcedure, Parameters, commandType: CommandType.StoredProcedure);

                var items = await multi.ReadAsync<T>().ConfigureAwait(false);
                var totalItems = await multi.ReadFirstAsync<int>().ConfigureAwait(false);

                decimal TotalOne = 0, TotalTwo = 0, TotalThree = 0, TotalFour = 0;
                if (!multi.IsConsumed)
                {
                    TotalOne = await multi.ReadSingleOrDefaultAsync<decimal>().ConfigureAwait(false);
                }
                if (!multi.IsConsumed)
                {
                    TotalTwo = await multi.ReadSingleOrDefaultAsync<decimal>().ConfigureAwait(false);
                }
                if (!multi.IsConsumed)
                {
                    TotalThree = await multi.ReadFirstOrDefaultAsync<decimal>().ConfigureAwait(false);
                }
                if (!multi.IsConsumed)
                {
                    TotalFour = await multi.ReadFirstOrDefaultAsync<decimal>().ConfigureAwait(false);
                }

                var result = new PagedResult<T>(totalItems, TotalOne, TotalTwo, TotalThree, TotalFour, pageNumber: PageNumber, PageSize, maxNavigationPages: MaxNavigationPages)
                {
                    Items = items
                };
                return result;
            }
        }

        public async Task<Tuple<IEnumerable<T1>, IEnumerable<T2>>> GetMultiple<T1, T2>(
            string sql,

            object parameters)
        {
            System.Console.WriteLine(sql + " " + parameters);
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("connection")))
            {
                using var gridReader = await db.QueryMultipleAsync(sql, parameters, commandType: CommandType.StoredProcedure);

                IEnumerable<T1> t1 = Enumerable.Empty<T1>();
                IEnumerable<T2> t2 = Enumerable.Empty<T2>();

                if (!gridReader.IsConsumed)
                    t1 = await gridReader.ReadAsync<T1>();
                if (!gridReader.IsConsumed)
                    t2 = await gridReader.ReadAsync<T2>();
                return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(t1, t2);
            }
        }

        public async Task<Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>>> GetMultiple<T1, T2, T3>(
            string sql,

            object parameters)
        {
            Console.WriteLine(sql + " " + parameters);

            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("connection")))
            {
                using var gridReader = await db.QueryMultipleAsync(sql, parameters, commandType: CommandType.StoredProcedure);
                IEnumerable<T1> t1 = Enumerable.Empty<T1>();
                IEnumerable<T2> t2 = Enumerable.Empty<T2>();
                IEnumerable<T3> t3 = Enumerable.Empty<T3>();

                if (!gridReader.IsConsumed)
                    t1 = await gridReader.ReadAsync<T1>();
                if (!gridReader.IsConsumed)
                    t2 = await gridReader.ReadAsync<T2>();
                if (!gridReader.IsConsumed)
                    t3 = await gridReader.ReadAsync<T3>();

                return new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>>(t1, t2, t3);
            }
        }

        public async Task<Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>>> GetMultiple<T1, T2, T3, T4>(
            string sql,

            object parameters)
        {
            Console.WriteLine(sql + " " + parameters);

            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("connection")))
            {
                using var gridReader = await db.QueryMultipleAsync(sql, parameters, commandType: CommandType.StoredProcedure);
                IEnumerable<T1> t1 = Enumerable.Empty<T1>();
                IEnumerable<T2> t2 = Enumerable.Empty<T2>();
                IEnumerable<T3> t3 = Enumerable.Empty<T3>();
                IEnumerable<T4> t4 = Enumerable.Empty<T4>();

                if (!gridReader.IsConsumed)
                    t1 = await gridReader.ReadAsync<T1>();
                if (!gridReader.IsConsumed)
                    t2 = await gridReader.ReadAsync<T2>();
                if (!gridReader.IsConsumed)
                    t3 = await gridReader.ReadAsync<T3>();
                if (!gridReader.IsConsumed)
                    t4 = await gridReader.ReadAsync<T4>();
                return new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>>(t1, t2, t3, t4);
            }
        }

        public async Task<Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>>> GetMultiple<T1, T2, T3, T4, T5>(
            string sql,

            object parameters)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("connection")))
            {
                using var gridReader = await db.QueryMultipleAsync(sql, parameters, commandType: CommandType.StoredProcedure);
                IEnumerable<T1> t1 = Enumerable.Empty<T1>();
                IEnumerable<T2> t2 = Enumerable.Empty<T2>();
                IEnumerable<T3> t3 = Enumerable.Empty<T3>();
                IEnumerable<T4> t4 = Enumerable.Empty<T4>();
                IEnumerable<T5> t5 = Enumerable.Empty<T5>();

                if (!gridReader.IsConsumed)
                    t1 = await gridReader.ReadAsync<T1>();
                if (!gridReader.IsConsumed)
                    t2 = await gridReader.ReadAsync<T2>();
                if (!gridReader.IsConsumed)
                    t3 = await gridReader.ReadAsync<T3>();
                if (!gridReader.IsConsumed)
                    t4 = await gridReader.ReadAsync<T4>();
                if (!gridReader.IsConsumed)
                    t5 = await gridReader.ReadAsync<T5>();
                return new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>>(t1, t2, t3, t4, t5);
            }
        }

        public async Task<Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>>> GetMultiple<T1, T2, T3, T4, T5, T6>(
            string sql,

            object parameters)
        {
            Console.WriteLine(sql + " " + parameters);

            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("connection")))
            {
                using var gridReader = await db.QueryMultipleAsync(sql, parameters, commandType: CommandType.StoredProcedure);
                IEnumerable<T1> t1 = Enumerable.Empty<T1>();
                IEnumerable<T2> t2 = Enumerable.Empty<T2>();
                IEnumerable<T3> t3 = Enumerable.Empty<T3>();
                IEnumerable<T4> t4 = Enumerable.Empty<T4>();
                IEnumerable<T5> t5 = Enumerable.Empty<T5>();
                IEnumerable<T6> t6 = Enumerable.Empty<T6>();

                if (!gridReader.IsConsumed)
                    t1 = await gridReader.ReadAsync<T1>();
                if (!gridReader.IsConsumed)
                    t2 = await gridReader.ReadAsync<T2>();
                if (!gridReader.IsConsumed)
                    t3 = await gridReader.ReadAsync<T3>();
                if (!gridReader.IsConsumed)
                    t4 = await gridReader.ReadAsync<T4>();
                if (!gridReader.IsConsumed)
                    t5 = await gridReader.ReadAsync<T5>();
                if (!gridReader.IsConsumed)
                    t6 = await gridReader.ReadAsync<T6>();
                return new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>>(t1, t2, t3, t4, t5, t6);
            }
        }

        public async Task<Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>, IEnumerable<T7>>> GetMultiple<T1, T2, T3, T4, T5, T6, T7>(
            string sql,

            object parameters)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("connection")))
            {
                using var gridReader = await db.QueryMultipleAsync(sql, parameters, commandType: CommandType.StoredProcedure);
                IEnumerable<T1> t1 = Enumerable.Empty<T1>();
                IEnumerable<T2> t2 = Enumerable.Empty<T2>();
                IEnumerable<T3> t3 = Enumerable.Empty<T3>();
                IEnumerable<T4> t4 = Enumerable.Empty<T4>();
                IEnumerable<T5> t5 = Enumerable.Empty<T5>();
                IEnumerable<T6> t6 = Enumerable.Empty<T6>();
                IEnumerable<T7> t7 = Enumerable.Empty<T7>();

                if (!gridReader.IsConsumed)
                    t1 = await gridReader.ReadAsync<T1>();
                if (!gridReader.IsConsumed)
                    t2 = await gridReader.ReadAsync<T2>();
                if (!gridReader.IsConsumed)
                    t3 = await gridReader.ReadAsync<T3>();
                if (!gridReader.IsConsumed)
                    t4 = await gridReader.ReadAsync<T4>();
                if (!gridReader.IsConsumed)
                    t5 = await gridReader.ReadAsync<T5>();
                if (!gridReader.IsConsumed)
                    t6 = await gridReader.ReadAsync<T6>();
                if (!gridReader.IsConsumed)
                    t7 = await gridReader.ReadAsync<T7>();
                return new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>, IEnumerable<T7>>(t1, t2, t3, t4, t5, t6, t7);
            }
        }

        public async Task<Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>, IEnumerable<T7>, Tuple<IEnumerable<T8>>>> GetMultiple<T1, T2, T3, T4, T5, T6, T7, T8>(
            string sql,

            object parameters)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("connection")))
            {
                using var gridReader = await db.QueryMultipleAsync(sql, parameters, commandType: CommandType.StoredProcedure);

                IEnumerable<T1> t1 = Enumerable.Empty<T1>();
                IEnumerable<T2> t2 = Enumerable.Empty<T2>();
                IEnumerable<T3> t3 = Enumerable.Empty<T3>();
                IEnumerable<T4> t4 = Enumerable.Empty<T4>();
                IEnumerable<T5> t5 = Enumerable.Empty<T5>();
                IEnumerable<T6> t6 = Enumerable.Empty<T6>();
                IEnumerable<T7> t7 = Enumerable.Empty<T7>();
                IEnumerable<T8> t8 = Enumerable.Empty<T8>();

                if (!gridReader.IsConsumed)
                    t1 = await gridReader.ReadAsync<T1>();
                if (!gridReader.IsConsumed)
                    t2 = await gridReader.ReadAsync<T2>();
                if (!gridReader.IsConsumed)
                    t3 = await gridReader.ReadAsync<T3>();
                if (!gridReader.IsConsumed)
                    t4 = await gridReader.ReadAsync<T4>();
                if (!gridReader.IsConsumed)
                    t5 = await gridReader.ReadAsync<T5>();
                if (!gridReader.IsConsumed)
                    t6 = await gridReader.ReadAsync<T6>();
                if (!gridReader.IsConsumed)
                    t7 = await gridReader.ReadAsync<T7>();
                if (!gridReader.IsConsumed)
                    t8 = await gridReader.ReadAsync<T8>();
                return new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>, IEnumerable<T7>, Tuple<IEnumerable<T8>>>
                (t1,
                 t2,
                 t3,
                 t4,
                 t5,
                 t6,
                 t7,
                 new Tuple<IEnumerable<T8>>(t8));
            }
        }

        public async Task<Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>, IEnumerable<T7>, Tuple<IEnumerable<T8>, IEnumerable<T9>>>> GetMultiple<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            string sql,

            object parameters)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("connection")))
            {
                using var gridReader = await db.QueryMultipleAsync(sql, parameters, commandType: CommandType.StoredProcedure);

                IEnumerable<T1> t1 = Enumerable.Empty<T1>();
                IEnumerable<T2> t2 = Enumerable.Empty<T2>();
                IEnumerable<T3> t3 = Enumerable.Empty<T3>();
                IEnumerable<T4> t4 = Enumerable.Empty<T4>();
                IEnumerable<T5> t5 = Enumerable.Empty<T5>();
                IEnumerable<T6> t6 = Enumerable.Empty<T6>();
                IEnumerable<T7> t7 = Enumerable.Empty<T7>();
                IEnumerable<T8> t8 = Enumerable.Empty<T8>();
                IEnumerable<T9> t9 = Enumerable.Empty<T9>();

                if (!gridReader.IsConsumed)
                    t1 = await gridReader.ReadAsync<T1>();
                if (!gridReader.IsConsumed)
                    t2 = await gridReader.ReadAsync<T2>();
                if (!gridReader.IsConsumed)
                    t3 = await gridReader.ReadAsync<T3>();
                if (!gridReader.IsConsumed)
                    t4 = await gridReader.ReadAsync<T4>();
                if (!gridReader.IsConsumed)
                    t5 = await gridReader.ReadAsync<T5>();
                if (!gridReader.IsConsumed)
                    t6 = await gridReader.ReadAsync<T6>();
                if (!gridReader.IsConsumed)
                    t7 = await gridReader.ReadAsync<T7>();
                if (!gridReader.IsConsumed)
                    t8 = await gridReader.ReadAsync<T8>();
                if (!gridReader.IsConsumed)
                    t9 = await gridReader.ReadAsync<T9>();
                return new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>, IEnumerable<T7>, Tuple<IEnumerable<T8>, IEnumerable<T9>>>(
                    t1,
                    t2,
                    t3,
                    t4,
                    t5,
                    t6,
                    t7,
                    new Tuple<IEnumerable<T8>, IEnumerable<T9>>(t8, t9));
            }
        }

        public async Task<Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>, IEnumerable<T7>, Tuple<IEnumerable<T8>, IEnumerable<T9>, IEnumerable<T10>>>> GetMultiple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            string sql,

            object parameters)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("connection")))
            {
                using var gridReader = await db.QueryMultipleAsync(sql, parameters, commandType: CommandType.StoredProcedure);

                IEnumerable<T1> t1 = Enumerable.Empty<T1>();
                IEnumerable<T2> t2 = Enumerable.Empty<T2>();
                IEnumerable<T3> t3 = Enumerable.Empty<T3>();
                IEnumerable<T4> t4 = Enumerable.Empty<T4>();
                IEnumerable<T5> t5 = Enumerable.Empty<T5>();
                IEnumerable<T6> t6 = Enumerable.Empty<T6>();
                IEnumerable<T7> t7 = Enumerable.Empty<T7>();
                IEnumerable<T8> t8 = Enumerable.Empty<T8>();
                IEnumerable<T9> t9 = Enumerable.Empty<T9>();
                IEnumerable<T10> t10 = Enumerable.Empty<T10>();

                if (!gridReader.IsConsumed)
                    t1 = await gridReader.ReadAsync<T1>();
                if (!gridReader.IsConsumed)
                    t2 = await gridReader.ReadAsync<T2>();
                if (!gridReader.IsConsumed)
                    t3 = await gridReader.ReadAsync<T3>();
                if (!gridReader.IsConsumed)
                    t4 = await gridReader.ReadAsync<T4>();
                if (!gridReader.IsConsumed)
                    t5 = await gridReader.ReadAsync<T5>();
                if (!gridReader.IsConsumed)
                    t6 = await gridReader.ReadAsync<T6>();
                if (!gridReader.IsConsumed)
                    t7 = await gridReader.ReadAsync<T7>();
                if (!gridReader.IsConsumed)
                    t8 = await gridReader.ReadAsync<T8>();
                if (!gridReader.IsConsumed)
                    t9 = await gridReader.ReadAsync<T9>();
                if (!gridReader.IsConsumed)
                    t10 = await gridReader.ReadAsync<T10>();
                return new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>, IEnumerable<T7>, Tuple<IEnumerable<T8>, IEnumerable<T9>, IEnumerable<T10>>>(
                    t1,
                    t2,
                    t3,
                    t4,
                    t5,
                    t6,
                    t7,
                    new Tuple<IEnumerable<T8>, IEnumerable<T9>, IEnumerable<T10>>(t8, t9, t10));
            }
        }

        public async Task<Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>, IEnumerable<T7>, Tuple<IEnumerable<T8>, IEnumerable<T9>, IEnumerable<T10>, IEnumerable<T11>>>> GetMultiple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            string sql,

            object parameters)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("connection")))
            {
                using var gridReader = await db.QueryMultipleAsync(sql, parameters, commandType: CommandType.StoredProcedure);

                IEnumerable<T1> t1 = Enumerable.Empty<T1>();
                IEnumerable<T2> t2 = Enumerable.Empty<T2>();
                IEnumerable<T3> t3 = Enumerable.Empty<T3>();
                IEnumerable<T4> t4 = Enumerable.Empty<T4>();
                IEnumerable<T5> t5 = Enumerable.Empty<T5>();
                IEnumerable<T6> t6 = Enumerable.Empty<T6>();
                IEnumerable<T7> t7 = Enumerable.Empty<T7>();
                IEnumerable<T8> t8 = Enumerable.Empty<T8>();
                IEnumerable<T9> t9 = Enumerable.Empty<T9>();
                IEnumerable<T10> t10 = Enumerable.Empty<T10>();
                IEnumerable<T11> t11 = Enumerable.Empty<T11>();

                if (!gridReader.IsConsumed)
                    t1 = await gridReader.ReadAsync<T1>();
                if (!gridReader.IsConsumed)
                    t2 = await gridReader.ReadAsync<T2>();
                if (!gridReader.IsConsumed)
                    t3 = await gridReader.ReadAsync<T3>();
                if (!gridReader.IsConsumed)
                    t4 = await gridReader.ReadAsync<T4>();
                if (!gridReader.IsConsumed)
                    t5 = await gridReader.ReadAsync<T5>();
                if (!gridReader.IsConsumed)
                    t6 = await gridReader.ReadAsync<T6>();
                if (!gridReader.IsConsumed)
                    t7 = await gridReader.ReadAsync<T7>();
                if (!gridReader.IsConsumed)
                    t8 = await gridReader.ReadAsync<T8>();
                if (!gridReader.IsConsumed)
                    t9 = await gridReader.ReadAsync<T9>();
                if (!gridReader.IsConsumed)
                    t10 = await gridReader.ReadAsync<T10>();
                if (!gridReader.IsConsumed)
                    t11 = await gridReader.ReadAsync<T11>();
                return new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>, IEnumerable<T7>, Tuple<IEnumerable<T8>, IEnumerable<T9>, IEnumerable<T10>, IEnumerable<T11>>>(
                    t1,
                    t2,
                    t3,
                    t4,
                    t5,
                    t6,
                    t7,
                    new Tuple<IEnumerable<T8>, IEnumerable<T9>, IEnumerable<T10>, IEnumerable<T11>>(t8, t9, t10, t11));
            }
        }

        public async Task<Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>, IEnumerable<T7>, Tuple<IEnumerable<T8>, IEnumerable<T9>, IEnumerable<T10>, IEnumerable<T11>, IEnumerable<T12>>>> GetMultiple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            string sql,

            object parameters)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("connection")))
            {
                using var gridReader = await db.QueryMultipleAsync(sql, parameters, commandType: CommandType.StoredProcedure);

                IEnumerable<T1> t1 = Enumerable.Empty<T1>();
                IEnumerable<T2> t2 = Enumerable.Empty<T2>();
                IEnumerable<T3> t3 = Enumerable.Empty<T3>();
                IEnumerable<T4> t4 = Enumerable.Empty<T4>();
                IEnumerable<T5> t5 = Enumerable.Empty<T5>();
                IEnumerable<T6> t6 = Enumerable.Empty<T6>();
                IEnumerable<T7> t7 = Enumerable.Empty<T7>();
                IEnumerable<T8> t8 = Enumerable.Empty<T8>();
                IEnumerable<T9> t9 = Enumerable.Empty<T9>();
                IEnumerable<T10> t10 = Enumerable.Empty<T10>();
                IEnumerable<T11> t11 = Enumerable.Empty<T11>();
                IEnumerable<T12> t12 = Enumerable.Empty<T12>();

                if (!gridReader.IsConsumed)
                    t1 = await gridReader.ReadAsync<T1>();
                if (!gridReader.IsConsumed)
                    t2 = await gridReader.ReadAsync<T2>();
                if (!gridReader.IsConsumed)
                    t3 = await gridReader.ReadAsync<T3>();
                if (!gridReader.IsConsumed)
                    t4 = await gridReader.ReadAsync<T4>();
                if (!gridReader.IsConsumed)
                    t5 = await gridReader.ReadAsync<T5>();
                if (!gridReader.IsConsumed)
                    t6 = await gridReader.ReadAsync<T6>();
                if (!gridReader.IsConsumed)
                    t7 = await gridReader.ReadAsync<T7>();
                if (!gridReader.IsConsumed)
                    t8 = await gridReader.ReadAsync<T8>();
                if (!gridReader.IsConsumed)
                    t9 = await gridReader.ReadAsync<T9>();
                if (!gridReader.IsConsumed)
                    t10 = await gridReader.ReadAsync<T10>();
                if (!gridReader.IsConsumed)
                    t11 = await gridReader.ReadAsync<T11>();
                if (!gridReader.IsConsumed)
                    t12 = await gridReader.ReadAsync<T12>();
                return new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>, IEnumerable<T7>, Tuple<IEnumerable<T8>, IEnumerable<T9>, IEnumerable<T10>, IEnumerable<T11>, IEnumerable<T12>>>(
                    t1,
                    t2,
                    t3,
                    t4,
                    t5,
                    t6,
                    t7,
                    new Tuple<IEnumerable<T8>, IEnumerable<T9>, IEnumerable<T10>, IEnumerable<T11>, IEnumerable<T12>>(t8, t9, t10, t11, t12));
            }
        }

        public async Task<Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>, IEnumerable<T7>, Tuple<IEnumerable<T8>, IEnumerable<T9>, IEnumerable<T10>, IEnumerable<T11>, IEnumerable<T12>, IEnumerable<T13>>>> GetMultiple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            string sql,

            object parameters)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("connection")))
            {
                using var gridReader = await db.QueryMultipleAsync(sql, parameters, commandType: CommandType.StoredProcedure);

                IEnumerable<T1> t1 = Enumerable.Empty<T1>();
                IEnumerable<T2> t2 = Enumerable.Empty<T2>();
                IEnumerable<T3> t3 = Enumerable.Empty<T3>();
                IEnumerable<T4> t4 = Enumerable.Empty<T4>();
                IEnumerable<T5> t5 = Enumerable.Empty<T5>();
                IEnumerable<T6> t6 = Enumerable.Empty<T6>();
                IEnumerable<T7> t7 = Enumerable.Empty<T7>();
                IEnumerable<T8> t8 = Enumerable.Empty<T8>();
                IEnumerable<T9> t9 = Enumerable.Empty<T9>();
                IEnumerable<T10> t10 = Enumerable.Empty<T10>();
                IEnumerable<T11> t11 = Enumerable.Empty<T11>();
                IEnumerable<T12> t12 = Enumerable.Empty<T12>();
                IEnumerable<T13> t13 = Enumerable.Empty<T13>();

                if (!gridReader.IsConsumed)
                    t1 = await gridReader.ReadAsync<T1>();
                if (!gridReader.IsConsumed)
                    t2 = await gridReader.ReadAsync<T2>();
                if (!gridReader.IsConsumed)
                    t3 = await gridReader.ReadAsync<T3>();
                if (!gridReader.IsConsumed)
                    t4 = await gridReader.ReadAsync<T4>();
                if (!gridReader.IsConsumed)
                    t5 = await gridReader.ReadAsync<T5>();
                if (!gridReader.IsConsumed)
                    t6 = await gridReader.ReadAsync<T6>();
                if (!gridReader.IsConsumed)
                    t7 = await gridReader.ReadAsync<T7>();
                if (!gridReader.IsConsumed)
                    t8 = await gridReader.ReadAsync<T8>();
                if (!gridReader.IsConsumed)
                    t9 = await gridReader.ReadAsync<T9>();
                if (!gridReader.IsConsumed)
                    t10 = await gridReader.ReadAsync<T10>();
                if (!gridReader.IsConsumed)
                    t11 = await gridReader.ReadAsync<T11>();
                if (!gridReader.IsConsumed)
                    t12 = await gridReader.ReadAsync<T12>();
                if (!gridReader.IsConsumed)
                    t13 = await gridReader.ReadAsync<T13>();
                return new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>, IEnumerable<T7>, Tuple<IEnumerable<T8>, IEnumerable<T9>, IEnumerable<T10>, IEnumerable<T11>, IEnumerable<T12>, IEnumerable<T13>>>(
                    t1,
                    t2,
                    t3,
                    t4,
                    t5,
                    t6,
                    t7,
                    new Tuple<IEnumerable<T8>, IEnumerable<T9>, IEnumerable<T10>, IEnumerable<T11>, IEnumerable<T12>, IEnumerable<T13>>(t8, t9, t10, t11, t12, t13));
            }
        }

        public async Task<Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>, IEnumerable<T7>, Tuple<IEnumerable<T8>, IEnumerable<T9>, IEnumerable<T10>, IEnumerable<T11>, IEnumerable<T12>, IEnumerable<T13>, IEnumerable<T14>>>> GetMultiple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            string sql,

            object parameters)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("connection")))
            {
                using var gridReader = await db.QueryMultipleAsync(sql, parameters, commandType: CommandType.StoredProcedure);

                IEnumerable<T1> t1 = Enumerable.Empty<T1>();
                IEnumerable<T2> t2 = Enumerable.Empty<T2>();
                IEnumerable<T3> t3 = Enumerable.Empty<T3>();
                IEnumerable<T4> t4 = Enumerable.Empty<T4>();
                IEnumerable<T5> t5 = Enumerable.Empty<T5>();
                IEnumerable<T6> t6 = Enumerable.Empty<T6>();
                IEnumerable<T7> t7 = Enumerable.Empty<T7>();
                IEnumerable<T8> t8 = Enumerable.Empty<T8>();
                IEnumerable<T9> t9 = Enumerable.Empty<T9>();
                IEnumerable<T10> t10 = Enumerable.Empty<T10>();
                IEnumerable<T11> t11 = Enumerable.Empty<T11>();
                IEnumerable<T12> t12 = Enumerable.Empty<T12>();
                IEnumerable<T13> t13 = Enumerable.Empty<T13>();
                IEnumerable<T14> t14 = Enumerable.Empty<T14>();

                if (!gridReader.IsConsumed)
                    t1 = await gridReader.ReadAsync<T1>();
                if (!gridReader.IsConsumed)
                    t2 = await gridReader.ReadAsync<T2>();
                if (!gridReader.IsConsumed)
                    t3 = await gridReader.ReadAsync<T3>();
                if (!gridReader.IsConsumed)
                    t4 = await gridReader.ReadAsync<T4>();
                if (!gridReader.IsConsumed)
                    t5 = await gridReader.ReadAsync<T5>();
                if (!gridReader.IsConsumed)
                    t6 = await gridReader.ReadAsync<T6>();
                if (!gridReader.IsConsumed)
                    t7 = await gridReader.ReadAsync<T7>();
                if (!gridReader.IsConsumed)
                    t8 = await gridReader.ReadAsync<T8>();
                if (!gridReader.IsConsumed)
                    t9 = await gridReader.ReadAsync<T9>();
                if (!gridReader.IsConsumed)
                    t10 = await gridReader.ReadAsync<T10>();
                if (!gridReader.IsConsumed)
                    t11 = await gridReader.ReadAsync<T11>();
                if (!gridReader.IsConsumed)
                    t12 = await gridReader.ReadAsync<T12>();
                if (!gridReader.IsConsumed)
                    t13 = await gridReader.ReadAsync<T13>();
                if (!gridReader.IsConsumed)
                    t14 = await gridReader.ReadAsync<T14>();
                return new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>, IEnumerable<T7>, Tuple<IEnumerable<T8>, IEnumerable<T9>, IEnumerable<T10>, IEnumerable<T11>, IEnumerable<T12>, IEnumerable<T13>, IEnumerable<T14>>>(
                    t1,
                    t2,
                    t3,
                    t4,
                    t5,
                    t6,
                    t7,
                    new Tuple<IEnumerable<T8>, IEnumerable<T9>, IEnumerable<T10>, IEnumerable<T11>, IEnumerable<T12>, IEnumerable<T13>, IEnumerable<T14>>(t8, t9, t10, t11, t12, t13, t14));
            }
        }

        public async Task<Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>, IEnumerable<T7>, Tuple<IEnumerable<T8>, IEnumerable<T9>, IEnumerable<T10>, IEnumerable<T11>, IEnumerable<T12>, IEnumerable<T13>, IEnumerable<T14>, Tuple<IEnumerable<T15>>>>> GetMultiple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            string sql,

            object parameters)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("connection")))
            {
                using var gridReader = await db.QueryMultipleAsync(sql, parameters, commandType: CommandType.StoredProcedure);

                IEnumerable<T1> t1 = Enumerable.Empty<T1>();
                IEnumerable<T2> t2 = Enumerable.Empty<T2>();
                IEnumerable<T3> t3 = Enumerable.Empty<T3>();
                IEnumerable<T4> t4 = Enumerable.Empty<T4>();
                IEnumerable<T5> t5 = Enumerable.Empty<T5>();
                IEnumerable<T6> t6 = Enumerable.Empty<T6>();
                IEnumerable<T7> t7 = Enumerable.Empty<T7>();
                IEnumerable<T8> t8 = Enumerable.Empty<T8>();
                IEnumerable<T9> t9 = Enumerable.Empty<T9>();
                IEnumerable<T10> t10 = Enumerable.Empty<T10>();
                IEnumerable<T11> t11 = Enumerable.Empty<T11>();
                IEnumerable<T12> t12 = Enumerable.Empty<T12>();
                IEnumerable<T13> t13 = Enumerable.Empty<T13>();
                IEnumerable<T14> t14 = Enumerable.Empty<T14>();
                IEnumerable<T15> t15 = Enumerable.Empty<T15>();

                if (!gridReader.IsConsumed)
                    t1 = await gridReader.ReadAsync<T1>();
                if (!gridReader.IsConsumed)
                    t2 = await gridReader.ReadAsync<T2>();
                if (!gridReader.IsConsumed)
                    t3 = await gridReader.ReadAsync<T3>();
                if (!gridReader.IsConsumed)
                    t4 = await gridReader.ReadAsync<T4>();
                if (!gridReader.IsConsumed)
                    t5 = await gridReader.ReadAsync<T5>();
                if (!gridReader.IsConsumed)
                    t6 = await gridReader.ReadAsync<T6>();
                if (!gridReader.IsConsumed)
                    t7 = await gridReader.ReadAsync<T7>();
                if (!gridReader.IsConsumed)
                    t8 = await gridReader.ReadAsync<T8>();
                if (!gridReader.IsConsumed)
                    t9 = await gridReader.ReadAsync<T9>();
                if (!gridReader.IsConsumed)
                    t10 = await gridReader.ReadAsync<T10>();
                if (!gridReader.IsConsumed)
                    t11 = await gridReader.ReadAsync<T11>();
                if (!gridReader.IsConsumed)
                    t12 = await gridReader.ReadAsync<T12>();
                if (!gridReader.IsConsumed)
                    t13 = await gridReader.ReadAsync<T13>();
                if (!gridReader.IsConsumed)
                    t14 = await gridReader.ReadAsync<T14>();
                if (!gridReader.IsConsumed)
                    t15 = await gridReader.ReadAsync<T15>();
                return new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>, IEnumerable<T7>, Tuple<IEnumerable<T8>, IEnumerable<T9>, IEnumerable<T10>, IEnumerable<T11>, IEnumerable<T12>, IEnumerable<T13>, IEnumerable<T14>, Tuple<IEnumerable<T15>>>>(
                    t1,
                    t2,
                    t3,
                    t4,
                    t5,
                    t6,
                    t7,
                    new Tuple<IEnumerable<T8>, IEnumerable<T9>, IEnumerable<T10>, IEnumerable<T11>, IEnumerable<T12>, IEnumerable<T13>, IEnumerable<T14>, Tuple<IEnumerable<T15>>>(
                        t8,
                        t9,
                        t10,
                        t11,
                        t12,
                        t13,
                        t14, new Tuple<IEnumerable<T15>>(t15)));
            }
        }

        public async Task<Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>, IEnumerable<T7>, Tuple<IEnumerable<T8>, IEnumerable<T9>, IEnumerable<T10>, IEnumerable<T11>, IEnumerable<T12>, IEnumerable<T13>, IEnumerable<T14>, Tuple<IEnumerable<T15>, IEnumerable<T16>>>>> GetMultiple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            string sql,

            object parameters)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("connection")))
            {
                using var gridReader = await db.QueryMultipleAsync(sql, parameters, commandType: CommandType.StoredProcedure);

                IEnumerable<T1> t1 = Enumerable.Empty<T1>();
                IEnumerable<T2> t2 = Enumerable.Empty<T2>();
                IEnumerable<T3> t3 = Enumerable.Empty<T3>();
                IEnumerable<T4> t4 = Enumerable.Empty<T4>();
                IEnumerable<T5> t5 = Enumerable.Empty<T5>();
                IEnumerable<T6> t6 = Enumerable.Empty<T6>();
                IEnumerable<T7> t7 = Enumerable.Empty<T7>();
                IEnumerable<T8> t8 = Enumerable.Empty<T8>();
                IEnumerable<T9> t9 = Enumerable.Empty<T9>();
                IEnumerable<T10> t10 = Enumerable.Empty<T10>();
                IEnumerable<T11> t11 = Enumerable.Empty<T11>();
                IEnumerable<T12> t12 = Enumerable.Empty<T12>();
                IEnumerable<T13> t13 = Enumerable.Empty<T13>();
                IEnumerable<T14> t14 = Enumerable.Empty<T14>();
                IEnumerable<T15> t15 = Enumerable.Empty<T15>();
                IEnumerable<T16> t16 = Enumerable.Empty<T16>();

                if (!gridReader.IsConsumed)
                    t1 = await gridReader.ReadAsync<T1>();
                if (!gridReader.IsConsumed)
                    t2 = await gridReader.ReadAsync<T2>();
                if (!gridReader.IsConsumed)
                    t3 = await gridReader.ReadAsync<T3>();
                if (!gridReader.IsConsumed)
                    t4 = await gridReader.ReadAsync<T4>();
                if (!gridReader.IsConsumed)
                    t5 = await gridReader.ReadAsync<T5>();
                if (!gridReader.IsConsumed)
                    t6 = await gridReader.ReadAsync<T6>();
                if (!gridReader.IsConsumed)
                    t7 = await gridReader.ReadAsync<T7>();
                if (!gridReader.IsConsumed)
                    t8 = await gridReader.ReadAsync<T8>();
                if (!gridReader.IsConsumed)
                    t9 = await gridReader.ReadAsync<T9>();
                if (!gridReader.IsConsumed)
                    t10 = await gridReader.ReadAsync<T10>();
                if (!gridReader.IsConsumed)
                    t11 = await gridReader.ReadAsync<T11>();
                if (!gridReader.IsConsumed)
                    t12 = await gridReader.ReadAsync<T12>();
                if (!gridReader.IsConsumed)
                    t13 = await gridReader.ReadAsync<T13>();
                if (!gridReader.IsConsumed)
                    t14 = await gridReader.ReadAsync<T14>();
                if (!gridReader.IsConsumed)
                    t15 = await gridReader.ReadAsync<T15>();
                if (!gridReader.IsConsumed)
                    t16 = await gridReader.ReadAsync<T16>();
                return new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>, IEnumerable<T7>, Tuple<IEnumerable<T8>, IEnumerable<T9>, IEnumerable<T10>, IEnumerable<T11>, IEnumerable<T12>, IEnumerable<T13>, IEnumerable<T14>, Tuple<IEnumerable<T15>, IEnumerable<T16>>>>(
                    t1,
                    t2,
                    t3,
                    t4,
                    t5,
                    t6,
                    t7,
                    new Tuple<IEnumerable<T8>, IEnumerable<T9>, IEnumerable<T10>, IEnumerable<T11>, IEnumerable<T12>, IEnumerable<T13>, IEnumerable<T14>, Tuple<IEnumerable<T15>, IEnumerable<T16>>>(
                        t8,
                        t9,
                        t10,
                        t11,
                        t12,
                        t13,
                        t14, new Tuple<IEnumerable<T15>, IEnumerable<T16>>(t15, t16)));
            }
        }

        public async Task<Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>, IEnumerable<T7>, Tuple<IEnumerable<T8>, IEnumerable<T9>, IEnumerable<T10>, IEnumerable<T11>, IEnumerable<T12>, IEnumerable<T13>, IEnumerable<T14>, Tuple<IEnumerable<T15>, IEnumerable<T16>, IEnumerable<T17>>>>> GetMultiple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(
            string sql,

            object parameters)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("connection")))
            {
                using var gridReader = await db.QueryMultipleAsync(sql, parameters, commandType: CommandType.StoredProcedure);

                IEnumerable<T1> t1 = Enumerable.Empty<T1>();
                IEnumerable<T2> t2 = Enumerable.Empty<T2>();
                IEnumerable<T3> t3 = Enumerable.Empty<T3>();
                IEnumerable<T4> t4 = Enumerable.Empty<T4>();
                IEnumerable<T5> t5 = Enumerable.Empty<T5>();
                IEnumerable<T6> t6 = Enumerable.Empty<T6>();
                IEnumerable<T7> t7 = Enumerable.Empty<T7>();
                IEnumerable<T8> t8 = Enumerable.Empty<T8>();
                IEnumerable<T9> t9 = Enumerable.Empty<T9>();
                IEnumerable<T10> t10 = Enumerable.Empty<T10>();
                IEnumerable<T11> t11 = Enumerable.Empty<T11>();
                IEnumerable<T12> t12 = Enumerable.Empty<T12>();
                IEnumerable<T13> t13 = Enumerable.Empty<T13>();
                IEnumerable<T14> t14 = Enumerable.Empty<T14>();
                IEnumerable<T15> t15 = Enumerable.Empty<T15>();
                IEnumerable<T16> t16 = Enumerable.Empty<T16>();
                IEnumerable<T17> t17 = Enumerable.Empty<T17>();

                if (!gridReader.IsConsumed)
                    t1 = await gridReader.ReadAsync<T1>();
                if (!gridReader.IsConsumed)
                    t2 = await gridReader.ReadAsync<T2>();
                if (!gridReader.IsConsumed)
                    t3 = await gridReader.ReadAsync<T3>();
                if (!gridReader.IsConsumed)
                    t4 = await gridReader.ReadAsync<T4>();
                if (!gridReader.IsConsumed)
                    t5 = await gridReader.ReadAsync<T5>();
                if (!gridReader.IsConsumed)
                    t6 = await gridReader.ReadAsync<T6>();
                if (!gridReader.IsConsumed)
                    t7 = await gridReader.ReadAsync<T7>();
                if (!gridReader.IsConsumed)
                    t8 = await gridReader.ReadAsync<T8>();
                if (!gridReader.IsConsumed)
                    t9 = await gridReader.ReadAsync<T9>();
                if (!gridReader.IsConsumed)
                    t10 = await gridReader.ReadAsync<T10>();
                if (!gridReader.IsConsumed)
                    t11 = await gridReader.ReadAsync<T11>();
                if (!gridReader.IsConsumed)
                    t12 = await gridReader.ReadAsync<T12>();
                if (!gridReader.IsConsumed)
                    t13 = await gridReader.ReadAsync<T13>();
                if (!gridReader.IsConsumed)
                    t14 = await gridReader.ReadAsync<T14>();
                if (!gridReader.IsConsumed)
                    t15 = await gridReader.ReadAsync<T15>();
                if (!gridReader.IsConsumed)
                    t16 = await gridReader.ReadAsync<T16>();
                if (!gridReader.IsConsumed)
                    t17 = await gridReader.ReadAsync<T17>();
                return new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>, IEnumerable<T7>, Tuple<IEnumerable<T8>, IEnumerable<T9>, IEnumerable<T10>, IEnumerable<T11>, IEnumerable<T12>, IEnumerable<T13>, IEnumerable<T14>, Tuple<IEnumerable<T15>, IEnumerable<T16>, IEnumerable<T17>>>>(
                    t1,
                    t2,
                    t3,
                    t4,
                    t5,
                    t6,
                    t7,
                    new Tuple<IEnumerable<T8>, IEnumerable<T9>, IEnumerable<T10>, IEnumerable<T11>, IEnumerable<T12>, IEnumerable<T13>, IEnumerable<T14>, Tuple<IEnumerable<T15>, IEnumerable<T16>, IEnumerable<T17>>>(
                        t8,
                        t9,
                        t10,
                        t11,
                        t12,
                        t13,
                        t14, new Tuple<IEnumerable<T15>, IEnumerable<T16>, IEnumerable<T17>>(t15, t16, t17)));
            }
        }

        public async Task<Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>, IEnumerable<T7>, Tuple<IEnumerable<T8>, IEnumerable<T9>, IEnumerable<T10>, IEnumerable<T11>, IEnumerable<T12>, IEnumerable<T13>, IEnumerable<T14>, Tuple<IEnumerable<T15>, IEnumerable<T16>, IEnumerable<T17>, IEnumerable<T18>>>>> GetMultiple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(
            string sql,

            object parameters)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("connection")))
            {
                using var gridReader = await db.QueryMultipleAsync(sql, parameters, commandType: CommandType.StoredProcedure);

                IEnumerable<T1> t1 = Enumerable.Empty<T1>();
                IEnumerable<T2> t2 = Enumerable.Empty<T2>();
                IEnumerable<T3> t3 = Enumerable.Empty<T3>();
                IEnumerable<T4> t4 = Enumerable.Empty<T4>();
                IEnumerable<T5> t5 = Enumerable.Empty<T5>();
                IEnumerable<T6> t6 = Enumerable.Empty<T6>();
                IEnumerable<T7> t7 = Enumerable.Empty<T7>();
                IEnumerable<T8> t8 = Enumerable.Empty<T8>();
                IEnumerable<T9> t9 = Enumerable.Empty<T9>();
                IEnumerable<T10> t10 = Enumerable.Empty<T10>();
                IEnumerable<T11> t11 = Enumerable.Empty<T11>();
                IEnumerable<T12> t12 = Enumerable.Empty<T12>();
                IEnumerable<T13> t13 = Enumerable.Empty<T13>();
                IEnumerable<T14> t14 = Enumerable.Empty<T14>();
                IEnumerable<T15> t15 = Enumerable.Empty<T15>();
                IEnumerable<T16> t16 = Enumerable.Empty<T16>();
                IEnumerable<T17> t17 = Enumerable.Empty<T17>();
                IEnumerable<T18> t18 = Enumerable.Empty<T18>();

                if (!gridReader.IsConsumed)
                    t1 = await gridReader.ReadAsync<T1>();
                if (!gridReader.IsConsumed)
                    t2 = await gridReader.ReadAsync<T2>();
                if (!gridReader.IsConsumed)
                    t3 = await gridReader.ReadAsync<T3>();
                if (!gridReader.IsConsumed)
                    t4 = await gridReader.ReadAsync<T4>();
                if (!gridReader.IsConsumed)
                    t5 = await gridReader.ReadAsync<T5>();
                if (!gridReader.IsConsumed)
                    t6 = await gridReader.ReadAsync<T6>();
                if (!gridReader.IsConsumed)
                    t7 = await gridReader.ReadAsync<T7>();
                if (!gridReader.IsConsumed)
                    t8 = await gridReader.ReadAsync<T8>();
                if (!gridReader.IsConsumed)
                    t9 = await gridReader.ReadAsync<T9>();
                if (!gridReader.IsConsumed)
                    t10 = await gridReader.ReadAsync<T10>();
                if (!gridReader.IsConsumed)
                    t11 = await gridReader.ReadAsync<T11>();
                if (!gridReader.IsConsumed)
                    t12 = await gridReader.ReadAsync<T12>();
                if (!gridReader.IsConsumed)
                    t13 = await gridReader.ReadAsync<T13>();
                if (!gridReader.IsConsumed)
                    t14 = await gridReader.ReadAsync<T14>();
                if (!gridReader.IsConsumed)
                    t15 = await gridReader.ReadAsync<T15>();
                if (!gridReader.IsConsumed)
                    t16 = await gridReader.ReadAsync<T16>();
                if (!gridReader.IsConsumed)
                    t17 = await gridReader.ReadAsync<T17>();
                if (!gridReader.IsConsumed)
                    t18 = await gridReader.ReadAsync<T18>();
                return new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>, IEnumerable<T7>, Tuple<IEnumerable<T8>, IEnumerable<T9>, IEnumerable<T10>, IEnumerable<T11>, IEnumerable<T12>, IEnumerable<T13>, IEnumerable<T14>, Tuple<IEnumerable<T15>, IEnumerable<T16>, IEnumerable<T17>, IEnumerable<T18>>>>(
                    t1,
                    t2,
                    t3,
                    t4,
                    t5,
                    t6,
                    t7,
                    new Tuple<IEnumerable<T8>, IEnumerable<T9>, IEnumerable<T10>, IEnumerable<T11>, IEnumerable<T12>, IEnumerable<T13>, IEnumerable<T14>, Tuple<IEnumerable<T15>, IEnumerable<T16>, IEnumerable<T17>, IEnumerable<T18>>>(
                        t8,
                        t9,
                        t10,
                        t11,
                        t12,
                        t13,
                        t14, new Tuple<IEnumerable<T15>, IEnumerable<T16>, IEnumerable<T17>, IEnumerable<T18>>(t15, t16, t17, t18)));
            }
        }

        public async Task<Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>, IEnumerable<T7>, Tuple<IEnumerable<T8>, IEnumerable<T9>, IEnumerable<T10>, IEnumerable<T11>, IEnumerable<T12>, IEnumerable<T13>, IEnumerable<T14>, Tuple<IEnumerable<T15>, IEnumerable<T16>, IEnumerable<T17>, IEnumerable<T18>, IEnumerable<T19>>>>> GetMultiple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(
            string sql,

            object parameters)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("connection")))
            {
                using var gridReader = await db.QueryMultipleAsync(sql, parameters, commandType: CommandType.StoredProcedure);

                IEnumerable<T1> t1 = Enumerable.Empty<T1>();
                IEnumerable<T2> t2 = Enumerable.Empty<T2>();
                IEnumerable<T3> t3 = Enumerable.Empty<T3>();
                IEnumerable<T4> t4 = Enumerable.Empty<T4>();
                IEnumerable<T5> t5 = Enumerable.Empty<T5>();
                IEnumerable<T6> t6 = Enumerable.Empty<T6>();
                IEnumerable<T7> t7 = Enumerable.Empty<T7>();
                IEnumerable<T8> t8 = Enumerable.Empty<T8>();
                IEnumerable<T9> t9 = Enumerable.Empty<T9>();
                IEnumerable<T10> t10 = Enumerable.Empty<T10>();
                IEnumerable<T11> t11 = Enumerable.Empty<T11>();
                IEnumerable<T12> t12 = Enumerable.Empty<T12>();
                IEnumerable<T13> t13 = Enumerable.Empty<T13>();
                IEnumerable<T14> t14 = Enumerable.Empty<T14>();
                IEnumerable<T15> t15 = Enumerable.Empty<T15>();
                IEnumerable<T16> t16 = Enumerable.Empty<T16>();
                IEnumerable<T17> t17 = Enumerable.Empty<T17>();
                IEnumerable<T18> t18 = Enumerable.Empty<T18>();
                IEnumerable<T19> t19 = Enumerable.Empty<T19>();

                if (!gridReader.IsConsumed)
                    t1 = await gridReader.ReadAsync<T1>();
                if (!gridReader.IsConsumed)
                    t2 = await gridReader.ReadAsync<T2>();
                if (!gridReader.IsConsumed)
                    t3 = await gridReader.ReadAsync<T3>();
                if (!gridReader.IsConsumed)
                    t4 = await gridReader.ReadAsync<T4>();
                if (!gridReader.IsConsumed)
                    t5 = await gridReader.ReadAsync<T5>();
                if (!gridReader.IsConsumed)
                    t6 = await gridReader.ReadAsync<T6>();
                if (!gridReader.IsConsumed)
                    t7 = await gridReader.ReadAsync<T7>();
                if (!gridReader.IsConsumed)
                    t8 = await gridReader.ReadAsync<T8>();
                if (!gridReader.IsConsumed)
                    t9 = await gridReader.ReadAsync<T9>();
                if (!gridReader.IsConsumed)
                    t10 = await gridReader.ReadAsync<T10>();
                if (!gridReader.IsConsumed)
                    t11 = await gridReader.ReadAsync<T11>();
                if (!gridReader.IsConsumed)
                    t12 = await gridReader.ReadAsync<T12>();
                if (!gridReader.IsConsumed)
                    t13 = await gridReader.ReadAsync<T13>();
                if (!gridReader.IsConsumed)
                    t14 = await gridReader.ReadAsync<T14>();
                if (!gridReader.IsConsumed)
                    t15 = await gridReader.ReadAsync<T15>();
                if (!gridReader.IsConsumed)
                    t16 = await gridReader.ReadAsync<T16>();
                if (!gridReader.IsConsumed)
                    t17 = await gridReader.ReadAsync<T17>();
                if (!gridReader.IsConsumed)
                    t18 = await gridReader.ReadAsync<T18>();
                if (!gridReader.IsConsumed)
                    t19 = await gridReader.ReadAsync<T19>();
                return new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>, IEnumerable<T7>, Tuple<IEnumerable<T8>, IEnumerable<T9>, IEnumerable<T10>, IEnumerable<T11>, IEnumerable<T12>, IEnumerable<T13>, IEnumerable<T14>, Tuple<IEnumerable<T15>, IEnumerable<T16>, IEnumerable<T17>, IEnumerable<T18>, IEnumerable<T19>>>>(
                    t1,
                    t2,
                    t3,
                    t4,
                    t5,
                    t6,
                    t7,
                    new Tuple<IEnumerable<T8>, IEnumerable<T9>, IEnumerable<T10>, IEnumerable<T11>, IEnumerable<T12>, IEnumerable<T13>, IEnumerable<T14>, Tuple<IEnumerable<T15>, IEnumerable<T16>, IEnumerable<T17>, IEnumerable<T18>, IEnumerable<T19>>>(
                        t8,
                        t9,
                        t10,
                        t11,
                        t12,
                        t13,
                        t14, new Tuple<IEnumerable<T15>, IEnumerable<T16>, IEnumerable<T17>, IEnumerable<T18>, IEnumerable<T19>>(t15, t16, t17, t18, t19)));
            }
        }

        public async Task<Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>, IEnumerable<T7>, Tuple<IEnumerable<T8>, IEnumerable<T9>, IEnumerable<T10>, IEnumerable<T11>, IEnumerable<T12>, IEnumerable<T13>, IEnumerable<T14>, Tuple<IEnumerable<T15>, IEnumerable<T16>, IEnumerable<T17>, IEnumerable<T18>, IEnumerable<T19>, IEnumerable<T20>>>>> GetMultiple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(
            string sql,

            object parameters)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("connection")))
            {
                using var gridReader = await db.QueryMultipleAsync(sql, parameters, commandType: CommandType.StoredProcedure);

                IEnumerable<T1> t1 = Enumerable.Empty<T1>();
                IEnumerable<T2> t2 = Enumerable.Empty<T2>();
                IEnumerable<T3> t3 = Enumerable.Empty<T3>();
                IEnumerable<T4> t4 = Enumerable.Empty<T4>();
                IEnumerable<T5> t5 = Enumerable.Empty<T5>();
                IEnumerable<T6> t6 = Enumerable.Empty<T6>();
                IEnumerable<T7> t7 = Enumerable.Empty<T7>();
                IEnumerable<T8> t8 = Enumerable.Empty<T8>();
                IEnumerable<T9> t9 = Enumerable.Empty<T9>();
                IEnumerable<T10> t10 = Enumerable.Empty<T10>();
                IEnumerable<T11> t11 = Enumerable.Empty<T11>();
                IEnumerable<T12> t12 = Enumerable.Empty<T12>();
                IEnumerable<T13> t13 = Enumerable.Empty<T13>();
                IEnumerable<T14> t14 = Enumerable.Empty<T14>();
                IEnumerable<T15> t15 = Enumerable.Empty<T15>();
                IEnumerable<T16> t16 = Enumerable.Empty<T16>();
                IEnumerable<T17> t17 = Enumerable.Empty<T17>();
                IEnumerable<T18> t18 = Enumerable.Empty<T18>();
                IEnumerable<T19> t19 = Enumerable.Empty<T19>();
                IEnumerable<T20> t20 = Enumerable.Empty<T20>();

                if (!gridReader.IsConsumed)
                    t1 = await gridReader.ReadAsync<T1>();
                if (!gridReader.IsConsumed)
                    t2 = await gridReader.ReadAsync<T2>();
                if (!gridReader.IsConsumed)
                    t3 = await gridReader.ReadAsync<T3>();
                if (!gridReader.IsConsumed)
                    t4 = await gridReader.ReadAsync<T4>();
                if (!gridReader.IsConsumed)
                    t5 = await gridReader.ReadAsync<T5>();
                if (!gridReader.IsConsumed)
                    t6 = await gridReader.ReadAsync<T6>();
                if (!gridReader.IsConsumed)
                    t7 = await gridReader.ReadAsync<T7>();
                if (!gridReader.IsConsumed)
                    t8 = await gridReader.ReadAsync<T8>();
                if (!gridReader.IsConsumed)
                    t9 = await gridReader.ReadAsync<T9>();
                if (!gridReader.IsConsumed)
                    t10 = await gridReader.ReadAsync<T10>();
                if (!gridReader.IsConsumed)
                    t11 = await gridReader.ReadAsync<T11>();
                if (!gridReader.IsConsumed)
                    t12 = await gridReader.ReadAsync<T12>();
                if (!gridReader.IsConsumed)
                    t13 = await gridReader.ReadAsync<T13>();
                if (!gridReader.IsConsumed)
                    t14 = await gridReader.ReadAsync<T14>();
                if (!gridReader.IsConsumed)
                    t15 = await gridReader.ReadAsync<T15>();
                if (!gridReader.IsConsumed)
                    t16 = await gridReader.ReadAsync<T16>();
                if (!gridReader.IsConsumed)
                    t17 = await gridReader.ReadAsync<T17>();
                if (!gridReader.IsConsumed)
                    t18 = await gridReader.ReadAsync<T18>();
                if (!gridReader.IsConsumed)
                    t19 = await gridReader.ReadAsync<T19>();
                if (!gridReader.IsConsumed)
                    t20 = await gridReader.ReadAsync<T20>();
                return new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>, IEnumerable<T7>, Tuple<IEnumerable<T8>, IEnumerable<T9>, IEnumerable<T10>, IEnumerable<T11>, IEnumerable<T12>, IEnumerable<T13>, IEnumerable<T14>, Tuple<IEnumerable<T15>, IEnumerable<T16>, IEnumerable<T17>, IEnumerable<T18>, IEnumerable<T19>, IEnumerable<T20>>>>(
                    t1,
                    t2,
                    t3,
                    t4,
                    t5,
                    t6,
                    t7,
                    new Tuple<IEnumerable<T8>, IEnumerable<T9>, IEnumerable<T10>, IEnumerable<T11>, IEnumerable<T12>, IEnumerable<T13>, IEnumerable<T14>, Tuple<IEnumerable<T15>, IEnumerable<T16>, IEnumerable<T17>, IEnumerable<T18>, IEnumerable<T19>, IEnumerable<T20>>>(
                        t8,
                        t9,
                        t10,
                        t11,
                        t12,
                        t13,
                        t14, new Tuple<IEnumerable<T15>, IEnumerable<T16>, IEnumerable<T17>, IEnumerable<T18>, IEnumerable<T19>, IEnumerable<T20>>(t15, t16, t17, t18, t19, t20)));
            }
        }

        public async Task<Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>, IEnumerable<T7>, Tuple<IEnumerable<T8>, IEnumerable<T9>, IEnumerable<T10>, IEnumerable<T11>, IEnumerable<T12>, IEnumerable<T13>, IEnumerable<T14>, Tuple<IEnumerable<T15>, IEnumerable<T16>, IEnumerable<T17>, IEnumerable<T18>, IEnumerable<T19>, IEnumerable<T20>, IEnumerable<T21>>>>> GetMultiple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>(
            string sql,

            object parameters)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("connection")))
            {
                using var gridReader = await db.QueryMultipleAsync(sql, parameters, commandType: CommandType.StoredProcedure);

                IEnumerable<T1> t1 = Enumerable.Empty<T1>();
                IEnumerable<T2> t2 = Enumerable.Empty<T2>();
                IEnumerable<T3> t3 = Enumerable.Empty<T3>();
                IEnumerable<T4> t4 = Enumerable.Empty<T4>();
                IEnumerable<T5> t5 = Enumerable.Empty<T5>();
                IEnumerable<T6> t6 = Enumerable.Empty<T6>();
                IEnumerable<T7> t7 = Enumerable.Empty<T7>();
                IEnumerable<T8> t8 = Enumerable.Empty<T8>();
                IEnumerable<T9> t9 = Enumerable.Empty<T9>();
                IEnumerable<T10> t10 = Enumerable.Empty<T10>();
                IEnumerable<T11> t11 = Enumerable.Empty<T11>();
                IEnumerable<T12> t12 = Enumerable.Empty<T12>();
                IEnumerable<T13> t13 = Enumerable.Empty<T13>();
                IEnumerable<T14> t14 = Enumerable.Empty<T14>();
                IEnumerable<T15> t15 = Enumerable.Empty<T15>();
                IEnumerable<T16> t16 = Enumerable.Empty<T16>();
                IEnumerable<T17> t17 = Enumerable.Empty<T17>();
                IEnumerable<T18> t18 = Enumerable.Empty<T18>();
                IEnumerable<T19> t19 = Enumerable.Empty<T19>();
                IEnumerable<T20> t20 = Enumerable.Empty<T20>();
                IEnumerable<T21> t21 = Enumerable.Empty<T21>();

                if (!gridReader.IsConsumed)
                    t1 = await gridReader.ReadAsync<T1>();
                if (!gridReader.IsConsumed)
                    t2 = await gridReader.ReadAsync<T2>();
                if (!gridReader.IsConsumed)
                    t3 = await gridReader.ReadAsync<T3>();
                if (!gridReader.IsConsumed)
                    t4 = await gridReader.ReadAsync<T4>();
                if (!gridReader.IsConsumed)
                    t5 = await gridReader.ReadAsync<T5>();
                if (!gridReader.IsConsumed)
                    t6 = await gridReader.ReadAsync<T6>();
                if (!gridReader.IsConsumed)
                    t7 = await gridReader.ReadAsync<T7>();
                if (!gridReader.IsConsumed)
                    t8 = await gridReader.ReadAsync<T8>();
                if (!gridReader.IsConsumed)
                    t9 = await gridReader.ReadAsync<T9>();
                if (!gridReader.IsConsumed)
                    t10 = await gridReader.ReadAsync<T10>();
                if (!gridReader.IsConsumed)
                    t11 = await gridReader.ReadAsync<T11>();
                if (!gridReader.IsConsumed)
                    t12 = await gridReader.ReadAsync<T12>();
                if (!gridReader.IsConsumed)
                    t13 = await gridReader.ReadAsync<T13>();
                if (!gridReader.IsConsumed)
                    t14 = await gridReader.ReadAsync<T14>();
                if (!gridReader.IsConsumed)
                    t15 = await gridReader.ReadAsync<T15>();
                if (!gridReader.IsConsumed)
                    t16 = await gridReader.ReadAsync<T16>();
                if (!gridReader.IsConsumed)
                    t17 = await gridReader.ReadAsync<T17>();
                if (!gridReader.IsConsumed)
                    t18 = await gridReader.ReadAsync<T18>();
                if (!gridReader.IsConsumed)
                    t19 = await gridReader.ReadAsync<T19>();
                if (!gridReader.IsConsumed)
                    t20 = await gridReader.ReadAsync<T20>();
                if (!gridReader.IsConsumed)
                    t21 = await gridReader.ReadAsync<T21>();
                return new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>, IEnumerable<T7>, Tuple<IEnumerable<T8>, IEnumerable<T9>, IEnumerable<T10>, IEnumerable<T11>, IEnumerable<T12>, IEnumerable<T13>, IEnumerable<T14>, Tuple<IEnumerable<T15>, IEnumerable<T16>, IEnumerable<T17>, IEnumerable<T18>, IEnumerable<T19>, IEnumerable<T20>, IEnumerable<T21>>>>(
                    t1,
                    t2,
                    t3,
                    t4,
                    t5,
                    t6,
                    t7,
                    new Tuple<IEnumerable<T8>, IEnumerable<T9>, IEnumerable<T10>, IEnumerable<T11>, IEnumerable<T12>, IEnumerable<T13>, IEnumerable<T14>, Tuple<IEnumerable<T15>, IEnumerable<T16>, IEnumerable<T17>, IEnumerable<T18>, IEnumerable<T19>, IEnumerable<T20>, IEnumerable<T21>>>(
                        t8,
                        t9,
                        t10,
                        t11,
                        t12,
                        t13,
                        t14, new Tuple<IEnumerable<T15>, IEnumerable<T16>, IEnumerable<T17>, IEnumerable<T18>, IEnumerable<T19>, IEnumerable<T20>, IEnumerable<T21>>(t15, t16, t17, t18, t19, t20, t21)));
            }
        }

        public async Task<Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>, IEnumerable<T7>, Tuple<IEnumerable<T8>, IEnumerable<T9>, IEnumerable<T10>, IEnumerable<T11>, IEnumerable<T12>, IEnumerable<T13>, IEnumerable<T14>, Tuple<IEnumerable<T15>, IEnumerable<T16>, IEnumerable<T17>, IEnumerable<T18>, IEnumerable<T19>, IEnumerable<T20>, IEnumerable<T21>, Tuple<IEnumerable<T22>>>>>> GetMultiple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>(
            string sql,

            object parameters)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("connection")))
            {
                using var gridReader = await db.QueryMultipleAsync(sql, parameters, commandType: CommandType.StoredProcedure);

                IEnumerable<T1> t1 = Enumerable.Empty<T1>();
                IEnumerable<T2> t2 = Enumerable.Empty<T2>();
                IEnumerable<T3> t3 = Enumerable.Empty<T3>();
                IEnumerable<T4> t4 = Enumerable.Empty<T4>();
                IEnumerable<T5> t5 = Enumerable.Empty<T5>();
                IEnumerable<T6> t6 = Enumerable.Empty<T6>();
                IEnumerable<T7> t7 = Enumerable.Empty<T7>();
                IEnumerable<T8> t8 = Enumerable.Empty<T8>();
                IEnumerable<T9> t9 = Enumerable.Empty<T9>();
                IEnumerable<T10> t10 = Enumerable.Empty<T10>();
                IEnumerable<T11> t11 = Enumerable.Empty<T11>();
                IEnumerable<T12> t12 = Enumerable.Empty<T12>();
                IEnumerable<T13> t13 = Enumerable.Empty<T13>();
                IEnumerable<T14> t14 = Enumerable.Empty<T14>();
                IEnumerable<T15> t15 = Enumerable.Empty<T15>();
                IEnumerable<T16> t16 = Enumerable.Empty<T16>();
                IEnumerable<T17> t17 = Enumerable.Empty<T17>();
                IEnumerable<T18> t18 = Enumerable.Empty<T18>();
                IEnumerable<T19> t19 = Enumerable.Empty<T19>();
                IEnumerable<T20> t20 = Enumerable.Empty<T20>();
                IEnumerable<T21> t21 = Enumerable.Empty<T21>();
                IEnumerable<T22> t22 = Enumerable.Empty<T22>();

                if (!gridReader.IsConsumed)
                    t1 = await gridReader.ReadAsync<T1>();
                if (!gridReader.IsConsumed)
                    t2 = await gridReader.ReadAsync<T2>();
                if (!gridReader.IsConsumed)
                    t3 = await gridReader.ReadAsync<T3>();
                if (!gridReader.IsConsumed)
                    t4 = await gridReader.ReadAsync<T4>();
                if (!gridReader.IsConsumed)
                    t5 = await gridReader.ReadAsync<T5>();
                if (!gridReader.IsConsumed)
                    t6 = await gridReader.ReadAsync<T6>();
                if (!gridReader.IsConsumed)
                    t7 = await gridReader.ReadAsync<T7>();
                if (!gridReader.IsConsumed)
                    t8 = await gridReader.ReadAsync<T8>();
                if (!gridReader.IsConsumed)
                    t9 = await gridReader.ReadAsync<T9>();
                if (!gridReader.IsConsumed)
                    t10 = await gridReader.ReadAsync<T10>();
                if (!gridReader.IsConsumed)
                    t11 = await gridReader.ReadAsync<T11>();
                if (!gridReader.IsConsumed)
                    t12 = await gridReader.ReadAsync<T12>();
                if (!gridReader.IsConsumed)
                    t13 = await gridReader.ReadAsync<T13>();
                if (!gridReader.IsConsumed)
                    t14 = await gridReader.ReadAsync<T14>();
                if (!gridReader.IsConsumed)
                    t15 = await gridReader.ReadAsync<T15>();
                if (!gridReader.IsConsumed)
                    t16 = await gridReader.ReadAsync<T16>();
                if (!gridReader.IsConsumed)
                    t17 = await gridReader.ReadAsync<T17>();
                if (!gridReader.IsConsumed)
                    t18 = await gridReader.ReadAsync<T18>();
                if (!gridReader.IsConsumed)
                    t19 = await gridReader.ReadAsync<T19>();
                if (!gridReader.IsConsumed)
                    t20 = await gridReader.ReadAsync<T20>();
                if (!gridReader.IsConsumed)
                    t21 = await gridReader.ReadAsync<T21>();
                if (!gridReader.IsConsumed)
                    t22 = await gridReader.ReadAsync<T22>();
                return new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>, IEnumerable<T7>, Tuple<IEnumerable<T8>, IEnumerable<T9>, IEnumerable<T10>, IEnumerable<T11>, IEnumerable<T12>, IEnumerable<T13>, IEnumerable<T14>, Tuple<IEnumerable<T15>, IEnumerable<T16>, IEnumerable<T17>, IEnumerable<T18>, IEnumerable<T19>, IEnumerable<T20>, IEnumerable<T21>, Tuple<IEnumerable<T22>>>>>(
                    t1,
                    t2,
                    t3,
                    t4,
                    t5,
                    t6,
                    t7,
                    new Tuple<IEnumerable<T8>, IEnumerable<T9>, IEnumerable<T10>, IEnumerable<T11>, IEnumerable<T12>, IEnumerable<T13>, IEnumerable<T14>, Tuple<IEnumerable<T15>, IEnumerable<T16>, IEnumerable<T17>, IEnumerable<T18>, IEnumerable<T19>, IEnumerable<T20>, IEnumerable<T21>, Tuple<IEnumerable<T22>>>>(
                        t8,
                        t9,
                        t10,
                        t11,
                        t12,
                        t13,
                        t14,
                         new Tuple<IEnumerable<T15>, IEnumerable<T16>, IEnumerable<T17>, IEnumerable<T18>, IEnumerable<T19>, IEnumerable<T20>, IEnumerable<T21>, Tuple<IEnumerable<T22>>>(
                             t15,
                             t16,
                             t17,
                             t18,
                             t19,
                             t20,
                             t21,
                             new Tuple<IEnumerable<T22>>(t22))));
            }
        }

        public async Task<Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>, IEnumerable<T7>, Tuple<IEnumerable<T8>, IEnumerable<T9>, IEnumerable<T10>, IEnumerable<T11>, IEnumerable<T12>, IEnumerable<T13>, IEnumerable<T14>, Tuple<IEnumerable<T15>, IEnumerable<T16>, IEnumerable<T17>, IEnumerable<T18>, IEnumerable<T19>, IEnumerable<T20>, IEnumerable<T21>, Tuple<IEnumerable<T22>, IEnumerable<T23>>>>>> GetMultiple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>(
            string sql,

            object parameters)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("connection")))
            {
                using var gridReader = await db.QueryMultipleAsync(sql, parameters, commandType: CommandType.StoredProcedure);

                IEnumerable<T1> t1 = Enumerable.Empty<T1>();
                IEnumerable<T2> t2 = Enumerable.Empty<T2>();
                IEnumerable<T3> t3 = Enumerable.Empty<T3>();
                IEnumerable<T4> t4 = Enumerable.Empty<T4>();
                IEnumerable<T5> t5 = Enumerable.Empty<T5>();
                IEnumerable<T6> t6 = Enumerable.Empty<T6>();
                IEnumerable<T7> t7 = Enumerable.Empty<T7>();
                IEnumerable<T8> t8 = Enumerable.Empty<T8>();
                IEnumerable<T9> t9 = Enumerable.Empty<T9>();
                IEnumerable<T10> t10 = Enumerable.Empty<T10>();
                IEnumerable<T11> t11 = Enumerable.Empty<T11>();
                IEnumerable<T12> t12 = Enumerable.Empty<T12>();
                IEnumerable<T13> t13 = Enumerable.Empty<T13>();
                IEnumerable<T14> t14 = Enumerable.Empty<T14>();
                IEnumerable<T15> t15 = Enumerable.Empty<T15>();
                IEnumerable<T16> t16 = Enumerable.Empty<T16>();
                IEnumerable<T17> t17 = Enumerable.Empty<T17>();
                IEnumerable<T18> t18 = Enumerable.Empty<T18>();
                IEnumerable<T19> t19 = Enumerable.Empty<T19>();
                IEnumerable<T20> t20 = Enumerable.Empty<T20>();
                IEnumerable<T21> t21 = Enumerable.Empty<T21>();
                IEnumerable<T22> t22 = Enumerable.Empty<T22>();
                IEnumerable<T23> t23 = Enumerable.Empty<T23>();

                if (!gridReader.IsConsumed)
                    t1 = await gridReader.ReadAsync<T1>();
                if (!gridReader.IsConsumed)
                    t2 = await gridReader.ReadAsync<T2>();
                if (!gridReader.IsConsumed)
                    t3 = await gridReader.ReadAsync<T3>();
                if (!gridReader.IsConsumed)
                    t4 = await gridReader.ReadAsync<T4>();
                if (!gridReader.IsConsumed)
                    t5 = await gridReader.ReadAsync<T5>();
                if (!gridReader.IsConsumed)
                    t6 = await gridReader.ReadAsync<T6>();
                if (!gridReader.IsConsumed)
                    t7 = await gridReader.ReadAsync<T7>();
                if (!gridReader.IsConsumed)
                    t8 = await gridReader.ReadAsync<T8>();
                if (!gridReader.IsConsumed)
                    t9 = await gridReader.ReadAsync<T9>();
                if (!gridReader.IsConsumed)
                    t10 = await gridReader.ReadAsync<T10>();
                if (!gridReader.IsConsumed)
                    t11 = await gridReader.ReadAsync<T11>();
                if (!gridReader.IsConsumed)
                    t12 = await gridReader.ReadAsync<T12>();
                if (!gridReader.IsConsumed)
                    t13 = await gridReader.ReadAsync<T13>();
                if (!gridReader.IsConsumed)
                    t14 = await gridReader.ReadAsync<T14>();
                if (!gridReader.IsConsumed)
                    t15 = await gridReader.ReadAsync<T15>();
                if (!gridReader.IsConsumed)
                    t16 = await gridReader.ReadAsync<T16>();
                if (!gridReader.IsConsumed)
                    t17 = await gridReader.ReadAsync<T17>();
                if (!gridReader.IsConsumed)
                    t18 = await gridReader.ReadAsync<T18>();
                if (!gridReader.IsConsumed)
                    t19 = await gridReader.ReadAsync<T19>();
                if (!gridReader.IsConsumed)
                    t20 = await gridReader.ReadAsync<T20>();
                if (!gridReader.IsConsumed)
                    t21 = await gridReader.ReadAsync<T21>();
                if (!gridReader.IsConsumed)
                    t22 = await gridReader.ReadAsync<T22>();
                if (!gridReader.IsConsumed)
                    t23 = await gridReader.ReadAsync<T23>();
                return new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>, IEnumerable<T7>, Tuple<IEnumerable<T8>, IEnumerable<T9>, IEnumerable<T10>, IEnumerable<T11>, IEnumerable<T12>, IEnumerable<T13>, IEnumerable<T14>, Tuple<IEnumerable<T15>, IEnumerable<T16>, IEnumerable<T17>, IEnumerable<T18>, IEnumerable<T19>, IEnumerable<T20>, IEnumerable<T21>, Tuple<IEnumerable<T22>, IEnumerable<T23>>>>>(
                    t1,
                    t2,
                    t3,
                    t4,
                    t5,
                    t6,
                    t7,
                    new Tuple<IEnumerable<T8>, IEnumerable<T9>, IEnumerable<T10>, IEnumerable<T11>, IEnumerable<T12>, IEnumerable<T13>, IEnumerable<T14>, Tuple<IEnumerable<T15>, IEnumerable<T16>, IEnumerable<T17>, IEnumerable<T18>, IEnumerable<T19>, IEnumerable<T20>, IEnumerable<T21>, Tuple<IEnumerable<T22>, IEnumerable<T23>>>>(
                        t8,
                        t9,
                        t10,
                        t11,
                        t12,
                        t13,
                        t14,
                         new Tuple<IEnumerable<T15>, IEnumerable<T16>, IEnumerable<T17>, IEnumerable<T18>, IEnumerable<T19>, IEnumerable<T20>, IEnumerable<T21>, Tuple<IEnumerable<T22>, IEnumerable<T23>>>(
                             t15,
                             t16,
                             t17,
                             t18,
                             t19,
                             t20,
                             t21,
                             new Tuple<IEnumerable<T22>, IEnumerable<T23>>(
                                 t22,
                                 t23))));
            }
        }

        public async Task<Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>, IEnumerable<T7>, Tuple<IEnumerable<T8>, IEnumerable<T9>, IEnumerable<T10>, IEnumerable<T11>, IEnumerable<T12>, IEnumerable<T13>, IEnumerable<T14>, Tuple<IEnumerable<T15>, IEnumerable<T16>, IEnumerable<T17>, IEnumerable<T18>, IEnumerable<T19>, IEnumerable<T20>, IEnumerable<T21>, Tuple<IEnumerable<T22>, IEnumerable<T23>, IEnumerable<T24>>>>>> GetMultiple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>(
            string sql,

            object parameters)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("connection")))
            {
                using var gridReader = await db.QueryMultipleAsync(sql, parameters, commandType: CommandType.StoredProcedure);

                IEnumerable<T1> t1 = Enumerable.Empty<T1>();
                IEnumerable<T2> t2 = Enumerable.Empty<T2>();
                IEnumerable<T3> t3 = Enumerable.Empty<T3>();
                IEnumerable<T4> t4 = Enumerable.Empty<T4>();
                IEnumerable<T5> t5 = Enumerable.Empty<T5>();
                IEnumerable<T6> t6 = Enumerable.Empty<T6>();
                IEnumerable<T7> t7 = Enumerable.Empty<T7>();
                IEnumerable<T8> t8 = Enumerable.Empty<T8>();
                IEnumerable<T9> t9 = Enumerable.Empty<T9>();
                IEnumerable<T10> t10 = Enumerable.Empty<T10>();
                IEnumerable<T11> t11 = Enumerable.Empty<T11>();
                IEnumerable<T12> t12 = Enumerable.Empty<T12>();
                IEnumerable<T13> t13 = Enumerable.Empty<T13>();
                IEnumerable<T14> t14 = Enumerable.Empty<T14>();
                IEnumerable<T15> t15 = Enumerable.Empty<T15>();
                IEnumerable<T16> t16 = Enumerable.Empty<T16>();
                IEnumerable<T17> t17 = Enumerable.Empty<T17>();
                IEnumerable<T18> t18 = Enumerable.Empty<T18>();
                IEnumerable<T19> t19 = Enumerable.Empty<T19>();
                IEnumerable<T20> t20 = Enumerable.Empty<T20>();
                IEnumerable<T21> t21 = Enumerable.Empty<T21>();
                IEnumerable<T22> t22 = Enumerable.Empty<T22>();
                IEnumerable<T23> t23 = Enumerable.Empty<T23>();
                IEnumerable<T24> t24 = Enumerable.Empty<T24>();

                if (!gridReader.IsConsumed)
                    t1 = await gridReader.ReadAsync<T1>();
                if (!gridReader.IsConsumed)
                    t2 = await gridReader.ReadAsync<T2>();
                if (!gridReader.IsConsumed)
                    t3 = await gridReader.ReadAsync<T3>();
                if (!gridReader.IsConsumed)
                    t4 = await gridReader.ReadAsync<T4>();
                if (!gridReader.IsConsumed)
                    t5 = await gridReader.ReadAsync<T5>();
                if (!gridReader.IsConsumed)
                    t6 = await gridReader.ReadAsync<T6>();
                if (!gridReader.IsConsumed)
                    t7 = await gridReader.ReadAsync<T7>();
                if (!gridReader.IsConsumed)
                    t8 = await gridReader.ReadAsync<T8>();
                if (!gridReader.IsConsumed)
                    t9 = await gridReader.ReadAsync<T9>();
                if (!gridReader.IsConsumed)
                    t10 = await gridReader.ReadAsync<T10>();
                if (!gridReader.IsConsumed)
                    t11 = await gridReader.ReadAsync<T11>();
                if (!gridReader.IsConsumed)
                    t12 = await gridReader.ReadAsync<T12>();
                if (!gridReader.IsConsumed)
                    t13 = await gridReader.ReadAsync<T13>();
                if (!gridReader.IsConsumed)
                    t14 = await gridReader.ReadAsync<T14>();
                if (!gridReader.IsConsumed)
                    t15 = await gridReader.ReadAsync<T15>();
                if (!gridReader.IsConsumed)
                    t16 = await gridReader.ReadAsync<T16>();
                if (!gridReader.IsConsumed)
                    t17 = await gridReader.ReadAsync<T17>();
                if (!gridReader.IsConsumed)
                    t18 = await gridReader.ReadAsync<T18>();
                if (!gridReader.IsConsumed)
                    t19 = await gridReader.ReadAsync<T19>();
                if (!gridReader.IsConsumed)
                    t20 = await gridReader.ReadAsync<T20>();
                if (!gridReader.IsConsumed)
                    t21 = await gridReader.ReadAsync<T21>();
                if (!gridReader.IsConsumed)
                    t22 = await gridReader.ReadAsync<T22>();
                if (!gridReader.IsConsumed)
                    t23 = await gridReader.ReadAsync<T23>();
                if (!gridReader.IsConsumed)
                    t24 = await gridReader.ReadAsync<T24>();
                return new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>, IEnumerable<T7>, Tuple<IEnumerable<T8>, IEnumerable<T9>, IEnumerable<T10>, IEnumerable<T11>, IEnumerable<T12>, IEnumerable<T13>, IEnumerable<T14>, Tuple<IEnumerable<T15>, IEnumerable<T16>, IEnumerable<T17>, IEnumerable<T18>, IEnumerable<T19>, IEnumerable<T20>, IEnumerable<T21>, Tuple<IEnumerable<T22>, IEnumerable<T23>, IEnumerable<T24>>>>>(
                    t1,
                    t2,
                    t3,
                    t4,
                    t5,
                    t6,
                    t7,
                    new Tuple<IEnumerable<T8>, IEnumerable<T9>, IEnumerable<T10>, IEnumerable<T11>, IEnumerable<T12>, IEnumerable<T13>, IEnumerable<T14>, Tuple<IEnumerable<T15>, IEnumerable<T16>, IEnumerable<T17>, IEnumerable<T18>, IEnumerable<T19>, IEnumerable<T20>, IEnumerable<T21>, Tuple<IEnumerable<T22>, IEnumerable<T23>, IEnumerable<T24>>>>(
                        t8,
                        t9,
                        t10,
                        t11,
                        t12,
                        t13,
                        t14,
                         new Tuple<IEnumerable<T15>, IEnumerable<T16>, IEnumerable<T17>, IEnumerable<T18>, IEnumerable<T19>, IEnumerable<T20>, IEnumerable<T21>, Tuple<IEnumerable<T22>, IEnumerable<T23>, IEnumerable<T24>>>(
                             t15,
                             t16,
                             t17,
                             t18,
                             t19,
                             t20,
                             t21,
                             new Tuple<IEnumerable<T22>, IEnumerable<T23>, IEnumerable<T24>>(
                                 t22,
                                 t23,
                                 t24))));
            }
        }
    }
}