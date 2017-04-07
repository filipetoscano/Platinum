--
-- Reset database
-- Drops: (referential) constraints, procedures, types, views and tables.
-- Other constraint types (unique, check) and indexes are automatically
-- dropped when the dependant table is dropped.
--

--
-- References and user-defined types
--
declare @stmt nvarchar(1000);
declare @cursor cursor;

set @cursor = cursor fast_forward for
    select distinct Statement = 'alter table [' + tc.TABLE_NAME + '] drop [' + rc.CONSTRAINT_NAME + ']'
    from INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS rc
    left outer join INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc
        on ( tc.CONSTRAINT_NAME = rc.CONSTRAINT_NAME )

    union all

    select 'drop procedure [' + name + ']'
    from sys.procedures

    union all

    select Statement = 'drop type [' + name + ']'
    from sys.types
    where is_user_defined = 1

    union all

    select 'drop view [' + name + ']'
    from sys.views

    union all

    select 'drop table [' + name + ']'
    from sys.tables;


open @cursor;

fetch next from @cursor
into @stmt;

while ( @@fetch_status = 0 )
begin
    print @stmt;
    exec sp_executesql @stmt;

    fetch next from @cursor
    into @stmt;
end;

/* eof */