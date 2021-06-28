using System;
using System.Threading.Tasks;

namespace SystemsRx.Threading
{
    public interface IThreadHandler
    {
        void For(int start, int end, Action<int> process);
        Task Run(Action process);
    }
}