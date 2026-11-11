USE [SportsCenterManagement];
GO

/* Run only after Manager has created the named class and today's scheduled session. */
DECLARE @MemberEmail varchar(100) = 'member.test02@example.com';
DECLARE @ClassName nvarchar(100) = N'C001';
DECLARE @SessionDate date = CONVERT(date, GETDATE());
DECLARE @StartTime time(0) = '08:00'; -- Change to the time selected in Assign Coach.
	
DECLARE @MemberId varchar(400);
DECLARE @SessionId int;

SELECT @MemberId = member.AccountId
FROM dbo.Account AS account
JOIN dbo.Member AS member ON member.AccountId = account.Id
WHERE account.Email = @MemberEmail;

IF @MemberId IS NULL
    THROW 51010, 'Member email was not found in Account/Member. Register that member first.', 1;

SELECT @SessionId = session.Id
FROM dbo.ClassSession AS session
JOIN dbo.Class AS class ON class.Id = session.ClassId
WHERE class.Name = @ClassName
  AND session.SessionDate = @SessionDate
  AND session.StartTime = @StartTime
  AND session.Status = 'SCHEDULED';

IF @SessionId IS NULL
    THROW 51011, 'Scheduled test session not found. Check class name, date, time, and status.', 1;

SET XACT_ABORT ON;
BEGIN TRANSACTION;

IF NOT EXISTS
(
    SELECT 1 FROM dbo.ClassRegistration
    WHERE SessionId = @SessionId AND MemberId = @MemberId AND Status = 'BOOKED'
)
BEGIN
    UPDATE dbo.ClassSession
    SET BookedCount = BookedCount + 1
    WHERE Id = @SessionId AND BookedCount < Capacity;

    IF @@ROWCOUNT <> 1
        THROW 51012, 'The session is full. Increase its capacity or choose another session.', 1;

    INSERT INTO dbo.ClassRegistration
        (SessionId, MemberId, Status, RegisteredVia, RegisteredBy)
    VALUES
        (@SessionId, @MemberId, 'BOOKED', 'MEMBER', @MemberId);
END;

COMMIT TRANSACTION;

SELECT registration.Id AS ClassRegistrationId,
       registration.SessionId,
       registration.MemberId,
       registration.Status,
       session.SessionDate,
       session.StartTime
FROM dbo.ClassRegistration AS registration
JOIN dbo.ClassSession AS session ON session.Id = registration.SessionId
WHERE registration.SessionId = @SessionId
  AND registration.MemberId = @MemberId
  AND registration.Status = 'BOOKED';
GO
