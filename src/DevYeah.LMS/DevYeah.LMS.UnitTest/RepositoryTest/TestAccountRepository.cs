using System;
using System.Linq;
using System.Linq.Expressions;
using DevYeah.LMS.Data;
using DevYeah.LMS.Models;
using TestSupport.EfHelpers;
using Xunit;

namespace DevYeah.LMS.UnitTest.RepositoryTest
{
    public class TestAccountRepository
    {
        [Fact]
        public void TestAddAccount()
        {
            var options = this.CreateUniqueClassOptions<LMSContext>();
            using (var context = new LMSContext(options))
            {
                // before testing
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                // after testing
                context.Database.EnsureDeleted();
            }
        }
    }
}
