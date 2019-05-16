namespace DevYeah.LMS.Business.ResultModels
{
    public enum IdentityResultCode
    {
        Success = 1,
        EmailConflict = 2,
        EmailError = 3,
        PasswordError = 4,
        SignUpFailure = 5,
        AccountUpdateFailure = 6,
        SignInFailure = 7,
        InactivatedAccount = 8,
        IncompleteArgument = 9,
        BackenException = 99,
        NetworkException = 88,
    }

    public enum StudentServiceResultCode
    {
        Success = 1,
        StudentNotExist = 2,
        StudentUpdateFail = 3,
        BackenException = 99,
        NetworkException = 88,
    }

    public enum VideoServiceResultCode
    {
        Success = 1,
        VideoNotExist = 2,
        VideoSaveFailure = 3,
        VideoUpdateFailure = 4,
        VideoDeleteFailure = 5,
        BackenException = 99,
        NetworkException = 88,
    }

    public enum PracticeServiceResultCode
    {
        Success = 1,
        PracticeNotExist = 2,
        PracticeSaveFailure = 3,
        PracticeUpdateFailure = 4,
        PracticeDeleteFailure = 5,
        BackenException = 99,
        NetworkException = 88,
    }

    public enum FileServiceResultCode
    {
        Success = 1,
        FileNotExist = 2,
        FileSaveFailure = 3,
        FileUpdateFailure = 4,
        FileDeleteFailure = 5,
        BackenException = 99,
        NetworkException = 88,
    }

    public enum QuizServiceResultCode
    {
        Success = 1,
        QuizNotExist = 2,
        QuizSaveFailure = 3,
        QuizUpdateFailure = 4,
        QuizDeleteFailure = 5,
        BackenException = 99,
        NetworkException = 88,
    }

    public enum CourseServiceResultCode
    {
        Success = 1,
        CourseNotExist = 2,
        CourseSaveFailure = 3,
        CourseUpdateFailure = 4,
        CourseDeleteFailure = 5,
        NoTopic = 6,
        NoResource = 7,
        BackenException = 99,
        NetworkException = 88,
    }
}
