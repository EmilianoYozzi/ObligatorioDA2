using BlogDataAccess.Interfaces;
using BlogDomain;
using Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BlogDataAccess.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DbContext dbContext;

        public CommentRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Comment Add(Comment comment)
        {
            try
            {
                dbContext.Set<Comment>().Add(comment);
                dbContext.SaveChanges();
                return comment;
            }
            catch (Exception e)
            {
                throw new UnexpectedDataAccessException(e);
            }
        }

        public bool Exists(Func<Comment, bool> func)
        {
            try
            {
                return dbContext.Set<Comment>().Any(func);
            }
            catch (Exception e)
            {
                throw new UnexpectedDataAccessException(e);
            }
        }

        public Comment Get(Func<Comment, bool> func)
        {
            try
            {
                return dbContext.Set<Comment>().Include(c => c.Answer).First(func);
            }
            catch (InvalidOperationException e)
            {
                throw new ResourceNotFoundException("Comment not found.");
            }
            catch (Exception e)
            {
                throw new UnexpectedDataAccessException(e);
            }
        }

        public bool Exists(string id) => Exists(c => c.Id.Equals(id));
        public Comment GetById(string id) => Get(c => c.Id.Equals(id));

        public Comment Update(Comment comment)
        {
            try
            {
                string id = comment.Id;
                Comment commentToUpdate = GetById(id);
                commentToUpdate.Text = comment.Text;
                commentToUpdate.Edited = true;
                dbContext.SaveChanges();
                return commentToUpdate;
            }
            catch (InvalidOperationException e)
            {
                throw new ResourceNotFoundException("Comment not found.");
            }
            catch (Exception e)
            {
                throw new UnexpectedDataAccessException(e);
            }
        }

        public void Delete(string id)
        {
            try
            {
                Comment commentToDelete = GetById(id);
                commentToDelete.Deleted = true;
                dbContext.SaveChanges();
            }
            catch (ResourceNotFoundException e)
            {
                throw e;
            }
            catch (Exception ex)
            {
                throw new UnexpectedDataAccessException(ex);
            }
        }
    }
}

