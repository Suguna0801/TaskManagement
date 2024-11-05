use  suguna;
CREATE TABLE Tasks (
    TaskId INT PRIMARY KEY IDENTITY,
    Title NVARCHAR(100),
    Description NVARCHAR(255),
    Deadline DATETIME,
    Role NVARCHAR(50) -- Frontend, Backend, Designer
);
SELECT COLUMN_NAME
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Tasks';

CREATE PROCEDURE AssignTask
    @TaskId INT,
    @AssignedUserId INT
AS
BEGIN
    UPDATE Tasks
    SET AssignedUserId = @AssignedUserId
    WHERE TaskId = @TaskId;
END

UPDATE Tasks
SET AssignedUserId = @AssignedUserId
WHERE TaskId = @TaskId;


DROP TABLE IF EXISTS Tasks;

ALTER TABLE Tasks
ADD AssignedUserId INT NULL,
    CONSTRAINT FK_Tasks_Users FOREIGN KEY (AssignedUserId) REFERENCES Users(UserId);

SELECT *
FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
WHERE TABLE_NAME = 'Tasks' AND CONSTRAINT_TYPE = 'FOREIGN KEY';

EXEC sp_fkeys 'Tasks';
select * from Tasks;
drop table UserTasks;
CREATE TABLE UserTasks (
    UserTaskId INT PRIMARY KEY IDENTITY,
    UserId INT, -- References Users table
    TaskId INT, -- References Tasks table
    Deadline DATETIME,
    IsCompleted BIT DEFAULT 0, -- 0 = Incomplete, 1 = Completed
    CompletionDate DATETIME NULL, -- Tracks when the task was completed, if applicable
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (TaskId) REFERENCES Tasks(TaskId)
);
SELECT TaskId, Title, Description, Deadline, IsCompleted
FROM UserTasks
WHERE AssignedUserId = @Username

CREATE PROCEDURE GetAssignedTasksForUser
    @Username NVARCHAR(50)  -- Declare the @Username parameter
AS
BEGIN
    SELECT 
        T.TaskId, 
        T.Title, 
        T.Description, 
        T.Deadline, 
        UT.IsCompleted  -- assuming IsCompleted is in UserTasks
    FROM 
        UserTasks UT	
    JOIN 
        Users U ON UT.UserId = U.Id
    JOIN 
        Tasks T ON UT.TaskId = T.TaskId  -- Join Tasks to get Title, Description, and other details
    WHERE 
        U.Username = @Username;  -- Filter by username
END
EXEC GetAssignedTasksForUser @Username = 'Nimalan';



select * from UserTasks;
ALTER TABLE Tasks
ADD AssignedUserId INT NULL; -- Adjust the data type as needed


ALTER TABLE Tasks
ALTER COLUMN Deadline DATETIME NULL;  -- If you want to keep the Deadline column but allow nulls

drop table Tasks;

ALTER TABLE Tasks
ADD CONSTRAINT FK_AssignedUser
FOREIGN KEY (AssignedUserId) REFERENCES Users(Id)
ON DELETE CASCADE;

ALTER TABLE Tasks
DROP CONSTRAINT FK_AssignedUser;


ALTER TABLE Tasks
ALTER COLUMN AssignedUserId INT NULL;

ALTER TABLE Tasks
ALTER COLUMN AssignedUserId INT NOT NULL;

ALTER TABLE Tasks
ALTER COLUMN AssignedUserId INT NULL;



CREATE PROCEDURE AddTask
    @Title NVARCHAR(255),
    @Description NVARCHAR(MAX),
    @AssignedUserId INT=NULL
AS
BEGIN
    INSERT INTO Tasks (Title, Description, AssignedUserId)
    VALUES (@Title, @Description, @AssignedUserId);
END

CREATE PROCEDURE AddTask
    @Title NVARCHAR(100),
    @Description NVARCHAR(255),
    @AssignedUserId INT,
    @Deadline DATETIME,
    @IsCompleted BIT
