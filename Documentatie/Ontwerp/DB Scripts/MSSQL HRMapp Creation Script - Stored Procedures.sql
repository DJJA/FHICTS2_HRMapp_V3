--Skillset
CREATE PROCEDURE sp_GetSkillsets
AS
begin
	SELECT * FROM Skillset;
end
GO

CREATE PROCEDURE sp_GetSkillsetById
@Id INT
AS
begin
	SELECT * FROM Skillset
	WHERE Id = @Id;
end
GO

CREATE PROCEDURE sp_AddSkillset
@Name VARCHAR(30),
@Description VARCHAR(255)
AS
begin
	INSERT INTO Skillset (Name, Description)
	VALUES (@Name, @Description);
	SELECT SCOPE_IDENTITY();
end
GO

CREATE PROCEDURE sp_UpdateSkillset
@Id INT,
@Name VARCHAR(30),
@Description VARCHAR(255)
AS
begin
	UPDATE Skillset
	SET Name = @Name, Description = @Description
	WHERE Id = @Id;
end
GO

--Task
CREATE PROCEDURE sp_GetTasks
AS
begin
	SELECT * FROM Task;
end
GO

CREATE PROCEDURE sp_GetTaskById
@Id INT
AS
begin
	SELECT * FROM Task
	WHERE Id = @Id;
end
GO

CREATE PROCEDURE sp_AddTask
@Name VARCHAR(50),
@Description VARCHAR(MAX),
@Duration INT
AS
begin
	INSERT INTO Task (Name, Description, Duration)
	VALUES (@Name, @Description, @Duration);
	SELECT SCOPE_IDENTITY();
end
GO

CREATE PROCEDURE sp_UpdateTask
@Id INT,
@Name VARCHAR(50),
@Description VARCHAR(MAX),
@Duration INT
AS
begin
	UPDATE Task
	SET Name = @Name, Description = @Description, Duration = @Duration
	WHERE Id = @Id;
end
GO

CREATE PROCEDURE sp_GetRequiredSkillsets
@TaskId INT
AS
begin
	SELECT * FROM Skillset
	WHERE Id IN
	(SELECT SkillsetId FROM Skillset_Task
	WHERE TaskId = @TaskId);
end
GO

CREATE TYPE SkillsetList
AS TABLE
(
	Id INT
);
GO

---- Without cursor
--CREATE PROCEDURE sp_UpdateRequiredSkillsets
--@TaskId INT,
--@List SkillsetList READONLY
--AS
--begin
--	SET NOCOUNT ON;
--	DELETE FROM Skillset_Task
--	WHERE TaskId = @TaskId AND SkillsetId NOT IN (SELECT Id FROM @List);
	
--	INSERT INTO Skillset_Task (SkillsetId, TaskId)
--		SELECT l.Id, @TaskId
--		FROM @List AS l
--		LEFT JOIN Skillset_Task AS st
--		ON l.Id = st.SkillsetId
--		WHERE SkillsetId IS NULL
--		OR SkillsetId NOT IN (
--			SELECT SkillsetId
--			FROM Skillset_Task
--			WHERE TaskId = @TaskId
--			GROUP BY SkillsetId
--		)
--		GROUP BY l.Id	
--end
--GO

-- Without cursor
CREATE PROCEDURE sp_UpdateRequiredSkillsets
@TaskId INT,
@List SkillsetList READONLY
AS
begin
	SET NOCOUNT ON;

	DELETE FROM Skillset_Task
	WHERE TaskId = @TaskId;
	
	INSERT INTO Skillset_Task (SkillsetId, TaskId)
		SELECT l.Id, @TaskId
		FROM @List AS l
end
GO


CREATE PROCEDURE sp_UpdateEmployee
@Id INT,
@FirstName VARCHAR(20),
@LastName VARCHAR(50),
@PhoneNumber VARCHAR(10),
@EmailAddress VARCHAR(50),
@Street VARCHAR(50),
@HouseNumber VARCHAR(10),
@ZipCode VARCHAR(10),
@City VARCHAR(30)
AS
begin
	UPDATE Employee
	SET FirstName = @FirstName,
	LastName = @LastName,
	PhoneNumber = @PhoneNumber,
	EmailAddress = @EmailAddress,
	Street = @Street,
	HouseNumber = @HouseNumber,
	ZipCode = @ZipCode,
	City = @City
	WHERE Id = @Id;
