using System;
using System.Threading.Tasks;

namespace Zeng.Sandbox.DependencyInjection
{
    public interface IProjectionWorker
    {
        Task Execute();
    }

    public abstract class ProjectionWorker<T> where T : Aggregator
    {
        private readonly IDocumentWriter<T> _documentWriter;
        private readonly IBookmark<T> _bookmark;
        private readonly ITransactionExecutor _transactionExecutor;

        protected ProjectionWorker(IDocumentWriter<T> documentWriter, IBookmark<T> bookmark, ITransactionExecutor transactionExecutor)
        {
            _documentWriter = documentWriter;
            _bookmark = bookmark;
            _transactionExecutor = transactionExecutor;
        }

        public async Task Execute()
        {
            await _transactionExecutor.Execute(InternalExecute);
        }

        private async Task InternalExecute()
        {
            Console.WriteLine($"{GetType().Name} executing");

            var value = await _bookmark.Read();
            await _documentWriter.Write(value);
        }
    }

    public class CustomerProjectionWorker : ProjectionWorker<Customer>, IProjectionWorker
    {
        public CustomerProjectionWorker(IDocumentWriter<Customer> documentWriter, IBookmark<Customer> bookmark, ITransactionExecutor transactionExecutor)
            : base(documentWriter, bookmark, transactionExecutor)
        {
        }
    }

    public class LoanProjectionWorker : ProjectionWorker<Loan>, IProjectionWorker
    {
        public LoanProjectionWorker(IDocumentWriter<Loan> documentWriter, IBookmark<Loan> bookmark, ITransactionExecutor transactionExecutor)
            : base(documentWriter, bookmark, transactionExecutor)
        {
        }
    }
}