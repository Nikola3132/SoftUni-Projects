/*************************
Problem 1. DDL
************************/

CREATE TABLE Planets(
Id INT IDENTITY,
[Name] VARCHAR(30) NOT NULL,

PRIMARY KEY(Id)
)

CREATE TABLE Spaceports(
Id INT IDENTITY,
[Name] VARCHAR(50) NOT NULL,
PlanetId INT NOT NULL FOREIGN KEY REFERENCES Planets(Id) 

PRIMARY KEY(Id)
)

CREATE TABLE Spaceships(
Id INT  IDENTITY,
[Name] VARCHAR(50) NOT NULL,
Manufacturer VARCHAR(30) NOT NULL,
LightSpeedRate INT DEFAULT(0)

PRIMARY KEY(Id)
)

CREATE TABLE Colonists(
Id INT IDENTITY,
FirstName VARCHAR(20) NOT NULL,
LastName VARCHAR(20) NOT NULL,
Ucn VARCHAR(10) NOT NULL UNIQUE,
BirthDate DATE NOT NULL

PRIMARY KEY(Id)
)

CREATE TABLE Journeys(
Id INT IDENTITY,
JourneyStart DATETIME NOT NULL,
JourneyEnd DATETIME NOT NULL,
Purpose VARCHAR(11),
DestinationSpaceportId INT NOT NULL FOREIGN KEY REFERENCES Spaceports(Id)   ,
SpaceshipId INT NOT NULL FOREIGN KEY REFERENCES Spaceships(Id)

PRIMARY KEY(Id)
)
ALTER TABLE Journeys
ADD CONSTRAINT ck_Journeys_Purpose CHECK(Purpose IN('Medical','Technical','Educational','Military'))


CREATE TABLE TravelCards (
Id INT IDENTITY,
CardNumber CHAR(10) NOT NULL UNIQUE,
JobDuringJourney VARCHAR(8) ,
ColonistId INT NOT NULL FOREIGN KEY REFERENCES Colonists(Id) ,
JourneyId INT NOT NULL FOREIGN KEY REFERENCES Journeys(Id) 

PRIMARY KEY(Id)
)
ALTER TABLE TravelCards
ADD CONSTRAINT ck_TravelCards_JobDuringJourney 
	CHECK (JobDuringJourney IN('Pilot','Engineer','Trooper','Cleaner','Cook'))

/*************************
Problem 2. Insert 
************************/
INSERT INTO Planets ([Name])
VALUES
('Mars'),
('Earth'),
('Jupiter'),
('Saturn')

INSERT INTO Spaceships ([Name],Manufacturer,LightSpeedRate)
VALUES
('Golf','VW',3),
('WakaWaka','Wakanda',4),
('Falcon9','SpaceX',1),
('Bed',	'Vidolov',6)

/*************************
Problem 3.	Update
************************/
UPDATE Spaceships
SET LightSpeedRate += 1
WHERE Id BETWEEN 8 AND 12

/*************************
Problem 4.  Delete
************************/
DELETE FROM TravelCards
WHERE JourneyId IN (1,2,3)

DELETE TOP(3) FROM Journeys

/*************************
Problem 5.Select all travel cards
************************/
SELECT 
	CardNumber
	,JobDuringJourney 
FROM 
	TravelCards 
ORDER BY CardNumber ASC

/*************************
Problem 6.Select all colonists
************************/
SELECT 
	Id
	, CONCAT(FirstName,' ',LastName) AS FullName
	,Ucn 
FROM 
	Colonists ORDER BY FirstName,LastName,Id 

/*************************
Problem 7.Select all military journeys
************************/
SELECT 
	Id
	,Convert(varchar(10),CONVERT(date,JourneyStart,106),103)
	,Convert(varchar(10),CONVERT(date,JourneyEnd,106),103)
FROM 
	Journeys 
WHERE	
	Purpose = 'Military'
ORDER BY JourneyStart

/*************************
Problem 8.Select all pilots
************************/
SELECT 
	c.Id,
	CONCAT(c.FirstName,' ',c.LastName ) AS FullName 
FROM 
	Colonists AS c 
INNER JOIN 
	TravelCards AS tc 
ON c.Id = tc.ColonistId 
	AND 
tc.JobDuringJourney = 'Pilot'
ORDER BY c.Id

/*************************
Problem 9.Count colonists
************************/
SELECT 
	COUNT(c.Id)
