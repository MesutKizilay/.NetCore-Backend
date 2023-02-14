namespace Core.Entities.Concrete
{
    public class UserOperaitonClaim : IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int OperaitonClaimId { get; set; }
    }
}