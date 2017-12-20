--/* Drop Database */
--DROP DATABASE HRMapp;



--/* Drop Tables */
--USE HRMapp;
--DROP TABLE Skillset;
--DROP TABLE Task;
--DROP TABLE Skillset_Task;

--/* Create Tables */
--USE HRMapp;


DROP TRIGGER BlockDuplicatesSkillset_Task

CREATE TRIGGER BlockDuplicatesSkillset_Task
ON Skillset_Task
INSTEAD OF INSERT
AS
BEGIN
SET NOCOUNT ON;
IF NOT EXISTS (
	SELECT 1 FROM inserted AS i
	INNER JOIN Skillset_Task AS st
	ON i.SkillsetId = st.SkillsetId
	AND i.TaskId = st.TaskId
	)
	BEGIN
		INSERT Skillset_Task (SkillsetId, TaskId)
		SELECT SkillsetId, TaskId FROM inserted;
	END
	ELSE
	BEGIN
		PRINT 'SkillsetId and TaskId pair already exists.';
	END
END

CREATE PROCEDURE sp_OutParamTest
@Id int,
@Name varchar(30) OUTPUT
AS
BEGIN
	SELECT @Name = Name
	FROM Skillset
	WHERE Id = @Id;
END
GO

DROP PROCEDURE sp_OutParamTest

ALTER PROCEDURE sp_UpdateRequiredSkillsets
@TaskId int,
@List SkillsetList READONLY
AS
BEGIN
	SET NOCOUNT ON;
	DELETE FROM Skillset_Task
	WHERE TaskId = @TaskId AND SkillsetId NOT IN (SELECT Id FROM @List);
	PRINT 'middle';
	
	--BEGIN TRY
	--	INSERT INTO Skillset_Task (SkillsetId, TaskId)
	--	SELECT Id, @TaskId FROM @List;
	--END TRY
	--BEGIN CATCH
	--	PRINT 'caugth';
	--END CATCH

	DECLARE @tempSkillsetId int

	DECLARE SkillsetTaskCursor CURSOR FOR
	SELECT Id FROM @List

	OPEN SkillsetTaskCursor

	FETCH NEXT FROM SkillsetTaskCursor into @tempSkillsetId

	WHILE(@@FETCH_STATUS = 0)
	BEGIN
		-- When using triggers
		--INSERT INTO Skillset_Task (SkillsetId, TaskId)
		--	VALUES (@tempSkillsetId, @TaskId)

		BEGIN TRY
			INSERT INTO Skillset_Task (SkillsetId, TaskId)
			VALUES (@tempSkillsetId, @TaskId)
		END TRY
		BEGIN CATCH
			PRINT 'Caught error!!';
		END CATCH
		
		FETCH NEXT FROM SkillsetTaskCursor into @tempSkillsetId
	END
	
	CLOSE SkillsetTaskCursor
	DEALLOCATE SkillsetTaskCursor
END

-- Without cursor
ALTER PROCEDURE sp_UpdateRequiredSkillsets
@TaskId int,
@List SkillsetList READONLY
AS
BEGIN
	SET NOCOUNT ON;
	DELETE FROM Skillset_Task
	WHERE TaskId = @TaskId AND SkillsetId NOT IN (SELECT Id FROM @List);
	PRINT 'middle';
	
	INSERT INTO Skillset_Task (SkillsetId, TaskId)
		SELECT l.Id, @TaskId
		--SELECT *
		FROM @List AS l
		LEFT JOIN Skillset_Task AS st
		ON l.Id = st.SkillsetId
		WHERE st.SkillsetId IS NULL
		OR (st.SkillsetId IS NOT NULL AND TaskId != @TaskId)		
END

DECLARE @lijstje SkillsetList

INSERT INTO @lijstje VALUES(3)
INSERT INTO @lijstje VALUES(4)
INSERT INTO @lijstje VALUES(5)

--SELECT * FROM @lijstje

DECLARE @taskId int
SET @taskId = 4

EXECUTE sp_UpdateRequiredSkillsets @taskId, @lijstje

/* Test */
DECLARE @lijstje SkillsetList

INSERT INTO @lijstje VALUES(3)
INSERT INTO @lijstje VALUES(4)
INSERT INTO @lijstje VALUES(5)
INSERT INTO @lijstje VALUES(8)

--SELECT * FROM @lijstje

DECLARE @taskId int
SET @taskId = 4

		SELECT l.Id, st.SkillsetId, st.TaskId
		FROM @lijstje AS l
		LEFT JOIN Skillset_Task AS st
		ON l.Id = st.SkillsetId
		--WHERE st.SkillsetId IS NULL
		--OR (st.SkillsetId IS NOT NULL AND TaskId != @TaskId)
		GROUP BY l.Id, TaskId
		HAVING TaskId != @TaskId
/* Test */
SELECT SkillsetId, TaskId
			FROM Skillset_Task
			GROUP BY SkillsetId, TaskId
			HAVING 