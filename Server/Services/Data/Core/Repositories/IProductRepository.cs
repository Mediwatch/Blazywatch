/*********************************************************
       Code Generated By  .  .  .  .  Delaney's Script Bot
       WWW .  .  .  .  .  .  .  .  .  www.scriptbot.io
       Version.  .  .  .  .  .  .  .  10.0.0.14 
       Build  .  .  .  .  .  .  .  .  20191113 216
       Template Name.  .  .  .  .  .  Mosh (Repository EF)
       Template Version.  .  .  .  .  20191011 001
       Author .  .  .  .  .  .  .  .  Delaney

                          .___,
                       ___('v')___
                       `"-\._./-"'
                           ^ ^

*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Services.Data.Core.Repositories
{
    public partial interface IProductRepository
    {
        bool IncludeDeleted { get; set; }
        Domain.Product Get(int? id);

        Domain.Product SingleOrDefault(Func<Domain.Product, bool> predicate);

        bool Add(Domain.Product entity);
        bool AddRange(IEnumerable<Domain.Product> entities);

        IEnumerable<Domain.Product> GetAll(); 
        IEnumerable<Domain.Product> Find(Func<Domain.Product, bool> predicate);
        IEnumerable<Domain.Product> GetWithBackets(int pageIndex, int pageSize);
        void Get(Domain.FormationPaypal basket);
        Domain.Product GetWithParents(int id);
        Domain.Product GetWithChildren(int id);
        Domain.Product GetWithFamily(int id);
        Domain.Product Get(string name);
        DataStoreResult Remove(int Id,
                               out string errorMessage);

        DataStoreResult Remove(Domain.Product entity,
                               out string errorMessage);

        DataStoreResult RemoveRange(IEnumerable<Domain.Product> entities,
                                    out string errorMessage);
    }
}