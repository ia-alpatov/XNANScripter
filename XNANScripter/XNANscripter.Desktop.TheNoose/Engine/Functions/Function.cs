using System.Collections.Generic;
using System.Threading.Tasks;

namespace XNANScripter.Engine.Functions
{
    internal interface Function
    {
        string name
        {
            get;
            set;
        }

        List<string> parameters { get; set; }

        string Parse(string _parameters);

        Task Run();
    }
}