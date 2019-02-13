/****************************
Problem	1.DDL (30 pts)
*****************************/
CREATE TABLE Categories(
Id INT PRIMARY KEY IDENTITY,
[Name] NVARCHAR(30) NOT NULL			
)

CREATE TABLE Items(
Id INT PRIMARY KEY IDENTITY,
[Name] NVARCHAR(30) NOT NULL,	
Price DECIMAL(15,2) NOT NULL,
CategoryId INT NOT NULL FOREIGN KEY REFERENCES Categories(Id)		
)

CREATE TABLE Employees(
Id INT PRIMARY KEY IDENTITY,
FirstName NVARCHAR(50) NOT NULL,	
LastName NVARCHAR(50) NOT NULL,	
Phone CHAR(12) NOT NULL,
Salary DECIMAL(15,2) NOT NULL
)

CREATE TABLE Orders(
Id INT PRIMARY KEY IDENTITY,
[DateTime] DATETIME NOT NULL,
EmployeeId INT NOT NULL FOREIGN KEY REFERENCES Employees(Id)
)

CREATE TABLE OrderItems(
OrderId INT NOT NULL FOREIGN KEY REFERENCES Orders(Id),
ItemId INT NOT NULL FOREIGN KEY REFERENCES Items(Id),
Quantity INT NOT NULL 

)
ALTER TABLE OrderItems
ADD CONSTRAINT CH_Quantity CHECK(Quantity >=1)

ALTER TABLE OrderItems
ADD CONSTRAINT PK_OrderId_ItemId PRIMARY KEY (OrderId,ItemId)


CREATE TABLE Shifts(
Id INT IDENTITY ,
EmployeeId INT FOREIGN KEY REFERENCES Employees(Id) NOT NULL,
CheckIn DATETIME NOT NULL,
CheckOut DATETIME NOT NULL 

CONSTRAINT PK_Id_EmployeeId PRIMARY KEY(Id,EmployeeId)
)

ALTER TABLE Shifts
ADD CONSTRAINT CH_Sifts_CheckOut 
	CHECK(CheckIn < CheckOut)


/****************************
Problem	2. Insert
*****************************/
INSERT INTO Employees (FirstName,LastName,Phone,Salary)
VALUES
('Stoyan','Petrov','888-785-8573',500.25),
('Stamat',	'Nikolov','789-613-1122',999995.25),
('Evgeni' ,	'Petkov','645-369-9517',1234.51),
('Krasimir','Vidolov','321-471-9982',50.25)

INSERT INTO Items (Name,Price,CategoryId)
VALUES
('Tesla battery',154.25,8),
('Chess',30.25,	8),
('Juice',5.32,1),
('Glasses',10,8),
('Bottle of water',	1,	1)

/****************************
Problem	3. Update
*****************************/
UPDATE Items
SET Price += 0.27 * Price
WHERE CategoryId IN(1,2,3)

/****************************
Problem	4. Delete
*****************************/
DELETE FROM OrderItems 
WHERE 
	OrderId = 48

/****************************
Problem	5. Richest People
*****************************/
SELECT 
	Id,
	FirstName
FROM
	Employees
WHERE
	Salary > 6500
ORDER BY 
	FirstName 
	,Id

/****************************
Problem	6. Cool Phone Numbers
*****************************/
SELECT
	CONCAT(FirstName ,' ', LastName) AS FullName 
	,Phone
FROM 
	Employees
WHERE
	LEFT(Phone,1) = '3'
ORDER BY
	FirstName,
	Phone

/****************************
Problem	7. Employee Statistics
*****************************/
WITH CTE_TakingInfoAboutOrderCount(EmpId,OrderCount)
AS
(
SELECT 
	o.EmployeeId
	,COUNT(o.Id) AS [Count] 
FROM 
	Orders AS o 
GROUP BY 
	o.EmployeeId 
)
SELECT 
	e.FirstName,e.LastName,cte.OrderCount
FROM 
	CTE_TakingInfoAboutOrderCount AS cte 
INNER JOIN 
	Employees AS e 
ON
	e.Id = cte.EmpId
ORDER BY 
	cte.OrderCount DESC,
	e.FirstName

/****************************
Problem	8. Hard Workers Club
*****************************/
WITH CTE_TakingInfoForEmpShift(EmpId,FirstName,LastName,WorkHours)
AS
(SELECT
	 e.Id
	,e.FirstName
	,e.LastName 
	,DATEDIFF(hour,s.CheckIn,s.CheckOut)
FROM 
	Employees AS e 
INNER JOIN 
	Shifts AS s 
ON 
	s.EmployeeId=e.Id
),
CTE_TakingAvgHoursPerEmp(EmpId,AvgHours)
AS
(
SELECT 
	cte.EmpId
	, AVG(cte.WorkHours) 
FROM
	CTE_TakingInfoForEmpShift AS cte 
GROUP BY 
	cte.EmpId
)
SELECT 
	e.FirstName
	,e.LastName
	,cteAvg.AvgHours 
