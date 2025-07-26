using System.Collections.Generic;


namespace XNANScripter.Engine.Functions
{
    internal class RubyonFunction : Function
    {
        public string name
        {
            get { return "rubyon"; }
            set { }
        }

        public List<string> parameters { get; set; }

        public RubyonFunction()
        {
        }

        public string Parse(string _parameters)
        {
            return string.Empty;
        }

        public async System.Threading.Tasks.Task Run()
        {
            parameters = null;
        }
    }
}