﻿using DomainModels.Models;
using DomainModels.NHibernate;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModels.Repository
{
    public class DocumentsRepository : IDocumentsRepository
    {
        public Documents Create()
        {
            return new Documents()
            {
                Uid = Guid.NewGuid(),
                NameDate = DateTime.Now
            };
        }

        public void Delete(Documents entity)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Delete(entity);
                    transaction.Commit();
                }
            }
        }

        public Documents Get(long Id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.Get<Documents>(Id);
            }
        }

        public ICollection<Documents> GetAll(User user)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<Documents>().And(d=>d.Author == user).List();
            }
        }

        public ICollection<Documents> SearchDocument(string search, User user)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<Documents>().And(d => d.Author == user)
                    .WhereRestrictionOn(d => d.Name).IsLike("%" + search + "%")
                    .List();
            }
        }

        public void Update(Documents entity)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    try
                    {
                        session.SaveOrUpdate(entity);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }
    }
}
