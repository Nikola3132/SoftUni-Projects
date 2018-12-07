using System;
using System.Collections.Generic;
using System.Text;

namespace DItests.Models.Contracts
{
    public interface IPerson
    {
        IPassport Passport { get; }
        IJob Job { get; }
        void SayHello();
        string ShowThePersonInfo();
    }
}
