--********************
--1.Problem
--********************
SELECT 
	COUNT(*) AS TotalCount 
FROM
	 WizzardDeposits
	 
--********************
--2.Problem
--******************** 
SELECT 
	MAX(MagicWandSize) AS LongestMagicWand 
FROM 
	WizzardDeposits

--********************
--3.Problem
--******************** 
SELECT 
	wd.DepositGroup, 
	MAX(wd.MagicWandSize) AS LongestMagicWand 
FROM 
	WizzardDeposits AS wd 
GROUP BY 
	wd.DepositGroup 

--********************
--4.Problem
--******************** 
--First OPTION
SELECT 
	DepositGroup 
FROM
		(SELECT TOP(2) wd.DepositGroup AS DepositGroup,
			AVG(wd.MagicWandSize) AS AvgSize 
		FROM 
			WizzardDeposits AS wd 
		GROUP BY 
			wd.DepositGroup 
		ORDER BY 
			AvgSize ASC) 
	WizzardDeposits

--Second OPTION
SELECT TOP (2)
	DepositGroup 
FROM 
	WizzardDeposits
GROUP BY 
	DepositGroup
ORDER BY AVG(MagicWandSize)

--********************
--5.Problem
--******************** 
SELECT
	DepositGroup AS dg, 
	SUM(wd.DepositAmount) AS TotalSum 
FROM
	WizzardDeposits AS wd 
GROUP BY 
	DepositGroup

--********************
--6.Problem
--******************** 

--First OPTION
SELECT 
	wd.DepositGroup,
	SUM(wd.DepositAmount) AS [TotalSum] 
FROM 
	WizzardDeposits AS wd 
GROUP BY 
	wd.MagicWandCreator ,wd.DepositGroup
HAVING 
	wd.MagicWandCreator = 'Ollivander family'

--Second OPTION
SELECT
	wd.DepositGroup,
	SUM(wd.DepositAmount) AS TotalSum
FROM 
	WizzardDeposits AS wd 
WHERE 
	wd.MagicWandCreator = 'Ollivander family' 
GROUP BY 
	DepositGroup

--********************
--7.Problem
--******************** 

--First OPTION
SELECT 
	wd.DepositGroup, 
	SUM(wd.DepositAmount) 
AS 
	TotalSum 
FROM
	WizzardDeposits AS wd
GROUP BY
	wd.MagicWandCreator, wd.DepositGroup
HAVING 
	wd.MagicWandCreator = 'Ollivander family' 
	AND
	SUM(wd.DepositAmount)  <150000
ORDER BY 
	TotalSum
DESC

--Second OPTION
SELECT
	DepositGroup,
	SUM(DepositAmount) AS TotalSum
FROM 
	WizzardDeposits
WHERE 
	MagicWandCreator = 'Ollivander family'
GROUP BY 
	DepositGroup
HAVING 
	SUM(DepositAmount) < 150000
ORDER BY 
	TotalSum 
DESC
 
 --********************
--8.Problem
--******************** 
SELECT 
	wd.DepositGroup
	,wd.MagicWandCreator
	, MIN(wd.DepositCharge) AS MinDepositCharge 
FROM
	WizzardDeposits AS wd 
GROUP BY 
	wd.DepositGroup
	,wd.MagicWandCreator
ORDER BY
	wd.MagicWandCreator
	,wd.DepositGroup

 --********************
--9.Problem
--******************** 

--First OPTION
SELECT
  IIF(Age >= 61, '[61+]', 
    IIF(Age = 0, '[0-10]', 
	  CONCAT('[',(Age-1)/10 * 10 + 1,'-', (Age-1)/10 * 10 + 10, ']'))) AS AgeGroup,
  COUNT(Age) AS WizardCount
FROM WizzardDeposits
GROUP BY 
  IIF(Age >= 61, '[61+]', 
    IIF(Age = 0, '[0-10]', 
	  CONCAT('[',(Age-1)/10 * 10 + 1,'-', (Age-1)/10 * 10 + 10, ']')))
