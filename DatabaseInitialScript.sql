USE [LMS]
GO

IF NOT EXISTS (SELECT NULL FROM sys.tables WHERE NAME='Account')
  CREATE TABLE dbo.Account (
    [Id]             UNIQUEIDENTIFIER   NOT NULL,
    [UserName]       NVARCHAR(50)       NOT NULL,
    [Email]          VARCHAR(254)       NOT NULL,
    [Password]       NVARCHAR(50)       NOT NULL,
    [Status]         TINYINT            NOT NULL,
    [Type]           TINYINT            NOT NULL,
    [CreatedDate]    DATETIME2(7)       DEFAULT SYSUTCDATETIME(),
    [ActivationTime] DATETIME2(7),
    [LastLoginTime]  DATETIME2(7),
    PRIMARY KEY ([Id])
  )
  GO

IF NOT EXISTS (SELECT NULL FROM sys.tables WHERE NAME='UserProfile')
  CREATE TABLE dbo.UserProfile (
    [Id]             UNIQUEIDENTIFIER   NOT NULL,
    [AccountId]      UNIQUEIDENTIFIER   NOT NULL,
    [RecoveryEmail]  VARCHAR(254),
    [FullName]       NVARCHAR(256),
    [Gender]         CHAR(1),
    [BirthDate]      DATE,
    [Bio]            NVARCHAR(500),
    PRIMARY KEY ([Id])
  )
  GO

IF NOT EXISTS (SELECT NULL FROM sys.tables WHERE NAME='Avatar')
  CREATE TABLE dbo.Avatar (
    [Id]             UNIQUEIDENTIFIER   NOT NULL,
    [UserProfileId]  UNIQUEIDENTIFIER   NOT NULL,
    [Dimension]      CHAR(1)            NOT NULL,
    [MimeType]       VARCHAR(20),
    [Size]           SMALLINT,
    PRIMARY KEY ([Id])
  )
  GO

IF NOT EXISTS (SELECT NULL FROM sys.tables WHERE NAME='Course')
  CREATE TABLE dbo.Course (
    [Id]              UNIQUEIDENTIFIER  NOT NULL,
    [InstructorId]    UNIQUEIDENTIFIER  NOT NULL,
    [Name]            NVARCHAR(100)     NOT NULL,
    [Overview]        NVARCHAR(800)     NOT NULL,
    [Edition]         TINYINT           NOT NULL,
    [AvgLearningTime] FLOAT             NOT NULL,
    PRIMARY KEY ([Id])
  )
  GO

IF NOT EXISTS (SELECT NULL FROM sys.tables WHERE NAME='AccountCourse')
  CREATE TABLE dbo.AccountCourse (
    [AccountId]      UNIQUEIDENTIFIER  NOT NULL,
    [CourseId]       UNIQUEIDENTIFIER  NOT NULL,
    [CreatedDate]    DATETIME2(7)      DEFAULT SYSUTCDATETIME(),
    PRIMARY KEY ([AccountId], [CourseId])
  )
  GO

IF NOT EXISTS (SELECT NULL FROM sys.tables WHERE NAME='Category')
  CREATE TABLE dbo.Category (
    [Id]             UNIQUEIDENTIFIER  NOT NULL,
    [Name]           NVARCHAR(50)      NOT NULL,
    PRIMARY KEY ([Id])
  )
  GO

IF NOT EXISTS (SELECT NULL FROM sys.tables WHERE NAME='CourseCategory')
  CREATE TABLE dbo.CourseCategory (
    [CourseId]       UNIQUEIDENTIFIER  NOT NULL,
    [CategoryId]     UNIQUEIDENTIFIER  NOT NULL,
    PRIMARY KEY ([CourseId], [CategoryId])
  )
  GO

IF NOT EXISTS (SELECT NULL FROM sys.tables WHERE NAME='Topic')
  CREATE TABLE dbo.Topic (
    [Id]             UNIQUEIDENTIFIER  NOT NULL,
    [CourseId]       UNIQUEIDENTIFIER  NOT NULL,
    [Title]          NVARCHAR(100)     NOT NULL,
    [DisplayOrder]   SMALLINT          NOT NULL,
    PRIMARY KEY ([Id])
  )
  GO

IF NOT EXISTS (SELECT NULL FROM sys.tables WHERE NAME='TopicProgress')
  CREATE TABLE dbo.TopicProgress (
    [AccountId]      UNIQUEIDENTIFIER  NOT NULL,
    [TopicId]        UNIQUEIDENTIFIER  NOT NULL,
    [StartTime]      DATETIME2(7)      DEFAULT SYSUTCDATETIME(),
    [EndTime]        DATETIME2(7),
    [Status]         TINYINT           NOT NULL,
    PRIMARY KEY ([AccountId], [TopicId])
  )
  GO

