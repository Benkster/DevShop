CREATE TABLE Categories (
	CategoryID int IDENTITY(1, 1) NOT NULL,
	Category nvarchar(150) NOT NULL,
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
	CountryCode nvarchar(3) NOT NULL,
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
	CompCode nvarchar(6) NOT NULL,
	CompName nvarchar(250) NOT NULL,
	CompAddName nvarchar(250) NULL,
	Description nvarchar(500) NULL,
	Tel nvarchar(30) NULL,
	Mail nvarchar(100) NULL,
	Website nvarchar(100) NULL,

	CONSTRAINT pk_Companies PRIMARY KEY (CompCode)
);


CREATE TABLE Roles (
	RoleNr int NOT NULL,
	Role nvarchar(100) NOT NULL,
	Description nvarchar(250) NULL,

	CONSTRAINT pk_Roles PRIMARY KEY (RoleNr)
);


CREATE TABLE Users (
	UserID int IDENTITY(1, 1) NOT NULL,
	FirstName nvarchar(150) NULL,
	LastName nvarchar(150) NOT NULL,
	UserName nvarchar(50) NOT NULL,
	PreTitle nvarchar(50) NULL,
	PostTitle nvarchar(50) NULL,
	Password nvarchar(700) NOT NULL,
	Mail nvarchar(300) NOT NULL,
	Tel nvarchar(30) NULL,
	RoleNr int NOT NULL,
	AuthCookie nvarchar(400) NULL,
	LastLogin datetime NULL,
	CompCode nvarchar(6) NOT NULL,

	CONSTRAINT pk_Users PRIMARY KEY (UserID),
	CONSTRAINT fk_Users_Roles FOREIGN KEY (RoleNr) REFERENCES Roles (RoleNr),
	CONSTRAINT fk_Users_Companies FOREIGN KEY (CompCode) REFERENCES Companies (CompCode)
);


CREATE TABLE Addresses (
	AddressID int IDENTITY(1, 1) NOT NULL,
	StateID int NOT NULL,
	ZIP nvarchar(5) NOT NULL,
	Street nvarchar(150) NOT NULL,
	HouseNr nvarchar(20) NOT NULL,
	Info nvarchar(300) NULL,

	CONSTRAINT pk_Addresses PRIMARY KEY (AddressID),
	CONSTRAINT fk_Addresses_Cities FOREIGN KEY (StateID, ZIP) REFERENCES Cities (StateID, ZIP)
);


CREATE TABLE CompanyAddresses (
	CompCode nvarchar(6) NOT NULL,
	AddressID int NOT NULL,

	CONSTRAINT pk_CompanyAddresses PRIMARY KEY (CompCode, AddressID),
	CONSTRAINT fk_CompanyAddresses_Companies FOREIGN KEY (CompCode) REFERENCES Companies (CompCode),
	CONSTRAINT fk_CompanyAddresses_Addresses FOREIGN KEY (AddressID) REFERENCES Addresses (AddressID)
);


CREATE TABLE ProductGroups (
	ProductGroupNr int NOT NULL,
	CompCode nvarchar(6) NOT NULL,
	GroupName nvarchar(150) NOT NULL,
	GroupDescription nvarchar(700) NULL,
	SortNr int NOT NULL,
	ParentID int NULL,
	CategoryID int NOT NULL,

	CONSTRAINT pk_ProductGroups PRIMARY KEY (ProductGroupNr, CompCode),
	CONSTRAINT fk_ProductGroups_Companies FOREIGN KEY (CompCode) REFERENCES Companies (CompCode),
	CONSTRAINT fk_ProductGroups_Categories FOREIGN KEY (CategoryID) REFERENCES Categories (CategoryID)
);


CREATE TABLE Products (
	ProductNr int NOT NULL,
	ProductGroupNr int NOT NULL,
	CompCode nvarchar(6) NOT NULL,
	Product nvarchar(150) NOT NULL,
	ProductDescription nvarchar(700) NULL,
	SortNr int NOT NULL,

	CONSTRAINT pk_Products PRIMARY KEY (ProductNr, ProductGroupNr, CompCode),
	CONSTRAINT fk_Products_ProductGroups FOREIGN KEY (ProductGroupNr, CompCode) REFERENCES ProductGroups (ProductGroupNr, CompCode)
);


