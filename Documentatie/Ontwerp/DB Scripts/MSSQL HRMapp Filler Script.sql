INSERT INTO Skillset (Name, Description)
VALUES	('Solderen 1', 'Het solderen van een 4-polige connector.'),
		('Solderen 2', 'Het solderen van een 8-polige connector.'),
		('Solderen 3', 'Het solderen van een 16-polige connector.'),
		('Solderen 4', 'Het solderen van een 32-polige connector.'),
		('Solderen 5', 'Het solderen van een 64-polige connector.'),
		('Deeg kneden', 'Het deeg van het brood kneden.'),
		('Deeg maken', 'Het maken van het deeg voor een brood.'),
		('Deeg in broodvorm kneden', 'Het vormgeven aan het deeg.'),
		('Brood bakken', 'Brood in de oven zetten en op tijd eruit halen.');

INSERT INTO Task (Name, Description, Duration)
VALUES	('Het solderen van de Bleh connector', 'Zie werkinstructies hier: <a>Werkinstructie Bleh connector</a>.', 80),
		('Connectors solderen aan de 555555 verlengkabel', 'Werkbeschrijving hier met pinouts van de connectoren.', 15),
		('Kabels voor de 555555 verlengkabel voorbereiden', 'Dit houdt in het knippen van de 4x0.5^2 kabel en het ontmantelen daarvan.', 5),
		('Maanzaadbrood bakken', '<Recept hier>', 180),
		('Wit tijgerbrood bakken', 'Grrrrr', 150);

INSERT INTO Skillset_Task (SkillsetId, TaskId)
VALUES	(4, 1),
		(2, 2),
		(6, 4),
		(7, 4),
		(8, 4),
		(9, 4),
		(9, 5),
		(6, 5),
		(7, 5),
		(8, 5);

INSERT INTO Employee (FirstName, LastName, PhoneNumber, EmailAddress, Street, HouseNumber, ZipCode, City)
VALUES	('Riley', 'Blue',			1000000000, 'r.blue@bedrijf.nl', 'een straat', '14', '3445AS', 'Uden'),
		('Lito', 'Rodriguez',		1612345678, 'l.rodriguez@bedrijf.nl', 'Een straat', '23', '7124NE', 'Eindhoven'),
		('Tom', 'Neville',			1234567789, 't.neville@bedrijf.nl', 'Andere straat', '12A', '6511EW', 'Leeuwarden'),
		('Charlie', 'Matheson',		4597548532, 'c.matheson@bedrijf.nl', 'Hoekstraat', '54Z', '7712DJ', 'Bladel'),
		('Micheal', 'Scofield',		3747625896, 'm.scofield@bedrijf.nl', 'Bochtlaan', '34', '8733AA', 'Limburg'),
		('Sara', 'Tancredi',		5498742682, 's.tancredi@bedrijf.nl', 'Rechtestraat', '2', '6893RT', 'Texel'),
		('Syd', 'Barrett',			4862587426, 's.barrett@bedrijf.nl', 'katlaan', '4', '7632ED', 'Amsterdam'),
		('David', 'Haller',			5642790532, 'd.haller@bedrijf.nl', 'beatrixlaan', '1', '6541TG', 'Bergen op zoom'),
		('Tyrion', 'Lannister',		1232655634, 't.lannister@bedrijf.nl', 'Kerkstraat', '15', '4386RR', 'Groningen'),
		('Daenerys', 'Targaryen',	4598723658, 'd.targaryen@bedrijf.nl', 'Dieneneweg', '23', '5443BJ', 'Den Haag');

INSERT INTO EmployeeHRManager (EmployeeId)
VALUES	(1), (2);

INSERT INTO EmployeeSalesManager (EmployeeId)
VALUES	(3), (4);

INSERT INTO EmployeeProductionWorker (EmployeeId, IsTeamLeader, TeamLeaderId)
VALUES	(5, 1, NULL),
		(6, 1, NULL),
		(7, 0, 5),
		(8, 0, 5),
		(9, 0, 6),
		(10, 0, 6);

INSERT INTO Product (Name, Description)
VALUES	('KlantA - 366875', 'Verlengkabel'),
		('KlantA - 344545', 'Pompkabel'),
		('KlantA - 377456', 'Pompkabel'),
		('KlantA - 334576', 'Pompkabel'),
		('KlantA - 654345', 'Aansturingskabel'),
		('KlantA - 543542', 'Aansturingskabel'),
		('KlantA - 098565', 'Verlengkabel'),
		('KlantA - 543578', 'Achteras besturing');

INSERT INTO "Order" (EmployeeSalesManagerId, Deadline, Customer, EntryDate)
VALUES	(3, GETDATE(), 'KlantA', GETDATE()),
		(3, GETDATE(), 'KlantA', GETDATE()),
		(4, GETDATE(), 'KlantB', GETDATE()),
		(4, GETDATE(), 'KlantA', GETDATE());