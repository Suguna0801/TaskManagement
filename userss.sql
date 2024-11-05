use suguna;


CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY,
    FirstName NVARCHAR(50),
    LastName NVARCHAR(50),
	DateOfBirth DATETIME,
    Email NVARCHAR(100),
    PhoneNumber NVARCHAR(15),
    Gender NVARCHAR(10),
    Address NVARCHAR(255),
    State NVARCHAR(50),
    City NVARCHAR(50),
	Username NVARCHAR(50),
    Password NVARCHAR(50)
);
select * from Users;
ALTER TABLE Users
ADD IsNewEmployee BIT NOT NULL DEFAULT 1;


EXEC sp_rename 'Users.UserId', 'Id', 'COLUMN';

ALTER TABLE Users
ADD Role NVARCHAR(50);

select * from Users;
drop table States;

SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'States';
IF OBJECT_ID('dbo.States', 'U') IS NOT NULL 
    DROP TABLE dbo.States;	

-- States Table
CREATE TABLE States (
    StateId INT PRIMARY KEY IDENTITY,
    StateName NVARCHAR(50) NOT NULL
);

select * from states;

-- Cities Table
CREATE TABLE Cities (
    CityId INT PRIMARY KEY IDENTITY,
    CityName NVARCHAR(50) NOT NULL,
    StateId INT FOREIGN KEY REFERENCES States(StateId)
);

drop table States;

drop table Cities;


CREATE PROCEDURE sp_GetCitiesByStateId
    @StateId INT
AS
BEGIN
    SELECT CityId, CityName FROM Cities WHERE StateId = @StateId;
END;

SELECT * 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_NAME = 'Cities';

SELECT * 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_NAME = 'States';

SELECT DB_NAME() AS CurrentDatabase;

SELECT TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE';


select * from Users;

DROP PROCEDURE IF EXISTS sp_InsertUser;
DROP PROCEDURE IF EXISTS sp_UpdateUser;
DROP PROCEDURE IF EXISTS sp_DeleteUser;
DROP PROCEDURE IF EXISTS sp_GetAllUsers;
DROP PROCEDURE IF EXISTS sp_GetUserById;
DROP PROCEDURE IF EXISTS sp_ValidateUser;

CREATE PROCEDURE sp_InsertUser
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @DateOfBirth DATETIME,
    @Email NVARCHAR(100),
    @PhoneNumber NVARCHAR(15),
    @Gender NVARCHAR(10),
    @Role NVARCHAR(50), -- Added Role parameter
    @Address NVARCHAR(255),
    @State NVARCHAR(50),
    @City NVARCHAR(50),
    @Username NVARCHAR(50),
    @Password NVARCHAR(255)
AS
BEGIN
    INSERT INTO Users (FirstName, LastName, DateOfBirth, Email, PhoneNumber, Gender, Role, Address, State, City, Username, Password)
    VALUES (@FirstName, @LastName, @DateOfBirth, @Email, @PhoneNumber, @Gender, @Role, @Address, @State, @City, @Username, @Password)
END


CREATE PROCEDURE sp_UpdateUser
    @Id INT,
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @DateOfBirth DATETIME,
    @Email NVARCHAR(100),
    @PhoneNumber NVARCHAR(15),
    @Gender NVARCHAR(10),
    @Role NVARCHAR(50), -- Added Role parameter
    @Address NVARCHAR(255),
    @State NVARCHAR(50),
    @City NVARCHAR(50),
    @Username NVARCHAR(50)
AS
BEGIN
    UPDATE Users
    SET FirstName = @FirstName,
        LastName = @LastName,
        DateOfBirth = @DateOfBirth,
        Email = @Email,
        PhoneNumber = @PhoneNumber,
        Gender = @Gender,
        Role = @Role, -- Updated Role field
        Address = @Address,
        State = @State,
        City = @City,
        Username = @Username
    WHERE Id = @Id
END


CREATE PROCEDURE sp_DeleteUser
    @Id INT
AS
BEGIN
    DELETE FROM Users WHERE Id = @Id
END

CREATE PROCEDURE sp_GetAllUsers
AS
BEGIN
    SELECT * FROM Users
END

CREATE PROCEDURE sp_GetUserById
    @Id INT
AS
BEGIN
    SELECT * FROM Users WHERE Id = @Id
END

CREATE PROCEDURE sp_ValidateUser
    @Username NVARCHAR(50),
    @Password NVARCHAR(100) -- In a real application, hash this
AS
BEGIN
    SELECT * FROM Users 
    WHERE Username = @Username AND Password = @Password -- Use hashed password in production
END

