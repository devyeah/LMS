using DevYeah.LMS.Models;
using System;
using System.Collections.Generic;

namespace DevYeah.LMS.Business.Interfaces
{
    interface ICourseService
    {
        Guid? CreateCourse(Course course);
        Guid? UpdateCourse(Course course);
        void DeleteCourse(string courseId);
        Guid? AddVideoResource(VideoRepo videoRepo);
        Guid? AddPracticeResource(PracticeRepo practiceRepo);
        Guid? AddFileResource(FileRepo fileRepo);
        Guid? AddQuizResource(QuizRepo quizRepo);
        Guid? UpdateVideoResource(VideoRepo videoRepo);
        Guid? UpdatePracticeResource(PracticeRepo practiceRepo);
        Guid? UpdateFileResource(FileRepo fileRepo);
        Guid? UpdateQuizResource(QuizRepo quizRepo);
        void DeleteVideoResource(string videoId);
        void DeletePracticeResource(string practiceId);
        void DeleteFileResource(string fileId);
        void DeleteQuizResource(string quizId);
        Course GetCourseByKey(string courseId);
        List<Topic> GetAllTopicsOfCourse(string courseId);
        List<Resource> GetAllResourceOfTopic(string topicId);
        VideoRepo GetVideoByKey(string videoId);
        PracticeRepo GetPracticeByKey(string practiceId);
        FileRepo GetFileByKey(string fileId);
        QuizRepo GetQuizByKey(string quizId);
    }
}
