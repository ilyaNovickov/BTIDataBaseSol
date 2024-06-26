﻿--Создание БД
create database BTIDataBase;
go
--Подключение БД
use BTIDataBase;
go
--Создание таблицы Зданий
create table BuildingsTable
(
	Kadastr nvarchar(20) primary key,
	Address nvarchar(60) not null,
	District nvarchar(15) not null,
	Land int not null,
	Year int not null,
	Material nvarchar(15) not null,
	Base nvarchar(15) not null,
	Comments nvarchar(max) null default null,
	Wear int not null default 0,
	Flow int not null,
	Line int not null,
	Square int not null,
	Picture varbinary(max) null,
	Flats int not null,
	Elevator bit not null default 0
)
go
--Создание таблицы квартир
create table FlatsTable
(
	FlatId int primary key identity,
	Flat int not null,
	Storey int not null,
	Rooms int not null,
	Level bit not null default 0,
	SquareFlat int not null,
	Dwell int not null,
	Branch int not null,
	Balcony int not null default 0,
	Height int not null,
	BuildingKadastr nvarchar(20) references BuildingsTable(Kadastr) on delete cascade ON UPDATE CASCADE
)
go
--Создание таблицы комнат
create table RoomsTable
(
	RoomId int primary key identity,
	Record int not null,
	SquareRoom int not null,
	Size nvarchar(40) not null,
	Name nvarchar(30) not null,
	Decoretion nvarchar(60) not null default N'Отсуствует',
	HeightRoom int not null,
	Socket int not null,
	Section int not null,
	Flat int references FlatsTable(FlatId) on delete cascade ON UPDATE CASCADE
)
go

--Добавление записей для зданий
insert BuildingsTable values (N'69:40:0100230:58', N'г Тверь, ул Горького, д. 140', 
N'Центральный', 4833, 1971, N'Кирпич', N'Бетон', default, default , 10, 15, 50, default , 50, default)
go
insert BuildingsTable values (N'69:40:0400099:174', N'г Тверь, ул Александра Завидова, д. 24', 
N'Центральный', 6853, 1982, N'Кирпич', N'Бетон', default, default , 5, 8, 30, default , 50, default)
go
insert BuildingsTable values (N'69:40:0300336:12', N'г Тверь, ул Марины Расковой, д. 41/42', 
N'Южный', 562, 1968, N'Кирпич', N'Бетон', default, 27, 2, 23, 45, default , 8, default)
go
--Добавление записей для квартир
insert into FlatsTable (Flat, Storey, Rooms, Level, SquareFlat, Dwell, Branch, Balcony, Height, BuildingKadastr) 
values (1, 1, 3, 0, 50, 30, 20, 0, 3, N'69:40:0100230:58')
go
insert FlatsTable values (2, 1, 3, 0, 50, 30, 20, 0, 3, N'69:40:0100230:58')
go
insert FlatsTable values (3, 2, 3, 0, 50, 28, 18, 4, 3, N'69:40:0100230:58')
go
insert FlatsTable values (18, 3, 4, 0, 30, 15, 10, 5, 3, N'69:40:0400099:174')
go
insert FlatsTable values (22, 3, 4, 0, 30, 15, 10, 5, 3, N'69:40:0400099:174')
go
insert FlatsTable values (3, 1, 4, 0, 30, 15, 15, 0, 3, N'69:40:0400099:174')
go
insert FlatsTable values (4, 2, 5, 0, 45, 30, 15, 0, 4, N'69:40:0300336:12')
go
--Добавление записей для комнат
insert RoomsTable values (1, 30, '5x6', N'Комната', N'Паркт, обои', 3, 10, 2, 1)
go
insert RoomsTable values (2, 5, '2.5x2', N'Сан узел', N'Плитка', 3, 0, 1, 1)
go
insert RoomsTable values (3, 15, '3x5', N'Кухня', N'Обои, ленолиум', 3, 3, 1, 1)
go
insert RoomsTable values (4, 30, '5x6', N'Комната', N'Паркт, обои', 3, 10, 2, 2)
go
insert RoomsTable values (5, 5, '2.5x2', N'Сан узел', N'Плитка, обои', 3, 0, 1, 2)
go
insert RoomsTable values (6, 15, '3x5', N'Кухня', N'Обои, ленолиум, натяжной потолок', 3, 3, 1, 2)
go
insert RoomsTable values (7, 30, '5x6', N'Комната', N'Лелониум, обои', 3, 10, 2, 3)
go
insert RoomsTable values (8, 5, '2.5x2', N'Сан узел', N'Плитка, обои', 3, 0, 1, 3)
go
insert RoomsTable values (6, 15, '3x5', N'Кухня', N'Обои, ленолиум, натяжной потолок', 3, 3, 1, 3)
go
insert RoomsTable values (1, 15, '5x3', N'Комната', N'Обои, лелониум', 3, 3, 2, 4)
go
insert RoomsTable values (2, 3, '1.5x2', N'Сан узел', N'Плитка', 3, 1, 1, 4)
go
insert RoomsTable values (3, 7, '4x1.75', N'Кухня', N'Обои, лелониум', 3, 2, 1, 4)
go
insert RoomsTable values (8, 15, '3x5', N'Комната', N'Обои, паркет', 3, 5, 2, 5)
go
insert RoomsTable values (9, 3, '1.5x2', N'Сан узел', N'Плитка', 3, 1, 1, 5)
go
insert RoomsTable values (10, 7, '4x1.75', N'Кухня', N'Обои, лелониум', 3, 2, 1, 5)
go
insert RoomsTable values (16, 15, '3x5', N'Комната', N'Обои, паркет', 3, 3, 2, 6)
go
insert RoomsTable values (17, 3, '1.5x2', N'Сан узел', N'Плитка', 3, 1, 1, 6)
go
insert RoomsTable values (18, 7, '2x3.5', N'Кухня', N'Обои, лелониум', 3, 2, 1, 6)
go
insert RoomsTable values (8, 20, '4x5', N'Комната', N'Обои, лелолиум', 4, 3, 2, 7)
go
insert RoomsTable values (8, 10, '4x2.5', N'Комната', N'Обои, лелолиум', 4, 2, 1, 7)
go
insert RoomsTable values (9, 5, '2.5x2', N'Сан узел', N'Плитка', 4, 1, 1, 7)
go
insert RoomsTable values (10, 10, '3x3.5', N'Кухня', N'Лелониум', 4, 2, 1, 7)
go

