USE master;

/* Create Database */
CREATE DATABASE HRMapp
GO

USE HRMapp;

CREATE TABLE Skillset(
	Id			INT				IDENTITY(1,1)	PRIMARY KEY,
	Name		VARCHAR(30)		NOT NULL		UNIQUE,
	Description	VARCHAR(255)	NOT NULL
)
GO

CREATE TABLE Task(
	Id			INT				IDENTITY(1,1)	PRIMARY KEY,
	Name		VARCHAR(50)		NOT NULL		UNIQUE,
	Description VARCHAR(MAX)	NOT NULL,
	Duration	INT
)
GO

CREATE TABLE Skillset_Task(
	SkillsetId	INT				NOT NULL		FOREIGN KEY REFERENCES Skillset(Id),
	TaskId		INT				NOT NULL		FOREIGN KEY REFERENCES Task(Id),
	PRIMARY KEY(SkillsetId, TaskId)
)
GO

CREATE TABLE Employee(
	Id				INT				IDENTITY(1,1)	PRIMARY KEY,
	FirstName		VARCHAR(20)		NOT NULL,
	LastName		VARCHAR(50)		NOT NULL,
	PhoneNumber		VARCHAR(10),
	EmailAddress	VARCHAR(50),
	Street			VARCHAR(50),
	HouseNumber		VARCHAR(10),
	ZipCode			VARCHAR(10),
	City			VARCHAR(30)
)
GO

CREATE TABLE EmployeeHRManager(
	EmployeeId	INT				FOREIGN KEY REFERENCES Employee(Id),
	PRIMARY KEY(EmployeeId)
)
GO

CREATE TABLE EmployeeSalesManager(
	EmployeeId	INT				FOREIGN KEY REFERENCES Employee(Id),
	PRIMARY KEY(EmployeeId)
)
GO

CREATE TABLE EmployeeProductionWorker(
	EmployeeId		INT		FOREIGN KEY REFERENCES Employee(Id),
	TeamLeaderId	INT,
	IsTeamLeader	BIT		NOT NULL,
	PRIMARY KEY(EmployeeId)
)
GO

ALTER TABLE EmployeeProductionWorker
	ADD CONSTRAINT fk_Employee
		FOREIGN KEY(TeamLeaderId) 
		REFERENCES EmployeeProductionWorker(EmployeeId) 
		ON DELETE NO ACTION;
GO

CREATE TRIGGER trRemoveTeamLeaderIds
ON EmployeeProductionWorker
INSTEAD OF DELETE
AS
begin
	UPDATE EmployeeProductionWorker
	SET TeamLeaderId = NULL
	WHERE TeamLeaderId IN(SELECT EmployeeId FROM deleted);

	DELETE EmployeeProductionWorker
	FROM EmployeeProductionWorker AS pw
	INNER JOIN deleted AS d
	ON pw.EmployeeId = d.EmployeeId;
end
GO

CREATE TABLE Skillset_ProductionWorker(
	SkillsetId					INT		NOT NULL		FOREIGN KEY REFERENCES Skillset(Id),
	EmployeeProductionWorkerId	INT		NOT NULL		FOREIGN KEY REFERENCES EmployeeProductionWorker(EmployeeId),
	PRIMARY KEY(SkillsetId, EmployeeProductionWorkerId)
)
GO

CREATE TABLE Task_ProductionWorker(
	Id							INT			IDENTITY(1,1)	PRIMARY KEY,		
	TaskId						INT			NOT NULL		FOREIGN KEY REFERENCES Task(Id),
	EmployeeProductionWorkerId	INT			NOT NULL		FOREIGN KEY REFERENCES EmployeeProductionWorker(EmployeeId),
	DateTime					DATETIME	NOT NULL
)
GO

--ALTER TABLE Skillset_Task
--	ADD CONSTRAINT unique_pair UNIQUE(SkillsetId, TaskId);
--	--DROP CONSTRAINT unique_pair
--GO


-- new requirements
CREATE TABLE Product(
	Id				INT					IDENTITY(1,1)		PRIMARY KEY,
	Name			VARCHAR(50)			NOT NULL			UNIQUE,
	Description		VARCHAR(MAX)
)
GO

CREATE TABLE Task_Product(
	TaskId			INT			NOT NULL		FOREIGN KEY REFERENCES Task(Id),
	ProductId		INT			NOT NULL		FOREIGN KEY REFERENCES Product(Id),
	PRIMARY KEY(TaskId, ProductId)	-- KLOPT DIT WEL?
)
GO

CREATE TABLE "Order"(
	Id							INT				IDENTITY(1,1)		PRIMARY KEY,
	EmployeeSalesManagerId		INT				NOT NULL			FOREIGN KEY REFERENCES EmployeeSalesManager(EmployeeId),
	Deadline					DATETIME		NOT NULL,
	Customer					VARCHAR(25)		NOT NULL,
	EntryDate					DATETIME		NOT NULL
)
GO

CREATE TABLE Order_Product(
	ProductId		INT			NOT NULL		FOREIGN KEY REFERENCES Product(Id),
	OrderId			INT			NOT NULL		FOREIGN KEY REFERENCES "Order"(Id),
	Amount			INT			NOT NULL
)
GO