FROM 
	Colonists AS c 
INNER JOIN 
	TravelCards AS tc 
ON c.Id = tc.ColonistId 
INNER JOIN 
	Journeys AS j
ON j.Id = tc.JourneyId
	AND
j.Purpose = 'Technical'

/*************************
Problem 10.	Select the fastest spaceship
************************/

DECLARE @Max INT = 
(SELECT 
	MAX(s.LightSpeedRate)
FROM 
	Spaceships AS s) 
	
	SELECT s.Name,sp.Name
FROM 
	Spaceships AS s 
INNER JOIN Journeys AS j 
	ON s.Id = j.SpaceshipId 
INNER JOIN Spaceports AS sp 
	ON j.DestinationSpaceportId = sp.Id 
WHERE s.LightSpeedRate = @Max

/*************************
Problem 11.Select spaceships with pilots younger than 30 years
************************/
SELECT 
	s.Name
	,s.Manufacturer 
FROM
	Journeys AS j 
INNER JOIN  
	Spaceships AS s 
ON 
	s.Id=j.SpaceshipId 
INNER JOIN 
	TravelCards AS tc 
ON 
	tc.JourneyId = j.Id 
INNER JOIN 
	Colonists AS co
ON 
	tc.ColonistId = co.Id 
WHERE 
	DATEDIFF(year ,co.BirthDate,'01/01/2019') < 30 
AND 
	tc.JobDuringJourney = 'Pilot'  
ORDER BY s.[Name]

/*************************
Problem 12. Select all educational mission planets and spaceports
************************/
SELECT 
	p.[Name]
	,sp.[Name]
FROM 
	Planets AS p 
INNER JOIN 
	Spaceports AS sp 
ON
	p.Id = sp.PlanetId 
INNER JOIN 
	Journeys AS j 
ON 
	sp.Id=j.DestinationSpaceportId 
WHERE 
	j.Purpose = 'Educational'
ORDER BY sp.[Name] DESC

/*************************
Problem13.Select all planets and their journey count
************************/
WITH CTE_TakingTheCountFromJourneys (PlanetName, [JourneysCount])
AS
(
SELECT 
	p.[Name]
	,COUNT(j.Id) AS [JourneysCount] 
FROM 
	Planets AS p 
INNER JOIN 
	Spaceports AS sp 
ON 
	p.Id = sp.PlanetId 
INNER JOIN 
	Journeys AS j 
ON 
	sp.Id=j.DestinationSpaceportId 
GROUP BY p.[Name]
)
SELECT * 
FROM 
	CTE_TakingTheCountFromJourneys 
ORDER BY 
	JourneysCount DESC
	,PlanetName ASC

/*************************
Problem 14.	Select the shortest journey
************************/
WITH CTE_TakingInfoAboutDiffs(JourneyId,PlanetName,SpaceportName,JourneyPurpose,[DateDiff])
AS
(
SELECT 
	j.Id
	,p.Name
	,sp.Name
	,j.Purpose
	,DATEDIFF(SECOND,j.JourneyStart,j.JourneyEnd)
FROM 
	Journeys AS j 
INNER JOIN 
	Spaceports AS sp 
ON 
	j.DestinationSpaceportId=sp.Id 
INNER JOIN 
	Planets AS p 
ON 
	p.Id = sp.PlanetId 
),
 CTE_TakingMinDiff ( MinDateDiff)
 AS
(
	SELECT 
		 MIN([DateDiff]) AS minDateDiff
	 FROM 
		CTE_TakingInfoAboutDiffs AS cte
)
SELECT 
	JourneyId
	,PlanetName
	,SpaceportName
	,JourneyPurpose 
FROM 
	CTE_TakingMinDiff ,CTE_TakingInfoAboutDiffs
WHERE 
	[DateDiff] = MinDateDiff

