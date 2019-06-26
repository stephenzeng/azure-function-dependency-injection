using System;
using System.Threading.Tasks;

namespace Zeng.Sandbox.DependencyInjection
{
    public interface ITransactionExecutor
    {
        Task Execute(Func<Task> action);
    }

    public class SqlTransactionExecutor: ITransactionExecutor
    {
        private readonly string _connectionString;

        public SqlTransactionExecutor(string connectionString)
        {
            _connectionString = connectionString;
        }


        public async Task Execute(Func<Task> action)
        {
            Console.WriteLine($"Executing action with {_connectionString}");
            await action();
        }
    }
}