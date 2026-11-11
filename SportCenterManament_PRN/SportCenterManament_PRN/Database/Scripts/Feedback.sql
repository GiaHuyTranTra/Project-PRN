USE [SportsCenterManagement];
GO

IF OBJECT_ID(N'dbo.Feedback', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Feedback
    (
        Id int IDENTITY(1,1) NOT NULL,
        ClassAttendanceId int NOT NULL,
        Rating int NOT NULL,
        Comment nvarchar(1000) NULL,
        CreatedAt datetime2(7) NOT NULL
            CONSTRAINT DF_Feedback_CreatedAt DEFAULT (sysutcdatetime()),

        CONSTRAINT PK_Feedback PRIMARY KEY CLUSTERED (Id),
        CONSTRAINT UQ_Feedback_ClassAttendance UNIQUE (ClassAttendanceId),
        CONSTRAINT CK_Feedback_Rating CHECK (Rating BETWEEN 1 AND 5),
        CONSTRAINT FK_Feedback_ClassAttendance
            FOREIGN KEY (ClassAttendanceId)
            REFERENCES dbo.ClassAttendance(Id)
    );
END;
GO
