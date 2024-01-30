DROP TABLE [Products]

CREATE TABLE [dbo].[Products](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY,
	[productname] [varchar](150),
	[productdesc] [varchar](250),
	[quantity] int,
	[price] decimal(10,2),
	[image] [varchar](250)
	)
DROP TABLE [Orders]

CREATE TABLE [dbo].[OrderHeader](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY,
	[customername] [varchar](150),	
	[email] [varchar](250),
	[ordernumber] [varchar](250)
)

CREATE TABLE [dbo].[OrderDetail](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY,
	[ordernumber] [varchar](250),
	[productid] [int],
	[orderquantity] [int],
	[price]  decimal(10,2)
)
delete from orderheader
delete from orderdetail

select * from orderheader
select * from orderdetail
select * from Products
update products set image='laptop.jpeg' where id in (1,3,5)
update products set image='battery.jpeg' where id in (2,4,6)

update products set quantity=20
delete from products
insert into products values('Lenova','Lenova Laptop',100,629.99);
insert into products values('Lenova Battery','Lenova Battery',100,119.99);
insert into products values('DELL','DELL Laptop',100,1629.99);
insert into products values('DELL Battery','DELL Battery',100,119.99);
insert into products values('HP','HP Laptop',100,829.99);
insert into products values('HP Battery','HP Battery',100,99.00);