CREATE TABLE States (
    StateId INT PRIMARY KEY IDENTITY(1,1),
    StateName NVARCHAR(100) NOT NULL
);

CREATE TABLE Cities (
    CityId INT PRIMARY KEY IDENTITY(1,1),
    CityName NVARCHAR(100) NOT NULL,
    StateId INT NOT NULL,
    FOREIGN KEY (StateId) REFERENCES States(StateId)
);

INSERT INTO States (StateName) VALUES ('Tamilnadu');
INSERT INTO States (StateName) VALUES ('Kerala');
INSERT INTO States (StateName) VALUES ('Karnataka');

INSERT INTO Cities (CityName, StateId) VALUES ('Chennai', 1);  
INSERT INTO Cities (CityName, StateId) VALUES ('Madurai', 1);
INSERT INTO Cities (CityName, StateId) VALUES ('Tirunelveli', 1);  
INSERT INTO Cities (CityName, StateId) VALUES ('Kochi', 2);  
INSERT INTO Cities (CityName, StateId) VALUES ('Thiruvanathapuram', 2);  
INSERT INTO Cities (CityName, StateId) VALUES ('Kollam', 2);  
INSERT INTO Cities (CityName, StateId) VALUES ('Hubli', 3);  
INSERT INTO Cities (CityName, StateId) VALUES ('Mysuru', 3);  
INSERT INTO Cities (CityName, StateId) VALUES ('Bangalore', 3);  

SELECT CityName 
FROM Cities 
WHERE StateId = (SELECT StateId FROM States WHERE StateName = @State);

DECLARE @State NVARCHAR(100);
DECLARE @City NVARCHAR(100);
SET @State = 'Tamilnadu';  -- Specify the state name you want to query
SET @City = 'Chennai';  -- Specify the city name you want to filter

-- Select city names based on the specified state and city
SELECT CityName 
FROM Cities 
WHERE StateId = (SELECT StateId FROM States WHERE StateName = @State)
AND CityName = @City;  -- Example of using another variable

-- Declare the variable
DECLARE @State NVARCHAR(100);
-- Set the value for the variable
SET @State = 'Tamilnadu';  -- Specify the state name you want to query



select CityName 
from Cities
where StateId=(select StateId from States where StateName= @State);

drop table States;
drop table Cities;

PRINT 'Before declaration';
DECLARE @State NVARCHAR(100);
PRINT 'After declaration';
SET @State = 'Tamilnadu';  
PRINT 'After setting value';


IF OBJECT_ID('dbo.Cities', 'U') IS NOT NULL
    DROP TABLE dbo.Cities;

IF OBJECT_ID('dbo.States', 'U') IS NOT NULL
    DROP TABLE dbo.States;




drop table Users;
drop table states;
drop table cities;

-- Create States Table
CREATE TABLE States (
    StateId INT PRIMARY KEY IDENTITY,
    StateName NVARCHAR(50) NOT NULL
);

-- Insert example data into States
INSERT INTO States (StateName) VALUES ('Tamilnadu');
INSERT INTO States (StateName) VALUES ('Kerala');
INSERT INTO States (StateName) VALUES ('Karnataka');

-- Create Cities Table
CREATE TABLE Cities (
    CityId INT PRIMARY KEY IDENTITY,
    CityName NVARCHAR(50) NOT NULL,
    StateId INT FOREIGN KEY REFERENCES States(StateId)
);

-- Insert example data into Cities
INSERT INTO Cities (CityName, StateId) VALUES ('Chennai', 1);
INSERT INTO Cities (CityName, StateId) VALUES ('Madurai', 1);
INSERT INTO Cities (CityName, StateId) VALUES ('Kochi', 2);
INSERT INTO Cities (CityName, StateId) VALUES ('Kollam', 2);
INSERT INTO Cities (CityName, StateId) VALUES ('Bangalore', 3);
INSERT INTO Cities (CityName, StateId) VALUES ('Mysore', 3);

-- Declare the variable
DECLARE @State NVARCHAR(100);
DECLARE @City NVARCHAR(100);

-- Set the value for the variables
SET @State = 'Tamilnadu';  -- Specify the state name you want to query
SET @City = 'Chennai';  -- Specify the city name you want to filter

set @State='Tamilnadu';
set @City='Chennai';

-- Select city names based on the specified state and city
SELECT CityName 
FROM Cities 
WHERE StateId = (SELECT StateId FROM States WHERE StateName = @State)
AND CityName = @City;  -- Example of using another variable

-- Example to select all cities for the specified state
SELECT CityName 
FROM Cities
WHERE StateId = (SELECT StateId FROM States WHERE StateName = @State);
