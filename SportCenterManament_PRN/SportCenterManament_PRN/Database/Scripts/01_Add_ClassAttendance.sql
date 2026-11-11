USE [SportsCenterManagement];
GO

IF OBJECT_ID(N'dbo.ClassAttendance', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.ClassAttendance
    (
        Id int IDENTITY(1,1) NOT NULL,
        ClassRegistrationId int NOT NULL,
        SessionId int NOT NULL,
        MemberId varchar(400) NOT NULL,
        IsPresent bit NOT NULL
            CONSTRAINT DF_ClassAttendance_IsPresent DEFAULT ((0)),
        Notes nvarchar(500) NULL,
        MarkedBy varchar(400) NULL,
        MarkedAt datetime2(7) NULL
            CONSTRAINT DF_ClassAttendance_MarkedAt DEFAULT (sysutcdatetime()),
        CONSTRAINT PK_ClassAttendance PRIMARY KEY CLUSTERED (Id),
        CONSTRAINT UQ_ClassAttendance_Registration UNIQUE (ClassRegistrationId),
        CONSTRAINT FK_ClassAttendance_Registration FOREIGN KEY (ClassRegistrationId)
            REFERENCES dbo.ClassRegistration (Id),
        CONSTRAINT FK_ClassAttendance_Session FOREIGN KEY (SessionId)
            REFERENCES dbo.ClassSession (Id),
        CONSTRAINT FK_ClassAttendance_Member FOREIGN KEY (MemberId)
            REFERENCES dbo.Member (AccountId),
        CONSTRAINT FK_ClassAttendance_MarkedBy FOREIGN KEY (MarkedBy)
            REFERENCES dbo.Account (Id)
    );
END;
GO
