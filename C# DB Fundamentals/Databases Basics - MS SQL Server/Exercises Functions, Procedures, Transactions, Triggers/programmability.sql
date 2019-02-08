/* ******************************************
Problem 1. Employees with Salary Above 35000
*********************************************/
CREATE PROCEDURE usp_GetEmployeesSalaryAbove35000 
AS
   SELECT
		e.FirstName
		,e.LastName 
	FROM 
		Employees AS e 
	WHERE 
		e.Salary >35000

EXEC usp_GetEmployeesSalaryAbove35000

/* ******************************************
Problem 2. Employees with Salary Above Number
*********************************************/
CREATE PROC usp_GetEmployeesSalaryAboveNumber @borderSalary DECIMAL(18,2)
AS
	SELECT
		 e.FirstName
		 ,e.LastName 
	FROM Employees AS e
	WHERE e.Salary >= @borderSalary

EXECUTE 
	usp_GetEmployeesSalaryAboveNumber 48100
	
/* ******************************************
Problem 3. Town Names Starting With
*********************************************/
CREATE PROC usp_GetTownsStartingWith @firstAlphabet VARCHAR(10)
AS
	SELECT 
		[Name] 
	FROM 
		Towns 
	WHERE 
		LEFT([Name],LEN(@firstAlphabet)) = @firstAlphabet

EXEC usp_GetTownsStartingWith 'bO'

/* ******************************************
Problem 4. Employees from Town
*********************************************/
CREATE PROC usp_GetEmployeesFromTown @townName VARCHAR(50)
AS
	SELECT 
		e.FirstName
		,e.LastName 
	FROM 
		Employees AS e 
			INNER JOIN Addresses AS a 
				ON e.AddressID = a.AddressID 
			INNER JOIN Towns AS t 
				ON a.TownID=t.TownID
	WHERE
	 t.[Name] = @townName

EXEC usp_GetEmployeesFromTown 'Sofia'

/* ******************************************
Problem 5. Salary Level Function
*********************************************/
CREATE OR ALTER FUNCTION ufn_GetSalaryLevel(@salary DECIMAL(18,2))
RETURNS VARCHAR(10)
AS
BEGIN
	DECLARE @SalaryLevel VARCHAR(10);
	IF(@salary <30000)
		BEGIN
			SET @SalaryLevel = 'Low';
		END
	ELSE IF(@salary <= 50000)
		BEGIN
			SET @SalaryLevel = 'Average';
		END 
	ELSE
		BEGIN
			SET @SalaryLevel = 'High'
		END
	RETURN @SalaryLevel
END

SELECT 
	e.Salary
	, dbo.ufn_GetSalaryLevel(e.Salary) AS SalaryLevel 
FROM 
	Employees AS e
	
/* ******************************************
Problem 6. Employees by Salary Level
*********************************************/
CREATE PROC usp_EmployeesBySalaryLevel (@levelOfSalary VARCHAR(10))
AS
	SELECT 
		e.FirstName
		,e.LastName 
	FROM 
		Employees AS e
	WHERE 
		dbo.ufn_GetSalaryLevel(e.Salary) = @levelOfSalary

EXEC usp_EmployeesBySalaryLevel 'high'

/* ******************************************
Problem 7. Define Function
*********************************************/
CREATE FUNCTION ufn_IsWordComprised ( @setOfLetters nvarchar(255),  @word nvarchar(255) )
RETURNS BIT
AS
BEGIN 
    DECLARE @l int = 1;
    DECLARE @exist bit = 1;
    WHILE LEN(@word) >= @l AND @exist > 0
    BEGIN
      DECLARE @charindex int; 
      DECLARE @letter char(1);
      SET @letter = SUBSTRING(@word, @l, 1)
      SET @charindex = CHARINDEX(@letter, @setOfLetters, 0)
      SET @exist =
        CASE
            WHEN  @charindex > 0 THEN 1
            ELSE 0
        END;
      SET @l += 1;
    END

    RETURN @exist
END

SELECT DBO.ufn_IsWordComprised('oistmiahf'	,'halves')

/* ******************************************
Problem 8. * Delete Employees and Departments
*********************************************/

ALTER TABLE Departments
ALTER COLUMN ManagerId INT Null

SELECT EmployeeID
FROM Employees AS e
INNER JOIN Departments AS d
ON E.DepartmentID = D.DepartmentID
WHERE D.Name IN ('Production', 'Production Control')