FROM 
	CTE_TakingAvgHoursPerEmp AS cteAvg 
INNER JOIN 
	Employees AS e 
ON 
	e.Id = cteAvg.EmpId 
WHERE
	cteAvg.AvgHours > 7
ORDER BY 
	cteAvg.AvgHours DESC
	,cteAvg.EmpId ASC

/****************************
Problem	9. The Most Expensive Order
*****************************/
SELECT TOP(1) 
	oi.OrderId
	,SUM(OI.Quantity*i.Price) 
FROM 
	OrderItems AS oi 
JOIN 
	Items AS i 
ON 
	oi.ItemId= i.Id 
GROUP BY 
	oi.OrderId 
ORDER BY 
	SUM(OI.Quantity*i.Price) DESC

/****************************
Problem	10. Rich Item, Poor Item
*****************************/
SELECT TOP(10) 
	oi.OrderId 
	,MAX(i.Price) AS ExpensivePrice 
	,MIN(i.Price) AS CheapPrice 
FROM 
	OrderItems AS oi 
INNER JOIN 
	Items AS i 
ON 
	i.Id = oi.ItemId 
GROUP BY 
	oi.OrderId 
ORDER BY 
	MAX(i.Price) DESC
	,oi.OrderId ASC

/****************************
Problem	11. Cashiers
*****************************/
SELECT DISTINCT 
	e.Id
	,e.FirstName
	,e.LastName 
FROM 
	Employees AS e 
INNER JOIN 
	Orders AS o 
ON 
	e.Id = o.EmployeeId 
ORDER BY e.Id

/****************************
Problem	12. Lazy Employees
*****************************/
SELECT DISTINCT
  e.Id,
  FirstName + ' ' + e.LastName AS [Full Name]
FROM Employees AS e
  JOIN Shifts AS s ON s.EmployeeId = e.Id
WHERE DATEDIFF(HOUR, s.CheckIn, s.CheckOut) < 4
ORDER BY e.Id

/****************************
Problem	13. Sellers
*****************************/
SELECT TOP 10 E.FirstName + ' ' + E.LastName AS [Full Name],
       SUM(OI.Quantity  * I.Price) AS [Total Price],
       SUM(OI.Quantity) AS ItemsCount
FROM Orders O
JOIN Employees E on O.EmployeeId = E.Id
JOIN OrderItems OI on O.Id = OI.OrderId
JOIN Items I on OI.ItemId = I.Id
WHERE O.DateTime < '2018-06-15'
GROUP BY E.Id, E.FirstName, E.LastName
ORDER BY [Total Price] DESC, ItemsCount DESC


/****************************
Problem	14. Tough days
*****************************/
SELECT 
	CONCAT(e.FirstName,' ',e.LastName) AS FullName
	,DATENAME(WEEKDAY,s.CheckOut) AS [Day of week] 
FROM 
	Employees AS e 
LEFT JOIN 
	Orders AS o 
ON
	e.Id = o.EmployeeId 
INNER JOIN 
	Shifts AS s 
ON 
	s.EmployeeId = e.Id 
WHERE 
	o.Id IS NULL 
	AND 
	DATEDIFF(HOUR,s.CheckIn,s.CheckOut) > 12 
ORDER BY 
	e.Id

/****************************
Problem	15. Top Order per Employee
*****************************/ 
SELECT emp.FirstName + ' ' + emp.LastName AS FullName, DATEDIFF(HOUR, s.CheckIn, s.CheckOut) AS WorkHours, e.TotalPrice AS TotalPrice FROM 
 (
    SELECT o.EmployeeId, SUM(oi.Quantity * i.Price) AS TotalPrice, o.DateTime,
	ROW_NUMBER() OVER (PARTITION BY o.EmployeeId ORDER BY o.EmployeeId, SUM(i.Price * oi.Quantity) DESC ) AS Rank
    FROM Orders AS o
    JOIN OrderItems AS oi ON oi.OrderId = o.Id
    JOIN Items AS i ON i.Id = oi.ItemId
GROUP BY o.EmployeeId, o.Id, o.DateTime
) AS e 
JOIN Employees AS emp ON emp.Id = e.EmployeeId
JOIN Shifts AS s ON s.EmployeeId = e.EmployeeId
WHERE e.Rank = 1 AND e.DateTime BETWEEN s.CheckIn AND s.CheckOut
ORDER BY FullName, WorkHours DESC, TotalPrice DESC


/****************************
Problem	16. Average Profit per Day
*****************************/
SELECT
DATEPART(DAY, o.DateTime)  AS [DayOfMonth],
CAST(AVG(i.Price * oi.Quantity)  AS decimal(15, 2)) AS TotalPrice
FROM Orders AS o
JOIN OrderItems AS oi ON oi.OrderId = o.Id
JOIN Items AS i ON i.Id = oi.ItemId
GROUP BY DATEPART(DAY, o.DateTime)
ORDER BY DayOfMonth ASC

