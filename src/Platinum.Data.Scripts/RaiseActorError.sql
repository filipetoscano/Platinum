--
-- RaiseActorError
-- 
--

if not exists ( select * from sys.procedures where name = 'RaiseActorError' )
    exec sp_executesql N'create procedure RaiseActorError as begin return 0; end;'
go


alter procedure RaiseActorError(
    @errorId varchar(100),
    @arg1 nvarchar(100) = null,
    @arg2 nvarchar(100) = null,
    @arg3 nvarchar(100) = null,
    @arg4 nvarchar(100) = null
)
as
begin

    declare @errorMessage nvarchar(400);

    set @errorMessage = N'�' + @errorId;

    if ( @arg1 is not null )
        set @errorMessage = @errorMessage + N' �' + @arg1;

    if ( @arg2 is not null )
        set @errorMessage = @errorMessage + N' �' + @arg2;

    if ( @arg3 is not null )
        set @errorMessage = @errorMessage + N' �' + @arg3;

    if ( @arg4 is not null )
        set @errorMessage = @errorMessage + N' �' + @arg4;

    raiserror( @errorMessage, 16, 1 );
end;
go

-- eof