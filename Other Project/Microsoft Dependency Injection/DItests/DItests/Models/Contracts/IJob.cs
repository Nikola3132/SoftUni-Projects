using System;
using System.Collections.Generic;
using System.Text;

namespace DItests.Models.Contracts
{
    public interface IJob
    {
        decimal Salary { get; }
        string JobType { get; }
        TimeSpan WorkTime { get; }
    }
}
