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

        IEnumerable<CommentGetModel> GetAll(string filter);
        
    }

    public class CommentService : ICommentService
    {
        private MoviesDbContext context;
        public CommentService(MoviesDbContext context)
        {
            this.context = context;
        }

        //public IEnumerable<CommentGetModel> GetAll(string filter)
        //{
        //    IQueryable<Comment> result = context.Comments;

        //    result = result.Where(c => c.Text.Contains(filter));

        //    return result.Select(c => CommentGetModel.FromComment(c));
        //}

        public IEnumerable<CommentGetModel> GetAll(string filter)
        {
            IQueryable<Movie> result = context.Movies.Include(c => c.Comments);

            List<CommentGetModel> resultComments = new List<CommentGetModel>();

            foreach (Movie m in result)
            {
                m.Comments.ForEach(c =>
                {

                    if (c.Text.Contains(filter))
                    {
                        CommentGetModel comment = new CommentGetModel
                        {
                            Id = c.Id,
                            Important = c.Important,
                            Text = c.Text,
                            MovieId = m.Id

                        };
                        resultComments.Add(comment);

                    }
                });
            }

            return resultComments;
        }




        }
}
