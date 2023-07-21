using BlogDataAccess.Interfaces;
using BlogDomain;
using Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogDataAccess.Repositories
{
    public class OffensiveWordsRepository : IOffensiveWordsRepository
    {
        private readonly DbContext dbContext;
        public OffensiveWordsRepository(DbContext context)
        {
            this.dbContext = context;
        }
        public OffensiveWordCollection Add(OffensiveWordCollection offensiveWordsCollection)
        {
            try
            {
                List<string> offensiveWords = offensiveWordsCollection.offensiveWords;
                OffensiveWordCollection currentList = Get();
                currentList.offensiveWords = offensiveWords.Union(currentList.offensiveWords).ToList();
                dbContext.SaveChanges();
                return currentList;
            }
            catch (Exception e)
            {
                throw new UnexpectedDataAccessException(e);
            }
        }
        private void CreateInstance()
        {
            try
            {
                OffensiveWordCollection collection = new OffensiveWordCollection();
                dbContext.Set<OffensiveWordCollection>().Add(collection);
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw new UnexpectedDataAccessException(e);
            }
        }

        public void Delete(OffensiveWordCollection offensiveWords)
        {
            try
            {
                OffensiveWordCollection currentList = Get();
                currentList.offensiveWords = currentList.offensiveWords.Except(offensiveWords.offensiveWords).ToList();
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw new UnexpectedDataAccessException(e);
            }
        }

        public OffensiveWordCollection Get()
        {
            OffensiveWordCollection collection = dbContext.Set<OffensiveWordCollection>().FirstOrDefault();
            if (collection == null)
            {
                CreateInstance();
                return dbContext.Set<OffensiveWordCollection>().FirstOrDefault();
            }
            return collection;
        }

    }
}
