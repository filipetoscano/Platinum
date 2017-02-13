
begin try
    raiserror( N'AE:UserNotFound', 16, 1 );
end try
begin catch
    throw;
end catch

/* eof */