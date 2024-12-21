using API.Interfaces;

namespace API.Data;

public class UnitOfWork(DataContext context, IUserRepositry userRepositry,
    ILikesRepositry likesRepositry, IMessageRepository messageRepository) : IUnitOfWork
{
    public IUserRepositry UserRepositry => userRepositry;

    public IMessageRepository MessageRepository => messageRepository;

    public ILikesRepositry LikesRepositry => likesRepositry;

    public async Task<bool> Complete()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public bool HasChanges()
    {
        return context.ChangeTracker.HasChanges();
    }
}