DELETE FROM EmployeesProjects
WHERE EmployeeID IN 
		(
			SELECT EmployeeID FROM Employees AS e
			INNER JOIN Departments AS d
			ON E.DepartmentID = D.DepartmentID
			WHERE D.Name IN ('Production', 'Production Control')
		)

UPDATE Employees
SET ManagerID = Null
WHERE ManagerID IN
		(
			SELECT EmployeeID FROM Employees AS e
			INNER JOIN Departments AS d
			ON E.DepartmentID = D.DepartmentID
			WHERE D.Name IN ('Production', 'Production Control')
		)

UPDATE Departments
SET ManagerID = Null
WHERE ManagerID IN
		(
			SELECT EmployeeID FROM Employees AS e
			INNER JOIN Departments AS d
			ON E.DepartmentID = D.DepartmentID
			WHERE D.Name IN ('Production', 'Production Control')
		)

DELETE FROM Employees
WHERE EmployeeID IN
		(
			SELECT EmployeeID FROM Employees AS e
			INNER JOIN Departments AS d
			ON E.DepartmentID = D.DepartmentID
			WHERE D.Name IN ('Production', 'Production Control')
		)

DELETE FROM Departments
WHERE Name IN ('Production', 'Production Control')

/* ******************************************
Problem 9. Find Full Name
*********************************************/
CREATE PROC usp_GetHoldersFullName  
AS 
	SELECT CONCAT(FirstName, ' ', LastName) AS FullName FROM AccountHolders 

EXEC usp_GetHoldersFullName

/* ******************************************
Problem 10. People with Balance Higher Than
*********************************************/
CREATE PROC usp_GetHoldersWithBalanceHigherThan @money MONEY
AS
WITH CTE_TakingTheFullBalance(Id,AllBalance,FirstName,LastName)
AS
(
SELECT 
	ah.Id
	, SUM(a.Balance) AS AllBalance ,
	ah.FirstName,
	ah.LastName
FROM 
	AccountHolders AS ah 
JOIN
	Accounts AS a 
ON 
	ah.Id = a.AccountHolderId 
GROUP BY ah.Id,ah.FirstName,ah.LastName
HAVING SUM(a.Balance) > @money
)
SELECT 
	cte.FirstName
	,cte.LastName 
FROM 
	CTE_TakingTheFullBalance AS cte
ORDER BY
	cte.FirstName ,cte.LastName
EXEC usp_GetHoldersWithBalanceHigherThan 2000

/* ******************************************
Problem 11. Future Value Function
*********************************************/
CREATE FUNCTION ufn_CalculateFutureValue (@sum DECIMAL(10,4), @yearlyInterestRate FLOAT, @numberOfYears INT)
RETURNS DECIMAL(10,4)
AS
BEGIN
 DECLARE @output DECIMAL(10,4)
 SET @output =CAST(@sum * (POWER(1 +@yearlyInterestRate,@numberOfYears)) AS decimal(10,4))
 RETURN @output
END

SELECT 
	DBO.ufn_CalculateFutureValue (1000,0.1,5)

/* ******************************************
Problem 12. Calculating Interest
*********************************************/
CREATE or alter PROC usp_CalculateFutureValueForAccount (@AccountId INT, @InterestRate FLOAT) AS
SELECT a.Id AS [Account Id],
	   ah.FirstName AS [First Name],
	   ah.LastName AS [Last Name],
	   a.Balance,
	   dbo.ufn_CalculateFutureValue(Balance, @InterestRate, 5) AS [Balance in 5 years]
  FROM AccountHolders AS ah
  JOIN Accounts AS a ON ah.Id = a.Id
 WHERE a.Id = @AccountId

exec usp_CalculateFutureValueForAccount 1 ,123.12

/* ******************************************
Problem 13. *Scalar Function: Cash in User Games Odd Rows
*********************************************/

------------------------
--FirstOption
------------------------
CREATE FUNCTION ufn_CashInUsersGames (@gameName VARCHAR(50))
RETURNS TABLE 
AS
RETURN
	WITH CTE_NumeringTheRows(RowNumber,Cash)
	AS
	(
	SELECT ROW_NUMBER() OVER (PARTITION BY ug.GameId ORDER BY ug.Cash DESC) AS RowNumber, ug.Cash FROM UsersGames AS ug JOIN Games AS g ON ug.GameId = g.Id WHERE g.[Name] = @gameName
	)
	SELECT SUM(Cash) AS SumCash FROM CTE_NumeringTheRows GROUP BY RowNumber HAVING RowNumber % 2 = 1
