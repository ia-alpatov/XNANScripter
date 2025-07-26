namespace XNANScripter.Engine.Procedures
{
    internal class Procedure
    {
        public readonly ushort startPoint, endPoint;
        public readonly string name;

        public Procedure(string _name, ushort _startPoint, ushort _endPoint)
        {
            this.name = _name;
            this.startPoint = _startPoint;
            this.endPoint = _endPoint;
        }
    }
}