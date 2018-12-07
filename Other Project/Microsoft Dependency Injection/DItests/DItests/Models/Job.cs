using DItests.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace DItests.Models
{
    public class Job : IJob
    {
        private const decimal salary = 3600m;
        private const string jobType = "C# Developer - Senior engineer";
        private static TimeSpan workTime = new TimeSpan(6, 30, 00);

        private IServiceProvider serviceProvider;

        public Job(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;

            this.Salary = salary;
            this.JobType = jobType;
            this.WorkTime = workTime;
        }

        public decimal Salary { get; }

        public string JobType { get; }

        public TimeSpan WorkTime { get; }

        public override string ToString()
        {
            return $"{Environment.NewLine}Job:{Environment.NewLine}      Salary: {this.Salary}{Environment.NewLine}       JobType: {this.JobType}{Environment.NewLine}       WorkTime {this.WorkTime.Hours}h. and {this.WorkTime.Minutes}m.";
        }
    }
}
