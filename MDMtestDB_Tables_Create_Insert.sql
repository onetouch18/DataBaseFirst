CREATE DATABASE MDMtestDB ON
(
	NAME = 'MDMtestDB',
	FILENAME = 'D:\MDMtestDB.mdf',
	SIZE = 10MB,
	MAXSIZE = 50MB,
	FILEGROWTH = 5MB
)
LOG ON
(
	NAME = 'LogMDMtestDB',
	FILENAME = 'D:\MDMtestDB.ldf',
	SIZE = 10MB,
	MAXSIZE = 50MB,
	FILEGROWTH = 5MB
)
GO

USE MDMtestDB 
GO

CREATE TABLE Customer
(
	CustomerId int identity not null, 
	[Name] nvarchar(max),
	[Address] nvarchar(max),
	PhoneNum nvarchar(max),
	PRIMARY KEY(CustomerId)
)
GO

CREATE TABLE [Order]
(
	OrderId int identity not null,
	PRIMARY KEY(OrderId),
	CustomerId int
	FOREIGN KEY REFERENCES Customer(CustomerId)
	ON DELETE CASCADE,
	[Number] nvarchar(max),
	Amount int,
	DueTime DateTime,
	ProcessedTime DateTime,
	[Description] nvarchar(max)
)
GO

INSERT Customer 
values
('Viktor Kozlovets', 'Kiev', '0509858280'),
('Alexandr Durov', 'Kharkov', '0755693294'),
('Olga Vlaeva', 'Donetsk', '0673046712'),
('Anna Pupenko', 'Lviv', '0958673465'),
('Sergey Kazanov', 'Odessa', '0779294039')
GO

INSERT [Order] 
values
(2, '023995', 3,'2016-04-23', '2016-05-23', 'Rocket'),
(2, '756665', 1,'2016-05-25 12:35:29.123', '2016-06-23 11:36:19.123', 'Gun'),
(1, '453454', 4,'2016-07-23 09:20:20.111', '2016-07-23 05:12:56.142', 'Car'),
(1, '436346', 2,'2016-07-28', '2016-05-28', 'Train'),
(3, '342564', 1,'2016-04-23', '2016-05-23', 'Doll'),
(4, '346477', 10,'2016-04-23', '2016-05-23', 'Crayons'),
(4, '346634', 12,'2016-04-23', '2016-05-23', 'Pencils'),
(5, '342342', 25,'2016-04-23', '2016-05-23', 'Soldier'),
(5, '554355', 4,'2016-04-23', '2016-05-23', 'Tank'),
(5, '753452', 5,'2016-04-23', '2016-05-23', 'Flag')
GO