--Создание процедуры добавления здания
create procedure AddBuilding
	@kadastr nvarchar(20),
	@address nvarchar(15),
	@district nvarchar(15),
	@land int,
	@year int,
	@material nvarchar(15),
	@base nvarchar(15),
	@flow int,
	@line int,
	@square int,
	@flats int,
	@comments nvarchar = null,
	@wear int = 0,
	@picture varbinary(max) = null,
	@elevator bit = 0
as insert into BuildingsTable values (@kadastr, @address, @district, @land, @year, 
@material, @base, @comments, @wear, @flow, @line, @square, @picture, @flats, @elevator)
go
--Создание процедуры добавления квартиры
create procedure AddFlat
	@buildingKadastr nvarchar,
	@flat int,
	@storey int,
	@rooms int,
	@squareFlat int,
	@dwell int,
	@branch int,
	@height int,
	@level bit = 0,
	@balcony int = 0
as insert into FlatsTable (Flat, Storey, Rooms, Level, SquareFlat, Dwell, Branch, Balcony, Height, BuildingKadastr) 
values (@flat, @storey, @rooms, @level ,@squareFlat, @dwell, @branch, @balcony, @height, @buildingKadastr )
go
--Добавление процедуры добавления помещения
create procedure AddRoom
	@flat int,
	@record int,
	@squareRoom int,
	@size nvarchar(40),
	@name nvarchar(30),
	@heightRoom int,
	@socket int,
	@section int,
	@decoretion nvarchar(60) = 'Отсуствует'
as insert into RoomsTable (Flat, Record, SquareRoom, Size, Name, Decoretion, HeightRoom, Socket, Section) 
values (@flat, @record, @squareRoom, @size, @name, @decoretion, @heightRoom, @socket, @section) 
go
--Получить квартиры по кадастру здания
create procedure GetFlats
	@buildingKadastr nvarchar 
as select * from FlatsTable where BuildingKadastr = @buildingKadastr
go
--Получить комнаты по Id квартиры
create procedure GetRooms
	@FlatId int 
as select * from RoomsTable where Flat = @FlatId
go
--Удалить здание по кадастру
create procedure DeleteBuilding
	@buildingKadastr nvarchar
as delete from BuildingsTable where kadastr = @buildingKadastr
go
--Удалить квартиру по Id
create procedure DeleteFlat
	@FlatId int
as delete from FlatsTable where FlatId = @FlatId
go
--Удалить комнату по Id
create procedure DeleteRoom
	 @RoomId int
as delete from RoomsTable where RoomId = @RoomId



