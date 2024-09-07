namespace OpenChess.Domain
{
    internal interface IPositionValidation
    {
        public Status ValidatePosition();
        public IPositionValidation SetNext(IPositionValidation validation);
    }
}