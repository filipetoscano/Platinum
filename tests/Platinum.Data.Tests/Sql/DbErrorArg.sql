declare @userId uniqueidentifier;
declare @int int;
declare @user varchar(36);

begin try
    set @userId = newid();
    set @int = 32;
    set @user = cast( @userId as varchar(36) );

    raiserror( N'AE:UserNotFound:%s:%d', 16, 1, @user, @int );
end try
begin catch
    throw;
end catch

/* eof */