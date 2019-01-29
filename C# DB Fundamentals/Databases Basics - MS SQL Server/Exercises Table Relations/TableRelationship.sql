--********************
--Problem 1.One-To-One Relationship
--********************
CREATE TABLE Persons (
PersonID INT IDENTITY(1,1) NOT NULL,
FirstName VARCHAR(50) NOT NULL,
Salary MONEY NOT NULL,
PassportID INT NOT NULL
)

CREATE TABLE Passports(
PassportID INT NOT NULL,
PassportNumber VARCHAR(50) NOT NULL
)

ALTER TABLE Persons
ADD CONSTRAINT PK_Persons_PersonID PRIMARY KEY(PersonID)

ALTER TABLE Passports
ADD CONSTRAINT PK_Passports_PassportID PRIMARY KEY(PassportID)

ALTER TABLE Persons
ADD CONSTRAINT FK_Persons_PassportID FOREIGN KEY(PassportID) REFERENCES Passports(PassportID)

ALTER TABLE Persons
ADD CONSTRAINT UQ_Persons_UniquePassportID UNIQUE(PassportID)

INSERT INTO Passports(PassportID,PassportNumber)
VALUES
(101,'N34FG21B'),
(102,'K65LO4R7'),
(103,'ZE657QP2')


INSERT INTO Persons (FirstName,Salary,PassportID) 
VALUES
('Roberto',43300.00,102),
('Tom',56100.00,103),
('Yana',60200.00,101)


--********************
--Problem 2.One-To-Many Relationship
--********************
CREATE TABLE Models(
ModelID INT IDENTITY(101,1) NOT NULL,
[Name] VARCHAR(40) NOT NULL,
ManufacturerID INT NOT NULL
)

CREATE TABLE Manufacturers(
ManufacturerID INT IDENTITY(1,1) NOT NULL,
[Name] VARCHAR(25) NOT NULL,
EstablishedOn DATETIME2 NOT NULL
)

ALTER TABLE Models
ADD CONSTRAINT PK_Models_ModelID PRIMARY KEY(ModelID)

ALTER TABLE Manufacturers
ADD CONSTRAINT PK_Manufactorers_ManufacturerID PRIMARY KEY(ManufacturerID)

ALTER TABLE Models
ADD CONSTRAINT FK_Models_ManufactorerID FOREIGN KEY(ManufacturerID) REFERENCES Manufacturers(ManufacturerID)

INSERT INTO Manufacturers([Name],EstablishedOn)
VALUES
('BMW','07/03/1916'),
('Tesla','01/01/2003'),
('Lada','01/05/1966')


INSERT INTO Models([Name],ManufacturerID)
VALUES
('X1',1),
('i6',1),
('Model S',	2),
('Model X',2),
('Model 3',2),
('Nova',3)


--********************
--Problem 3.Many-To-Many Relationship
--********************
CREATE TABLE Students(
StudentID INT IDENTITY(1,1) NOT NULL,
[Name] VARCHAR(50) NOT NULL
)

CREATE TABLE Exams(
ExamID INT IDENTITY(101,1) NOT NULL,
[Name] VARCHAR(50) NOT NULL
)

CREATE TABLE StudentsExams(
StudentID INT NOT NULL,
ExamID INT NOT NULL
)

ALTER TABLE Students
ADD CONSTRAINT PK_Students_StudentID PRIMARY KEY(StudentID)

ALTER TABLE Exams
ADD CONSTRAINT PK_Exams_ExamID PRIMARY KEY(ExamID)

ALTER TABLE StudentsExams
ADD CONSTRAINT PK_StudentsExams_StEx PRIMARY KEY(StudentID,ExamID)

ALTER TABLE StudentsExams
ADD CONSTRAINT FK_StudentsExams_StudentID FOREIGN KEY(StudentID) REFERENCES Students(StudentID)

ALTER TABLE StudentsExams
ADD CONSTRAINT FK_StudentsExams_ExamID FOREIGN KEY(ExamID) REFERENCES Exams(ExamID)

INSERT INTO Students ([Name])
VALUES
('Mila'),
('Toni'),
('Ron')

INSERT INTO Exams([Name])
VALUES
('SpringMVC'),
('Neo4j'),
('Oracle 11g')

INSERT INTO StudentsExams(StudentID,ExamID)
VALUES
(1,101),
(1,102),
(2,101),
(3,103),
(2,102),
(2,103)

--********************
--Problem 4.Self-Referencing 
--********************
CREATE TABLE Teachers(
TeacherID INT IDENTITY(101,1) NOT NULL,
[Name] VARCHAR(50) NOT NULL,
ManagerID INT

PRIMARY KEY(TeacherID)
)

INSERT INTO Teachers([Name],ManagerID)
VALUES
('John',NULL),
('Maya',106),
('Silvia',106),
('Ted',105),
('Mark',101),
('Greta',101)