end
GO

--HRManager
CREATE PROCEDURE sp_GetHRManagers
AS
begin
	SELECT * 
	FROM EmployeeHRManager AS d
	INNER JOIN Employee AS e
	ON d.EmployeeId = e.Id;
end
GO

CREATE PROCEDURE sp_GetHRManagerById
@Id INT
AS
begin
	SELECT *
	FROM EmployeeHRManager AS d
	INNER JOIN Employee AS e
	ON d.EmployeeId = e.Id
	WHERE EmployeeId = @Id;
end
GO

CREATE PROCEDURE sp_AddHRManager
@FirstName VARCHAR(20),
@LastName VARCHAR(50),
@PhoneNumber VARCHAR(10),
@EmailAddress VARCHAR(50),
@Street VARCHAR(50),
@HouseNumber VARCHAR(10),
@ZipCode VARCHAR(10),
@City VARCHAR(30)
AS
begin
	INSERT INTO Employee(FirstName, LastName, PhoneNumber, EmailAddress, Street, HouseNumber, ZipCode, City)
	VALUES (@FirstName, @LastName, @PhoneNumber, @EmailAddress, @Street, @HouseNumber, @ZipCode, @City);
	DECLARE @Id INT = SCOPE_IDENTITY();
	INSERT INTO EmployeeHRManager (EmployeeId)
	VALUES (@Id);
	SELECT @Id;
end
GO

CREATE PROCEDURE sp_UpdateHRManager
@Id INT,
@FirstName VARCHAR(20),
@LastName VARCHAR(50),
@PhoneNumber VARCHAR(10),
@EmailAddress VARCHAR(50),
@Street VARCHAR(50),
@HouseNumber VARCHAR(10),
@ZipCode VARCHAR(10),
@City VARCHAR(30)
AS
begin
	EXECUTE sp_UpdateEmployee @Id, @FirstName, @LastName, @PhoneNumber, @EmailAddress, @Street, @HouseNumber, @ZipCode, @City;
end
GO


--SalesManager
CREATE PROCEDURE sp_GetSalesManagers
AS
begin
	SELECT * 
	FROM EmployeeSalesManager AS d
	INNER JOIN Employee AS e
	ON d.EmployeeId = e.Id;
end
GO

CREATE PROCEDURE sp_GetSalesManagerById
@Id INT
AS
begin
	SELECT *
	FROM EmployeeSalesManager AS d
	INNER JOIN Employee AS e
	ON d.EmployeeId = e.Id
	WHERE EmployeeId = @Id;
end
GO

CREATE PROCEDURE sp_AddSalesManager
@FirstName VARCHAR(20),
@LastName VARCHAR(50),
@PhoneNumber VARCHAR(10),
@EmailAddress VARCHAR(50),
@Street VARCHAR(50),
@HouseNumber VARCHAR(10),
@ZipCode VARCHAR(10),
@City VARCHAR(30)
AS
begin
	INSERT INTO Employee(FirstName, LastName, PhoneNumber, EmailAddress, Street, HouseNumber, ZipCode, City)
	VALUES (@FirstName, @LastName, @PhoneNumber, @EmailAddress, @Street, @HouseNumber, @ZipCode, @City);
	DECLARE @Id INT = SCOPE_IDENTITY();
	INSERT INTO EmployeeSalesManager (EmployeeId)
	VALUES (@Id);
	SELECT @Id;
end
GO

CREATE PROCEDURE sp_UpdateSalesManager
@Id INT,
@FirstName VARCHAR(20),
@LastName VARCHAR(50),
@PhoneNumber VARCHAR(10),
@EmailAddress VARCHAR(50),
@Street VARCHAR(50),
@HouseNumber VARCHAR(10),
@ZipCode VARCHAR(10),
@City VARCHAR(30)
AS
begin
	EXECUTE sp_UpdateEmployee @Id, @FirstName, @LastName, @PhoneNumber, @EmailAddress, @Street, @HouseNumber, @ZipCode, @City;
