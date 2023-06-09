﻿using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Models;
using SocialMedia.Repository.IRepository;

namespace SocialMedia.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<List<Post>> GetFeed(int myId, int pageSize = 0, int pageNumber = 1)
        {
            if (pageNumber > 0)
            {
                if (pageSize > 100)
                    pageSize = 100;
            }

            User user = await _db.Users
                .Include(u => u.Friends)
                    .ThenInclude(f => f.Posts)
                        .ThenInclude(p => p.Comments)
                .Include(u => u.Friends)
                    .ThenInclude(f => f.Posts)
                        .ThenInclude(p => p.Likes)
                .Include(u => u.Groups)
                    .ThenInclude(g => g.Posts)
                        .ThenInclude(p => p.Comments)
                .Include(u => u.Groups)
                    .ThenInclude(g => g.Posts)
                        .ThenInclude(p => p.Likes)
                .FirstOrDefaultAsync(u => u.Id == myId);

            List<Post> feed = user.Friends
                .SelectMany(f => f.Posts)
                .Concat(user.Groups.SelectMany(g => g.Posts))
                .OrderByDescending(p => p.CreatedDate)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToList();

            return feed;
        }


        public async Task<User> UpdateAsync(User entity)
        {
            entity.UpdatedDate = DateTime.Now;
            _db.Users.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