ALTER TABLE Teachers
ADD CONSTRAINT FK_Teachers_ManagerID FOREIGN KEY(ManagerID) REFERENCES Teachers(TeacherID) ON DELETE NO ACTION


--********************
--Problem 5.Online Store Database
--********************
CREATE TABLE Cities(
CityID INT NOT NULL IDENTITY(1,1),
[Name] VARCHAR(50) NOT NULL
PRIMARY KEY(CityID)
)

CREATE TABLE Customers(
CustomerID INT NOT NULL IDENTITY(1,1),
[Name] VARCHAR(50) NOT NULL,
BirthDate DATE NOT NULL,
CityID INT NOT NULL
PRIMARY KEY(CustomerID)
)

CREATE TABLE Orders(
OrderID INT NOT NULL IDENTITY(1,1),
CustomerID INT NOT NULL
PRIMARY KEY(OrderID)
)

CREATE TABLE Items(
ItemID INT NOT NULL IDENTITY(1,1),
[Name] VARCHAR(50) NOT NULL,
ItemTypeID INT NOT NULL
PRIMARY KEY(ItemID)
)

CREATE TABLE OrderItems(
OrderID INT NOT NULL,
ItemID INT NOT NULL
PRIMARY KEY(OrderID,ItemID)
)

CREATE TABLE ItemTypes(
ItemTypeID INT IDENTITY(1,1) NOT NULL,
[Name] VARCHAR(50) NOT NULL

PRIMARY KEY (ItemTypeID)
)

ALTER TABLE Orders
ADD CONSTRAINT FK_Orders_CustomerID FOREIGN KEY(CustomerID) REFERENCES Customers(CustomerID)

ALTER TABLE Customers
ADD CONSTRAINT FK_Customers_CityID FOREIGN KEY(CityID) REFERENCES Cities(CityID)

ALTER TABLE OrderItems
ADD CONSTRAINT FK_OrderItems_OrderID FOREIGN KEY(OrderID) REFERENCES Orders(OrderID)

ALTER TABLE OrderItems
ADD CONSTRAINT FK_OrderItems_ItemID FOREIGN KEY(ItemID) REFERENCES Items(ItemID)

ALTER TABLE Items
ADD CONSTRAINT FK_Items_ItemTypeID FOREIGN KEY(ItemTypeID) REFERENCES ItemTypes(ItemTypeID)


--********************
--Problem 6.University Database
--********************
CREATE TABLE Majors(
 MajorID INT NOT NULL,
 Name VARCHAR(50)
)
DROP TABLE Majors

CREATE TABLE Payments(
 PaymentID INT NOT NULL,
 PaymentDate DATE,
 PaymentAmount DECIMAL(15,2),
 StudentID INT NOT NULL
)

CREATE TABLE Students(
 StudentID INT NOT NULL,
 StudentNumber VARCHAR(50),
 StudentName VARCHAR(50),
 MajorID INT NOT NULL
)

CREATE TABLE Agenda(
 StudentID INT NOT NULL,
 SubjectID INT NOT NULL
)

CREATE TABLE Subjects(
 SubjectID INT NOT NULL,
 SubjectName VARCHAR(50)  
)

ALTER TABLE Majors 
ADD CONSTRAINT PK_MajorID 
PRIMARY KEY (MajorID)

ALTER TABLE Payments
ADD CONSTRAINT PK_PaymentID
PRIMARY KEY(PaymentID)

ALTER TABLE Students
ADD CONSTRAINT PK_StudentID
PRIMARY KEY(StudentID)

ALTER TABLE Payments
ADD CONSTRAINT FK_StudentID 
FOREIGN KEY (StudentID) REFERENCES Students(StudentID)

ALTER TABLE Students
ADD CONSTRAINT FK_MajorID
FOREIGN KEY(MajorID) REFERENCES Majors(MajorID)

ALTER TABLE Subjects
ADD CONSTRAINT PK_SubjectID
PRIMARY KEY(SubjectID)

ALTER TABLE Agenda
ADD CONSTRAINT PK_Agenda
PRIMARY KEY(SubjectID,StudentID)

ALTER TABLE Agenda
ADD CONSTRAINT FK_Agenda
FOREIGN KEY(StudentID) REFERENCES Students(StudentID),
FOREIGN KEY(SubjectID) REFERENCES Subjects(SubjectID)


--********************
--Problem 9.*Peaks in Rila
--********************
SELECT
	m.MountainRange
	,p.PeakName
	,p.Elevation
FROM
	Mountains
AS m 
	JOIN Peaks AS p ON p.MountainId = m.Id  
WHERE 
	m.MountainRange = 'Rila' 
ORDER BY p.Elevation DESC 