CREATE TABLE Units (
	UnitCode nvarchar(3) NOT NULL,
	Unit nvarchar(50) NOT NULL,

	CONSTRAINT pk_Units PRIMARY KEY (UnitCode),
	CONSTRAINT uc_Units UNIQUE (Unit)
);


CREATE TABLE ArticleHeaders (
	ArticleHeaderID int IDENTITY(1, 1) NOT NULL,
	ProductNr int NOT NULL,
	ProductGroupNr int NOT NULL,
	CompCode nvarchar(6) NOT NULL,
	F1Name nvarchar(50) NULL,
	F2Name nvarchar(50) NULL,
	F3Name nvarchar(50) NULL,
	F4Name nvarchar(50) NULL,
	F5Name nvarchar(50) NULL,
	F6Name nvarchar(50) NULL,

	CONSTRAINT pk_ArticleHeaders PRIMARY KEY (ArticleHeaderID),
	CONSTRAINT fk_ArticleHeaders_Products FOREIGN KEY (ProductNr, ProductGroupNr, CompCode) REFERENCES Products (ProductNr, ProductGroupNr, CompCode)
);


CREATE TABLE Articles (
	ArticleNr int NOT NULL,
	ProductNr int NOT NULL,
	ProductGroupNr int NOT NULL,
	CompCode nvarchar(6) NOT NULL,
	Article nvarchar(150) NOT NULL,
	ArticleDescription nvarchar(700) NULL,
	ArticleCode nvarchar(30) NULL,
	EAN nvarchar(13) NULL,
	SortNr int NOT NULL,
	UnitAmount int NOT NULL,
	PackagingUnit nvarchar(3) NOT NULL,
	BillingUnit nvarchar(3) NOT NULL,
	Price money NOT NULL,
	Discount decimal(3, 2) NULL,
	OverruleUserDiscount bit NULL,
	ArticleHeaderID int NOT NULL,
	F1 nvarchar(250) NULL,
	F2 nvarchar(250) NULL,
	F3 nvarchar(250) NULL,
	F4 nvarchar(250) NULL,
	F5 nvarchar(250) NULL,
	F6 nvarchar(250) NULL,

	CONSTRAINT pk_Articles PRIMARY KEY (ArticleNr, ProductNr, ProductGroupNr, CompCode),
	CONSTRAINT fk_Articles_Products FOREIGN KEY (ProductNr, ProductGroupNr, CompCode) REFERENCES Products (ProductNr, ProductGroupNr, CompCode),
	CONSTRAINT fk_Articles_Packaging_Units FOREIGN KEY (PackagingUnit) REFERENCES Units (UnitCode),
	CONSTRAINT fk_Articles_Billing_Units FOREIGN KEY (BillingUnit) REFERENCES Units (UnitCode),
	CONSTRAINT fk_Articles_ArticleHeaders FOREIGN KEY (ArticleHeaderID) REFERENCES ArticleHeaders (ArticleHeaderID)
);


CREATE TABLE UserDiscounts (
	UserDiscountID int IDENTITY(1, 1) NOT NULL,
	UserID int NOT NULL,
	ArticleNr int NOT NULL,
	ProductNr int NOT NULL,
	ProductGroupNr int NOT NULL,
	CompCode nvarchar(6) NOT NULL,
	Discount decimal(3, 2) NOT NULL,

	CONSTRAINT pk_UserDiscounts PRIMARY KEY (UserDiscountID),
	CONSTRAINT fk_UserDiscounts_Users FOREIGN KEY (UserID) REFERENCES Users (UserID),
	CONSTRAINT fk_UserDiscounts_Articles FOREIGN KEY (ArticleNr, ProductNr, ProductGroupNr, CompCode) REFERENCES Articles (ArticleNr, ProductNr, ProductGroupNr, CompCode)
);