IF NOT EXISTS (SELECT NULL FROM sys.tables WHERE NAME='Resource')
  CREATE TABLE dbo.Resource (
    [Id]             UNIQUEIDENTIFIER  NOT NULL,
    [TopicId]        UNIQUEIDENTIFIER  NOT NULL,
    [Type]           TINYINT           NOT NULL,
    [Title]          NVARCHAR(100)     NOT NULL,
    [DisplayOrder]   SMALLINT          NOT NULL,
    [Content]        NVARCHAR(1000)    NOT NULL,
    [Status]         TINYINT           NOT NULL,
    PRIMARY KEY ([Id])
  )
  GO

IF NOT EXISTS (SELECT NULL FROM sys.tables WHERE NAME='VideoRepo')
  CREATE TABLE dbo.VideoRepo (
    [Id]             UNIQUEIDENTIFIER  NOT NULL,
    [ResourceId]     UNIQUEIDENTIFIER  NOT NULL,
    [Url]            NVARCHAR(500)     NOT NULL,
    [MimeType]       VARCHAR(20)       NOT NULL,
    PRIMARY KEY ([Id])
  )
  GO

IF NOT EXISTS (SELECT NULL FROM sys.tables WHERE NAME='PracticeRepo')
  CREATE TABLE dbo.PracticeRepo (
    [Id]             UNIQUEIDENTIFIER  NOT NULL,
    [ResourceId]     UNIQUEIDENTIFIER  NOT NULL,
    [Instruction]    NVARCHAR(500)     NOT NULL,
    [CodeTemplate]   NVARCHAR(1000),
    [HelpMessage]    NVARCHAR(1000),
    [CorrectResult]  NVARCHAR(1000),
    PRIMARY KEY ([Id])
  )
  GO

IF NOT EXISTS (SELECT NULL FROM sys.tables WHERE NAME='FileRepo')
  CREATE TABLE dbo.FileRepo (
    [Id]             UNIQUEIDENTIFIER  NOT NULL,
    [ResourceId]     UNIQUEIDENTIFIER  NOT NULL,
    [Type]           VARCHAR(20)       NOT NULL,
    [Url]            NVARCHAR(500)     NOT NULL,
    [Size]           INT,
    PRIMARY KEY ([Id])
  )
  GO

IF NOT EXISTS (SELECT NULL FROM sys.tables WHERE NAME='QuizRepo')
  CREATE TABLE dbo.QuizRepo (
    [Id]             UNIQUEIDENTIFIER  NOT NULL,
    [ResourceId]     UNIQUEIDENTIFIER  NOT NULL,
    [Question]       NVARCHAR(500)     NOT NULL,
    [Option]         NVARCHAR(500)     NOT NULL,
    [Answer]         NVARCHAR(500)     NOT NULL,
    PRIMARY KEY ([Id])
  )
  GO

IF NOT EXISTS (SELECT NULL FROM sys.tables WHERE NAME='OperationHistory')
  CREATE TABLE dbo.OperationHistory (
    [Id]             UNIQUEIDENTIFIER  NOT NULL,
    [Type]           TINYINT           NOT NULL,
    [TableName]      VARCHAR(50)       NOT NULL,
    [PrimaryKey]     UNIQUEIDENTIFIER  NOT NULL,
    [ColumnName]     VARCHAR(50),
    [ColumnValue]    NVARCHAR(4000),
    [CreatedTime]    DATETIME2(7)      DEFAULT SYSUTCDATETIME(),
    [OperatorId]     UNIQUEIDENTIFIER  NOT NULL,
    PRIMARY KEY ([Id])
  )
  GO