----------------
--SecondOption
------------------------
CREATE FUNCTION ufn_CashInUsersGames(@gameName varchar(max))
RETURNS @returnedTable TABLE
(
SumCash money
)
AS
BEGIN
	DECLARE @result money

	SET @result = 
	(SELECT SUM(ug.Cash) AS Cash
	FROM
		(SELECT Cash, GameId, ROW_NUMBER() OVER (ORDER BY Cash DESC) AS RowNumber
		FROM UsersGames
		WHERE GameId = (SELECT Id FROM Games WHERE Name = @gameName)
		) AS ug
	WHERE ug.RowNumber % 2 != 0
	)

	INSERT INTO @returnedTable SELECT @result
	RETURN
END

SELECT * 
  FROM 
	dbo.ufn_CashInUsersGames('Love in a mist') 

/* ******************************************
Problem 14. Create Table Logs
*********************************************/
CREATE TABLE Logs
(
             LogId     INT NOT NULL IDENTITY,
             AccountId INT NOT NULL,
             OldSum    MONEY NOT NULL,
             NewSum    MONEY NOT NULL,
             CONSTRAINT PK_Logs PRIMARY KEY(LogId),
             CONSTRAINT FK_Logs_Accounts FOREIGN KEY(AccountId) REFERENCES Accounts(Id)
);
G
-- For Judge - only the trigger creation

CREATE TRIGGER tr_ChangeBalance ON Accounts AFTER UPDATE
AS 
BEGIN 
	INSERT INTO Logs 
	SELECT inserted.Id, deleted.Balance, inserted.Balance
	FROM inserted
	JOIN deleted
	ON inserted.Id = deleted.Id
END	

UPDATE Accounts 
SET Balance += 555
WHERE Id = 2


/* ******************************************
Problem 15. Create Table Emails
*********************************************/

--(Id, Recipient, Subject, Body). 
CREATE TABLE NotificationEmails(
Id INT IDENTITY NOT NULL,
Recipient INT NOT NULL,
[Subject] VARCHAR(60) NOT NULL,
Body VARCHAR(80) NOT NULL
)

ALTER TABLE NotificationEmails
ADD CONSTRAINT PK_NotificationEmails_Id PRIMARY KEY(Id)

ALTER TABLE NotificationEmails
ADD CONSTRAINT FK_Notification_Recipient FOREIGN KEY(Recipient) REFERENCES Accounts(Id)

CREATE TRIGGER tr_EmailSender ON Logs AFTER INSERT
AS
BEGIN
	DECLARE @SubjectConst VARCHAR(60) = 'Balance change for account: '
	INSERT INTO NotificationEmails 
	SELECT 
		i.AccountId
		,CONCAT(@SubjectConst,i.AccountId)
		,CONCAT('ON ', CONVERT( varchar,getdate(), 0),' your balance was changed from ',i.OldSum, ' to ',i.NewSum) 
	FROM 
		inserted AS i
END
/* ******************************************
Problem 16. Deposit Money
*********************************************/
CREATE PROC usp_DepositMoney @AccountId INT
,@MoneyAmount DECIMAL(15,4)
AS
BEGIN
	BEGIN TRANSACTION
	IF((SELECT Id FROM Accounts WHERE Id=@AccountId) IS NULL)
		BEGIN
			ROLLBACK
			RAISERROR('The account is not found!',16,1)
		RETURN
	END
	IF(@MoneyAmount < 0)
		BEGIN
			ROLLBACK
			RAISERROR('You didn''t succeeded to send the money!',16,2)
			RETURN
		END
	UPDATE Accounts
	SET Balance+= @MoneyAmount
	WHERE Id = @AccountId
	COMMIT
END

/* ******************************************
Problem 17. Withdraw Money
*********************************************/

CREATE PROC usp_WithdrawMoney @AccountId INT
,@MoneyAmount DECIMAL(15,4)
AS
BEGIN
	BEGIN TRANSACTION
	IF((SELECT Id FROM Accounts WHERE Id=@AccountId) IS NULL)
		BEGIN
			RAISERROR('The account is not found!',16,1)
		RETURN
	END
	IF(@MoneyAmount < 0)
		BEGIN
			RAISERROR('Your wanted money amount should be bigger than zero!',16,2)
			RETURN
		END
	UPDATE Accounts
	SET Balance -= @MoneyAmount
	WHERE Id = @AccountId

	IF((SELECT Balance FROM Accounts WHERE Id = @AccountId) < 0)
	BEGIN
	ROLLBACK;
	RAISERROR('You have not enough money for the transaction',16,3)
	RETURN
	END

	COMMIT
