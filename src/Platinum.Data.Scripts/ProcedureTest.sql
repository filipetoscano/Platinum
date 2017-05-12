--
--
--

declare @int int;
declare @datetime datetime;
declare @varchar varchar(10);
declare @guid uniqueidentifier;

set @int = 5;
set @datetime = '2010-01-01';
set @varchar = 'Hey there';
set @guid = newid();

exec dbo.RaiseActorError 'XPTO', @int, @datetime, @varchar, @guid;

-- eof