AS
BEGIN
    INSERT INTO Tasks (Title, Description, AssignedUserId, Deadline, IsCompleted)
    VALUES (@Title, @Description, @AssignedUserId, @Deadline, @IsCompleted);
END

drop procedure AddTask;

CREATE PROCEDURE GetAllTasks
AS
BEGIN
    SELECT * FROM Tasks;
END

CREATE PROCEDURE GetAssignedTasks
AS
BEGIN
    SELECT Id, Title, Description, Deadline, IsCompleted, AssignedUserId
    FROM Tasks
    WHERE AssignedUserId IS NOT NULL
END

-- Stored Procedure to Retrieve Unassigned Tasks
CREATE PROCEDURE GetUnassignedTasks
AS
BEGIN
    SELECT Id, Title, Description, Deadline, IsCompleted
    FROM Tasks
    WHERE AssignedUserId IS NULL
END
drop procedure GetassignedTasks;
drop procedure GetUnassignedTasks;
CREATE PROCEDURE UpdateTask
    @TaskId INT,
    @Title NVARCHAR(255),
    @Description NVARCHAR(MAX)
AS
BEGIN
    UPDATE Tasks
    SET Title = @Title,
        Description = @Description

    WHERE TaskId = @TaskId;
END

drop procedure UpdateTask;

CREATE PROCEDURE DeleteTask
    @TaskId INT
AS
BEGIN
    DELETE FROM Tasks WHERE TaskId = @TaskId;
END

drop procedure DeleteTask;
ALTER TABLE Tasks
ADD CONSTRAINT FK_AssignedUser FOREIGN KEY (AssignedUserId) REFERENCES Users(Id);

ALTER TABLE Tasks
DROP CONSTRAINT FK_AssignedUser;

CREATE INDEX IDX_AssignedUser ON Tasks (AssignedUserId);
CREATE INDEX IDX_Deadline ON Tasks (Deadline);

CREATE PROCEDURE AddTask
@Title NVARCHAR(255),
@Description NVARCHAR(MAX),
@AssignedUserId INT,
@Deadline DATETIME
AS
BEGIN
    INSERT INTO Tasks (Title, Description, AssignedUserId, Deadline)
    VALUES (@Title, @Description, @AssignedUserId, @Deadline);
END

CREATE PROCEDURE sp_AssignTaskToUser
    @TaskId INT,
    @UserId INT
AS
BEGIN
    UPDATE Tasks
    SET AssignedUserId = @UserId
    WHERE Id = @TaskId;
END

CREATE PROCEDURE AssignTaskToUser
    @TaskId INT,
    @UserId INT
AS
BEGIN
    UPDATE Tasks
    SET AssignedUserId = @UserId, IsCompleted = 0 -- assuming you want to mark it uncompleted upon assignment
    WHERE Id = @TaskId
END


EXEC sp_AssignTaskToUser @TaskId = 1, @UserId = 5;  -- Assign task with ID 1 to user with ID 5


CREATE PROCEDURE sp_GetAssignedTasksByUserId
    @UserId INT
AS
BEGIN
    SELECT * FROM Tasks
    WHERE AssignedUserId = @UserId;
END;

CREATE PROCEDURE GetUnassignedTasks
AS
BEGIN
    SELECT Id, Title, Description, Deadline, IsCompleted
    FROM Tasks
    WHERE AssignedUserId IS NULL
END

CREATE PROCEDURE GetAssignedTasksForUser
    @Username NVARCHAR(255)
AS
BEGIN
    SELECT Id, Title, Description, Deadline, IsCompleted
    FROM Tasks
    WHERE AssignedUserId = (SELECT Id FROM Users WHERE Username = @Username)
      AND IsCompleted = 0; -- Assuming IsCompleted is a flag indicating task completion
END
drop procedure GetAssignedTasksForUser;

CREATE PROCEDURE GetCompletedTasksForUser
    @Username NVARCHAR(255)
AS
BEGIN
    SELECT Id, Title, Description
    FROM Tasks
    WHERE AssignedUserId = (SELECT Id FROM Users WHERE Username = @Username)
      AND IsCompleted = 1; -- Assuming IsCompleted is a flag indicating task completion
