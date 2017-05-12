declare @userId uniqueidentifier;
declare @int int;

begin try
    set @userId = newid();
    set @int = 32;

    exec RaiseActorError N'Sql_DbErrorArg_ErrorWithArgs', @userId, @int;
end try
begin catch
    throw;
end catch

/* eof */