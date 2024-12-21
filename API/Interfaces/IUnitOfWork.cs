namespace API.Interfaces;

public interface IUnitOfWork
{
    IUserRepositry UserRepositry {get;}
    IMessageRepository MessageRepository {get;}
    ILikesRepositry LikesRepositry {get;}
    Task<bool> Complete();
    bool HasChanges();
}