END

/* ******************************************
Problem 18. Money Transfer
*********************************************/
CREATE PROC usp_TransferMoney @SenderId INT,
@ReceiverId INT, @Amount DECIMAL(15,4)
AS
BEGIN
	BEGIN TRAN	
		BEGIN TRY
			EXEC usp_WithdrawMoney @SenderId,@Amount
			EXEC usp_DepositMoney @ReceiverId, @Amount
			COMMIT TRAN usp_TransferMoney
		END TRY
		BEGIN CATCH
			SELECT ERROR_MESSAGE();
			ROLLBACK TRANSACTION usp_TransferMoney
			RETURN
		END CATCH
	END

/* ******************************************
Problem 19. Trigger
*********************************************/
CREATE TRIGGER tr_BuyItem ON UserGameItems AFTER INSERT
AS
BEGIN
	DECLARE @userLevel INT; 
	DECLARE @itemLevel INT;  

	SET @userLevel = (SELECT ug.[Level] 
	FROM inserted
	JOIN UsersGames AS ug
	ON ug.Id = inserted.UserGameId)

	SET @itemLevel = (SELECT i.MinLevel
	FROM inserted
	JOIN Items AS i
	ON i.Id = inserted.ItemId)

	IF(@userLevel < @itemLevel)
	BEGIN 
		RAISERROR('User can not buy this item!', 16, 1);
		ROLLBACK;
		RETURN;  
	END
END 

INSERT INTO UserGameItems
VALUES 
(4, 1)

UPDATE UsersGames
SET Cash += 50000
FROM UsersGames AS ug
JOIN Users AS u
ON u.Id = ug.UserId
JOIN Games AS g
ON g.Id = ug.GameId
WHERE g.Name = 'Bali' 
AND u.Username IN ('baleremuda', 'loosenoise', 'inguinalself', 'buildingdeltoid', 'monoxidecos')
GO

CREATE PROCEDURE udp_BuyItems(@username NVARCHAR(MAX))
AS 
BEGIN 
	DECLARE @counter INT = 251;
	DECLARE @userId INT = (SELECT u.Id FROM Users AS u WHERE u.Username = @username);
	
	WHILE(@counter <= 539)
	BEGIN 
		DECLARE @itemValue DECIMAL(16,2) = (SELECT i.Price FROM Items AS i WHERE i.Id = @counter);
		DECLARE @UserCash DECIMAL(16,2) = (SELECT u.Cash FROM UsersGames AS u WHERE u.UserId = @userId);

		IF(@itemValue <= @UserCash)
		BEGIN 
			INSERT INTO UserGameItems 
			VALUES 
			(@counter, @userId)

			UPDATE UsersGames
			SET Cash -= @itemValue
			WHERE UserId = @userId
		END

		SET @counter += 1; 
		IF(@counter = 300)
		BEGIN
			SET @counter = 501; 
		END
	END
END

EXECUTE udp_BuyItems 'baleremuda'
EXECUTE udp_BuyItems 'loosenoise'
EXECUTE udp_BuyItems 'inguinalself'
EXECUTE udp_BuyItems 'buildingdeltoid'
EXECUTE udp_BuyItems 'monoxidecos'

/* ******************************************
20. *Massive Shopping 
*********************************************/

DECLARE @gameId INT = (SELECT Id FROM Games AS g WHERE g.[Name] = 'Safflower'); 
DECLARE @userId INT = (SELECT u.Id FROM Users AS u WHERE u.Username = 'Stamat');	
DECLARE @userGameId INT = (SELECT ug.Id FROM UsersGames AS ug WHERE ug.GameId = @gameId AND ug.UserId = @userId);
DECLARE @userCash DECIMAL(15, 2) = (SELECT ug.Cash FROM UsersGames AS ug WHERE ug.Id = @userGameId);  
DECLARE @itemsPricesSummed DECIMAL(15, 2) = (SELECT SUM(i.Price) FROM Items AS i WHERE i.MinLevel BETWEEN 11 AND 12); 

