using System;
using System.Collections.Generic;
using System.Text;

namespace DItests.Models.Contracts
{
    public interface IPassport
    {
         string PersonalPin { get;}
         string BirthPlace { get; }
         string Name { get; }
         int Age { get; }
    }
}
