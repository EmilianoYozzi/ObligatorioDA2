using Microsoft.EntityFrameworkCore;
using BlogDataAccess.Interfaces;
using BlogDomain;
using Exceptions;

namespace BlogDataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbContext dbContext;

        public UserRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public User Add(User aUser)
        {
            try
            {
                VerifyValidUsername(aUser);
                dbContext.Set<User>().Add(aUser);
                dbContext.SaveChanges();
                return aUser;
            }
            catch (ArgumentException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new UnexpectedDataAccessException(e);
            }
        }

        private void VerifyValidUsername(User user)
        {
            if (Exists(u => u.Username.Equals(user.Username)))
                throw new ArgumentException("Username already exists.");
        }

        public void Delete(Func<User, bool> func)
        {
            try
            {
                Func<User, bool> combined = u => func(u) && !u.Deleted;
                User userToDelete = Get(combined);
                userToDelete.Deleted = true;
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw new UnexpectedDataAccessException(e);
            }
        }

        public User[] GetAllUsers() => dbContext.Set<User>().ToArray();        

        public User GetByUsername(string username) => Get((u) => u.Username.Equals(username));        

        public User GetByEmail(string email) => Get((u) => u.Email.Equals(email));        

        public User Update(User user)
        {
            try
            {
                string username = user.Username;
                User userToUpdate = Get((u) => u.Username == username);
                userToUpdate.Update(user);
                dbContext.SaveChanges();
                return userToUpdate;
            }
            catch (Exception e)
            {
                throw new UnexpectedDataAccessException(e);
            }
        }

        public User Get(Func<User, bool> func)
        {
            try
            {
                Func<User, bool> combined = u => func(u) && !u.Deleted;
                return dbContext.Set<User>().Include(u => u.Notifications).First(combined);
            }
            catch (InvalidOperationException e)
            {
                throw new ResourceNotFoundException("User not found");
            }
            catch (Exception e)
            {
                throw new UnexpectedDataAccessException(e);
            }
        }

        public bool Exists(Func<User, bool> func)
        {
            try
            {
                Func<User, bool> combined = u => func(u) && !u.Deleted;
                return dbContext.Set<User>().Any(func);
            }
            catch (Exception e)
            {
                throw new UnexpectedDataAccessException(e);
            }
        }

        public bool Exists(string username) =>
            Exists(u => u.Username.Equals(username));

        public void DeleteByUsername(string username) =>
            Delete(u => u.Username.Equals(username));

        public User[] GetMultiple(Func<User, bool> func)
        {
            Func<User, bool> combined = u => func(u) && !u.Deleted;
            return dbContext.Set<User>().Where(combined).ToArray();
        }
    }
}