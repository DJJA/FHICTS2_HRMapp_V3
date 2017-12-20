-- Laat alle werknemer informatie zien
SELECT *
--SELECT FirstName, LastName, IsTeamLeader, TeamLeaderId
FROM Employee AS e
LEFT JOIN EmployeeProductionWorker AS pw ON e.Id = pw.EmployeeId
LEFT JOIN EmployeeHRManager AS hr ON e.Id = hr.EmployeeId
LEFT JOIN EmployeeSalesManager AS sm ON e.Id = sm.EmployeeId;

-- Laat alle werknemers zien die geen productiemederwerker of teamleider zijn
SELECT FirstName, LastName, IsTeamLeader, TeamLeaderId
FROM Employee AS e
LEFT JOIN EmployeeProductionWorker AS pw ON e.Id = pw.EmployeeId
WHERE pw.EmployeeId is null;

-- Laat alle productiemedewerkers zien met hun teamleider als ze die hebben
SELECT ep.EmployeeId, e.FirstName, e.LastName, ep.TeamLeaderId, e2.FirstName, e2.LastName
FROM EmployeeProductionWorker AS ep
INNER JOIN Employee AS e ON ep.EmployeeId = e.Id
LEFT JOIN Employee AS e2 ON ep.TeamLeaderId = e2.Id;

SELECT ep.EmployeeId, e.FirstName, e.LastName, ep.TeamLeaderId, (SELECT FirstName FROM Employee WHERE Id = ep.TeamLeaderId) AS FirstName, (SELECT LastName FROM Employee WHERE Id = ep.TeamLeaderId) AS LastName
FROM EmployeeProductionWorker AS ep
INNER JOIN Employee AS e ON ep.EmployeeId = e.Id


-- Hoe vaak een product is besteld
SELECT p.Name, sq.Aantal_keer_besteld, sq.Totale_hoeveelheid_besteld
FROM Product AS p
INNER JOIN (
	SELECT op.ProductId, COUNT(op.ProductId) AS Aantal_keer_besteld, SUM(op.Amount) AS Totale_hoeveelheid_besteld
	FROM Order_Product AS op
	GROUP BY op.ProductId
	--HAVING COUNT(op.ProductId) > 2
	) AS sq ON p.Id = sq.ProductId;

-- Hoeveel teammembers elke teamleider heeft
SELECT e.FirstName, e.LastName, sq.TeamMemberCount
FROM Employee AS e
INNER JOIN (
	SELECT ep.TeamLeaderId, COUNT(ep.TeamLeaderId) AS TeamMemberCount
	FROM EmployeeProductionWorker AS ep
	WHERE ep.IsTeamLeader = 0
	GROUP BY ep.TeamLeaderId
	) AS sq ON e.Id = sq.TeamLeaderId;