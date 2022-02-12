using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Infrastructure;
using Infrastructure.Core;
using Infrastructure.Helpers;
using Infrastructure.Services;
using Model.Data;
using Model.Entities;
using FormsDal.Contexts;

namespace FormsDal.Services
{
    public class GeneralService : BaseService
    {

        #region Get

        public async Task<string> IsConnected()
        {

            var results = "Test";
                //await DbContext.FormsStatuses.Select(x => x).ToListAsync();
            

            return results;
        }

        #endregion Get

    }
}
