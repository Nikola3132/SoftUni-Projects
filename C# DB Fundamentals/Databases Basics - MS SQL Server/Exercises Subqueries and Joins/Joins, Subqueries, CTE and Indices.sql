--********************
--Problem 1.Employee Address
--********************
SELECT 
	e.EmployeeID
	,e.JobTitle
	,e.AddressID
	,a.AddressText 
FROM
	Employees AS e 
INNER JOIN Addresses AS a 
ON e.AddressID = a.AddressID 
ORDER BY 
	e.AddressID ASC

--********************
--02. Addresses with Towns 
--********************
SELECT TOP(50) 
	e.FirstName
	,e.LastName
	,t.[Name] AS Town
	,a.AddressText 
FROM
	Employees AS e 
  INNER JOIN 
		Addresses AS a ON e.AddressID = a.AddressID 
  INNER JOIN 
		Towns AS t ON a.TownID = t.TownID 
ORDER BY
	FirstName ASC
	,LastName ASC

--********************
--03. Sales Employees 
--********************
SELECT 
	e.EmployeeID
	,e.FirstName
	,e.LastName
	,d.[Name] AS DepartmentName  
FROM
	Employees AS e 
  INNER JOIN 
		Departments AS d ON d.DepartmentID=e.DepartmentID 
  AND 
		d.[Name] = 'Sales' 
ORDER BY 
	e.EmployeeID ASC 

--********************
--04. Employee Departments 
--********************
SELECT TOP(5) 
	e.EmployeeID
	,e.FirstName
	,e.Salary,d.[Name] AS DepartmentName 
FROM Employees AS e 
	  INNER JOIN 
			Departments AS d 
		ON e.DepartmentID = d.DepartmentID 
WHERE 
	e.Salary > 15000 
ORDER BY e.DepartmentID ASC

--********************
--Problem 5.Employees Without Project
--********************
SELECT TOP(3) 
	e.EmployeeID
	, e.FirstName 
FROM Employees AS e 
		LEFT JOIN 
			EmployeesProjects AS ep 
		ON e.EmployeeID = ep.EmployeeID 
WHERE 
	ep.ProjectID IS NULL
ORDER BY e.EmployeeID ASC

--********************
--Problem 6.Employees Hired After
--********************
SELECT 
	e.FirstName
	,e.LastName
	,e.HireDate,d.[Name] AS DeptName 
FROM 
	Employees AS e 
		INNER JOIN 
			Departments AS d 
		ON 
			e.DepartmentID = d.DepartmentID 
		AND 
			d.[Name] IN('Sales' ,'Finance')
WHERE 
	e.HireDate > '1.1.1999' 
ORDER BY e.HireDate ASC

--********************
--Problem 7.Employees with Project
--********************
SELECT TOP(5) 
	e.EmployeeID
	,e.FirstName
	,p.[Name] AS ProjectName 
FROM 
	Employees AS e 
		INNER JOIN 
			EmployeesProjects AS ep 
		ON 
			e.EmployeeID = ep.EmployeeID 
			INNER JOIN  
				Projects AS p 
				ON 
					ep.ProjectID = p.ProjectID 
				AND 
					p.EndDate IS NULL 
				AND 
					p.StartDate > CONVERT(datetime2,'13.08.2002',103) 
ORDER BY e.EmployeeID ASC

--********************
--Problem 8.Employee 24
--********************
SELECT 
	e.EmployeeID
	,e.FirstName
	,	CASE
			 WHEN YEAR(P.StartDate)>= 2005 
			 THEN  NULL 
			 ELSE p.[Name] 
		END 
	AS ProjectName 
FROM
	Employees AS e 
			INNER JOIN 
					EmployeesProjects AS ep 
				ON
						e.EmployeeID = ep.EmployeeID 
					AND
						e.EmployeeID = 24 
				INNER JOIN 
					Projects AS p 
						ON 
							ep.ProjectID = p.ProjectID