END



CREATE PROCEDURE AssignTaskToUser
    @UserId INT,
    @TaskId INT,
    @Deadline DATETIME
AS
BEGIN
    INSERT INTO UserTasks (UserId, TaskId, Deadline, IsCompleted)
    VALUES (@UserId, @TaskId, @Deadline, 0) -- Initially not completed
END

drop procedure AssignTaskToUser;

CREATE PROCEDURE sp_CompleteTask
    @UserTaskId INT
AS
BEGIN
    UPDATE UserTasks
    SET IsCompleted = 1,
        CompletionDate = GETDATE()
    WHERE UserTaskId = @UserTaskId
END

CREATE PROCEDURE sp_GetUserTasks
    @UserId INT
AS
BEGIN
    SELECT ut.UserTaskId, t.Title, t.Description, ut.Deadline, ut.IsCompleted, ut.CompletionDate
    FROM UserTasks ut
    JOIN Tasks t ON ut.TaskId = t.TaskId
    WHERE ut.UserId = @UserId
END

CREATE PROCEDURE AssignTaskToUser
    @UserId INT,
    @TaskId INT,
    @Deadline DATETIME
AS
BEGIN
    INSERT INTO UserTasks (UserId, TaskId, Deadline, IsCompleted, CompletionDate)
    VALUES (@UserId, @TaskId, @Deadline, 0, NULL);
END
drop procedure AssignTaskToUser;
USE suguna; -- Ensure you are in the correct database


CREATE PROCEDURE AssignUserTask
    @TaskId INT,
    @UserId INT,
    @Deadline DATE
AS
BEGIN
    -- Check if the task and user exist before assigning
    IF EXISTS (SELECT 1 FROM Tasks WHERE TaskId = @TaskId)
        AND EXISTS (SELECT 1 FROM Users WHERE Id = @UserId)
    BEGIN
        -- Insert into UserTasks table to assign the task
        INSERT INTO UserTasks (TaskId, UserId, Deadline, IsCompleted)
        VALUES (@TaskId, @UserId, @Deadline, 0); -- 0 for incomplete by default

        PRINT 'Task assigned successfully.';
    END
    ELSE
    BEGIN
        -- Handle cases where task or user doesn't exist
        PRINT 'Task or User not found.';
    END
END;
drop procedure AssignUserTask;
CREATE PROCEDURE AssignUserTask
    @TaskId INT,
    @AssignedUserId INT,
    @Deadline DATETIME
AS
BEGIN
    UPDATE Tasks
    SET AssignedUserId = @AssignedUserId,
        Deadline = @Deadline
    WHERE TaskId = @TaskId;
END

GO

CREATE PROCEDURE GetUsersByRole
    @Role NVARCHAR(50)
AS
BEGIN
    SELECT Id, FirstName, LastName
    FROM Users
    WHERE Role = @Role
END

drop procedure AssignTaskToUser;

CREATE PROCEDURE AssignTaskToUser
    @TaskId INT,
    @UserId INT
AS
BEGIN
    -- Update the task to assign it to the specified user
    UPDATE Tasks
    SET AssignedUserId = @UserId
    WHERE TaskId = @TaskId;

    -- Remove the user from new employees if applicable
    UPDATE Users
    SET IsNewEmployee = 0
    WHERE Id = @UserId;
END;

CREATE PROCEDURE AssignTaskToUser
    @TaskId INT,
    @UserId INT
AS
BEGIN
    -- Update the task to assign it to the specified user
    UPDATE Tasks
    SET AssignedUserId = @UserId
    WHERE TaskId = @TaskId;

    -- Remove the user from new employees if applicable
    UPDATE Users
    SET IsNewEmployee = 0
    WHERE Id = @UserId;
END;

drop procedure AssignTaskToUser;

SELECT * FROM Tasks WHERE AssignedUserId IS NOT NULL;

SELECT * FROM Users WHERE IsNewEmployee = 1;