end
GO


--ProductionWorker
CREATE PROCEDURE sp_GetProductionWorkers
AS
begin
	SELECT *
	FROM EmployeeProductionWorker AS d
	INNER JOIN Employee AS e
	ON d.EmployeeId = e.Id
	WHERE IsTeamLeader = 0;
end
GO

CREATE PROCEDURE sp_GetProductionWorkerById
@Id INT
AS
begin
	SELECT *
	FROM EmployeeProductionWorker AS d
	INNER JOIN Employee AS e
	ON d.EmployeeId = e.Id
	WHERE IsTeamLeader = 0
	AND EmployeeId = @Id;
end
GO

CREATE PROCEDURE sp_AddProductionWorker
@FirstName VARCHAR(20),
@LastName VARCHAR(50),
@PhoneNumber VARCHAR(10),
@EmailAddress VARCHAR(50),
@Street VARCHAR(50),
@HouseNumber VARCHAR(10),
@ZipCode VARCHAR(10),
@City VARCHAR(30),
@TeamLeaderId INT
AS
begin
	INSERT INTO Employee(FirstName, LastName, PhoneNumber, EmailAddress, Street, HouseNumber, ZipCode, City)
	VALUES (@FirstName, @LastName, @PhoneNumber, @EmailAddress, @Street, @HouseNumber, @ZipCode, @City);
	DECLARE @Id INT = SCOPE_IDENTITY();
	INSERT INTO EmployeeProductionWorker (EmployeeId, IsTeamLeader, TeamLeaderId)
	VALUES (@Id, 0, @TeamLeaderId);
	SELECT @Id;
end
GO

CREATE PROCEDURE sp_UpdateProductionWorker
@Id INT,
@FirstName VARCHAR(20),
@LastName VARCHAR(50),
@PhoneNumber VARCHAR(10),
@EmailAddress VARCHAR(50),
@Street VARCHAR(50),
@HouseNumber VARCHAR(10),
@ZipCode VARCHAR(10),
@City VARCHAR(30),
@TeamLeaderId INT
AS
begin
	EXECUTE sp_UpdateEmployee @Id, @FirstName, @LastName, @PhoneNumber, @EmailAddress, @Street, @HouseNumber, @ZipCode, @City;
	UPDATE EmployeeProductionWorker
	SET TeamLeaderId = @TeamLeaderId
	WHERE EmployeeId = @Id;
end
GO


--TeamLeader
CREATE PROCEDURE sp_GetTeamLeaders
AS
begin
	SELECT *
	FROM EmployeeProductionWorker AS d
	INNER JOIN Employee AS e
	ON d.EmployeeId = e.Id
	WHERE IsTeamLeader = 1;
end
GO

CREATE PROCEDURE sp_GetTeamLeaderById
@Id INT
AS
begin
	SELECT *
	FROM EmployeeProductionWorker AS d
	INNER JOIN Employee AS e
	ON d.EmployeeId = e.Id
	WHERE IsTeamLeader = 1
	AND EmployeeId = @Id;
end
GO

CREATE PROCEDURE sp_AddTeamLeader
@FirstName VARCHAR(20),
@LastName VARCHAR(50),
@PhoneNumber VARCHAR(10),
@EmailAddress VARCHAR(50),
@Street VARCHAR(50),
@HouseNumber VARCHAR(10),
@ZipCode VARCHAR(10),
@City VARCHAR(30)
AS
begin
	INSERT INTO Employee(FirstName, LastName, PhoneNumber, EmailAddress, Street, HouseNumber, ZipCode, City)
	VALUES (@FirstName, @LastName, @PhoneNumber, @EmailAddress, @Street, @HouseNumber, @ZipCode, @City);
	DECLARE @Id INT = SCOPE_IDENTITY();
	INSERT INTO EmployeeProductionWorker (EmployeeId, IsTeamLeader)
	VALUES (@Id, 1);
	SELECT @Id;
