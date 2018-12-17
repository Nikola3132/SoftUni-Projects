namespace TheTankGame.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Contracts;

    public class CommandInterpreter : ICommandInterpreter
    {
        private readonly IManager tankManager;

        public CommandInterpreter(IManager tankManager)
        {
            this.tankManager = tankManager;
        }

        public string ProcessInput(IList<string> inputParameters)
        {
            string command = inputParameters[0];

            string result = string.Empty;
            Type type = this.tankManager.GetType();
            
            MethodInfo[] methodInfos = type.GetMethods(BindingFlags.Public | BindingFlags.Instance|BindingFlags.InvokeMethod);
            MethodInfo currentMethod = methodInfos.Where(x => x.Name.Contains(command)).FirstOrDefault();
            
            IList<string> listForInvoke = new List<string>();
            foreach (var item in inputParameters.Skip(1))
            {
                listForInvoke.Add(item);
            }
            
            result = (string)currentMethod.Invoke(this.tankManager, new object[] { listForInvoke });
            return result;
        }
    }
}