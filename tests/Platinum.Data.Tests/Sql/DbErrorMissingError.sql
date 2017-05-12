
begin try
    exec RaiseActorError N'Sql_DbErrorMissingError_MissingError';
end try
begin catch
    throw;
end catch

/* eof */