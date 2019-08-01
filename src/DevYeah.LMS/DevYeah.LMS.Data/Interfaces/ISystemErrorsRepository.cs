using System;
using System.Runtime.CompilerServices;
using DevYeah.LMS.Models;

namespace DevYeah.LMS.Data.Interfaces
{
    public interface ISystemErrorsRepository : IRepository<SystemErrors>
    {
        void AddLog(Exception ex, [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0, 
            [CallerFilePath] string filePath = "");
    }
}
