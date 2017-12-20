CREATE TYPE IntegerList
AS TABLE
(
	Id INT
);
GO

CREATE TYPE OrderItems
AS TABLE
(
	ProductId INT,
	Amount INT
);
GO

--###########################################################################################################################################################
-- Task
--###########################################################################################################################################################
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

CREATE PROCEDURE sp_UpdateTaskQualifiedEmployees
@TaskId INT,
@QualifiedEmployeeIds IntegerList READONLY
AS
begin
	DELETE FROM EmployeeProductionWorker_Task
	WHERE TaskId = @TaskId;
	
	INSERT INTO EmployeeProductionWorker_Task (EmployeeProductionWorkerId, TaskId)
		SELECT qe.Id, @TaskId
		FROM @QualifiedEmployeeIds AS qe
end
GO

CREATE PROCEDURE sp_AddTask
@ProductId INT,
@Name VARCHAR(50),
@Description VARCHAR(MAX),
@Duration INT,
@QualifiedEmployeeIds IntegerList READONLY
AS
begin
	INSERT INTO Task (ProductId, Name, Description, Duration)
	VALUES (@ProductId, @Name, @Description, @Duration);
	DECLARE @TaskId INT = SCOPE_IDENTITY();
	EXECUTE sp_UpdateTaskQualifiedEmployees @TaskId = @TaskId, @QualifiedEmployeeIds = @QualifiedEmployeeIds;
	SELECT @TaskId;
end
GO

CREATE PROCEDURE sp_UpdateTask
@Id INT,
@Name VARCHAR(50),
@Description VARCHAR(MAX),
@Duration INT,
@QualifiedEmployeeIds IntegerList READONLY
AS
begin
	UPDATE Task
	SET Name = @Name, Description = @Description, Duration = @Duration
	WHERE Id = @Id;
	EXECUTE sp_UpdateTaskQualifiedEmployees @TaskId = @Id, @QualifiedEmployeeIds = @QualifiedEmployeeIds;
end
GO

CREATE PROCEDURE sp_DeleteTaskById
@Id INT
AS
begin
	DELETE FROM Task
	WHERE Id = @Id;
end
GO

CREATE PROCEDURE sp_GetTasksByProductId
@ProductId int
AS
begin
	SELECT *
	FROM Task
	WHERE ProductId = @ProductId;
end
GO


--###########################################################################################################################################################
-- Product
--###########################################################################################################################################################
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

--###########################################################################################################################################################
-- Order
--###########################################################################################################################################################
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

CREATE FUNCTION fn_GetOrderItems(@OrderId INT)
RETURNS TABLE
AS
RETURN
(
	SELECT op.ProductId, op.Amount, p.Name
	FROM Order_Product AS op
	INNER JOIN Product AS p ON op.ProductId = p.Id
	WHERE OrderId = @OrderId
);
GO

CREATE PROCEDURE sp_UpdateOrderItems
@OrderId INT,
@OrderItems OrderItems READONLY
AS
begin
	DELETE FROM Order_Product
	WHERE OrderId = @OrderId;

	INSERT INTO Order_Product (OrderId, ProductId, Amount)
		SELECT @OrderId, oi.ProductId, oi.Amount
		FROM @OrderItems AS oi
end
GO

CREATE PROCEDURE sp_AddOrder
@EmployeeSalesManagerId INT,
@Deadline DATETIME,
@Customer VARCHAR(25),
@OrderItems OrderItems READONLY
AS
begin
	INSERT INTO "Order" (EmployeeSalesManagerId, Deadline, EntryDate, Customer)
	VALUES (@EmployeeSalesManagerId, @Deadline, GETDATE(), @Customer);
	DECLARE @Id INT = SCOPE_IDENTITY();
	EXECUTE sp_UpdateOrderItems @OrderId = @Id, @OrderItems = @OrderItems;
	SELECT @Id;
end
GO

CREATE PROCEDURE sp_UpdateOrder
@Id INT,
@Deadline DATETIME,
@Customer VARCHAR(25),
@OrderItems OrderItems READONLY
AS
begin
	UPDATE "Order"
	SET Deadline = @Deadline, Customer = @Customer
	WHERE Id = @Id;
	EXECUTE sp_UpdateOrderItems @OrderId = @Id, @OrderItems = @OrderItems;
end
GO

--###########################################################################################################################################################
-- Employee
--###########################################################################################################################################################
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

CREATE PROCEDURE sp_GetEmployeesByTaskId
@TaskId INT
AS
begin
	SELECT e.Id, e.FirstName, e.LastName
	FROM (
		EmployeeProductionWorker AS ep
		INNER JOIN EmployeeProductionWorker_Task AS et
		ON ep.EmployeeId = et.EmployeeProductionWorkerId
		)
	INNER JOIN Employee AS e
	ON ep.EmployeeId = e.Id
	WHERE et.TaskId = @TaskId;
end
GO

-------------------------------------------------------------------------------------------------------------------------------------------------------------
-- HRManager
-------------------------------------------------------------------------------------------------------------------------------------------------------------
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

-------------------------------------------------------------------------------------------------------------------------------------------------------------
-- SalesManager
-------------------------------------------------------------------------------------------------------------------------------------------------------------
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

-------------------------------------------------------------------------------------------------------------------------------------------------------------
-- ProductionWorker
-------------------------------------------------------------------------------------------------------------------------------------------------------------
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

-------------------------------------------------------------------------------------------------------------------------------------------------------------
-- TeamLeader
-------------------------------------------------------------------------------------------------------------------------------------------------------------
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

-------------------------------------------------------------------------------------------------------------------------------------------------------------
-- Change Employee Type
-------------------------------------------------------------------------------------------------------------------------------------------------------------
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