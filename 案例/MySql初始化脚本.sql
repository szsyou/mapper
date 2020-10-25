create database myshare;

-- 打开数据库
use myshare;

-- 创建账户表
create table account(id int PRIMARY key auto_increment,
name varchar(20),
rest int)auto_increment=1;

-- 初始化账户信息表
insert into account(name,rest) 
select 'jack',10000
union ALL
SELECT 'tom',6000;

-- 创建人员表
create table emp(eno varchar(20) PRIMARY key,ename varchar(20),sex varchar(6));

-- 初始化人员表
insert into emp
SELECT 'A001','Hellen','male'
UNION ALL
SELECT 'B002','Rose','female';

-- 添加用户信息存储过程
create PROCEDURE sp_account_add(_name VARCHAR(20),_rest int)
begin
  insert into account(name,rest) values(_name,_rest);
end;

-- 创建搜索和返回总记录数的存储过程
create PROCEDURE sp_accountInfo(_content varchar(20),out _recordCount int)
BEGIN   
   set @search=CONCAT('%',_content,'%');
   select count(*) into _recordCount from account where name like @search;
   select * from account where name like @search;
end;

-- 调用存储过程
call sp_accountInfo('',@temp);
SELECT @temp;


