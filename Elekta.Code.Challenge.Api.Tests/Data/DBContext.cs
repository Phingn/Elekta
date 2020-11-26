using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Elekta.Code.Challenge.Api.Data;
using Elekta.Code.Challenge.Api.Interfaces;
using Elekta.Code.Challenge.Api.Repository;

namespace Elekta.Code.Challenge.Api.Tests.Data
{
    public static class DBContext
    { 
        public static DataContext DataSourceMemoryContext(string databaseInstance)
        {
            DbContextOptions<DataContext> options;
            var builder = new DbContextOptionsBuilder<DataContext>();
            builder.UseInMemoryDatabase(databaseName: databaseInstance);
            options = builder.Options;
            DataContext dataContext = new DataContext(options);
            dataContext.Database.EnsureDeleted();
            dataContext.Database.EnsureCreated();
            return dataContext;
        }
    }
}
