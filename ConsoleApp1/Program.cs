using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        public static DataClasses1DataContext context = new DataClasses1DataContext();

        static void Main(string[] args)
        {
            FiltrarPedidosXFecha();
            Console.Read();
        }        

        static void IntroToLINQ()
        {
            // The three parts of a LINQ Query:
            // 1. Data source
            int[] numbers = new int[7] { 0, 1, 2, 3, 4, 5, 6 };

            // 2. Query creation
            // numQuery is an IEnumerable<int>
            var numQuery =
                from num in numbers
                where (num % 2) == 0
                select num;

            // 3. Query execution
            foreach (int num in numQuery)
            {
                Console.Write("{0, 1} ", num);
            }
        }

        static void DataSource()
        {
            var queryAllCustomers = from cust in context.clientes
                                    select cust;

            foreach (var item in queryAllCustomers)
            {
                Console.WriteLine(item.NombreCompañia);
            }
        }

        static void DataSourceLambda()
        {
            var queryAllCustomers = context.clientes;

            foreach (var item in queryAllCustomers)
            {
                Console.WriteLine(item.NombreCompañia);
            }
        }


        static void Filtering()
        {
            var queryLondonCustomers = from cust in context.clientes
                                       where cust.Ciudad == "Londres"
                                       select cust;
            
            foreach (var item in queryLondonCustomers)
            {
                Console.WriteLine(item.Ciudad);
            }
        }

        static void FilteringLambda()
        {
            var queryLondonCustomers = context.clientes.Where(c => c.Ciudad == "Londres");

            foreach (var item in queryLondonCustomers)
            {
                Console.WriteLine(item.Ciudad);
            }
        }


        static void Ordering()
        {
            var queryLondonCustomers3 = from cust in context.clientes
                                        where cust.Ciudad == "Londres"
                                        orderby cust.NombreCompañia ascending
                                        select cust;

            foreach (var item in queryLondonCustomers3)
            {
                Console.WriteLine(item.NombreCompañia);
            }
        }

        static void OrderingLambda()
        {
            var queryLondonCustomers3 = context.clientes
                                        .Where(c => c.Ciudad == "Londres")
                                        .OrderBy(c => c.NombreCompañia);

            foreach (var item in queryLondonCustomers3)
            {
                Console.WriteLine(item.NombreCompañia);
            }
        }


        static void Grouping()
        {
            var queryCustomersByCity =
                from cust in context.clientes
                group cust by cust.Ciudad;

            // customerGroup is an Igrouping<string, Customer>
            foreach (var customerGroup in queryCustomersByCity)
            {
                Console.WriteLine(customerGroup.Key);

                foreach (clientes customer in customerGroup)
                {
                    Console.WriteLine("     {0}", customer.NombreCompañia);
                }
            }
        }

        static void GroupingLambda()
        {
            var queryCustomersByCity = context.clientes
                                        .GroupBy(c => c.Ciudad);

            // customerGroup is an Igrouping<string, Customer>
            foreach (var customerGroup in queryCustomersByCity)
            {
                Console.WriteLine(customerGroup.Key);

                foreach (clientes customer in customerGroup)
                {
                    Console.WriteLine("     {0}", customer.NombreCompañia);
                }
            }
        }


        static void Grouping2()
        {
            var custQuery =
                from cust in context.clientes
                group cust by cust.Ciudad into custGroup
                where custGroup.Count() > 2
                orderby custGroup.Key
                select custGroup;

            foreach (var item in custQuery)
            {
                Console.WriteLine(item.Key);
            }
        }

        static void Grouping2Lambda()
        {
            var custQuery = context.clientes
                            .GroupBy(c => c.Ciudad)
                            .Where(c => c.Key.Count() > 2)
                            .OrderBy(c => c.Key);

            foreach (var item in custQuery)
            {
              Console.WriteLine(item.Key);
            }
        }


        static void Joining()
        {
            var innerJoinQuery = from cust in context.clientes
                                 join dist in context.Pedidos on cust.idCliente equals dist.IdCliente
                                 select new { CustomerName = cust.NombreCompañia, DistributorName = dist.PaisDestinatario };
            
            foreach (var item in innerJoinQuery)
            {
                Console.WriteLine(item.CustomerName);
            }
        }

        static void JoiningLambda()
        {
            var innerJoinQuery = context.clientes
                                 .Join(context.Pedidos,
                                    c => c.idCliente,
                                    p => p.IdCliente,
                                    (a, b) => new { a.NombreCompañia, b.PaisDestinatario });

            foreach (var item in innerJoinQuery)
            {
                Console.WriteLine(item.NombreCompañia);
            }
        }

        static void FiltrarPedidosXFecha()
        {
            var innerJoinQuery = context.clientes
                                 .Join(context.Pedidos,
                                    c => c.idCliente,
                                    p => p.IdCliente,
                                    (c, p) => new { c.NombreCompañia,
                                                    cantidadPedido = p.IdCliente.Count(),
                                                    p.FechaPedido })
                                 .Where(p => p.FechaPedido.ToString() == "1994-08-08");
                                 //.GroupBy(p => p.NombreCompañia);

            foreach (var item in innerJoinQuery)
            {
                Console.WriteLine(item.NombreCompañia + ": " + item.cantidadPedido);
            }
        }
    }
}
