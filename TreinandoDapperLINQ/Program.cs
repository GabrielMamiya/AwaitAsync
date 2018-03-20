using System;
using System.Linq;
using System.Collections.Generic;
using System.Timers;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Dapper;

namespace TreinandoDapperLINQ
{
    class MainClass 
    {
        public static MySqlConnection connection = new MySqlConnection();

        public static void Main(string[] args)
        { 
            

            connection.ConnectionString = "Data Source=localhost; Initial Catalog = gabriel;User ID = root; Password = gabriel00";

            try{
                connection.Open();
                Console.WriteLine("Conexao com o servidor aberta!");
            }
            catch (Exception e){
                Console.WriteLine("Erro: " + e.Message);
            }

            List<Pessoas> listaDePessoas = connection.Query<Pessoas>("select * from pessoas").ToList();

            var listaFiltrada = listaDePessoas.Where(x => x.nome == "Mitsuaki").Select(x => x).ToList();


            listaDePessoas.ForEach(item => {
                Console.ForegroundColor = ConsoleColor.Red; 
                Console.WriteLine(item.nome);
            });

            Console.WriteLine();

            listaFiltrada.ForEach(item => {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(item.nome);
            });

            Console.ForegroundColor = ConsoleColor.Black;

            ////INSERE NO BANCO
            Console.WriteLine();
            TestAsyncAwaitMethods();
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();

        }

        public async static void TestAsyncAwaitMethods()
        {
            await LongRunningMethod();

        }

        public static async Task<int> LongRunningMethod()
        {
            Console.WriteLine("Starting inserts on database gabriel...");
            Pessoas p = new Pessoas() { nome = "Iara", sobrenome = "Versace" };
            await Task.Delay(5000);
            await Task.Run(() => {
                connection.Execute("insert into pessoas (nome, sobrenome) values (@nome, @sobrenome)", p);
                Console.WriteLine("Acabou o cadastro");
            });
            Console.WriteLine("Os cadastros foram inseridos com sucesso!...");
            return 1;
        }

    }

}
