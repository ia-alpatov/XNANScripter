namespace XNANScripter.Engine.Procedures
{
    internal class SubroutineLevel
    {
        public ushort StoppedLine;
        public string LeftoverValue;

        public SubroutineLevel(ushort _StoppedLine, string _LeftOverValue)
        {
            this.StoppedLine = _StoppedLine;
            this.LeftoverValue = _LeftOverValue;
        }
    }
}