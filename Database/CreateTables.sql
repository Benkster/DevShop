CREATE TABLE Roles (
	RoleNr int NOT NULL,
	Role nvarchar(100) NOT NULL,
	RoleDescr nvarchar(200) NULL,

	CONSTRAINT pk_Roles PRIMARY KEY (RoleNr),
	CONSTRAINT uc_Roles UNIQUE (Role)
);


CREATE TABLE Users (
	UserID int IDENTITY(1, 1) NOT NULL,
	FirstName nvarchar(150) NOT NULL,
	LastName nvarchar(150) NOT NULL,
	PreTitle nvarchar(50) NULL,
	PostTitle nvarchar(50) NULL,
	Password nvarchar(400) NOT NULL,
	Mail nvarchar(60) NOT NULL,
	Tel nvarchar(30) NULL,
	RoleNr int NOT NULL,

	CONSTRAINT pk_Users PRIMARY KEY (UserID),
	CONSTRAINT fk_Users_Roles FOREIGN KEY (RoleNr) REFERENCES Roles (RoleNr),
	CONSTRAINT uc_Users UNIQUE (Mail)
);


CREATE TABLE Categories (
	CategoryID int IDENTITY(1, 1) NOT NULL,
	Title nvarchar(150) NOT NULL,
	Description nvarchar(350) NULL,
	ParentID int NULL,

	CONSTRAINT pk_Categories PRIMARY KEY (CategoryID)
);


CREATE TABLE Countries (
	CountryID int IDENTITY(1, 1) NOT NULL,
	CountryCode nvarchar(3) NOT NULL,
	Country nvarchar(150) NOT NULL,

	CONSTRAINT pk_Countries PRIMARY KEY (CountryID, CountryCode),
	CONSTRAINT uc_Countries UNIQUE (CountryCode)
);


CREATE TABLE States (
	StateID int IDENTITY(1, 1) NOT NULL,
	CountryID int NOT NULL,
	CountryCode nvarchar(5) NOT NULL,
	State nvarchar(150) NOT NULL,

	CONSTRAINT pk_States PRIMARY KEY (StateID),
	CONSTRAINT fk_States_Countries FOREIGN KEY (CountryID, CountryCode) REFERENCES Countries (CountryID, CountryCode)
);


CREATE TABLE Cities (
	StateID int NOT NULL,
	ZIP nvarchar(5) NOT NULL,
	City nvarchar(150) NOT NULL,

	CONSTRAINT pk_Cities PRIMARY KEY (StateID, ZIP),
	CONSTRAINT fk_Cities_States FOREIGN KEY (StateID) REFERENCES States (StateID)
);


CREATE TABLE Companies (
	CompanyCode nvarchar(7) NOT NULL,
	CompName nvarchar(250) NOT NULL,
	CompAddName nvarchar(250) NULL,
	CompDescr nvarchar(500) NULL,
	Tel nvarchar(30) NULL,
	Fax nvarchar(30) NULL,
	Mail nvarchar(60) NULL,
	Website nvarchar(100) NULL,

	CONSTRAINT pk_Companies PRIMARY KEY (CompanyCode)
);


CREATE TABLE Addresses (
	AddressID int IDENTITY(1, 1) NOT NULL,
	StateID int NOT NULL,
	ZIP nvarchar(5) NOT NULL,
	Street nvarchar(150) NOT NULL,
	HousNr nvarchar(20) NOT NULL,
	AddressInfo nvarchar(300) NULL,

	CONSTRAINT pk_Addresses PRIMARY KEY (AddressID),
	CONSTRAINT fk_Addresses_Cities FOREIGN KEY (StateID, ZIP) REFERENCES Cities (StateID, ZIP)
);


CREATE TABLE CompanyAddresses (
	CompanyCode nvarchar(7) NOT NULL,
	AddressID int NOT NULL,

	CONSTRAINT pk_CompanyAddresses PRIMARY KEY (CompanyCode, AddressID),
	CONSTRAINT fk_CompanyAddresses_Companies FOREIGN KEY (CompanyCode) REFERENCES Companies (CompanyCode),
	CONSTRAINT fk_CompanyAddresses_Addresses FOREIGN KEY (AddressID) REFERENCES Addresses (AddressID)
);


CREATE TABLE ProductGroups (
	ProductGroupNr int NOT NULL,
	CompanyCode nvarchar(7) NOT NULL,
	GroupName nvarchar(150) NOT NULL,
	GroupDescr nvarchar(500) NULL,
	OrderNr int NOT NULL,
	ParentNr int NULL,

	CONSTRAINT pk_ProductGroups PRIMARY KEY (ProductGroupNr, CompanyCode),
	CONSTRAINT fk_ProductGroups_Companies FOREIGN KEY (CompanyCode) REFERENCES Companies (CompanyCode)
);


CREATE TABLE Products (
	ProductNr int NOT NULL,
	ProductGroupNr int NOT NULL,
	ProductName nvarchar(150) NOT NULL,
	ProductDescr nvarchar(800) NOT NULL,
	OrderNr int NOT NULL,

	CONSTRAINT pk_Products PRIMARY KEY (ProductGroupNr, ProductNr),
	CONSTRAINT fk_Products_ProductGroups FOREIGN KEY (ProductGroupNr) REFERENCES ProductGroups (ProductGroupNr)
);


CREATE TABLE Units (
	UnitCode nvarchar(3) NOT NULL,
	Unit nvarchar(50),

	CONSTRAINT pk_Units PRIMARY KEY (UnitCode)
);


CREATE TABLE Articles (
	ArticleNr int NOT NULL,
	ProductNr int NOT NULL,
	ProductGroupNr int NOT NULL,
	ArticleName nvarchar(150) NOT NULL,
	ArticleDescr nvarchar(800) NULL,
	ArticleCode nvarchar(30) NULL,
	EAN nvarchar(13) NULL,
	OrderNr int NOT NULL,
	UnitAmount int NULL,
	BillingUnit nvarchar(3) NOT NULL,
	OrderUnit nvarchar(3) NOT NULL,
	F1 nvarchar(150) NULL,
	F2 nvarchar(150) NULL,
	F3 nvarchar(150) NULL,
	F4 nvarchar(150) NULL,
	F5 nvarchar(150) NULL,
	Price decimal(5, 3) NOT NULL,
	Discount decimal(3, 2) NULL,
	OverruleUserDiscount bit NOT NULL,

	CONSTRAINT pk_Articles PRIMARY KEY (ProductNr, ArticleNr),
	CONSTRAINT fk_Articles_Products FOREIGN KEY (ProductNr, ProductGroupNr) REFERENCES Products (ProductGroupNr, ProductNr),
	CONSTRAINT fk_Articles_Billing_Units FOREIGN KEY (BillingUnit) REFERENCES Units (UnitCode),
	CONSTRAINT fk_Articles_Order_Units FOREIGN KEY (OrderUnit) REFERENCES Units (UnitCode)
);


CREATE TABLE UserDiscounts (
	UserDiscountID int IDENTITY(1, 1) NOT NULL,
	UserID int NOT NULL,
	ProductNr int NOT NULL,
	ArticleNr int NOT NULL,
	Discount decimal(3, 2) NOT NULL,

	CONSTRAINT pk_UserDiscounts PRIMARY KEY (UserDiscountID),
	CONSTRAINT fk_UserDiscounts_Users FOREIGN KEY (UserID) REFERENCES Users (UserID),
	CONSTRAINT fk_UserDiscounts_Articles FOREIGN KEY (ProductNr, ArticleNr) REFERENCES Articles (ProductNr, ArticleNr)
);