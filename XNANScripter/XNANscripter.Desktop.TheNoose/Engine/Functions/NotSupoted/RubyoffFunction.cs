using System.Collections.Generic;


namespace XNANScripter.Engine.Functions
{
    internal class RubyoffFunction : Function
    {
        public string name
        {
            get { return "rubyoff"; }
            set { }
        }

        public List<string> parameters { get; set; }

        public RubyoffFunction()
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