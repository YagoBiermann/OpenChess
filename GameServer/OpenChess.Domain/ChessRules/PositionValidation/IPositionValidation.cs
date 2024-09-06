namespace OpenChess.Domain
{
    internal interface IPositionValidation
    {
        public CurrentPositionStatus ValidatePosition();
        public IPositionValidation SetNext(IPositionValidation validation);
    }
}