
begin try
    exec RaiseActorError 'Sql_DbError_ErrorPlain';
end try
begin catch
    throw;
end catch

/* eof */