HAVING COUNT(Age) > 0
ORDER BY AgeGroup

--Second OPTION
SELECT 
	CASE
	  WHEN w.Age BETWEEN 0 AND 10 THEN '[0-10]'
	  WHEN w.Age BETWEEN 11 AND 20 THEN '[11-20]'
	  WHEN w.Age BETWEEN 21 AND 30 THEN '[21-30]'
	  WHEN w.Age BETWEEN 31 AND 40 THEN '[31-40]'
	  WHEN w.Age BETWEEN 41 AND 50 THEN '[41-50]'
	  WHEN w.Age BETWEEN 51 AND 60 THEN '[51-60]'
	  WHEN w.Age >= 61 THEN '[61+]'
	END AS [AgeGroup],
COUNT(*) AS [WizardCount]
	FROM WizzardDeposits AS w
GROUP BY CASE
	  WHEN w.Age BETWEEN 0 AND 10 THEN '[0-10]'
	  WHEN w.Age BETWEEN 11 AND 20 THEN '[11-20]'
	  WHEN w.Age BETWEEN 21 AND 30 THEN '[21-30]'
	  WHEN w.Age BETWEEN 31 AND 40 THEN '[31-40]'
	  WHEN w.Age BETWEEN 41 AND 50 THEN '[41-50]'
	  WHEN w.Age BETWEEN 51 AND 60 THEN '[51-60]'
	  WHEN w.Age >= 61 THEN '[61+]'
	END

 --********************
--10.Problem
--******************** 
SELECT 
	LEFT(wd.FirstName,1) AS FirstLetter
FROM 
	WizzardDeposits AS wd
WHERE 
	wd.DepositGroup = 'Troll Chest' 
GROUP BY 
	LEFT(wd.FirstName,1) 
ORDER BY 
	FirstLetter

 --********************
--11.Problem
--******************** 
SELECT
	wd.DepositGroup
	,wd.IsDepositExpired
	,AVG(wd.DepositInterest) AS AverageInterest
FROM
	WizzardDeposits AS wd 
WHERE
	wd.DepositStartDate>'01/01/1985'
GROUP BY
	wd.IsDepositExpired
	,wd.DepositGroup
ORDER BY
	wd.DepositGroup DESC
	,wd.IsDepositExpired ASC

 --********************
--12.Problem
--********************

--First OPTION
	SELECT TOP 1
  (SELECT DepositAmount FROM WizzardDeposits WHERE Id = (SELECT MIN(Id) FROM WizzardDeposits)) - 
  (SELECT DepositAmount FROM WizzardDeposits WHERE Id = (SELECT MAX(Id) FROM WizzardDeposits)) 
  AS SumDifference
FROM WizzardDeposits

-- Second OPTION
SELECT SUM(DepoDifference) AS SumDifference
FROM 
  (SELECT
     LAG(FirstName) OVER (ORDER BY Id) AS [Host Wizard],
     LAG(DepositAmount) OVER (ORDER BY Id) AS [Host Wizard Deposit],
     FirstName AS [Guest Wizard], 
     DepositAmount AS [Guest Wizard Deposit],
	 LAG(DepositAmount) OVER (ORDER BY Id) - DepositAmount AS [DepoDifference]
   FROM WizzardDeposits) AS Differences

-- Third OPTION
DECLARE @currentDeposit DECIMAL(8,2)
DECLARE @previousDeposit DECIMAL(8,2)
DECLARE @sumDifferences DECIMAL(8,2) = 0
DECLARE wizardCursor CURSOR FOR SELECT DepositAmount FROM WizzardDeposits

