namespace OpenChess.Domain
{
    internal abstract class PGNBuilder
    {
        protected int _count;
        public string Result { get; protected set; } = "";

        public PGNBuilder(int count)
        {
            _count = count;
        }

        public abstract PGNBuilder Build();
    }
}