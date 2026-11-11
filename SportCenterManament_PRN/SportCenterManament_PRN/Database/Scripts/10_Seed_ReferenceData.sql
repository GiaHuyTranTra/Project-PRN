USE [SportsCenterManagement];
GO

SET XACT_ABORT ON;
BEGIN TRANSACTION;

DECLARE @ExpectedRoles TABLE (Id int NOT NULL, Name nvarchar(50) NOT NULL);
INSERT INTO @ExpectedRoles (Id, Name)
VALUES
    (1, N'CenterManager'),
    (2, N'Coach'),
    (3, N'Member'),
    (4, N'Receptionist');

IF EXISTS
(
    SELECT 1
    FROM @ExpectedRoles AS expected
    JOIN dbo.Role AS actual
      ON actual.Id = expected.Id OR actual.Name = expected.Name
    WHERE actual.Id <> expected.Id OR actual.Name <> expected.Name
)
BEGIN
    THROW 51001, 'Role IDs or names do not match the application role mapping.', 1;
END;

INSERT INTO dbo.Role (Id, Name)
SELECT expected.Id, expected.Name
FROM @ExpectedRoles AS expected
WHERE NOT EXISTS
(
    SELECT 1 FROM dbo.Role AS actual WHERE actual.Id = expected.Id
);

DECLARE @Disciplines TABLE (Name nvarchar(80) NOT NULL, Description nvarchar(500) NULL);
INSERT INTO @Disciplines (Name, Description)
VALUES
    (N'Fitness', N'Dữ liệu mẫu cho kiểm thử'),
    (N'Yoga', N'Dữ liệu mẫu cho kiểm thử'),
    (N'Bơi lội', N'Dữ liệu mẫu cho kiểm thử'),
    (N'Cầu lông', N'Dữ liệu mẫu cho kiểm thử');

INSERT INTO dbo.Discipline (Name, Description, IsActive)
SELECT sample.Name, sample.Description, 1
FROM @Disciplines AS sample
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.Discipline AS existing
    WHERE existing.NormalizedName = dbo.NormalizeFlow2Name(sample.Name)
);

IF NOT EXISTS
(
    SELECT 1 FROM dbo.Room
    WHERE NormalizedName = dbo.NormalizeFlow2Name(N'Phòng tập demo')
)
BEGIN
    INSERT INTO dbo.Room (Name, Location, RoomType, Capacity, IsActive)
    VALUES (N'Phòng tập demo', N'Cơ sở chính', N'GYM', 30, 1);
END;

COMMIT TRANSACTION;
GO