/****************************
Problem	17. Top Products
*****************************/
SELECT 
	i.[Name] AS Item
	,c.[Name] AS Category
	,SUM(oi.Quantity) AS Count
	,SUM(i.Price * oi.Quantity) AS TotalPrice
FROM 
	Items AS i 
INNER JOIN Categories AS c ON i.CategoryId = c.Id 
LEFT JOIN OrderItems AS oi ON oi.ItemId = i.Id
GROUP BY 
	i.[Name]
	,oi.ItemId
	,c.[Name]
ORDER BY 
	SUM(i.Price * oi.Quantity) DESC
	,SUM(oi.Quantity) DESC

/****************************
Problem	18. Promotion days
*****************************/
CREATE FUNCTION udf_GetPromotedProducts
	(
	@CurrentDate DATETIME, @StartDate DATETIME, @EndDate DATETIME
	,@Discount DECIMAL(15,2), @FirstItemId INT, @SecondItemId INT, @ThirdItemId INT
	)
RETURNS VARCHAR(MAX)
AS
BEGIN
	DECLARE @FirstId INT= (SELECT Id FROM Items WHERE Id = @FirstItemId)
	DECLARE @SecondId INT= (SELECT Id FROM Items WHERE Id = @SecondItemId)
	DECLARE @ThirdId INT= (SELECT Id FROM Items WHERE Id = @ThirdItemId)

	IF(@FirstId IS NULL OR @SecondId IS NULL OR @ThirdId IS NULL)
	BEGIN
		RETURN('One of the items does not exists!');
	END
	
	IF(@CurrentDate NOT BETWEEN @StartDate AND @EndDate)
	BEGIN
		RETURN('The current date is not within the promotion dates!');
	END

	DECLARE @FirstItemName VARCHAR(30) = (
										 SELECT 
											[Name] 
										FROM Items 
										WHERE Id = @FirstItemId)
	 
	 DECLARE @SecondItemName VARCHAR(30) = (
										 SELECT 
											[Name] 
										FROM Items 
										WHERE Id = @SecondItemId)

	DECLARE @ThirdItemName VARCHAR(30) = (
										 SELECT 
											[Name] 
										FROM Items 
										WHERE Id = @ThirdItemId)

	DECLARE @FirstItemDiscountPrice VARCHAR(30) = CAST((
													SELECT
														CAST(Price - (Price*(@Discount/100))AS DECIMAL(15,2)) 
													FROM Items 
													WHERE Id = @FirstItemId) AS VARCHAR(30))

	DECLARE @SecondItemDiscountPrice VARCHAR(30) = CAST((
													SELECT
														CAST(Price -(Price*(@Discount/100)) AS DECIMAL(15,2)) 
													FROM Items 
													WHERE Id = @SecondItemId) AS VARCHAR(30))
													
	DECLARE @ThirdItemDiscountPrice VARCHAR(30) = CAST((
													SELECT
														CAST(Price - (Price*(@Discount/100)) AS DECIMAL(15,2)) 
													FROM Items 
													WHERE Id = @ThirdItemId) AS  VARCHAR(30))

	RETURN @FirstItemName + 
	' price: '+ 
	@FirstItemDiscountPrice+
	' <-> '
	+@SecondItemName+
	' price: '+
	@SecondItemDiscountPrice+
	' <-> '+
	@ThirdItemName+
	' price: '+
	@ThirdItemDiscountPrice
END


/****************************
Problem	19. Cancel order
*****************************/
CREATE PROC usp_CancelOrder(@OrderId INT, @CancelDate DATETIME)
AS
BEGIN
	BEGIN TRAN

	DECLARE @OrderDate DATETIME = (SELECT DateTime FROM Orders WHERE Id = @OrderId)
	DELETE FROM OrderItems WHERE OrderId = @OrderId
	DELETE FROM Orders WHERE Id = @OrderId

	IF(@OrderDate IS NULL)
	BEGIN
		RAISERROR('The order does not exist!',16,1);
		ROLLBACK;
		RETURN
	END

	IF(DATEDIFF(DAY,@OrderDate,@CancelDate) >= 3)
	BEGIN
	RAISERROR('You cannot cancel the order!',16,2)
	ROLLBACK;
	RETURN
	END

	COMMIT
END


/****************************
Problem	20. Deleted Order
*****************************/
SELECT * INTO DeletedOrders FROM OrderItems
TRUNCATE TABLE DeletedOrders

CREATE TRIGGER tr_TransferingDataIntoDeletedOrders ON OrderItems 
AFTER DELETE
AS
BEGIN	
	INSERT INTO DeletedOrders (ItemId,OrderId,Quantity)
	SELECT ItemId,OrderId,Quantity FROM deleted
END

