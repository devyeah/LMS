using System;
using System.Runtime.CompilerServices;
using DevYeah.LMS.Data.Interfaces;
using DevYeah.LMS.Models;
using Microsoft.EntityFrameworkCore;

namespace DevYeah.LMS.Data
{
    public class SystemErrorsRepository : Repository<SystemErrors>, ISystemErrorsRepository
    {
        public SystemErrorsRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public void AddLog(Exception ex, [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0, 
            [CallerFilePath] string filePath = "")
        {
            var error = new SystemErrors { Exception = ex.StackTrace, CallerFilePath = filePath, CallerLineNumber = lineNumber,
                CallerMemberName = memberName };
            Add(error);
            SaveChanges();
        }
    }
}