end
GO

CREATE PROCEDURE sp_UpdateTeamLeader
@Id INT,
@FirstName VARCHAR(20),
@LastName VARCHAR(50),
@PhoneNumber VARCHAR(10),
@EmailAddress VARCHAR(50),
@Street VARCHAR(50),
@HouseNumber VARCHAR(10),
@ZipCode VARCHAR(10),
@City VARCHAR(30)
AS
begin
	EXECUTE sp_UpdateEmployee @Id, @FirstName, @LastName, @PhoneNumber, @EmailAddress, @Street, @HouseNumber, @ZipCode, @City;
end
GO

CREATE PROCEDURE sp_RemoveEmployeeType
@Id INT
AS
begin
	DELETE FROM EmployeeProductionWorker
	WHERE EmployeeId = @Id;
	DELETE FROM EmployeeHRManager
	WHERE EmployeeId = @Id;
	DELETE FROM EmployeeSalesManager
	WHERE EmployeeId = @Id;
end
GO

CREATE PROCEDURE sp_ChangeEmployeeTypeToHRManager
@Id INT,
@FirstName VARCHAR(20),
@LastName VARCHAR(50),
@PhoneNumber VARCHAR(10),
@EmailAddress VARCHAR(50),
@Street VARCHAR(50),
@HouseNumber VARCHAR(10),
@ZipCode VARCHAR(10),
@City VARCHAR(30)
AS
begin
	EXECUTE sp_RemoveEmployeeType @Id;
	INSERT INTO EmployeeHRManager (EmployeeId)
	VALUES (@Id);
	EXECUTE sp_UpdateEmployee @Id, @FirstName, @LastName, @PhoneNumber, @EmailAddress, @Street, @HouseNumber, @ZipCode, @City;
end
GO

CREATE PROCEDURE sp_ChangeEmployeeTypeToSalesManager
@Id INT,
@FirstName VARCHAR(20),
@LastName VARCHAR(50),
@PhoneNumber VARCHAR(10),
@EmailAddress VARCHAR(50),
@Street VARCHAR(50),
@HouseNumber VARCHAR(10),
@ZipCode VARCHAR(10),
@City VARCHAR(30)
AS
begin
	EXECUTE sp_RemoveEmployeeType @Id;
	INSERT INTO EmployeeSalesManager (EmployeeId)
	VALUES (@Id);
	EXECUTE sp_UpdateEmployee @Id, @FirstName, @LastName, @PhoneNumber, @EmailAddress, @Street, @HouseNumber, @ZipCode, @City;
end
GO

CREATE PROCEDURE sp_ChangeEmployeeTypeToProductionWorker
@Id INT,
@FirstName VARCHAR(20),
@LastName VARCHAR(50),
@PhoneNumber VARCHAR(10),
@EmailAddress VARCHAR(50),
@Street VARCHAR(50),
@HouseNumber VARCHAR(10),
@ZipCode VARCHAR(10),
@City VARCHAR(30),
@TeamLeaderId INT
AS
begin
	EXECUTE sp_RemoveEmployeeType @Id;
	INSERT INTO EmployeeProductionWorker (EmployeeId, IsTeamLeader, TeamLeaderId)
	VALUES (@Id, 0, @TeamLeaderId);
	EXECUTE sp_UpdateEmployee @Id, @FirstName, @LastName, @PhoneNumber, @EmailAddress, @Street, @HouseNumber, @ZipCode, @City;
end
GO

CREATE PROCEDURE sp_ChangeEmployeeTypeToTeamLeader
@Id INT,
@FirstName VARCHAR(20),
@LastName VARCHAR(50),
@PhoneNumber VARCHAR(10),
@EmailAddress VARCHAR(50),
@Street VARCHAR(50),
@HouseNumber VARCHAR(10),
@ZipCode VARCHAR(10),
@City VARCHAR(30)
AS
begin
	EXECUTE sp_RemoveEmployeeType @Id;
	INSERT INTO EmployeeProductionWorker (EmployeeId, IsTeamLeader)
	VALUES (@Id, 1);
	EXECUTE sp_UpdateEmployee @Id, @FirstName, @LastName, @PhoneNumber, @EmailAddress, @Street, @HouseNumber, @ZipCode, @City;