IF(@userCash >= @itemsPricesSummed)
BEGIN
	BEGIN TRANSACTION
		INSERT UserGameItems
		SELECT i.Id, @UserGameId
		FROM Items AS i
		WHERE i.MinLevel BETWEEN 11 AND 12

		UPDATE UsersGames 
		SET Cash -= @itemsPricesSummed
		WHERE Id = @UserGameId 
	COMMIT
END

SET @itemsPricesSummed = (SELECT SUM(i.Price) FROM Items AS i WHERE i.MinLevel BETWEEN 19 AND 21); 
SET @UserCash = (SELECT ug.Cash FROM UsersGames AS ug WHERE ug.Id = @UserGameId);  

IF(@UserCash >= @itemsPricesSummed)
BEGIN 	
	BEGIN TRANSACTION
		INSERT UserGameItems
		SELECT i.Id, @UserGameId
		FROM Items AS i
		WHERE i.MinLevel BETWEEN 19 AND 21

		UPDATE UsersGames 
		SET Cash -= @itemsPricesSummed
		WHERE Id = @UserGameId 
	COMMIT TRANSACTION 
END

SELECT i.[Name] 
FROM UsersGames AS ug
JOIN Users AS u
ON u.Id = ug.UserId
JOIN Games AS g
ON g.Id = ug.GameId
JOIN UserGameItems AS ugi
ON ugi.UserGameId = ug.Id
JOIN Items AS i
ON i.Id = ugi.ItemId
WHERE (u.Username = 'Stamat' 
AND g.[Name] = 'Safflower')
ORDER BY i.[Name]

GO
/* ******************************************
Problem 21. Employees with Three Projects
*********************************************/
CREATE PROC usp_AssignProject(@employeeId INT, @projectID INT)
AS
BEGIN
	DECLARE @MaxProjects INT = 3;
	DECLARE @EmployeeProjects INT = 
									(SELECT 
										COUNT(ep.ProjectID) 
									FROM 
										EmployeesProjects AS ep 
									WHERE 
										ep.EmployeeID = @employeeId)

	IF(@EmployeeProjects > @MaxProjects)
	BEGIN
		RAISERROR('The employee has too many projects!',16,1)
		RETURN
	END

	BEGIN TRANSACTION
		INSERT INTO EmployeesProjects (EmployeeID,ProjectID) 
		VALUES
		(@employeeId,@projectID)

		DECLARE @ProjectsOfEmployee INT = 
											(SELECT 
												COUNT(ep.ProjectID) 
											FROM 
												EmployeesProjects AS ep 
											WHERE 
												ep.EmployeeID = @employeeId)

		IF(@ProjectsOfEmployee > @MaxProjects)
			BEGIN
				ROLLBACK;
				RAISERROR('The employee has too many projects!',16,2);
			END
		ELSE
			COMMIT;
END

WITH CTE_TakingEmpProjects(EmployeeID,ProjectCount)
AS
(
SELECT EmployeeID AS epId, COUNT(ProjectID) AS pID FROM EmployeesProjects GROUP BY EmployeeID 
)
SELECT EmployeeID,ProjectCount FROM CTE_TakingEmpProjects WHERE ProjectCount <=2 ORDER BY EmployeeID ASC  

EXEC  usp_AssignProject @employeeId = 219
					,@projectID = 24

SELECT * FROM EmployeesProjects WHERE EmployeeID = 219

/* ******************************************
Problem 21. Delete Employees
*********************************************/
CREATE TABLE Deleted_Employees
(
	EmployeeId INT NOT NULL IDENTITY, 
	FirstName NVARCHAR(64) NOT NULL, 
	LastName NVARCHAR(64) NOT NULL, 
	MiddleName NVARCHAR(64), 
	JobTitle NVARCHAR(64) NOT NULL, 
	DepartmentID INT NOT NULL, 
	Salary DECIMAL(15, 2) NOT NULL

	CONSTRAINT PK_Deleted_Emp
	PRIMARY KEY (EmployeeId), 

	CONSTRAINT FK_DeletedEmp_Departments
	FOREIGN KEY (DepartmentID)
	REFERENCES Departments(DepartmentID)
)

CREATE TRIGGER tr_DeletedEmp 
ON Employees 
AFTER DELETE 
AS
	INSERT INTO Deleted_Employees
	SELECT 	
		d.FirstName, 
		d.LastName, 
		d.MiddleName, 
		d.JobTitle, 
		d.DepartmentID, 
		d.Salary
	FROM deleted as d