/** Alter table **/
IF NOT EXISTS (SELECT NULL FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Course' AND COLUMN_NAME = 'Level')
  ALTER TABLE dbo.Course
	  ADD [Level] TINYINT NOT NULL
  GO

/** Constraints **/
IF NOT EXISTS (SELECT NULL FROM sys.objects WHERE NAME='FK_UserProfile_Account')
  ALTER TABLE dbo.UserProfile 
    ADD CONSTRAINT [FK_UserProfile_Account] FOREIGN KEY ([AccountId])
    REFERENCES dbo.Account ([Id])
  GO

IF NOT EXISTS (SELECT NULL FROM sys.objects WHERE NAME='FK_Avatar_UserProfile')
  ALTER TABLE dbo.Avatar
    ADD CONSTRAINT [FK_Avatar_UserProfile] FOREIGN KEY ([UserProfileId])
    REFERENCES dbo.UserProfile ([Id])
  GO

IF NOT EXISTS (SELECT NULL FROM sys.objects WHERE NAME='FK_Course_Account')
  ALTER TABLE dbo.Course
    ADD CONSTRAINT [FK_Course_Account] FOREIGN KEY ([InstructorId])
    REFERENCES dbo.Account ([Id])
  GO

IF NOT EXISTS (SELECT NULL FROM sys.objects WHERE NAME='FK_AccountCourse_Account')
  ALTER TABLE dbo.AccountCourse 
    ADD CONSTRAINT [FK_AccountCourse_Account] FOREIGN KEY ([AccountId])
    REFERENCES dbo.Account ([Id])
  GO

IF NOT EXISTS (SELECT NULL FROM sys.objects WHERE NAME='FK_AccountCourse_Course')
  ALTER TABLE dbo.AccountCourse
    ADD CONSTRAINT [FK_AccountCourse_Course] FOREIGN KEY ([CourseId])
    REFERENCES dbo.Course ([Id])
  GO

IF NOT EXISTS (SELECT NULL FROM sys.objects WHERE NAME='FK_CourseCategory_Course')
  ALTER TABLE dbo.CourseCategory
    ADD CONSTRAINT [FK_CourseCategory_Course] FOREIGN KEY ([CourseId])
    REFERENCES dbo.Course ([Id])
  GO

IF NOT EXISTS (SELECT NULL FROM sys.objects WHERE NAME='FK_CourseCategory_Category')
  ALTER TABLE dbo.CourseCategory
    ADD CONSTRAINT [FK_CourseCategory_Category] FOREIGN KEY ([CategoryId])
    REFERENCES dbo.Category ([Id])
  GO

IF NOT EXISTS (SELECT NULL FROM sys.objects WHERE NAME='FK_Topic_Course')
  ALTER TABLE dbo.Topic
    ADD CONSTRAINT [FK_Topic_Course] FOREIGN KEY ([CourseId])
    REFERENCES dbo.Course ([Id])
  GO

IF NOT EXISTS (SELECT NULL FROM sys.objects WHERE NAME='FK_TopicProgress_Account')
  ALTER TABLE dbo.TopicProgress
    ADD CONSTRAINT [FK_TopicProgress_Account] FOREIGN KEY ([AccountId])
    REFERENCES dbo.Account ([Id])
  GO

IF NOT EXISTS (SELECT NULL FROM sys.objects WHERE NAME='FK_TopicProgress_Topic')
  ALTER TABLE dbo.TopicProgress
    ADD CONSTRAINT [FK_TopicProgress_Topic] FOREIGN KEY ([TopicId])
    REFERENCES dbo.Topic ([Id])
  GO

IF NOT EXISTS (SELECT NULL FROM sys.objects WHERE NAME='FK_Resource_Topic')
  ALTER TABLE dbo.Resource
    ADD CONSTRAINT [FK_Resource_Topic] FOREIGN KEY ([TopicId])
    REFERENCES dbo.Topic ([Id])
  GO

IF NOT EXISTS (SELECT NULL FROM sys.objects WHERE NAME='FK_VideoRepo_Resource')
  ALTER TABLE dbo.VideoRepo
    ADD CONSTRAINT [FK_VideoRepo_Resource] FOREIGN KEY ([ResourceId])
    REFERENCES dbo.Resource ([Id])
  GO

IF NOT EXISTS (SELECT NULL FROM sys.objects WHERE NAME='FK_PracticeRepo_Resource')
  ALTER TABLE dbo.PracticeRepo
    ADD CONSTRAINT [FK_PracticeRepo_Resource] FOREIGN KEY ([ResourceId])
    REFERENCES dbo.Resource ([Id])
  GO

IF NOT EXISTS (SELECT NULL FROM sys.objects WHERE NAME='FK_FileRepo_Resource')
  ALTER TABLE dbo.FileRepo
    ADD CONSTRAINT [FK_FileRepo_Resource] FOREIGN KEY ([ResourceId])
    REFERENCES dbo.Resource ([Id])
  GO

IF NOT EXISTS (SELECT NULL FROM sys.objects WHERE NAME='FK_QuizRepo_Resource')
  ALTER TABLE dbo.QuizRepo
    ADD CONSTRAINT [FK_QuizRepo_Resource] FOREIGN KEY ([ResourceId])
    REFERENCES dbo.Resource ([Id])
  GO

IF NOT EXISTS (SELECT NULL FROM sys.objects WHERE NAME='FK_OperationHistory_Account')
  ALTER TABLE dbo.OperationHistory
    ADD CONSTRAINT [FK_OperationHistory_Account] FOREIGN KEY ([OperatorId])
    REFERENCES dbo.Account ([Id])
  GO