--********************
--Problem 9.Employee Manager
--********************
SELECT 
	e.EmployeeID
	,e.FirstName
	,e.ManagerID
	,e.FirstName
FROM
	Employees AS e 
INNER JOIN
	Employees AS em 
ON 
	em.EmployeeID = e.ManagerID 
WHERE 
	e.ManagerID IN(3,7) 
ORDER BY e.EmployeeID ASC

--********************
--Problem 10.Employee Summary
--********************
SELECT TOP(50) 
	e.EmployeeID
	,CONCAT(e.FirstName,' ',e.LastName) AS EmployeeName
	, CONCAT(em.FirstName,' ',em.LastName) AS ManagerName
	, d.[Name] AS DepartmentName 
FROM 
	Employees AS e 
INNER JOIN Employees AS em 
	ON
	e.ManagerID = em.EmployeeID 
INNER JOIN 
	Departments AS d 
ON 
	e.DepartmentID = d.DepartmentID 
ORDER BY e.EmployeeID

--********************
--Problem 11.Min Average Salary
--********************
SELECT 
	MIN(ResultTable.avgS) AS MinAverageSalary 
FROM
	(SELECT 
		e.DepartmentID
		,AVG(e.Salary) AS avgS 
	FROM 
		Employees AS e 
	GROUP BY e.DepartmentID) AS ResultTable

--********************
--Problem 12.Highest Peaks in Bulgaria
--********************
SELECT
	mc.CountryCode
	,m.MountainRange
	,p.PeakName
	,p.Elevation 
FROM
	MountainsCountries AS mc 
INNER JOIN 
	Mountains AS m 
ON
	mc.MountainId = m.Id 
INNER JOIN 
	Peaks AS p 
ON 
	m.Id = p.MountainId  
WHERE 
	mc.CountryCode = 'BG'
AND 
	p.Elevation > 2835  
ORDER BY p.Elevation DESC

--********************
--Problem 13.Count Mountain Ranges
--********************
SELECT
	mc.CountryCode 
	,COUNT(m.MountainRange) AS MontainRanges 
FROM 
	MountainsCountries AS mc 
INNER JOIN 
	Mountains AS m 
ON 
	mc.MountainId= m.Id 
GROUP BY 
	mc.CountryCode 
HAVING mc.CountryCode IN ('US', 'RU', 'BG')

--********************
--Problem 14.Countries with Rivers
--********************
WITH CountriesRiver_CTE (CountryName,RiverName,ContinentCode)
AS
(
SELECT 
	c.CountryName,r.RiverName,c.ContinentCode 
FROM Countries AS c 
	LEFT JOIN CountriesRivers AS cr 
ON c.CountryCode=cr.CountryCode 
	LEFT JOIN Rivers AS r 
ON cr.RiverId=r.Id
)
,
CountriesContinents_CTE (CountryName,RiverName)
AS
(
SELECT cr.CountryName,cr.RiverName 
FROM CountriesRiver_CTE AS cr 
	INNER JOIN Continents AS cont
ON cr.ContinentCode = cont.ContinentCode AND cr.ContinentCode = 'AF'
)

SELECT TOP(5) 
	CountryName
	,RiverName 
FROM 
	CountriesContinents_CTE 
ORDER BY CountryName ASC

--********************
--Problem 14.Countries with Rivers
--********************
WITH ContinentContries_CTE (ContinentCode,CurrencyCode,CurrencyUsage)
AS
(
SELECT ct.ContinentCode,c.CurrencyCode,COUNT(c.CurrencyCode) FROM Continents AS ct INNER JOIN Countries AS c ON c.ContinentCode=ct.ContinentCode GROUP BY ct.ContinentCode,c.CurrencyCode
)
,
ContinentCurrencies_CTE
AS
(
SELECT DISTINCT ContinentCode,CurrencyCode FROM ContinentContries_CTE
)