OPEN wizardCursor
FETCH NEXT FROM wizardCursor INTO @currentDeposit
WHILE (@@FETCH_STATUS = 0)
BEGIN
  IF (@previousDeposit IS NOT NULL)
  BEGIN
    SET @sumDifferences += @previousDeposit - @currentDeposit
  END
  SET @previousDeposit = @currentDeposit
  FETCH NEXT FROM wizardCursor INTO @currentDeposit
END
CLOSE wizardCursor
DEALLOCATE wizardCursor

SELECT @sumDifferences AS SumDifference

-- Fourth OPTION
SELECT SUM(DepoDifference) AS SumDifference
FROM 
  (SELECT
     FirstName AS [Host Wizard], 
     DepositAmount AS [Host Wizard Deposit],
     LEAD(FirstName) OVER (ORDER BY Id) AS [Guest Wizzard],
     LEAD(DepositAmount) OVER (ORDER BY Id) AS [Guest Wizard Deposit],
     DepositAmount - LEAD(DepositAmount) OVER (ORDER BY Id) AS [DepoDifference]
   FROM WizzardDeposits) AS Differences

 --********************
--13.Problem
--********************
SELECT
	e.DepartmentID
	,SUM(e.Salary)
FROM
	Employees as e
GROUP BY
	e.DepartmentID

 --********************
--14.Problem
--********************
SELECT
	e.DepartmentID
	,MIN(e.Salary) AS MinimumSalary
FROM
	Employees AS e
WHERE
	e.HireDate > '2000/01/01' 
	AND 
	DepartmentID IN(2, 5, 7)
GROUP BY 
	e.DepartmentID

 --********************
--15.Problem
--********************
SELECT * INTO 
	NewTable
FROM 
	Employees 
WHERE 
Salary > 30000 

DELETE FROM 
	NewTable 
WHERE 
	ManagerID = 42

UPDATE NewTable
SET 
	Salary+=5000
WHERE 
	DepartmentID = 1

SELECT
	nt.DepartmentID
	,AVG(nt.Salary) 
FROM 
	NewTable AS nt
GROUP BY 
	DepartmentID

--********************
--16.Problem
--********************
SELECT
	e.DepartmentID
	,MAX(e.Salary) AS MaxSalary
FROM
	Employees AS e
GROUP BY
	e.DepartmentID
HAVING
	MAX(e.Salary)
	NOT BETWEEN 30000 AND 70000

--********************
--17.Problem
--********************
SELECT
	COUNT(*) AS [Count] 
FROM 
	Employees 
WHERE
	ManagerID IS NULL

--********************
--18.Problem
--********************

--First OPTION
SELECT 
  DepartmentID,
  (SELECT DISTINCT Salary FROM Employees
   WHERE DepartmentID = e.DepartmentID
   ORDER BY Salary DESC 
   OFFSET 2 ROWS FETCH NEXT 1 ROWS ONLY) AS ThirdHighestSalary
FROM Employees e
WHERE 
  (SELECT DISTINCT Salary FROM Employees
   WHERE DepartmentID = e.DepartmentID
   ORDER BY Salary DESC 
   OFFSET 2 ROWS FETCH NEXT 1 ROWS ONLY) IS NOT NULL
GROUP BY DepartmentID

--Second OPTION
SELECT so.DepartmentID, so.Salary as [ThirdHighestSalary]
	FROM (Select e.DepartmentID, e.Salary, RANK() OVER( PARTITION BY e.DepartmentID ORDER BY e.Salary DESC) as Rank
	FROM Employees as e 
	Group by e.DepartmentID, e.Salary) as so
WHERE so.Rank = 3
ORDER BY so.DepartmentID

--********************
--19.Problem
--********************
SELECT TOP 10
  FirstName, LastName, DepartmentID
FROM Employees AS emp1
WHERE Salary >
  (SELECT AVG(Salary)
   FROM Employees AS emp2
   WHERE emp1.DepartmentID = emp2.DepartmentID
   GROUP BY DepartmentID)
ORDER BY DepartmentID
