namespace XNANScripter.Engine.Window_Menubar
{
    public class MenuItem
    {
        private string name;
        private string function;
        private int level;

        public MenuItem(string _name, string _function, int _level)
        {
            this.name = _name;
            this.function = _function;
            this.level = _level;
        }
    }
}