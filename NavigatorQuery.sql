
create database navigator
use navigator

--create table
create table dbo.Kandidat(
	JMBG numeric(13) primary key check(JMBG>999999999999 and JMBG<10000000000000) ,
	Ime varchar(50) not null,
	Prezime varchar(50) not null,
	GodinaRodjenja date not null,
	Email varchar(50) not null,
	Telefon varchar(20) not null,
	Napomena varchar(2000) not null,
	Zaposlen bit not null,
	DatumPoslednjeIzmene date not null
)

--ispis svih kandidata
select * from Kandidat;
drop table Kandidat
--dodavanje kandidata
insert  into Kandidat(JMBG,Ime,Prezime,GodinaRodjenja,Email,Telefon,Napomena,Zaposlen,DatumPoslednjeIzmene)
values(1238567891234,'Petar','Petrovic','2019-08-20 10:22:34','petar@gmail.com','065/456-789','Petar ',1,'2019-08-20 10:22:34')
