USE master;

/* Create Database */
CREATE DATABASE HRMapp
GO

USE HRMapp;

CREATE TABLE Product(
	Id				INT					IDENTITY(1,1)		PRIMARY KEY,
	Name			VARCHAR(50)			NOT NULL			UNIQUE,
	Description		VARCHAR(MAX)
)
GO

CREATE TABLE Task(
	Id			INT				IDENTITY(1,1)	PRIMARY KEY,
	ProductId	INT				FOREIGN KEY REFERENCES Product(Id),
	Name		VARCHAR(50)		NOT NULL,
	Description VARCHAR(MAX)	NOT NULL,
	Duration	INT
)
GO

ALTER TABLE Task
	ADD CONSTRAINT UQ_ProductId_Name UNIQUE(ProductId, Name)
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

CREATE TABLE DivisionOfTasks(
	Id							INT			IDENTITY(1,1)			PRIMARY KEY,
	EmployeeProductionWorkerId	INT			FOREIGN KEY REFERENCES EmployeeProductionWorker(EmployeeId),	
	TaskId						INT			FOREIGN KEY REFERENCES Task(Id),
	DateTime					DATETIME	NOT NULL
)
GO

CREATE TABLE EmployeeProductionWorker_Task(
	EmployeeProductionWorkerId	INT			FOREIGN KEY REFERENCES EmployeeProductionWorker(EmployeeId),
	TaskId						INT,
	PRIMARY KEY(EmployeeProductionWorkerId, TaskId)
)
GO

ALTER TABLE EmployeeProductionWorker_Task
	ADD CONSTRAINT fk_EmployeeProductionWorker_TaskId
		FOREIGN KEY(TaskId) 
		REFERENCES Task(Id) 
		ON DELETE CASCADE;
GO

CREATE TABLE "Order"(
	Id							INT				IDENTITY(1,1)		PRIMARY KEY,
	EmployeeSalesManagerId		INT				FOREIGN KEY REFERENCES EmployeeSalesManager(EmployeeId),
	Deadline					DATETIME		NOT NULL,
	Customer					VARCHAR(25)		NOT NULL,
	EntryDate					DATETIME		NOT NULL
)
GO

CREATE TABLE Order_Product(
	OrderId			INT			FOREIGN KEY REFERENCES "Order"(Id),
	ProductId		INT			FOREIGN KEY REFERENCES Product(Id),
	Amount			INT			NOT NULL
)
GO