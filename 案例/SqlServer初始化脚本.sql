--�������ݿ�
create database myshare
go

--�����ݿ�
use myshare
go

--�����˻���
create table account(id int PRIMARY key identity(1,1),
name varchar(20),
rest int)
go

--Ϊ�˻����ʼ������
insert into account(name,rest) 
select 'jack',10000
union ALL
SELECT 'tom',6000
go

--������Ա��
create table emp(eno varchar(20) PRIMARY key,ename varchar(20),sex varchar(6))
go

--Ϊ��Ա���ʼ����Ա����
insert into emp
SELECT 'A001','Hellen','male'
UNION ALL
SELECT 'B002','Rose','female'
go

-- ����û���Ϣ�洢����
create PROCEDURE sp_account_add(@_name VARCHAR(20),@_rest int)
as
begin
  insert into account(name,rest) select @_name,@_rest
end
go

--�����˻���Ϣ�����洢����
create PROCEDURE sp_accountInfo(@_content varchar(20),@_recordCount int output)
as
BEGIN
   declare @searchContent varchar(50)
   set @searchContent='%'+ @_content +'%'
   select @_recordCount=count(*) from account where name like @searchContent
   select * from account where name like @searchContent
end
go

--���ô洢����
exec sp_account_add 'toma',3000
go

declare @tempCount int 
exec sp_accountInfo 'jack',@tempCount output
select @tempCount
go
 


