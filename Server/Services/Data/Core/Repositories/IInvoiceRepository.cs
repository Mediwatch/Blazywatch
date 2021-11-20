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
    public partial interface IInvoiceRepository
    {
        bool IncludeDeleted { get; set; }
        Domain.Invoice Get(int? id);

        Domain.Invoice SingleOrDefault(Func<Domain.Invoice, bool> predicate);

        bool Add(Domain.Invoice entity);
        bool AddRange(IEnumerable<Domain.Invoice> entities);

        IEnumerable<Domain.Invoice> GetAll(); 
        IEnumerable<Domain.Invoice> Find(Func<Domain.Invoice, bool> predicate);
        DataStoreResult Remove(int Id);

        DataStoreResult Remove(Domain.Invoice entity);

        DataStoreResult RemoveRange(IEnumerable<Domain.Invoice> entities);
    }
}