CREATE DATABASE WeatherTestProjectDb;

GO

USE WeatherTestProjectDb;

CREATE TABLE Users 
(
	id bigint NOT NULL PRIMARY KEY,	
	chosenCity nvarchar(255)
);

CREATE TABLE WeatherHistory 
(
	id bigint NOT NULL IDENTITY(1,1) PRIMARY KEY,
	userId bigint NOT NULL FOREIGN KEY REFERENCES Users(id),
	requestDatetime datetime NOT NULL,
	city nvarchar(255) NOT NULL,
	temperatureF float NOT NULL,
	description nvarchar(255) NOT NULL,
	pressure float NOT NULL,
	humidity float NOT NULL,

)
