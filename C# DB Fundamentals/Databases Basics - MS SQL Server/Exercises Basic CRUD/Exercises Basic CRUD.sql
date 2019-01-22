--********************
--2.Problem
--********************
SELECT * FROM Departments

--********************
--3.Problem
--********************
SELECT 
	[Name] 
FROM 
	Departments

--********************
--4.Problem
--********************
SELECT 
	FirstName, LastName,Salary 
FROM 
	Employees

--********************
--5.Problem
--********************
SELECT 
	FirstName, MiddleName, LastName 
FROM 
	Employees

--********************
--6.Problem
--********************
SELECT 
	FirstName + '.' + LastName + '@softuni.bg' 
AS 
	[Full Email Adress] 
FROM 
	Employees

--********************
--7.Problem
--********************
SELECT DISTINCT 
	Salary 
FROM 
	Employees 

--********************
--8.Problem
--********************
SELECT * FROM 
	Employees 
WHERE 
	JobTitle = 'Sales Representative' 

--********************
--9.Problem
--********************
SELECT 
	FirstName, LastName, JobTitle 
FROM 
	Employees 
WHERE 
	Salary BETWEEN 20000 AND 30000

--********************
--10.Problem
--********************
SELECT 
	FirstName + ' ' + MiddleName + ' ' + LastName AS [Full Name] 
FROM 
	Employees 
WHERE 
	Salary IN (25000, 14000, 12500 , 23600)

--********************
--11.Problem
--********************
SELECT 
	FirstName, LastName 
FROM 
	Employees 
WHERE 
	ManagerID IS NULL

--********************
--12.Problem
--********************
SELECT
	 FirstName,LastName,Salary
 FROM 
	Employees 
 WHERE 
	Salary > 50000 
ORDER BY Salary DESC

--********************
--13.Problem
--********************
SELECT TOP(5) FirstName, LastName FROM Employees ORDER BY Salary DESC

--********************
--14.Problem
--********************
SELECT
	FirstName, LastName 
FROM 
	Employees 
WHERE 
	DepartmentId <>4

--********************
--15.Problem
--********************
SELECT * FROM 
	Employees 
ORDER BY 
	Salary DESC, FirstName ASC, LastName DESC, MiddleName ASC

--********************
--16.Problem
--********************
CREATE VIEW 
	V_EmployeesSalaries 
AS 
 SELECT
	FirstName, LastName,Salary
 FROM 
	Employees

--********************
--17.Problem
--********************
CREATE VIEW 
	V_EmployeeNameJobTitle 
AS 
 SELECT 
FirstName + ' ' + 
	CASE
		 WHEN MiddleName IS NULL 
			THEN 
				' ' 
			ELSE 
				MiddleName + ' ' 
	END + 
    LastName AS [Full Name] , JobTitle 
FROM 
	Employees

--********************
--18.Problem
--********************
SELECT DISTINCT 
	JobTitle 
FROM 
Employees

--********************
--19.Problem
--********************
SELECT TOP(10) * FROM Projects WHERE StartDate IS NOT NULL ORDER BY StartDate,[Name]

--********************
--20.Problem
--********************
SELECT TOP(7) FirstName,LastName,HireDate FROM Employees ORDER BY HireDate DESC

--********************
--21.Problem
--********************
UPDATE Employees 
	SET Salary = Salary+(Salary * 0.12)
WHERE DepartmentID IN(1, 2, 4,11)

SELECT Salary FROM Employees

UPDATE Employees 
	SET Salary = Salary-(Salary * 0.12)
WHERE DepartmentID IN(1, 2, 4,11)

--********************
--22.Problem
--********************
SELECT 
	PeakName 
FROM 
	Peaks 
ORDER BY PeakName ASC

--********************
--23.Problem
--********************
SELECT TOP(30)
	CountryName, [Population] 
FROM 
	Countries 
WHERE 
	ContinentCode = 'EU' 
ORDER BY [Population] DESC

--********************
--24.Problem
--********************
SELECT 
	CountryName,CountryCode,
	CASE
		 WHEN CurrencyCode = 'EUR' 
			THEN 'Euro' 
			ELSE 'Not Euro' 
		 END
FROM
	Countries 
ORDER BY CountryName ASC

--********************
--25.Problem
--********************
SELECT [Name] FROM Characters ORDER BY [Name] ASC