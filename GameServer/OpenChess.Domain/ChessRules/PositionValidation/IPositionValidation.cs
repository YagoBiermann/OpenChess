namespace OpenChess.Domain
{
    internal interface IPositionValidation
    {
        public CurrentPositionStatus ValidatePosition(CurrentPositionStatus? checkState = null);
        public IPositionValidation SetNext(IPositionValidation validation);
    }
}