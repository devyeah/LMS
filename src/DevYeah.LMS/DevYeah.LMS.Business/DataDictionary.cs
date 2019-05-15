namespace DevYeah.LMS.Business
{
    public enum AccountStatus
    {
        Inactivated = 1,
        Activated = 2,
        Deleted = 9,
    }

    public enum AccountType
    {
        Student = 1,
        Instructor = 2,
        Admin = 3,
    }

    public enum TrackTopicStatus
    {
        InProgress = 1,
        Done = 2,
    }

    public enum ResourceStatus
    {
        Available = 1,
        Unavailable = 2,
        Deleted = 9,
    }

    public enum OperationType
    {
        Insert = 1,
        Update = 2,
        Delete = 3,
    }

    public enum CourseLevel
    {
        Beginner = 1,
        Intermediate = 2,
        Advanced = 3,
        AllLevel = 4,
    }
}
