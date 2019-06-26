using System;
using System.Threading.Tasks;

namespace Zeng.Sandbox.DependencyInjection
{
    public interface IBookmark<T> where T: Aggregator
    {
        Task<int> Read();
    }

    public class Bookmark<T>: IBookmark<T> where T: Aggregator
    {
        public Task<int> Read()
        {
            Console.WriteLine($"Bookmark {typeof(T).Name} read");
            return Task.FromResult(1);
        }
    }

    public interface IDocumentWriter<T> where T : Aggregator
    {
        Task Write(int value);
    }

    public class DocumentWriter<T> : IDocumentWriter<T> where T : Aggregator
    {
        public Task Write(int value)
        {
            Console.WriteLine($"DocumentWriter {typeof(T).Name} write");
            return Task.CompletedTask;
        }
    }
}