--********************
--Problem 15.*Continents and Currencies
--********************
WITH ContinentContriesCurrencies_CTE 
AS
(
 SELECT cont.ContinentCode,c.CurrencyCode,COUNT(c.CountryCode) AS CurrencyUsage,DENSE_RANK() OVER(PARTITION BY cont.ContinentCode ORDER BY COUNT(c.CountryCode) DESC) AS CurrancyRank FROM Continents AS cont
 INNER JOIN Countries AS c ON c.ContinentCode = cont.ContinentCode
 GROUP BY cont.ContinentCode,c.CurrencyCode
 HAVING COUNT(C.CountryCode) > 1
 --ORDER BY cont.ContinentCode ASC
)
SELECT ccc.ContinentCode,ccc.CurrencyCode,ccc.CurrencyUsage FROM ContinentContriesCurrencies_CTE AS ccc WHERE ccc.CurrancyRank = 1  ORDER BY ccc.ContinentCode ASC

--********************
--Problem 16.Countries without any Mountains
--********************
SELECT 
	COUNT(c.CountryCode) AS CountryCode 
FROM 
	Countries AS c 
LEFT JOIN 
	MountainsCountries AS mc 
ON 
	c.CountryCode = mc.CountryCode 
WHERE 
	mc.MountainId IS NULL

--********************
--Problem 17.Highest Peak and Longest River by Count
--********************
SELECT TOP(5)
	c.CountryName,
	MAX(p.Elevation) AS HighestPeakElevation,
	MAX(r.[Length]) AS LongestRiverLength
	
FROM 
	Countries AS c 
LEFT JOIN MountainsCountries AS mc 
	ON c.CountryCode = mc.CountryCode 
LEFT JOIN CountriesRivers AS cr 
	ON c.CountryCode = cr.CountryCode 
LEFT JOIN Rivers AS r 
	ON cr.RiverId = r.Id 
LEFT JOIN Peaks AS p 
	ON mc.MountainId=p.MountainId
GROUP BY c.CountryName
ORDER BY 
MAX(p.Elevation) DESC
, MAX(r.[Length]) DESC
, c.CountryName ASC

--********************
--Problem 18.* Highest Peak Name and Elevation by Country
--********************
WITH Peak_CTE 
AS
(
SELECT 
	c.CountryName AS Country,
	p.PeakName AS [Highest Peak Name],
	 p.Elevation AS [Highest Peak Elevation],
	 m.MountainRange AS Mountain,
	DENSE_RANK() OVER(PARTITION BY c.CountryName ORDER BY p.Elevation DESC) AS PeakElevationRank
FROM 
	Countries AS c 
LEFT JOIN MountainsCountries AS mc 
	ON c.CountryCode = mc.CountryCode 
LEFT JOIN CountriesRivers AS cr 
	ON c.CountryCode = cr.CountryCode 
LEFT JOIN Rivers AS r 
	ON cr.RiverId = r.Id 
LEFT JOIN Peaks AS p 
	ON mc.MountainId=p.MountainId
LEFT JOIN Mountains AS m
	ON m.Id= mc.MountainId
)

SELECT TOP(5) 
	pcte.Country
	,CASE
		 WHEN 
			pcte.[Highest Peak Name] IS NULL 
			THEN '(no highest peak)' 
		ELSE 
			pcte.[Highest Peak Name] 
	END,
	CASE 
		WHEN 
			pcte.[Highest Peak Elevation] IS NULL 
			THEN 0 
		ELSE 
			pcte.[Highest Peak Elevation] 
	END,
	CASE 
		WHEN 
			pcte.Mountain IS NULL 
			THEN '(no mountain)' 
		ELSE 
		pcte.Mountain 
	END 
FROM 
	Peak_CTE AS pcte 
WHERE 
	pcte.PeakElevationRank = 1
ORDER BY 
	pcte.Country ASC
	, pcte.[Highest Peak Name] ASC