end
GO


CREATE PROCEDURE sp_GetEmployeeSkillsets
@EmployeeId INT
AS
begin
	SELECT * FROM Skillset
	WHERE Id IN
	(SELECT SkillsetId FROM Skillset_ProductionWorker
	WHERE EmployeeProductionWorkerId = @EmployeeId);
end
GO

CREATE PROCEDURE sp_UpdateEmployeeSkillsets
@EmployeeId INT,
@List SkillsetList READONLY
AS
begin
	SET NOCOUNT ON;

	DELETE FROM Skillset_ProductionWorker
	WHERE EmployeeProductionWorkerId = @EmployeeId;
	
	INSERT INTO Skillset_ProductionWorker (SkillsetId, EmployeeProductionWorkerId)
		SELECT l.Id, @EmployeeId
		FROM @List AS l
end
GO

CREATE PROCEDURE sp_GetTeamMembers
@TeamLeaderId INT
AS
begin
	SELECT *
	FROM EmployeeProductionWorker AS ep
	INNER JOIN Employee AS e
	ON ep.EmployeeId = e.Id
	WHERE ep.TeamLeaderId = @TeamLeaderId;
end
GO


--Product
CREATE PROCEDURE sp_GetProducts
AS
begin
	SELECT * FROM Product;
end
GO

CREATE PROCEDURE sp_GetProductById
@Id INT
AS
begin
	SELECT * FROM Product
	WHERE Id = @Id;
end
GO

CREATE PROCEDURE sp_AddProduct
@Name VARCHAR(50),
@Description VARCHAR(MAX)
AS
begin
	INSERT INTO Product (Name, Description)
	VALUES (@Name, @Description);
	SELECT SCOPE_IDENTITY();
end
GO

CREATE PROCEDURE sp_UpdateProduct
@Id INT,
@Name VARCHAR(50),
@Description VARCHAR(MAX)
AS
begin
	UPDATE Product
	SET Name = @Name, Description = @Description
	WHERE Id = @Id;
end
GO

CREATE PROCEDURE sp_GetProductRequiredTasks
@ProductId INT
AS
begin
	SELECT * FROM Task
	WHERE Id IN
	(SELECT TaskId FROM Task_Product
	WHERE ProductId = @ProductId);
end
GO

CREATE PROCEDURE sp_UpdateProductRequiredTasks
@ProductId INT,
@List SkillsetList READONLY
AS
begin
	SET NOCOUNT ON;

	DELETE FROM Task_Product
	WHERE ProductId = @ProductId;
	
	INSERT INTO Task_Product (TaskId, ProductId)
		SELECT l.Id, @ProductId
		FROM @List AS l
end
GO


--Order
CREATE PROCEDURE sp_GetOrders
AS
begin
	SELECT * FROM "Order";
end
GO

CREATE PROCEDURE sp_GetOrderById
@Id INT
AS
begin
	SELECT * FROM "Order"
	WHERE Id = @Id;
end
GO

CREATE PROCEDURE sp_AddOrder
@EmployeeSalesManagerId INT,
@Deadline DATETIME,
@Customer VARCHAR(25)
AS
begin
	INSERT INTO "Order" (EmployeeSalesManagerId, Deadline, EntryDate, Customer)
	VALUES (@EmployeeSalesManagerId, @Deadline, GETDATE(), @Customer);
	SELECT SCOPE_IDENTITY();
end
GO

CREATE PROCEDURE sp_UpdateOrder
@Id INT,
@Deadline DATETIME,
@Customer VARCHAR(25)
AS
begin
	UPDATE "Order"
	SET Deadline = @Deadline, Customer = @Customer
	WHERE Id = @Id;
end
GO