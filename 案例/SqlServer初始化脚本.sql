--创建数据库
create database myshare
go

--打开数据库
use myshare
go

--创建账户表
create table account(id int PRIMARY key identity(1,1),
name varchar(20),
rest int)
go

--为账户表初始化数据
insert into account(name,rest) 
select 'jack',10000
union ALL
SELECT 'tom',6000
go

--创建人员表
create table emp(eno varchar(20) PRIMARY key,ename varchar(20),sex varchar(6))
go

--为人员表初始化人员数据
insert into emp
SELECT 'A001','Hellen','male'
UNION ALL
SELECT 'B002','Rose','female'
go

-- 添加用户信息存储过程
create PROCEDURE sp_account_add(@_name VARCHAR(20),@_rest int)
as
begin
  insert into account(name,rest) select @_name,@_rest
end
go

--创建账户信息搜索存储过程
create PROCEDURE sp_accountInfo(@_content varchar(20),@_recordCount int output)
as
BEGIN
   declare @searchContent varchar(50)
   set @searchContent='%'+ @_content +'%'
   select @_recordCount=count(*) from account where name like @searchContent
   select * from account where name like @searchContent
end
go

--调用存储过程
exec sp_account_add 'toma',3000
go

declare @tempCount int 
exec sp_accountInfo 'jack',@tempCount output
select @tempCount
go
 


