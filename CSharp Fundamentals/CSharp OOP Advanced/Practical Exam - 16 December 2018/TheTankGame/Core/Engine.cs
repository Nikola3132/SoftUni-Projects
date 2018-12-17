namespace TheTankGame.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using IO.Contracts;

    public class Engine : IEngine
    {
        private bool isRunning;
        private readonly IReader reader;
        private readonly IWriter writer;
        private readonly ICommandInterpreter commandInterpreter;

        public Engine(
            IReader reader, 
            IWriter writer, 
            ICommandInterpreter commandInterpreter)
        {
            this.reader = reader;
            this.writer = writer;
            this.commandInterpreter = commandInterpreter;

            this.isRunning = false;
        }

        public void Run()
        {
            string input = reader.ReadLine();

            while (true)
            {
                IList<string> splittedInput = input.Split();

                writer.WriteLine(this.commandInterpreter.ProcessInput(splittedInput));
                if (input == "Terminate")
                {
                    break;
                }
                input = reader.ReadLine().Trim();
            }
            
        }
    }
}