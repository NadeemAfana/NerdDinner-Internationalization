using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System;
using NerdDinner.Models;

namespace NerdDinner.Tests.Fakes
{
    public class FakeDinnerRepository : IDinnerRepository
    {
        private List<Dinner> context;

        public FakeDinnerRepository(List<Dinner> dinners)
        {
            context = dinners;
        }

        public void DeleteRsvp(RSVP rsvp)
        {            
        }

        public IQueryable<Dinner> FindByLocation(float latitude, float longitude)
        {
            return (from dinner in context
                    where dinner.Latitude == latitude && dinner.Longitude == longitude
                    select dinner).AsQueryable();
        }

        public IQueryable<Dinner> FindDinnersByText(string q)
        {
            return All.Where(d => d.Title.Contains(q)
                            || d.Description.Contains(q)
                            || d.HostedBy.Contains(q));
        }

        public IQueryable<Dinner> FindUpcomingDinners()
        {
            return from dinner in All
                   where dinner.EventDate >= DateTime.Now
                   orderby dinner.EventDate
                   select dinner;
        }

        public IQueryable<Dinner> All
        {
            get { return context.AsQueryable(); }
        }

        public IQueryable<Dinner> AllIncluding(params System.Linq.Expressions.Expression<Func<Dinner, object>>[] includeProperties)
        {
            IQueryable<Dinner> query = All;
            foreach (var includeProperty in includeProperties)
            {
                // query = query.Include(includeProperty);
            }
            return query;
        }

        public void Delete(int id)
        {
            var dinner = Find(id);
            context.Remove(dinner);
        }

        public Dinner Find(int id)
        {
            return context.Find(x => x.DinnerID == id);
        }

        public void InsertOrUpdate(Dinner dinner)
        {
            context.Add(dinner);
        }

        public void Save()
        {
        }
    }
}
