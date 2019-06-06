using Lab2.Models;
using Lab2.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2.Service
{
    public interface ICommentService
    {

        PaginatedList<CommentGetModel> GetAll(int page, string filter);
        Comment GetById(int id);
        Comment Create(Comment comment, User addedBy);
        Comment Upsert(int id, Comment expense);
        Comment Delete(int id);

    }

    public class CommentService : ICommentService
    {
        private MoviesDbContext context;
        public CommentService(MoviesDbContext context)
        {
            this.context = context;
        }


        public Comment Create(Comment comment, User addedBy)
        {
            comment.Owner = addedBy;
            context.Comments.Add(comment);
            context.SaveChanges();
            return comment;
        }
       

        public Comment Delete(int id)
        {
            var existing = context.Comments.FirstOrDefault(comment => comment.Id == id);
            if (existing == null)
            {
                return null;
            }
            context.Comments.Remove(existing);
            context.SaveChanges();
            return existing;
        }

        public PaginatedList<CommentGetModel> GetAll(int page, string filter)
        {
            IQueryable<Comment> result = context
                .Comments
                .Where(c => string.IsNullOrEmpty(filter) || c.Text.Contains(filter))
                .OrderBy(c => c.Id)
                .Include(c => c.Movie);
            var paginatedResult = new PaginatedList<CommentGetModel>();
            paginatedResult.CurrentPage = page;

            paginatedResult.NumberOfPages = (result.Count() - 1) / PaginatedList<CommentGetModel>.EntriesPerPage + 1;
            result = result
                .Skip((page - 1) * PaginatedList<CommentGetModel>.EntriesPerPage)
                .Take(PaginatedList<CommentGetModel>.EntriesPerPage);
            paginatedResult.Entries = result.Select(c => CommentGetModel.FromComment(c)).ToList();

            return paginatedResult;
        }

        public Comment Upsert(int id, Comment comment)
        {
            var existing = context.Comments.AsNoTracking().FirstOrDefault(c => c.Id == id);
            if (existing == null)
            {
                context.Comments.Add(comment);
                context.SaveChanges();
                return comment;
            }
            comment.Id = id;
            context.Comments.Update(comment);
            context.SaveChanges();
            return comment;
        }

        public Comment GetById(int id)
        {
            return context.Comments.FirstOrDefault(c => c.Id == id);
        }


    }
}
