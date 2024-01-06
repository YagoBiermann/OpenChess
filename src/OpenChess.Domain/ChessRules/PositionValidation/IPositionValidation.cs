namespace OpenChess.Domain
{
    internal interface IPositionValidation
    {
        public CheckState ValidatePosition(CheckState? checkState = null);
        public IPositionValidation SetNext(IPositionValidation validation);
    }
}