/*************************
Problem 15.	Select the less popular job
************************/
WITH CTE_TakingJourneysInfo(JourneyId,JourneyDiffs)
AS
(
SELECT 
	j.Id 
	,DATEDIFF(minute,j.JourneyStart,j.JourneyEnd) AS Diff 
FROM 
	Journeys AS j 
),
CTE_TakingMaxDiff (MaxDiff)
AS
(
SELECT 
	MAX(JourneyDiffs) 
FROM 
	CTE_TakingJourneysInfo AS cte 
),
CTE_TakingTheLongestJourneyId (LongestJourneyId)
AS
(
SELECT 
	CTE_TakingJourneysInfo.JourneyId 
FROM 
	CTE_TakingMaxDiff
	,CTE_TakingJourneysInfo 
WHERE 
	CTE_TakingJourneysInfo.JourneyDiffs = CTE_TakingMaxDiff.MaxDiff
),
CTE_TakingInfoAboutTheColonistCountPerJob(JobName,ColonistsCount)
AS
(
SELECT
	tc.JobDuringJourney
	,COUNT(tc.ColonistId)
FROM
	TravelCards AS tc 
INNER JOIN 
	CTE_TakingTheLongestJourneyId AS ttlji 
ON 
	tc.JourneyId = ttlji.LongestJourneyId
WHERE 
	tc.JourneyId = ttlji.LongestJourneyId
GROUP BY tc.JobDuringJourney
)
SELECT TOP(1)
	LongestJourneyId
	,JobName 
FROM 
	CTE_TakingInfoAboutTheColonistCountPerJob 
	,CTE_TakingTheLongestJourneyId 
ORDER BY ColonistsCount ASC

/*************************
Problem 16.Select Second Oldest Important Colonist
************************/
WITH CTE_TakingColonistInfoAndRank(FullName,JobDuringJourney,[Rank])
AS
(
SELECT 
	c.FirstName + ' ' + c.LastName  AS FullName
	,tc.JobDuringJourney
	,DENSE_RANK() OVER(PARTITION BY tc.JobDuringJourney ORDER BY c.BirthDate ASC) AS RANK
FROM 
	Colonists AS c 
INNER JOIN 
	TravelCards AS tc 
ON 
	tc.ColonistId=c.Id
INNER JOIN
	Journeys AS j
ON
	j.Id = tc.JourneyId
)
SELECT JobDuringJourney
	,FullName
	,[Rank] 
FROM 
	CTE_TakingColonistInfoAndRank 
WHERE [Rank] = 2

/*************************
Problem 17.Planets and Spaceports
************************/
SELECT
	p.[Name] AS planetName
	,COUNT(s.Id) AS SpacePortCount 
FROM 
	Planets AS p 
LEFT JOIN 
	Spaceports AS s 
ON 
	s.PlanetId=p.Id 
GROUP BY 
	p.[Name]
ORDER BY 
	COUNT(s.Id) DESC
	,p.[Name]
	
/*************************
Problem 18.Get Colonists Count
************************/
CREATE FUNCTION udf_GetColonistsCount(@PlanetName VARCHAR(30))
RETURNS INT
AS
BEGIN
	RETURN (SELECT COUNT(*) FROM Journeys AS j
	JOIN Spaceports AS s ON s.Id = j.DestinationSpaceportId
	JOIN Planets AS p ON p.Id = s.PlanetId
	JOIN TravelCards AS tc ON tc.JourneyId = j.Id
	JOIN Colonists AS c ON c.Id = tc.ColonistId
	WHERE p.Name = @PlanetName)
END	

SELECT dbo.udf_GetColonistsCount('Otroyphus')

/*************************
Problem 19.Change Journey Purpose
************************/
CREATE PROC usp_ChangeJourneyPurpose(@JourneyId INT , @NewPurpose VARCHAR(11))
AS
BEGIN
	BEGIN TRANSACTION
	
	IF((SELECT Id FROM Journeys WHERE Id = @JourneyId) IS NULL)
	BEGIN 
		RAISERROR('The journey does not exist!',16,1)
		RETURN
	END
	
	IF((SELECT Purpose FROM Journeys WHERE Id = @JourneyId) = @NewPurpose)
	BEGIN
		RAISERROR('You cannot change the purpose!',16,1)
		RETURN
	END

	UPDATE Journeys
	SET Purpose = @NewPurpose
	WHERE Id = @JourneyId

	COMMIT
END


/*************************
Problem 20.	Deleted Journeys
************************/
SELECT * INTO DeletedTable FROM Journeys
TRUNCATE TABLE DeletedTable




CREATE TRIGGER t_DeleteJourney
	ON Journeys
	AFTER DELETE
AS
	BEGIN
		INSERT INTO DeletedJourneys(Id,JourneyStart,JourneyEnd,Purpose,DestinationSpaceportId,
		SpaceshipId)
		SELECT Id,JourneyStart,JourneyEnd, Purpose, DestinationSpaceportId, SpaceshipId FROM deleted
	END